import sqlite3
import os
import json

DB_PATH = os.path.join(os.path.dirname(__file__), '..', 'player_data', 'players.db')
DEFINITIONS_PATH = os.path.join(os.path.dirname(__file__), '..', 'definitions.json')
PLAYER_DATA_DIR = os.path.join(os.path.dirname(__file__), '..', 'player_data')
LEADERBOARD_JSON_PATH = os.path.join(PLAYER_DATA_DIR, 'leaderboard.json')

MINION_NAMES = ["Kevin", "Stuart", "Bob", "Dave", "Jerry", "Carl", "Mel", "Otto", "Tim", "Mark", "Phil", "Paul", "Donny", "Ken", "Mike"]


def get_db_connection():
    conn = sqlite3.connect(DB_PATH)
    conn.row_factory = sqlite3.Row
    return conn

def init_db():
    conn = get_db_connection()
    conn.execute('''
        CREATE TABLE IF NOT EXISTS players (
            uid TEXT PRIMARY KEY,
            ID TEXT,
            version INTEGER,
            nextId INTEGER,
            villainQueue TEXT,
            inventory TEXT,
            pendingTransactions TEXT,
            unlocks TEXT,
            purchasedSales TEXT,
            triggers TEXT,
            lastLevelUpTime INTEGER,
            lastGameStartTime INTEGER,
            firstGameStartTime INTEGER,
            lastPlayedTime INTEGER,
            totalGameplayDurationSinceLastLevelUp INTEGER,
            totalAccumulatedGameplayDuration INTEGER,
            targetExpansionID INTEGER,
            timezoneOffset INTEGER,
            country TEXT,
            completedOrders INTEGER,
            highestFtueLevel INTEGER,
            socialRewards TEXT,
            mtxPurchaseTracking TEXT,
            completedQuestsTotal INTEGER,
            currentItemCount INTEGER,
            PlatformStoreTransactionIDs TEXT,
            helpTipsTrackingData TEXT,
            
            name TEXT,
            PlayerLevel INTEGER,
            xp INTEGER,
            Time_played INTEGER,
            DISCORD TEXT,
            discord_username TEXT,
            discord_avatar TEXT,
            FACEBOOK TEXT,
            GOOGLE_PLAY TEXT,
            
            last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        )
    ''')
    conn.commit()
    conn.close()

def get_level_from_xp(xp, xp_needed_list):
    level = 0
    for threshold in xp_needed_list:
        if xp >= threshold:
            level += 1
        else:
            break
    return level

def resolve_master_uid(user_id, conn=None):
    """
    Finds the exact 'uid' string (might be a comma-separated list) 
    in the database that contains the given user_id.
    """
    if not user_id:
        return None
        
    local_conn = False
    if conn is None:
        conn = get_db_connection()
        local_conn = True
        
    user_id_str = str(user_id)
    
    # 1. Exact match
    row = conn.execute("SELECT uid FROM players WHERE uid = ?", (user_id_str,)).fetchone()
    if row:
        if local_conn: conn.close()
        return row['uid']
        
    # 2. Match within comma-separated list
    # We search for: 'id, ', ', id,', ', id', or just 'id' if it's the only one.
    query = "SELECT uid FROM players WHERE uid = ? OR uid LIKE ? OR uid LIKE ? OR uid LIKE ?"
    params = (user_id_str, f"{user_id_str}, %", f"%, {user_id_str}, %", f"%, {user_id_str}")
    row = conn.execute(query, params).fetchone()
    
    if row:
        if local_conn: conn.close()
        return row['uid']

    # 3. Check DISCORD uids list as fallback
    search_cursor = conn.execute("SELECT uid FROM players WHERE DISCORD LIKE ?", (f'%"uids":%"{user_id_str}"%',))
    search_row = search_cursor.fetchone()
    
    res = search_row['uid'] if search_row else None
    if local_conn: conn.close()
    return res

def update_player_in_db(user_id, player_data):
    # Find the record that "owns" this user_id
    record_uid = resolve_master_uid(user_id) or str(user_id)
    
    # 1. Re-calculate Level and XP
    inventory = player_data.get('inventory', [])
    xp = 0
    if isinstance(inventory, list):
        for item in inventory:
            if item.get('Definition') == 0:
                xp = item.get('Quantity', 0)
                break
    
    level = 0
    try:
        if os.path.exists(DEFINITIONS_PATH):
            with open(DEFINITIONS_PATH, 'r') as f:
                defs = json.load(f)
                xp_needed_list = defs.get('levelXPTable', {}).get('xpNeededList', [])
                level = get_level_from_xp(xp, xp_needed_list)
    except:
        pass

    conn = get_db_connection()
    row = conn.execute('SELECT * FROM players WHERE uid = ?', (record_uid,)).fetchone()

    # 2. High Playtime Guard: If existing record has more playtime, don't overwrite critical fields
    incoming_playtime = player_data.get('totalAccumulatedGameplayDuration', 0)
    existing_playtime = row['totalAccumulatedGameplayDuration'] if row else 0
    
    # 5s margin for safety (client might have slightly different drift)
    is_stale_update = row and incoming_playtime < (existing_playtime - 5) 
    if is_stale_update:
        print(f"[DB] PROTECTING {record_uid}: Incoming playtime {incoming_playtime} is less than existing {existing_playtime}. Skipping progress update.")
        # We only update last_updated and lastPlayedTime to keep the session alive
        conn.execute("UPDATE players SET last_updated = CURRENT_TIMESTAMP, lastPlayedTime = ? WHERE uid = ?", 
                     (player_data.get('lastPlayedTime', 0), record_uid))
        conn.commit()
        conn.close()
        return

    # 3. Build fields
    fields = {
        'uid': record_uid,
        'ID': record_uid, # Store the list in Both ID and uid as requested
        'version': player_data.get('version', 0),
        'nextId': player_data.get('nextId', 0),
        'villainQueue': json.dumps(player_data.get('villainQueue', [])),
        'inventory': json.dumps(player_data.get('inventory', [])),
        'pendingTransactions': json.dumps(player_data.get('pendingTransactions', [])),
        'unlocks': json.dumps(player_data.get('unlocks', [])),
        'purchasedSales': json.dumps(player_data.get('purchasedSales', [])),
        'triggers': json.dumps(player_data.get('triggers', [])),
        'lastLevelUpTime': player_data.get('lastLevelUpTime', 0),
        'lastGameStartTime': player_data.get('lastGameStartTime', 0),
        'firstGameStartTime': player_data.get('firstGameStartTime', 0),
        'lastPlayedTime': player_data.get('lastPlayedTime', 0),
        'totalGameplayDurationSinceLastLevelUp': player_data.get('totalGameplayDurationSinceLastLevelUp', 0),
        'totalAccumulatedGameplayDuration': player_data.get('totalAccumulatedGameplayDuration', 0),
        'targetExpansionID': player_data.get('targetExpansionID', 0),
        'timezoneOffset': player_data.get('timezoneOffset', 0),
        'country': player_data.get('country', ''),
        'completedOrders': player_data.get('completedOrders', 0),
        'highestFtueLevel': player_data.get('highestFtueLevel', 0),
        'socialRewards': json.dumps(player_data.get('socialRewards', [])),
        'mtxPurchaseTracking': json.dumps(player_data.get('mtxPurchaseTracking', [])),
        'completedQuestsTotal': player_data.get('completedQuestsTotal', 0),
        'currentItemCount': player_data.get('currentItemCount', 0),
        'PlatformStoreTransactionIDs': json.dumps(player_data.get('PlatformStoreTransactionIDs', [])),
        'helpTipsTrackingData': json.dumps(player_data.get('helpTipsTrackingData', [])),
        'PlayerLevel': level,
        'xp': xp,
        'Time_played': player_data.get('totalAccumulatedGameplayDuration', 0),
        'DISCORD': player_data.get('discord_info', row['DISCORD'] if row else ''),
        'FACEBOOK': player_data.get('facebook_id', row['FACEBOOK'] if row else ''),
        'GOOGLE_PLAY': player_data.get('google_id', row['GOOGLE_PLAY'] if row else '')
    }

    if row:
        # Prioritize discord_username if name is currently a default minion name or missing
        discord_name = fields.get('discord_username') or row.get('discord_username')
        current_name = row.get('name', '')
        
        if discord_name and (current_name in MINION_NAMES or not current_name):
            fields['name'] = discord_name
        else:
            fields['name'] = current_name
        
        placeholders = ", ".join([f"{k} = ?" for k in fields.keys()])

        query = f"UPDATE players SET {placeholders}, last_updated = CURRENT_TIMESTAMP WHERE uid = ?"
        params = list(fields.values()) + [record_uid]
        conn.execute(query, params)
    else:
        fields['name'] = random_minion_name()
        cols = ", ".join(fields.keys())
        placeholders = ", ".join(["?"] * len(fields))
        query = f"INSERT INTO players ({cols}) VALUES ({placeholders})"
        conn.execute(query, list(fields.values()))
    
    conn.commit()
    conn.close()

def get_player_data(user_id):
    record_uid = resolve_master_uid(user_id)
    if not record_uid:
        return None
        
    conn = get_db_connection()
    row = conn.execute('SELECT * FROM players WHERE uid = ?', (record_uid,)).fetchone()
    conn.close()
    
    if not row: return None
        
    data = {}
    for key in row.keys():
        val = row[key]
        if key in ['villainQueue', 'inventory', 'pendingTransactions', 'unlocks', 'purchasedSales', 
                  'triggers', 'socialRewards', 'mtxPurchaseTracking', 'PlatformStoreTransactionIDs', 'helpTipsTrackingData']:
            data[key] = json.loads(val or '[]')
        else:
            data[key] = val
            
    # CRITICAL: Return the specific ID the client expects
    data['ID'] = str(user_id)
    return data

def player_exists(user_id):
    return resolve_master_uid(user_id) is not None

def get_uid_by_discord_id(discord_id):
    conn = get_db_connection()
    row = conn.execute("SELECT uid FROM players WHERE DISCORD LIKE ?", (f'%"id": "{discord_id}"%',)).fetchone()
    conn.close()
    return row['uid'] if row else None

def link_discord_to_player(uid, discord_profile):
    """
    Consolidates multiple accounts into ONE row where 'uid' and 'ID' columns 
    contain all linked game IDs as a comma-separated string.
    """
    discord_id = str(discord_profile.get('id'))
    conn = get_db_connection()
    
    # 1. Find all rows linked to this Discord ID
    cursor = conn.execute(
        "SELECT uid, totalAccumulatedGameplayDuration, DISCORD FROM players WHERE uid = ? OR DISCORD LIKE ?", 
        (str(uid), f'%"id": "{discord_id}"%')
    )
    rows = cursor.fetchall()
    
    all_uids = set([str(uid)])
    candidates = []
    
    for row in rows:
        r_uid = str(row['uid'])
        # Split by comma in case it's already a list
        for part in r_uid.split(', '):
            all_uids.add(part.strip())
        try:
            d_data = json.loads(row['DISCORD'] or '{}')
            for part in d_data.get('uids', []):
                all_uids.add(str(part))
        except:
            pass
        candidates.append({
            'uid': r_uid,
            'playtime': row['totalAccumulatedGameplayDuration'] or 0
        })
    
    if not candidates:
        conn.close()
        return

    # 2. Pick the row with longest playtime to survive
    survivor = max(candidates, key=lambda x: x['playtime'])
    survivor_record_uid = survivor['uid']
    
    # 3. Create the consolidated UID list string
    consolidated_uid_str = ", ".join(sorted(list(all_uids)))
    
    # 4. Update the survivor row
    discord_profile['uids'] = list(all_uids)
    
    # Extract useful info for columns
    discord_username = discord_profile.get('username', '')
    discord_avatar = discord_profile.get('avatar', '')
    
    # max() already picked the survivor by playtime, we just update it with the new consolidated UID list.
    conn.execute(
        "UPDATE players SET uid = ?, ID = ?, name = ?, DISCORD = ?, discord_username = ?, discord_avatar = ?, last_updated = CURRENT_TIMESTAMP WHERE uid = ?", 
        (consolidated_uid_str, consolidated_uid_str, discord_username, json.dumps(discord_profile), discord_username, discord_avatar, survivor_record_uid)
    )

    
    # 5. Delete other redundant rows
    for cand in candidates:
        if cand['uid'] != survivor_record_uid:
            print(f"[DB] Consolidating row {cand['uid']} and DELETING it.")
            conn.execute("DELETE FROM players WHERE uid = ?", (cand['uid'],))
            
    conn.commit()
    conn.close()
    print(f"[DB] Linked Discord {discord_id}. Consolidated UIDs: {consolidated_uid_str} | Survivor: {survivor_record_uid}")

def migrate_files_to_db():
    if not os.path.exists(PLAYER_DATA_DIR): return
    for f in os.listdir(PLAYER_DATA_DIR):
        if f.endswith('.json') and f != 'leaderboard.json':
            user_id = f.replace('.json', '')
            try:
                with open(os.path.join(PLAYER_DATA_DIR, f), 'r') as jf:
                    update_player_in_db(user_id, json.load(jf))
                os.remove(os.path.join(PLAYER_DATA_DIR, f))
            except: pass

def random_minion_name():
    import random
    return random.choice(MINION_NAMES)

if __name__ == "__main__":
    init_db()
