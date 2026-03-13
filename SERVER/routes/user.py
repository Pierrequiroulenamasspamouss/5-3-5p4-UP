from flask import Blueprint, request, jsonify
import uuid
import os

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
        "userId": new_id_str, "synergyId": f"syn_{new_id_str}",
        "secret": "mock", "sessionKey": "mock",
        "isNewUser": True, "isTester": True, "country": "US"
    })

# --- TSE ENDPOINTS MOCKS ---
@user_bp.route('/rest/tse/event/<int:event_id>/team/user/<user_id>', methods=['GET', 'POST'])
def get_tse_event_team(event_id, user_id):
    """
    Mocks the Timed Social Event / Team Request for the user.
    """
    import json
    from flask import current_app
    data = {
        "eventId": event_id,
        "team": {
            "id": 1001,
            "socialEventId": event_id,
            "members": [{
                "id": str(user_id),
                "externalId": str(user_id),
                "userId": str(user_id),
                "type": 0,
                "secret": "mock",
                "sessionKey": "mock"
            }],
            "orderProgress": []
        },
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
    import json
    from flask import current_app
    return current_app.response_class(
        json.dumps({}, separators=(', ', ': ')),
        mimetype='application/json'
    )

@user_bp.route('/rest/tse/event/<int:event_id>/team/<team_id>/user/<user_id>/<action>', methods=['GET', 'POST'])
def tse_team_actions(event_id, team_id, user_id, action):
    """ Mocks join/leave/invite/reject/order/reward etc. """
    import json
    from flask import current_app
    data = {
        "eventId": event_id,
        "team": {
            "id": int(team_id) if str(team_id).isdigit() else 1001,
            "socialEventId": event_id,
            "members": [],
            "orderProgress": []
        },
        "userEvent": {
            "rewardClaimed": True if action == "reward" else False,
            "invitations": []
        },
        "error": None
    }
    return current_app.response_class(
        json.dumps(data, separators=(', ', ': ')),
        mimetype='application/json'
    )
