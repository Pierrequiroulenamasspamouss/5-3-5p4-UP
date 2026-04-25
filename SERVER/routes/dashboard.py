from flask import Blueprint, jsonify, request, render_template, redirect
import json
import os
import sqlite3
from utils.db import get_db_connection, DB_PATH, PLAYER_DATA_DIR, DEFINITIONS_PATH

dashboard_bp = Blueprint('dashboard', __name__)

EMPTY_PLAYER_JSON = os.path.join(os.path.dirname(__file__), '..', 'empty_player.json')



# Very simple unauthenticated token implementation for the session (real apps should use JWT or proper sessions)
# Since UID + password check is lightweight, we use a basic static "token" generation.
def gen_token(uid):
    return f"tok_{uid}"

def verify_session(data):
    uid = data.get('uid')
    token = data.get('token')
    if token != gen_token(uid):
        return False
    return True

@dashboard_bp.route('/dashboard', methods=['GET'])
def render_dashboard():
    return render_template('dashboard.html')

@dashboard_bp.route('/api/dashboard/login', methods=['POST'])
def dashboard_login():
    data = request.json
    uid = data.get('uid')
    pwd = data.get('password', '')
    
    conn = get_db_connection()
    row = conn.execute("SELECT password, custom_name, discord_username FROM players WHERE uid = ?", (uid,)).fetchone()
    
    if not row:
        conn.close()
        return jsonify({"error": "Player not found. Login to the game first to create the account."})
    
    db_pwd = row['password']
    
    if db_pwd and db_pwd != pwd:
        conn.close()
        return jsonify({"error": "Incorrect password."})
        
    conn.close()
    
    name = row['custom_name'] or row['discord_username'] or "Player"
    secured = bool(db_pwd)
    
    return jsonify({
        "msg": "Login successful",
        "token": gen_token(uid),
        "name": name,
        "secured": secured
    })

@dashboard_bp.route('/api/dashboard/set_password', methods=['POST'])
def set_password():
    data = request.json
    if not verify_session(data): return jsonify({"error": "Unauthorized"})
    
    uid = data.get('uid')
    pwd = data.get('password')
    
    conn = get_db_connection()
    # Check if a password already exists
    row = conn.execute("SELECT password FROM players WHERE uid = ?", (uid,)).fetchone()
    if row and row['password']:
        conn.close()
        return jsonify({"error": "Profile is already secured."})
        
    conn.execute("UPDATE players SET password = ? WHERE uid = ?", (pwd, uid))
    conn.commit()
    conn.close()
    return jsonify({"msg": "Password updated successfully!"})

@dashboard_bp.route('/api/dashboard/reset_save', methods=['POST'])
def reset_save():
    data = request.json
    if not verify_session(data): return jsonify({"error": "Unauthorized"})
    uid = data.get('uid')
    
    try:
        with open(EMPTY_PLAYER_JSON, 'r') as f:
            empty_data = json.load(f)
            inventory_str = json.dumps(empty_data.get('inventory', {}))
    except Exception as e:
        return jsonify({"error": f"Failed to read empty_player.json: {e}"})
        
    conn = get_db_connection()
    conn.execute('''
        UPDATE players 
        SET inventory = ?, DISCORD = '', discord_username = '', discord_avatar = '',
            FACEBOOK = '', GOOGLE_PLAY = '', custom_name = '', custom_avatar = ''
        WHERE uid = ?
    ''', (inventory_str, uid))
    conn.commit()
    conn.close()
    
    return jsonify({"msg": "Save data has been successfully reset."})

@dashboard_bp.route('/api/dashboard/unlink_socials', methods=['POST'])
def unlink_socials():
    data = request.json
    if not verify_session(data): return jsonify({"error": "Unauthorized"})
    uid = data.get('uid')
    
    conn = get_db_connection()
    conn.execute("UPDATE players SET DISCORD = '', discord_username = '', discord_avatar = '', FACEBOOK = '', GOOGLE_PLAY = '' WHERE uid = ?", (uid,))
    conn.commit()
    conn.close()
    
    return jsonify({"msg": "Socials unlinked successfully."})

@dashboard_bp.route('/api/dashboard/update_profile', methods=['POST'])
def update_profile():
    data = request.json
    if not verify_session(data): return jsonify({"error": "Unauthorized"})
    uid = data.get('uid')
    
    conn = get_db_connection()
    conn.execute("UPDATE players SET custom_name = ?, custom_avatar = ? WHERE uid = ?", 
                 (data.get('custom_name'), data.get('custom_avatar'), uid))
    conn.commit()
    conn.close()
    return jsonify({"msg": "Profile updated successfully."})

@dashboard_bp.route('/api/dashboard/backup_save', methods=['GET'])
def backup_save():
    uid = request.args.get('uid')
    token = request.args.get('token')
    if token != gen_token(uid): return "Unauthorized", 401
    
    conn = get_db_connection()
    row = conn.execute("SELECT * FROM players WHERE uid = ?", (uid,)).fetchone()
    conn.close()
    
    if not row: return "Not Found", 404
    
    # Dump entire row as JSON
    d = dict(row)
    # Parse internally nested JSON for a cleaner export
    for k in ['inventory', 'purchasedSales', 'unlocks', 'DISCORD']:
        if d.get(k):
            try: d[k] = json.loads(d[k])
            except: pass
            
    from flask import Response
    return Response(json.dumps(d, indent=2), mimetype='application/json', headers={'Content-Disposition': f'attachment;filename=player_{uid}.json'})

@dashboard_bp.route('/api/dashboard/upload_save', methods=['POST'])
def upload_save():
    data = request.json
    if not verify_session(data): return jsonify({"error": "Unauthorized"})
    uid = data.get('uid')
    payload = data.get('payload', {})
    
    if not isinstance(payload, dict): return jsonify({"error": "Invalid payload"})
    
    fields_to_update = {}
    if 'inventory' in payload: fields_to_update['inventory'] = json.dumps(payload['inventory'])
    if 'PurchasedSales' in payload: fields_to_update['purchasedSales'] = json.dumps(payload['PurchasedSales'])
    
    if not fields_to_update:
        # Check lowercase keys just in case
        if 'purchasedsales' in payload: fields_to_update['purchasedSales'] = json.dumps(payload['purchasedsales'])
        
    if not fields_to_update:
        return jsonify({"error": "No meaningful inventory fields found to update."})
        
    conn = get_db_connection()
    for k, v in fields_to_update.items():
        conn.execute(f"UPDATE players SET {k} = ? WHERE uid = ?", (v, uid))
    conn.commit()
    conn.close()
    
    return jsonify({"msg": "Save uploaded and applied!"})

@dashboard_bp.route('/api/dashboard/migrate_save', methods=['POST'])
def migrate_save():
    data = request.json
    if not verify_session(data): return jsonify({"error": "Unauthorized"})
    
    uid = data.get('uid')
    target = data.get('target_uid')
    direction = data.get('direction')
    target_pwd = data.get('target_password', '')
    
    conn = get_db_connection()
    # Check target
    target_row = conn.execute("SELECT password, inventory FROM players WHERE uid = ?", (target,)).fetchone()
    if not target_row:
        conn.close()
        return jsonify({"error": "Target UID not found."})
        
    if target_row['password'] and target_row['password'] != target_pwd:
        conn.close()
        return jsonify({"error": "Incorrect password for target UID."})
        
    current_row = conn.execute("SELECT inventory FROM players WHERE uid = ?", (uid,)).fetchone()
    
    if direction == 'to':
        # current -> target
        conn.execute("UPDATE players SET inventory = ? WHERE uid = ?", (current_row['inventory'], target))
    elif direction == 'from':
        # target -> current
        conn.execute("UPDATE players SET inventory = ? WHERE uid = ?", (target_row['inventory'], uid))
    else:
        conn.close()
        return jsonify({"error": "Invalid direction."})
        
    conn.commit()
    conn.close()
    return jsonify({"msg": f"Save migrated successfully {direction} {target}."})

def get_loc_text(key):
    # Quick simple search through EN.json to map LocKeys
    loc_path = os.path.join(os.path.dirname(__file__), '..', 'loc_text', 'EN.json')
    try:
        with open(loc_path, 'r', encoding='utf-8') as f:
            translations = json.load(f)
            return translations.get(key, key)
    except:
        return key

@dashboard_bp.route('/api/dashboard/inventory', methods=['GET'])
def inventory():
    uid = request.args.get('uid')
    token = request.args.get('token')
    if token != gen_token(uid): return jsonify({"error": "Unauthorized"})
    
    conn = get_db_connection()
    row = conn.execute("SELECT inventory FROM players WHERE uid = ?", (uid,)).fetchone()
    conn.close()
    
    if not row or not row['inventory']: return jsonify({"inventory": []})
    
    # Load definitions for nice names
    defs = {}
    try:
        with open(DEFINITIONS_PATH, 'r', encoding='utf-8') as f:
            d = json.load(f)
            for item in d.get('itemDefinitions', []):
                defs[item.get('id')] = item.get('localizedKey', f"Item {item.get('id')}")
            for item in d.get('currencyItemDefinitions', []):
                defs[item.get('id')] = item.get('localizedKey', f"Currency {item.get('id')}")
    except:
        pass

    inv_json = json.loads(row['inventory'])
    res = []
    
    if isinstance(inv_json, list):
        for item in inv_json:
            item_def = item.get('Definition')
            amount = item.get('Quantity')
            if item_def is not None and amount is not None:
                loc_key = defs.get(item_def, f"Item {item_def}")
                name = get_loc_text(loc_key)
                res.append({"id": item_def, "name": name, "amount": amount})
    elif isinstance(inv_json, dict):
        for item_id, amount in inv_json.items():
            if not str(item_id).isdigit():
                continue
            item_id = int(item_id)
            loc_key = defs.get(item_id, f"Item {item_id}")
            name = get_loc_text(loc_key)
            res.append({"id": item_id, "name": name, "amount": amount})
        
    return jsonify({"inventory": res})

@dashboard_bp.route('/api/dashboard/update_inventory_item', methods=['POST'])
def update_inventory_item():
    data = request.json
    if not verify_session(data): return jsonify({"error": "Unauthorized"})
    
    uid = data.get('uid')
    item_id = int(data.get('item_id'))
    amount = int(data.get('amount'))
    action = data.get('action') # 'update' or 'delete'
    
    conn = get_db_connection()
    row = conn.execute("SELECT inventory FROM players WHERE uid = ?", (uid,)).fetchone()
    
    if not row:
        conn.close()
        return jsonify({"error": "Player not found."})
        
    inv_json = json.loads(row['inventory'])
    
    if isinstance(inv_json, list):
        found = False
        for i in range(len(inv_json)-1, -1, -1):
            if inv_json[i].get('Definition') == item_id:
                if action == 'delete':
                    del inv_json[i]
                else:
                    inv_json[i]['Quantity'] = amount
                found = True
        
        if not found and action != 'delete':
            # Need a fake ID, maybe max + 1
            max_id = max([x.get('ID', 0) for x in inv_json]) if len(inv_json) > 0 else 0
            inv_json.append({"ID": max_id + 1, "Definition": item_id, "Quantity": amount})
    elif isinstance(inv_json, dict):
        if action == 'delete':
            inv_json.pop(str(item_id), None)
        else:
            inv_json[str(item_id)] = amount
            
    conn.execute("UPDATE players SET inventory = ? WHERE uid = ?", (json.dumps(inv_json), uid))
    conn.commit()
    conn.close()
    
    return jsonify({"msg": "Inventory updated display refreshed!"})
