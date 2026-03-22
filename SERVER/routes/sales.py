from flask import Blueprint, jsonify, current_app
import os
import json

sales_bp = Blueprint('sales', __name__)

SERVER_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DEFINITIONS_PATH = os.path.join(SERVER_DIR, "definitions.json")
MARKET_PRICES_PATH = os.path.join(SERVER_DIR, "MarketPrices.json")

_cached_definitions = None
_cached_defs_indexed = None

def get_defs():
    global _cached_definitions, _cached_defs_indexed
    if _cached_definitions is None:
        if os.path.exists(DEFINITIONS_PATH):
            try:
                with open(DEFINITIONS_PATH, 'r') as f:
                    _cached_definitions = json.load(f)
                
                # Index all relevant definitions once
                _cached_defs_indexed = {}
                for key in ['salePackDefinitions', 'storeItemDefinitions', 'currencyStorePackDefinitions']:
                    for d in _cached_definitions.get(key, []):
                        _cached_defs_indexed[str(d.get("id", ""))] = d
            except Exception as e:
                print(f"[SALES] GetDefs Error: {e}", flush=True)
    return _cached_definitions, _cached_defs_indexed

@sales_bp.route('/rest/market_prices', methods=['GET'])
def get_market_prices():
    """
    Returns SKU to Price mappings.
    """
    if not os.path.exists(MARKET_PRICES_PATH):
        return jsonify({"default": "$9.99"})
    
    try:
        with open(MARKET_PRICES_PATH, 'r') as f:
            prices = json.load(f)
        return jsonify(prices)
    except Exception as e:
        print(f"[PRICES] Error: {e}", flush=True)
        return jsonify({"default": "$9.99"})

@sales_bp.route('/rest/sales/<user_id>/v2', methods=['GET'])
def get_sales(user_id):
    """
    Returns available sales for a user, filtered by ShopSchedule.json and player level.
    """
    from utils.db import get_player_data
    print(f"[SALES] Fetching sales for user {user_id}", flush=True)
    
    # 1. Get Player Profile
    profile = get_player_data(user_id)
    if not profile:
        profile = {}

    # 2. Load Schedule
    schedule = {}
    SCHEDULE_PATH = os.path.join(SERVER_DIR, "ShopSchedule.json")
    if os.path.exists(SCHEDULE_PATH):
        try:
            with open(SCHEDULE_PATH, 'r') as f:
                schedule = json.load(f)
        except Exception as e:
            print(f"[SALES] Schedule Error: {e}", flush=True)

    active_packs_cfg = schedule.get("active_packs", {})

    try:
        definitions, all_defs = get_defs()
        if not definitions:
            return jsonify([])
        
        # 3. Get purchase history
        purchased_sales = profile.get("purchasedSales", [])
        purchase_counts = {}
        for sale in purchased_sales:
            if isinstance(sale, dict):
                sale_id = str(sale.get("ID", "")) if "ID" in sale else str(sale.get("id", ""))
            else:
                sale_id = str(sale)
            if sale_id:
                purchase_counts[sale_id] = purchase_counts.get(sale_id, 0) + 1

        user_sales = []
        handled_ids = set()
        
        # 4. Process scheduled packs
        for pack_id, cfg in active_packs_cfg.items():
            if pack_id not in all_defs:
                continue
                
            sale_def = all_defs[pack_id].copy()
            
            # Resolve references (e.g. StoreItem -> SalePack)
            if "ReferencedDefID" in sale_def:
                orig_ref_id = sale_def["ReferencedDefID"]
                ref_id_str = str(orig_ref_id)
                if ref_id_str in all_defs:
                    ref_def = all_defs[ref_id_str].copy()
                    ref_def["id"] = int(pack_id)
                    # CRITICAL: Preserve ReferencedDefID for the client-side mediator!
                    ref_def["ReferencedDefID"] = orig_ref_id
                    sale_def = ref_def

            max_purchases = cfg.get("max_purchases", 1)
            current_count = purchase_counts.get(pack_id, 0)
            
            if max_purchases != -1 and current_count >= max_purchases:
                sale_def["DISABLED"] = True
                sale_def["UTCENDDATE"] = 1
            else:
                sale_def["UTCSTARTDATE"] = cfg.get("start_utc", 0)
                sale_def["UTCENDDATE"] = cfg.get("end_utc", 2147483647)
                sale_def["CANBUYTHISMANYTIMES"] = max_purchases - current_count if max_purchases != -1 else -1
                sale_def["UNLOCKLEVEL"] = cfg.get("min_level", 0)
                sale_def["DISABLED"] = False
            
            # FTUE and other flags cleanup
            sale_def["STOREUNLOCKFTUELEVEL"] = 0
            sale_def["UNLOCKQUESTID"] = 0
            sale_def["UNLOCKBYTRIGGER"] = False
            sale_def["IMPRESSIONS"] = 999
            
            user_sales.append({
                "SaleId": int(pack_id),
                "SaleDefinition": json.dumps(sale_def)
            })
            handled_ids.add(str(pack_id))

        # 5. FORCE DISABLE or LEVEL-LOCK potential sales not in the schedule
        # This fixes the "14 notifications" issue.
        # Permanent offers (currency, building packs) get a default level-lock (min level 1).
        # Everything else is fully disabled.
        
        PERMANENT_IDS = {
            # Premium Currency Store Items
            "9098", "9099", "9100", "9101", "9102", "9103", "9104", "9105", "9106",
            # Grind Currency Store Items
            "9107", "9108", "9109", "9110", "9111", "9112",
            # Building/Aspirational Packs Store Items
            "9129", "9130", "9131", "9132", "9133", "9134", "9135", "9136", "9137", "9138", "9139", "9140", "9141", "9142", "9143"
        }

        currency_store = definitions.get("currencyStoreDefinition", {})
        categories = currency_store.get("currencyStoreCategoryDefinitions", [])
        
        disable_targets = set()
        for cat in categories:
            for item_id in cat.get("StoreItemDefinitionIDs", []):
                disable_targets.add(str(item_id))
        
        for d in definitions.get('salePackDefinitions', []):
            disable_targets.add(str(d.get("id", "")))

        for pid_str in disable_targets:
            if pid_str not in handled_ids and pid_str in all_defs:
                orig_def = all_defs[pid_str]
                d_copy = orig_def.copy()
                d_copy["id"] = int(pid_str)
                if "ReferencedDefID" in orig_def:
                    d_copy["ReferencedDefID"] = orig_def["ReferencedDefID"]
                
                if pid_str in PERMANENT_IDS:
                    # Level lock it but keep it enabled
                    d_copy["DISABLED"] = False
                    d_copy["UNLOCKLEVEL"] = 1 # Default lock for level 0 avoiders
                else:
                    # Fully disable it
                    d_copy["DISABLED"] = True
                    d_copy["UTCSTARTDATE"] = 0
                    d_copy["UTCENDDATE"] = 1
                
                user_sales.append({
                    "SaleId": int(pid_str),
                    "SaleDefinition": json.dumps(d_copy)
                })
                handled_ids.add(pid_str)
            
        print(f"[SALES] Returning {len(user_sales)} sale overrides for user {user_id}", flush=True)
        return jsonify(user_sales)
        
    except Exception as e:
        print(f"[SALES] Route Error: {e}", flush=True)
        return jsonify([])
