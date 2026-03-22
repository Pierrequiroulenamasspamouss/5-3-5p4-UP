from flask import Blueprint, jsonify, current_app
import os
import json

sales_bp = Blueprint('sales', __name__)

SERVER_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DEFINITIONS_PATH = os.path.join(SERVER_DIR, "definitions.json")
MARKET_PRICES_PATH = os.path.join(SERVER_DIR, "MarketPrices.json")

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
        print(f"[PRICES] Error: {e}")
        return jsonify({"default": "$9.99"})

@sales_bp.route('/rest/sales/<user_id>/v2', methods=['GET'])
def get_sales(user_id):
    """
    Returns available sales for a user, filtered by ShopSchedule.json and player level.
    """
    from utils.db import get_player_data
    print(f"[SALES] Fetching sales for user {user_id}", flush=True)
    
    # 1. Get Player Level
    profile = get_player_data(user_id)
    player_level = profile.get('PlayerLevel', 0) if profile else 0
    print(f"[SALES] User Level: {player_level}", flush=True)

    if not os.path.exists(DEFINITIONS_PATH):
        return jsonify([])

    # 2. Load Schedule
    schedule = {}
    SCHEDULE_PATH = os.path.join(SERVER_DIR, "ShopSchedule.json")
    if os.path.exists(SCHEDULE_PATH):
        try:
            with open(SCHEDULE_PATH, 'r') as f:
                schedule = json.load(f)
        except Exception as e:
            print(f"[SALES] Schedule Error: {e}")

    active_packs_cfg = schedule.get("active_packs", {})

    try:
        with open(DEFINITIONS_PATH, 'r') as f:
            definitions = json.load(f)
        
        sale_pack_definitions = definitions.get('salePackDefinitions', [])
        
        user_sales = []
        
        for sale_def in sale_pack_definitions:
            pack_id = str(sale_def.get("id", ""))
            
            # --- REFINED LOGIC ---
            is_scheduled = pack_id in active_packs_cfg
            
            if is_scheduled:
                cfg = active_packs_cfg[pack_id]
                start_utc = cfg.get("start_utc", 0)
                end_utc = cfg.get("end_utc", 2147483647)
                min_level = cfg.get("min_level", 0)
                max_purchases = cfg.get("max_purchases", 1)
                
                # Apply base fields
                sale_def["UTCSTARTDATE"] = start_utc
                sale_def["UTCENDDATE"] = end_utc
                sale_def["CANBUYTHISMANYTIMES"] = max_purchases
                
                # RE-ENABLE LEVEL LOCKING: Set UnlockLevel so client can show "Locked" UI
                sale_def["UNLOCKLEVEL"] = min_level
                sale_def["DISABLED"] = False
            else:
                # FORCE EXPIRE (Not in schedule)
                sale_def["UTCSTARTDATE"] = 0
                sale_def["UTCENDDATE"] = 1 # Past
                sale_def["DISABLED"] = True
            
            # Global cleanup to ensure we don't have other client-side blocks
            # (Uppercase to match DeserializeProperty in client)
            sale_def["STOREUNLOCKFTUELEVEL"] = 0
            sale_def["UNLOCKQUESTID"] = 0
            sale_def["UNLOCKBYTRIGGER"] = False
            sale_def["IMPRESSIONS"] = 999
            
            user_sales.append({
                "SaleId": int(pack_id),
                "SaleDefinition": json.dumps(sale_def)
            })
            
        print(f"[SALES] Returning {len(user_sales)} total packs ({sum(1 for s in user_sales if '\"UTCENDDATE\": 1' not in s['SaleDefinition'])} visible)", flush=True)
        return jsonify(user_sales)
        
    except Exception as e:
        print(f"[SALES] Error: {e}")
        return jsonify([])
