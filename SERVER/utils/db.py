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
    conn.execute('''
        CREATE TABLE IF NOT EXISTS tse_teams (
            team_id INTEGER PRIMARY KEY AUTOINCREMENT,
            event_id INTEGER NOT NULL,
            order_progress TEXT DEFAULT '[]',
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        )
    ''')
    conn.execute('''
        CREATE TABLE IF NOT EXISTS tse_members (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            team_id INTEGER NOT NULL,
            user_id TEXT NOT NULL,
            reward_claimed INTEGER DEFAULT 0,
            FOREIGN KEY (team_id) REFERENCES tse_teams(team_id),
            UNIQUE(team_id, user_id)
        )
    ''')
    conn.execute('''
        CREATE TABLE IF NOT EXISTS tse_invitations (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            event_id INTEGER NOT NULL,
            team_id INTEGER NOT NULL,
            inviter_uid TEXT NOT NULL,
            invitee_uid TEXT NOT NULL,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (team_id) REFERENCES tse_teams(team_id)
        )
    ''')
    conn.commit()
    conn.close()

def get_tse_team_for_user(event_id, user_id):
    """Get a user's team for an event from DB. Returns (team_dict, reward_claimed) or (None, False)."""
    conn = get_db_connection()
    row = conn.execute('''
        SELECT t.team_id, t.event_id, t.order_progress, m.reward_claimed
        FROM tse_teams t
        JOIN tse_members m ON t.team_id = m.team_id
        WHERE t.event_id = ? AND m.user_id = ?
    ''', (event_id, str(user_id))).fetchone()
    
    if not row:
        conn.close()
        return None, False
    
    team_id = row['team_id']
    order_progress = json.loads(row['order_progress'] or '[]')
    reward_claimed = bool(row['reward_claimed'])
    
    # Get all members
    members = conn.execute('SELECT user_id FROM tse_members WHERE team_id = ?', (team_id,)).fetchall()
    conn.close()
    
    member_list = []
    for m in members:
        mid = m['user_id']
        member_list.append({
            "id": mid, "externalId": mid, "userId": mid,
            "type": 1,
            "secret": "mock", "sessionKey": "mock",
            "iconUrl": f"http://localhost:44732/api/{mid}/icon.png"
        })
    
    team = {
        "id": team_id,
        "socialEventId": event_id,
        "members": member_list,
        "orderProgress": order_progress
    }
    return team, reward_claimed

def create_tse_team_for_user(event_id, user_id):
    """Create a new team for a user. Returns (team_dict, False)."""
    conn = get_db_connection()
    cursor = conn.execute(
        'INSERT INTO tse_teams (event_id, order_progress) VALUES (?, ?)',
        (event_id, '[]')
    )
    team_id = cursor.lastrowid
    conn.execute(
        'INSERT INTO tse_members (team_id, user_id) VALUES (?, ?)',
        (team_id, str(user_id))
    )
    conn.commit()
    conn.close()
    
    team = {
        "id": team_id,
        "socialEventId": event_id,
        "members": [
            {"id": str(user_id), "externalId": str(user_id), "userId": str(user_id),
             "type": 1,
             "secret": "mock", "sessionKey": "mock",
             "iconUrl": f"http://localhost:44732/api/{user_id}/icon.png"}
        ],
        "orderProgress": []
    }
    return team, False

def save_tse_order_progress(team_id, order_progress):
    """Save order progress to DB."""
    conn = get_db_connection()
    conn.execute('UPDATE tse_teams SET order_progress = ? WHERE team_id = ?',
                 (json.dumps(order_progress), team_id))
    conn.commit()
    conn.close()

def claim_tse_reward(team_id, user_id):
    """Mark reward as claimed for a user on a team. Returns True if newly claimed, False if already claimed."""
    conn = get_db_connection()
    row = conn.execute('SELECT reward_claimed FROM tse_members WHERE team_id = ? AND user_id = ?',
                       (team_id, str(user_id))).fetchone()
    if not row:
        conn.close()
        return False
    if row['reward_claimed']:
        conn.close()
        return False  # Already claimed
    conn.execute('UPDATE tse_members SET reward_claimed = 1 WHERE team_id = ? AND user_id = ?',
                 (team_id, str(user_id)))
    conn.commit()
    conn.close()
    return True

def get_tse_team_by_id(team_id):
    """Get team info by ID."""
    conn = get_db_connection()
    row = conn.execute('SELECT * FROM tse_teams WHERE team_id = ?', (team_id,)).fetchone()
    if not row:
        conn.close()
        return None
    
    event_id = row['event_id']
    order_progress = json.loads(row['order_progress'] or '[]')
    
    members = conn.execute('SELECT user_id FROM tse_members WHERE team_id = ?', (team_id,)).fetchall()
    conn.close()
    
    member_list = []
    for m in members:
        mid = m['user_id']
        member_list.append({
            "id": mid, "externalId": mid, "userId": mid,
            "type": 1,
            "secret": "mock", "sessionKey": "mock",
            "iconUrl": f"http://localhost:44732/api/{mid}/icon.png"
        })
    
    return {
        "id": team_id,
        "socialEventId": event_id,
        "members": member_list,
        "orderProgress": order_progress
    }

def join_tse_team(team_id, user_id):
    """Add a user to a team."""
    conn = get_db_connection()
    # Check if team is full (max 4 members usually)
    count = conn.execute('SELECT COUNT(*) FROM tse_members WHERE team_id = ?', (team_id,)).fetchone()[0]
    if count >= 4:
        conn.close()
        return False, "TEAM_FULL"
    
    try:
        conn.execute('INSERT INTO tse_members (team_id, user_id) VALUES (?, ?)', (team_id, str(user_id)))
        # Also remove any invitations for this user to this team
        conn.execute('DELETE FROM tse_invitations WHERE team_id = ? AND invitee_uid = ?', (team_id, str(user_id)))
        conn.commit()
    except sqlite3.IntegrityError:
        pass # Already a member
    
    conn.close()
    return True, None

def leave_tse_team(team_id, user_id):
    """Remove a user from a team."""
    conn = get_db_connection()
    conn.execute('DELETE FROM tse_members WHERE team_id = ? AND user_id = ?', (team_id, str(user_id)))
    # If no members left, maybe delete team? (Optional, skipping for now)
    conn.commit()
    conn.close()
    return True

def create_tse_invitation(event_id, team_id, inviter_uid, invitee_uid):
    """Create an invitation."""
    conn = get_db_connection()
    conn.execute(
        'INSERT INTO tse_invitations (event_id, team_id, inviter_uid, invitee_uid) VALUES (?, ?, ?, ?)',
        (event_id, team_id, str(inviter_uid), str(invitee_uid))
    )
    conn.commit()
    conn.close()

def get_tse_invitations(user_id):
    """Get all invitations for a user."""
    conn = get_db_connection()
    rows = conn.execute('''
        SELECT i.event_id, i.team_id, i.inviter_uid
        FROM tse_invitations i
        WHERE i.invitee_uid = ?
    ''', (str(user_id),)).fetchall()
    
    invites = []
    for r in rows:
        team_id = r['team_id']
        # Get team view info
        team_row = conn.execute('SELECT order_progress FROM tse_teams WHERE team_id = ?', (team_id,)).fetchone()
        if not team_row: continue
        
        orders = json.loads(team_row['order_progress'] or '[]')
        member_count = conn.execute('SELECT COUNT(*) FROM tse_members WHERE team_id = ?', (team_id,)).fetchone()[0]
        
        invites.append({
            "eventId": r['event_id'],
            "team": {
                "id": team_id,
                "socialEventId": r['event_id'],
                "membersCount": member_count,
                "completedOrdersCount": len(orders)
            },
            "inviter": {
                "id": r['inviter_uid'], "externalId": r['inviter_uid'], "userId": r['inviter_uid'],
                "type": 0
            }
        })
    conn.close()
    return invites

def reject_tse_invitation(event_id, team_id, user_id):
    """Remove an invitation."""
    conn = get_db_connection()
    conn.execute('DELETE FROM tse_invitations WHERE event_id = ? AND team_id = ? AND invitee_uid = ?',
                 (event_id, team_id, str(user_id)))
    conn.commit()
    conn.close()
    return True

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
            if item.get('Definition') == 2:
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
    row_data = conn.execute('SELECT * FROM players WHERE uid = ?', (record_uid,)).fetchone()
    row = dict(row_data) if row_data else None

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
'DISCORD': player_data.get('discord_info', row.get('DISCORD', '') if row else ''),
        'FACEBOOK': player_data.get('facebook_id', row.get('FACEBOOK', '') if row else ''),
        'GOOGLE_PLAY': player_data.get('google_id', row.get('GOOGLE_PLAY', '') if row else '')
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
    
    for row_obj in rows:
        row = dict(row_obj)
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
