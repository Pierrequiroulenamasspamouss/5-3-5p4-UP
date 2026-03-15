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
        for i, sale_def in enumerate(sale_pack_definitions[:5]):  # Return first 5 for now
            user_sales.append({
                "saleId": sale_def.get("id", i),
                "saleDefinition": json.dumps(sale_def)
            })
            
        return jsonify(user_sales)
        
    except Exception as e:
        print(f"[SALES] Error: {e}")
        return jsonify([])
