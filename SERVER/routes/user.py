from flask import Blueprint, request, jsonify, current_app, render_template
import uuid
import os
import random
import json
import requests
from utils.db import player_exists, link_discord_to_player, get_uid_by_discord_id
from urllib.parse import quote

user_bp = Blueprint('user', __name__)

# Cache to store Discord profiles between browser authentication and game-side linking
# Key: Game UID (from state), Value: Discord Profile Dict
pending_discord_logins = {}

@user_bp.route('/rest/user/login', methods=['POST'])
@user_bp.route('/rest/user/session', methods=['POST'])
def login():
    try:
        data = request.get_json(force=True, silent=True) or {}
    except Exception:
        data = {}
    
    print(f"[LOGIN] Raw Request Data: {request.data.decode('utf-8', errors='ignore')}")
    uid_str = data.get('userId', data.get('UserID', '1000000000'))
    
    # Check if this user already has an entry in the database
    is_new = not player_exists(uid_str)
    
    print(f"[LOGIN] UserID={uid_str} | isNewUser={is_new}")
    
    # Fetch existing social identities to prevent NRE in client
    from utils.db import get_db_connection, resolve_master_uid
    conn = get_db_connection()
    master_uid = resolve_master_uid(uid_str, conn)
    social_identities = []
    if master_uid:
        row = conn.execute("SELECT DISCORD, FACEBOOK, GOOGLE_PLAY FROM players WHERE uid = ?", (master_uid,)).fetchone()
        if row:
            if row['DISCORD']:
                try:
                    d = json.loads(row['DISCORD'])
                    social_identities.append({
                        "id": d.get('id'),
                        "externalId": d.get('id'),
                        "userId": uid_str,
                        "type": 4 # facebook (mocked)
                    })
                except: pass
            if row['FACEBOOK']:
                # Facebook is mocked as discord in this server
                try:
                    f = json.loads(row['FACEBOOK'])
                    social_identities.append({
                        "id": f.get('id'),
                        "externalId": f.get('id'),
                        "userId": uid_str,
                        "type": 1 # discord (mocked)
                    })
                except: pass
    conn.close()

    return jsonify({
        "userId": uid_str, 
        "sessionId": str(uuid.uuid4()),
        "synergyId": f"syn_{uid_str}",
        "isNewUser": is_new, 
        "isTester": True, 
        "country": "US",
        "tosVersion": "1.0", 
        "privacyVersion": "1.0",
        "socialIdentities": social_identities
    })

@user_bp.route('/rest/user/register', methods=['POST'])
def register():
    try:
        data = request.get_json(force=True, silent=True) or {}
    except Exception:
        data = {}
        
    # Check if a client-provided UID is present (used for offline-to-online transition)
    requested_uid = data.get('userId')
    if requested_uid:
        new_id_str = requested_uid
        print(f"[REGISTER] Using requested UserID={new_id_str}")
    else:
        new_numeric_id = 1000000000 + int(uuid.uuid4().int % 2000000000)
        new_id_str = str(new_numeric_id)
        print(f"[REGISTER] Generated UserID={new_id_str}", flush=True)
    
    return jsonify({
        "userId": new_id_str,
        "sessionId": str(uuid.uuid4()),
        "id": new_id_str,
        "externalId": new_id_str,
        "synergyId": f"syn_{new_id_str}",
        "secret": "mock",
        "sessionKey": "mock",
        "isNewUser": True,
        "isTester": True,
        "country": "US",
        "type": 0
    })

# TSE routes moved to routes/game.py

@user_bp.route('/rest/v2/user/<user_id>/identity', methods=['POST'])
def link_identity(user_id):
    """
    Account linking with conflict detection.
    Expects AccountLinkRequest: { "credentials": "...", "externalId": "...", "identityType": "..." }
    Returns UserIdentity on success, or 409 with AccountLinkErrorResponse if already linked.
    """
    from utils.db import resolve_master_uid, get_db_connection
    try:
        data = request.get_json(force=True, silent=True) or {}
    except Exception:
        data = {}
        
    external_id = data.get('externalId', '')
    identity_type = data.get('identityType', 'discord')
    
    print(f"[IDENTITY] Linking user {user_id} with {identity_type} ID {external_id}")
    
    # Check if this external ID is already linked to a DIFFERENT player
    master_uid = resolve_master_uid(user_id) or str(user_id)
    
    if identity_type in ['discord', 'facebook'] and external_id:
        existing_uid = get_uid_by_discord_id(external_id)
        if existing_uid and existing_uid != master_uid:
            # This Discord account is already linked to another player
            print(f"[IDENTITY] CONFLICT: Discord {external_id} already linked to {existing_uid}, requesting user is {master_uid}")
            return jsonify({
                "error": {
                    "code": 409,
                    "responseCode": 409,
                    "description": "Social account already linked to another user",
                    "message": "CONFLICT",
                    "details": {
                        "conflictType": identity_type,
                        "conflictUserId": existing_uid,
                        "conflictIdentityId": external_id
                    },
                    "exceptionDetails": ""
                }
            }), 409
    
    # No conflict — link normally
    if identity_type in ['discord', 'facebook'] and external_id:
        # Store the Discord link in the DB
        conn = get_db_connection()
        
        # Try to get extra info from pending cache
        profile = pending_discord_logins.get(user_id, {})
        if not profile and external_id:
            # Fallback: find any pending login for this discord ID
            for p_uid, p_profile in pending_discord_logins.items():
                if str(p_profile.get('id')) == str(external_id):
                    profile = p_profile
                    break
        
        discord_username = profile.get('username', '')
        discord_avatar = profile.get('avatar', '')
        
        discord_json = json.dumps({"id": external_id, "uids": [str(user_id)]})
        
        # Update Discord info and also name/avatar if they are default
        conn.execute(
            "UPDATE players SET DISCORD = ?, discord_username = ?, discord_avatar = ? WHERE uid = ?", 
            (discord_json, discord_username, discord_avatar, master_uid)
        )
        
        # If user has a default name, update it with discord name
        from utils.db import MINION_NAMES
        row = conn.execute("SELECT name FROM players WHERE uid = ?", (master_uid,)).fetchone()
        if row and (not row['name'] or row['name'] in MINION_NAMES) and discord_username:
            conn.execute("UPDATE players SET name = ? WHERE uid = ?", (discord_username, master_uid))
            
        conn.commit()
        conn.close()
        
        # Clean up cache
        if user_id in pending_discord_logins:
            del pending_discord_logins[user_id]
    
    return jsonify({
        "userId": user_id,
        "externalId": data.get('externalId', 'mock_external_id'),
        "type": identity_type,
        "id": data.get('externalId', 'mock_external_id')
    })

@user_bp.route('/rest/v2/user/<user_id>/identity/<anon_id>', methods=['POST'])
def relink_identity_forward(user_id, anon_id):
    """
    Forward re-link: The current player (user_id) takes over the linked account's data.
    The Discord link moves from the conflict user (toUserId) to this player.
    Expects AccountReLinkRequest: { "toUserId": "...", "identityType": "...", "externalId": "...", "credentials": "..." }
    """
    from utils.db import resolve_master_uid, get_db_connection, get_player_data
    try:
        data = request.get_json(force=True, silent=True) or {}
    except Exception:
        data = {}
    
    to_user_id = data.get('toUserId', '')
    external_id = data.get('externalId', '')
    identity_type = data.get('identityType', 'discord')
    
    print(f"[RELINK-FORWARD] User {user_id} wants to take over account from {to_user_id} (Discord: {external_id})")
    
    conn = get_db_connection()
    
    # Resolve both UIDs
    current_master = resolve_master_uid(user_id, conn) or str(user_id)
    conflict_master = resolve_master_uid(to_user_id, conn) or str(to_user_id)
    
    if conflict_master and conflict_master != current_master:
        # Move Discord from conflict to current player
        # Get the conflict player's Discord data
        conflict_row = conn.execute("SELECT DISCORD FROM players WHERE uid = ?", (conflict_master,)).fetchone()
        discord_data = conflict_row['DISCORD'] if conflict_row else None
        
        if discord_data:
            try:
                d = json.loads(discord_data)
                # Add current user's UID to the uids list
                uids = d.get('uids', [])
                if str(user_id) not in uids:
                    uids.append(str(user_id))
                d['uids'] = uids
                discord_data = json.dumps(d)
            except:
                pass
        
        # Consolidate: merge current player into conflict player's row
        # The current player's UID becomes part of the conflict player's UID list
        all_uids = set()
        for uid_str in [current_master, conflict_master]:
            for part in uid_str.split(', '):
                all_uids.add(part.strip())
        consolidated_uid_str = ", ".join(sorted(list(all_uids)))
        
        # Update the conflict player's row to include this UID
        conn.execute(
            "UPDATE players SET uid = ?, ID = ?, DISCORD = ?, last_updated = CURRENT_TIMESTAMP WHERE uid = ?",
            (consolidated_uid_str, consolidated_uid_str, discord_data, conflict_master)
        )
        
        # Delete the current player's separate row if it exists and is different
        if current_master != conflict_master:
            conn.execute("DELETE FROM players WHERE uid = ?", (current_master,))
        
        conn.commit()
        print(f"[RELINK-FORWARD] Merged {current_master} into {conflict_master}. New UID: {consolidated_uid_str}")
    
    conn.close()
    
    # Return UserIdentity so the client can reload with the new user
    return jsonify({
        "userId": to_user_id,
        "externalId": external_id,
        "type": identity_type,
        "id": external_id
    })

@user_bp.route('/rest/v2/user/<user_id>/identity/<anon_id>/reverseLink', methods=['POST'])
def relink_identity_reverse(user_id, anon_id):
    """
    Reverse re-link: The current player (user_id) keeps their own data.
    The Discord link stays on the conflict user; current player remains unlinked.
    Expects AccountReLinkRequest: { "toUserId": "...", "identityType": "...", "externalId": "...", "credentials": "..." }
    """
    try:
        data = request.get_json(force=True, silent=True) or {}
    except Exception:
        data = {}
    
    to_user_id = data.get('toUserId', '')
    external_id = data.get('externalId', '')
    identity_type = data.get('identityType', 'discord')
    
    print(f"[RELINK-REVERSE] User {user_id} keeps their data. Discord moves from {to_user_id} to {user_id}")
    
    if identity_type in ['discord', 'facebook']:
        from utils.db import resolve_master_uid, get_db_connection
    conn = get_db_connection()
    
    # 1. Unlink from conflict user
    conflict_master = resolve_master_uid(to_user_id, conn) or str(to_user_id)
    conn.execute(
        "UPDATE players SET DISCORD = '', discord_username = '', discord_avatar = '', last_updated = CURRENT_TIMESTAMP WHERE uid = ?",
        (conflict_master,)
    )
    
    # 2. Link to current user
    master_uid = resolve_master_uid(user_id, conn) or str(user_id)
    discord_json = json.dumps({"id": external_id, "uids": [str(user_id)]})
    
    # Get extra info if available
    profile = pending_discord_logins.get(user_id, {})
    discord_username = profile.get('username', '')
    discord_avatar = profile.get('avatar', '')
    
    conn.execute(
        "UPDATE players SET DISCORD = ?, discord_username = ?, discord_avatar = ?, last_updated = CURRENT_TIMESTAMP WHERE uid = ?",
        (discord_json, discord_username, discord_avatar, master_uid)
    )
    
    conn.commit()
    conn.close()
    
    # Clean up cache
    if user_id in pending_discord_logins:
        del pending_discord_logins[user_id]
    
    # Just return success so the client knows to keep the current session.
    return jsonify({
        "userId": user_id,
        "externalId": external_id,
        "type": identity_type,
        "id": external_id
    })

@user_bp.route('/rest/v2/user/<user_id>/discord/unlink', methods=['POST'])
def unlink_discord(user_id):
    """
    Unlinks Discord from a player account, keeping the player's progress intact.
    The player row remains with their UID, but DISCORD data is cleared.
    """
    from utils.db import resolve_master_uid, get_db_connection
    
    master_uid = resolve_master_uid(user_id) or str(user_id)
    print(f"[IDENTITY] UNLINKING Discord from user {master_uid}")
    
    conn = get_db_connection()
    
    # Get current Discord data for logging
    row = conn.execute("SELECT DISCORD, discord_username FROM players WHERE uid = ?", (master_uid,)).fetchone()
    if row and row['DISCORD']:
        print(f"[IDENTITY] Removing Discord link: {row['discord_username']} from {master_uid}")
    
    # Clear Discord fields but keep the player row and all progress
    # If UID was consolidated (comma-separated), split it back to just the requesting user's ID
    conn.execute(
        "UPDATE players SET DISCORD = '', discord_username = '', discord_avatar = '', last_updated = CURRENT_TIMESTAMP WHERE uid = ?",
        (master_uid,)
    )
    
    # If the UID was a consolidated comma-separated list, we trim it to just this user
    if ',' in master_uid:
        # Keep only the requesting user's ID
        conn.execute(
            "UPDATE players SET uid = ?, ID = ? WHERE uid = ?",
            (str(user_id), str(user_id), master_uid)
        )
        print(f"[IDENTITY] Trimmed consolidated UID from {master_uid} to {user_id}")
    
    conn.commit()
    conn.close()
    
    return jsonify({"success": True, "userId": str(user_id)})

@user_bp.route('/token', methods=['POST'])
def get_dcn_token():
    """
    Mocks the DCN token endpoint.
    Expects json with app_token.
    Returns Token and Expires_In.
    """
    from datetime import datetime, timedelta
    
    # Mock token valid for 24 hours
    expires_at = datetime.now() + timedelta(hours=24)
    # The client expects ISO format that .NET can parse
    expires_str = expires_at.strftime('%Y-%m-%dT%H:%M:%S')
    
    print(f"[DCN] Token requested. Returning mock token.")
    
    return jsonify({
        "Token": "mock_dcn_token_12345",
        "Expires_In": expires_str
    })

# --- DISCORD LIVE LOGIN ---
def get_discord_config():
    env_path = os.path.join(os.path.dirname(__file__), '.env')
    config = {}
    if os.path.exists(env_path):
        with open(env_path, 'r') as f:
            for line in f:
                if '=' in line:
                    k, v = line.strip().split('=', 1)
                    config[k] = v
    return config

def get_discord_redirect_uri(request):
    config = get_discord_config()
    # If the request comes from bluebridge.homeonthewater.com, use the public redirect URI
    if request.host and 'bluebridge.homeonthewater.com' in request.host:
        return config.get('DISCORD_REDIRECT_URI_public')
    return config.get('DISCORD_REDIRECT_URI')

@user_bp.route('/auth/discord/login', methods=['GET'])
def discord_login():
    config = get_discord_config()
    client_id = config.get('DISCORD_CLIENT_ID')
    redirect_uri = get_discord_redirect_uri(request)
    
    if not redirect_uri or not client_id:
        return "Discord config not set properly", 500
    
    if isinstance(redirect_uri, bytes):
        redirect_uri = redirect_uri.decode('utf-8')
    
    uid = quote(request.args.get('uid', ''))
    
    auth_url = (
        f"https://discord.com/api/oauth2/authorize"
        f"?client_id={client_id}"
        f"&redirect_uri={quote(redirect_uri)}"
        f"&response_type=code"
        f"&scope=identify"
        f"&state={uid}"
    )
    
    from flask import redirect
    return redirect(auth_url)

@user_bp.route('/auth/discord/callback', methods=['GET'])
def discord_callback():
    config = get_discord_config()
    client_id = config.get('DISCORD_CLIENT_ID')
    client_secret = config.get('DISCORD_CLIENT_SECRET')
    redirect_uri = get_discord_redirect_uri(request)
    
    code = request.args.get('code')
    uid = request.args.get('state') # This is the internal game UID
    
    if not code:
        return "Error: No code provided", 400
        
    # 1. Exchange code for token
    token_url = "https://discord.com/api/oauth2/token"
    data = {
        'client_id': client_id,
        'client_secret': client_secret,
        'grant_type': 'authorization_code',
        'code': code,
        'redirect_uri': redirect_uri
    }
    headers = {'Content-Type': 'application/x-www-form-urlencoded'}
    
    token_response = requests.post(token_url, data=data, headers=headers)
    token_data = token_response.json()
    
    if 'access_token' not in token_data:
        return f"Error: Failed to get token - {token_data.get('error_description', token_data.get('error', 'Unknown error'))}", 400
        
    access_token = token_data['access_token']
    
    # 2. Fetch user profile
    user_url = "https://discord.com/api/users/@me"
    user_headers = {'Authorization': f"Bearer {access_token}"}
    user_response = requests.get(user_url, headers=user_headers)
    user_profile = user_response.json()
    
    discord_id = user_profile.get('id')
    if not discord_id:
        return "Error: Failed to fetch Discord profile", 400
        
    # 3. Store in pending cache instead of linking immediately
    # This allows the game to trigger the conflict resolution UI (Save Selection)
    # the target_uid here is the game UID passed via 'state' (which is 'uid' here)
    target_uid = uid if uid else discord_id
    pending_discord_logins[target_uid] = user_profile
    
    print(f"[DISCORD] Auth successful for Discord {user_profile.get('username')} ({discord_id}). Pending link for game UID {target_uid}")
    
    # Manually serve the completion page to avoid TemplateNotFound issues with Blueprints
    try:
        html_path = os.path.abspath(os.path.join(os.path.dirname(__file__), '..', 'html', 'login_complete.html'))
        with open(html_path, 'r', encoding='utf-8') as f:
            return f.read()
    except Exception as e:
        print(f"[ERROR] Failed to read login_complete.html: {e}")
        return "Login Complete! You can close this window and return to the game."

# --- LEGACY FACEBOOK MOCKS (Redirected to Discord) ---
@user_bp.route('/auth/facebook/login', methods=['GET'])
def fb_login():
    return discord_login()

@user_bp.route('/auth/discord/status', methods=['GET'])
@user_bp.route('/auth/facebook/status', methods=['GET'])
def discord_status_check():
    """Checks for Discord linkage status for a given UID."""
    uid = request.args.get('uid', '')
    # Check pending logins first (this is what the game is waiting for)
    if uid in pending_discord_logins:
        profile = pending_discord_logins[uid]
        discord_id = profile.get('id')
        print(f"[AUTH] Status check: UID {uid} has pending Discord link {discord_id}")
        return jsonify({
            "status": "success", 
            "token": "discord_pending", 
            "uid": discord_id  # Return Discord ID so game uses it as social ID
        })

    from utils.db import resolve_master_uid, get_db_connection
    master_uid = resolve_master_uid(uid)
    
    conn = get_db_connection()
    row = conn.execute("SELECT DISCORD FROM players WHERE uid = ?", (str(master_uid),)).fetchone()
    conn.close()
    
    if row and row['DISCORD']:
        try:
            d = json.loads(row['DISCORD'])
            discord_id = d.get('id')
            return jsonify({"status": "success", "token": "discord_linked", "uid": discord_id})
        except:
            pass
            
    return jsonify({"status": "pending"})
