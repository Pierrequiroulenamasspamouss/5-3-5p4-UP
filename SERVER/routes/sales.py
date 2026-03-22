from flask import Blueprint, jsonify, current_app
import os
import json

sales_bp = Blueprint('sales', __name__)

SERVER_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DEFINITIONS_PATH = os.path.join(SERVER_DIR, "definitions.json")

@sales_bp.route('/rest/sales/<user_id>/v2', methods=['GET'])
def get_sales(user_id):
    """
    Returns available sales for a user.
    The client expects a list of UserSale objects.
    """
    print(f"[SALES] Fetching sales for user {user_id}")
    
    if not os.path.exists(DEFINITIONS_PATH):
        return jsonify([])

    try:
        with open(DEFINITIONS_PATH, 'r') as f:
            definitions = json.load(f)
        
        sale_pack_definitions = definitions.get('salePackDefinitions', [])
        
        # We need to return a list of UserSale
        # UserSale structure (from client code analysis):
        # {
        #   "SaleId": int/string,
        #   "SaleDefinition": string (JSON of SalePackDefinition)
        # }
        
        user_sales = []
        # The 'now' variable is no longer strictly needed for UTCEndDate as it's set to a fixed future date above.
        # However, keeping it for consistency if other logic might use it.
        now = 2000000000 # Year 2033
        for i, sale_def in enumerate(sale_pack_definitions):
            # The following modifications are now largely redundant due to the aggressive unlocking block above,
            # but are kept to reflect the original code's intent if the above block were ever removed or changed.
            # Ensure the sale is not expired
            if sale_def.get("UTCEndDate", 0) > 0:
                sale_def["UTCEndDate"] = now
            
            # Start date in the past
            if "UTCStartDate" in sale_def:
                sale_def["UTCStartDate"] = 1
                
            # Remove blockers and force unlock
            sale_def["storeUnlockFTUELevel"] = 0
            sale_def["unlockQuestId"] = 0
            sale_def["UnlockByTrigger"] = False
            sale_def["Disabled"] = False
            sale_def["Impressions"] = 999
            sale_def["canBuyThisManyTimes"] = 999 # This will be overridden by -1 if the above block ran
            sale_def["type"] = "Upsell"
            
            # Additional unlock logic as per instruction
            sale_def["UnlockLevel"] = 0
            sale_def["unlockLevel"] = 0
            sale_def["disabled"] = False
            
            user_sales.append({
                "saleId": sale_def.get("id", i),
                "saleDefinition": json.dumps(sale_def)
            })
            
        return jsonify(user_sales)
        
    except Exception as e:
        print(f"[SALES] Error: {e}")
        return jsonify([])
