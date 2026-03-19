from flask import Blueprint, request, jsonify, current_app
import uuid
import os
import random
import json
import requests
from utils.db import player_exists, link_discord_to_player, get_uid_by_discord_id


user_bp = Blueprint('user', __name__)

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
    
    return jsonify({
        "userId": uid_str, 
        "sessionId": str(uuid.uuid4()),
        "synergyId": f"syn_{uid_str}",
        "isNewUser": is_new, 
        "isTester": True, 
        "country": "US",
        "tosVersion": "1.0", 
        "privacyVersion": "1.0"
    })

@user_bp.route('/rest/user/register', methods=['POST'])
def register():
    new_numeric_id = 1000000000 + int(uuid.uuid4().int % 2000000000)
    new_id_str = str(new_numeric_id)
    
    print(f"[REGISTER] UserID={new_id_str}", flush=True)
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

# --- TSE ENDPOINTS MOCKS ---
tse_teams = {} # event_id -> { team_id: { members: [], progress: [] } }

@user_bp.route('/rest/tse/event/<int:event_id>/team/user/<user_id>', methods=['GET', 'POST'])
def get_tse_event_team(event_id, user_id):
    """
    Mocks the Timed Social Event / Team Request for the user.
    Simulates "fake friends" if a team is created or joined.
    """
    # Simple state persistence
    if event_id not in tse_teams:
        tse_teams[event_id] = {}
        
    # Clean old integer lists from previous runs
    if event_id in tse_teams:
        for tid in list(tse_teams[event_id].keys()):
            if tse_teams[event_id][tid].get('orderProgress') and isinstance(tse_teams[event_id][tid]['orderProgress'][0], int):
                del tse_teams[event_id][tid]

    # Find user's team or create one if POST
    user_team = None
    for tid, team in tse_teams[event_id].items():
        if any(m['userId'] == str(user_id) for m in team['members']):
            user_team = team
            user_team['id'] = tid
            break
            
    if not user_team and request.method == 'POST':
        tid = 1001 + len(tse_teams[event_id])
        user_team = {
            "id": tid,
            "socialEventId": event_id,
            "members": [
                {
                    "id": str(user_id),
                    "externalId": str(user_id),
                    "userId": str(user_id),
                    "type": 0,
                    "secret": "mock",
                    "sessionKey": "mock"
                },
                # FAKE FRIENDS
                {"id": "2001", "externalId": "2001", "userId": "2001", "type": 0, "name": "Dave"},
                {"id": "2002", "externalId": "2002", "userId": "2002", "type": 0, "name": "Phil"},
                {"id": "2003", "externalId": "2003", "userId": "2003", "type": 0, "name": "Stuart"}
            ],
            "orderProgress": [{"orderId": o, "completedByUserId": random.choice(["2001", "2002", "2003"])} for o in random.sample([1, 2, 3, 4, 5, 6, 7, 8, 9], random.randint(1, 4))] # Initial friend progress
        }
        tse_teams[event_id][tid] = user_team
    
    data = {
        "eventId": event_id,
        "team": user_team,
        "userEvent": {
            "rewardClaimed": False,
            "invitations": []
        },
        "error": None
    }
    
    return current_app.response_class(
        json.dumps(data, separators=(', ', ': ')),
        mimetype='application/json'
    )

@user_bp.route('/rest/tse/event/<int:event_id>/teams', methods=['GET', 'POST'])
def get_tse_teams(event_id):
    # Return few random teams if needed
    return current_app.response_class(
        json.dumps({}, separators=(', ', ': ')),
        mimetype='application/json'
    )

@user_bp.route('/rest/tse/event/<int:event_id>/team/<int:team_id>/user/<user_id>/<action>', methods=['GET', 'POST'])
def tse_team_actions(event_id, team_id, user_id, action):
    team = tse_teams.get(event_id, {}).get(team_id)
    if not team and action != "join":
        return jsonify({"error": "Team not found"}), 404
        
    reward_claimed = False
    
    if action == "order":
        try:
            order_data = request.get_json(force=True, silent=True) or {}
            order_id = order_data.get('orderId')
            existing_orders = [o['orderId'] for o in team['orderProgress']]
            
            if order_id not in existing_orders:
                team['orderProgress'].append({"orderId": order_id, "completedByUserId": str(user_id)})
            
            all_possible_orders = [1, 2, 3, 4, 5, 6, 7, 8, 9] # Standard 9 orders
            # Re-eval existing orders after append
            existing_orders = [o['orderId'] for o in team['orderProgress']]
            remaining = [o for o in all_possible_orders if o not in existing_orders]
            if remaining:
                num_to_add = random.randint(1, min(2, len(remaining)))
                friends_orders = random.sample(remaining, num_to_add)
                for fo in friends_orders:
                    team['orderProgress'].append({"orderId": fo, "completedByUserId": random.choice(["2001", "2002", "2003"])})
                print(f"[TSE] Friends completed orders: {friends_orders}")
        except Exception as e:
            print(f"[TSE] Error simulating friend progress: {e}")
    elif action == "reward":
        reward_claimed = True
        
    data = {
        "eventId": event_id,
        "team": team,
        "userEvent": {
            "rewardClaimed": reward_claimed,
            "invitations": []
        },
        "error": None
    }
    return current_app.response_class(
        json.dumps(data, separators=(', ', ': ')),
        mimetype='application/json'
    )

@user_bp.route('/rest/v2/user/<user_id>/identity', methods=['POST'])
def link_identity(user_id):
    """
    Mocks account linking.
    Expects AccountLinkRequest: { "credentials": "...", "externalId": "...", "identityType": "..." }
    Returns UserIdentity object.
    """
    try:
        data = request.get_json(force=True, silent=True) or {}
    except Exception:
        data = {}
        
    print(f"[IDENTITY] Linking user {user_id} with {data.get('identityType')} ID {data.get('externalId')}")
    
    # Return UserIdentity sequence
    return jsonify({
        "userId": user_id,
        "externalId": data.get('externalId', 'mock_external_id'),
        "type": data.get('identityType', 'discord')
    })

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
    
    # uid is passed from the game client to link the account
    uid = request.args.get('uid', '')
    
    auth_url = (
        f"https://discord.com/api/oauth2/authorize"
        f"?client_id={client_id}"
        f"&redirect_uri={requests.utils.quote(redirect_uri)}"
        f"&response_type=code"
        f"&scope=identify"
        f"&state={uid}" # Passing uid in state to retrieve it in callback
    )
    return f"<html><body><script>window.location.href='{auth_url}';</script></body></html>"

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
        
    # 3. Link to player in DB
    # If uid is empty, it means the user logged in directly without a pending game account.
    target_uid = uid if uid else get_uid_by_discord_id(discord_id)
    if not target_uid:
        target_uid = discord_id
        
    link_discord_to_player(target_uid, user_profile)
    
    print(f"[DISCORD] Linked user {target_uid} with Discord {user_profile.get('username')} ({discord_id})")
    
    return """
    <html>
    <body>
        <h2>Login Complete!</h2>
        <p>You can close this window and return to the game.</p>
        <script>setTimeout(() => window.close(), 3000);</script>
    </body>
    </html>
    """

# --- LEGACY FACEBOOK MOCKS (Redirected to Discord) ---
@user_bp.route('/auth/facebook/login', methods=['GET'])
def fb_login():
    return discord_login()

@user_bp.route('/auth/discord/status', methods=['GET'])
@user_bp.route('/auth/facebook/status', methods=['GET'])
def discord_status_check():
    """Checks for Discord linkage status for a given UID."""
    uid = request.args.get('uid', '')
    if not uid:
        return jsonify({"status": "pending"})
        
    from utils.db import resolve_master_uid, get_db_connection
    master_uid = resolve_master_uid(uid)
    
    conn = get_db_connection()
    row = conn.execute("SELECT DISCORD FROM players WHERE uid = ?", (str(master_uid),)).fetchone()
    conn.close()
    
    if row and row['DISCORD']:
        # Return success with the MASTER UID
        return jsonify({"status": "success", "token": "discord_linked", "uid": master_uid})
    return jsonify({"status": "pending"})
