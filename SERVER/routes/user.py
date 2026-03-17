from flask import Blueprint, request, jsonify
import uuid
import os
import random
import json

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
    
    # Check if this user already has a save file
    # This determines if they get the intro video or skip straight to the game
    player_file = os.path.join(os.path.dirname(__file__), '..', 'player_data', f'{uid_str}.json')
    is_new = not os.path.exists(player_file)
    
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
    from flask import current_app
    
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
    from flask import current_app
    # Return few random teams if needed
    return current_app.response_class(
        json.dumps({}, separators=(', ', ': ')),
        mimetype='application/json'
    )

@user_bp.route('/rest/tse/event/<int:event_id>/team/<int:team_id>/user/<user_id>/<action>', methods=['GET', 'POST'])
def tse_team_actions(event_id, team_id, user_id, action):
    from flask import current_app
    
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
        "type": data.get('identityType', 'facebook')
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

# --- FACEBOOK SERVER-DRIVEN LOGIN MOCKS ---
fb_tokens = {}

@user_bp.route('/auth/facebook/login', methods=['GET'])
def fb_login():
    uid = request.args.get('uid', '1000000000')
    html = f"""
    <html>
    <body>
        <h2>Mock Facebook Login</h2>
        <p>Logging in for user: {uid}</p>
        <button onclick="login()">Log In to Facebook</button>
        <script>
            function login() {{
                window.location.href = '/auth/facebook/callback#access_token=SERVER_MOCK_TOKEN_FOR_{uid}&expires_in=5184000&state={uid}';
            }}
        </script>
    </body>
    </html>
    """
    return html

@user_bp.route('/auth/facebook/callback', methods=['GET'])
def fb_callback():
    html = """
    <html>
    <body>
        <h2>Processing Login...</h2>
        <script>
            var hash = window.location.hash.substring(1);
            var params = new URLSearchParams(hash.replace(/&/g, '&amp;'));
            // simple parse since URLSearchParams might not like fragment
            var parsed = {};
            hash.split('&').forEach(function(pair) {
                var kv = pair.split('=');
                parsed[kv[0]] = kv[1];
            });
            var token = parsed['access_token'];
            var state = parsed['state'];
            
            fetch('/auth/facebook/save', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ uid: state, token: token })
            }).then(() => {
                document.body.innerHTML = "<h2>Login Complete! You can close this window and return to the game.</h2>";
            });
        </script>
    </body>
    </html>
    """
    return html

@user_bp.route('/auth/facebook/save', methods=['POST'])
def fb_save():
    data = request.get_json(force=True, silent=True) or {}
    uid = data.get('uid')
    token = data.get('token')
    if uid and token:
        fb_tokens[uid] = token
    return jsonify({"success": True})

@user_bp.route('/auth/facebook/status', methods=['GET'])
def fb_status():
    uid = request.args.get('uid', '')
    token = fb_tokens.get(uid)
    if token:
        fb_tokens.pop(uid, None) # one time use
        return jsonify({"status": "success", "token": token, "uid": uid})
    return jsonify({"status": "pending"})
