from flask import Blueprint, request, jsonify, send_file, current_app, redirect

import os
import json
from utils.profile import generate_new_player_profile
from utils.db import update_player_in_db, get_db_connection, init_db

game_bp = Blueprint('game', __name__)

BASE_HOST = "http://localhost"
SERVER_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
VIDEO_PATH = os.path.join(SERVER_DIR, "assets", "video.mp4")
INTRO_VIDEO_PATH = os.path.join(SERVER_DIR, "assets", "intro.mp4")
DEFINITIONS_PATH = os.path.join(SERVER_DIR, "definitions.json")
CONFIG_PATH = os.path.join(SERVER_DIR, "config.json")
MANIFEST_PATH = os.path.join(SERVER_DIR, "DLC_Manifest.json")
MARKETPLACE_PATH = os.path.join(SERVER_DIR, "marketplace", "marketplace.json")
DLC_DIR = os.path.join(SERVER_DIR, "DLC")

@game_bp.route('/DLC/<path:filename>')
def serve_dlc(filename):
    file_path = os.path.join(DLC_DIR, filename)
    if os.path.exists(file_path):
        return send_file(file_path)
    return "", 404

@game_bp.route('/video.mp4')
def serve_video():
    if os.path.exists(VIDEO_PATH): 
        return send_file(VIDEO_PATH, mimetype='video/mp4')
    return "", 404

@game_bp.route('/videos/<path:filename>')
def serve_intro_video(filename):
    file_path = os.path.join(SERVER_DIR, "assets", filename)
    if os.path.exists(file_path):
        return send_file(file_path)
    
    if filename.startswith('intro'):
        if os.path.exists(INTRO_VIDEO_PATH):
            return send_file(INTRO_VIDEO_PATH, mimetype='video/mp4')
    return "", 404

@game_bp.route('/configs/<path:path>', methods=['GET'])
@game_bp.route('/rest/config/<path:path>', methods=['GET'])
def get_config(path):
    if os.path.exists(CONFIG_PATH):
        return send_file(CONFIG_PATH, mimetype='application/json')
    return jsonify({})

@game_bp.route('/marketplace/marketplace.json', methods=['GET'])
def get_marketplace():
    if os.path.exists(MARKETPLACE_PATH):
        return send_file(MARKETPLACE_PATH, mimetype='application/json')
    return jsonify({})

@game_bp.route('/rest/dlc/manifests/<path:filename>', methods=['GET'])
def get_manifest(filename):
    print(f"[GAME] REQUESTING MANIFEST: {filename}", flush=True)
    if os.path.exists(MANIFEST_PATH):
        print(f"[GAME] SERVING REAL MANIFEST from {MANIFEST_PATH}", flush=True)
        return send_file(MANIFEST_PATH, mimetype='application/json')
    print(f"[GAME] WARNING: MANIFEST NOT FOUND at {MANIFEST_PATH}, serving dummy", flush=True)
    return jsonify({ "id": filename.replace(".json", ""), "baseURL": f"{BASE_HOST}:44733/assets/", "assets": {}, "bundles": [], "bundledAssets": [] })

@game_bp.route('/rest/definitions/<path:filename>', methods=['GET'])
def get_definitions(filename):
    print(f"[GAME] REQUESTING DEFINITIONS: {filename}", flush=True)
    if os.path.exists(DEFINITIONS_PATH): 
        size = os.path.getsize(DEFINITIONS_PATH)
        print(f"[GAME] SERVING DEFINITIONS ({size} bytes) from {DEFINITIONS_PATH}", flush=True)
        # Verify content briefly in logs
        with open(DEFINITIONS_PATH, 'r') as f:
            data = json.load(f)
            cats = [c.get('id') for c in data.get('currencyStoreDefinition', {}).get('categoryDefinitions', [])]
            print(f"[GAME] CATEGORIES IN FILE: {cats}", flush=True)
        
        response = send_file(DEFINITIONS_PATH, mimetype='application/json')
        response.headers['Cache-Control'] = 'no-cache, no-store, must-revalidate'
        return response
    print(f"[GAME] WARNING: DEFINITIONS NOT FOUND at {DEFINITIONS_PATH}", flush=True)
    return jsonify({})

@game_bp.route('/rest/gamestate/<user_id>', methods=['GET'])
def get_gamestate(user_id):
    from utils.db import get_player_data, get_uid_by_discord_id
    
    print(f"[GAME] LOADING PROFILE for user {user_id}")
    
    # 1. Try direct UID lookup
    profile = get_player_data(user_id)
    
    # 2. If not found, try Discord ID resolution
    if not profile:
        resolved_uid = get_uid_by_discord_id(user_id)
        if resolved_uid:
            print(f"[GAME] RESOLVED Discord ID {user_id} to UID {resolved_uid}")
            profile = get_player_data(resolved_uid)
            
    # 3. If still not found, generate and save new profile
    if not profile:
        print(f"[GAME] GENERATING NEW PROFILE for user {user_id} using empty_player.json template")
        from utils.profile import generate_new_player_profile
        from utils.db import update_player_in_db
        new_profile = generate_new_player_profile(user_id)
        update_player_in_db(user_id, new_profile)
        profile = get_player_data(user_id)
    
    if profile:
        json_str = json.dumps(profile, ensure_ascii=False)
        return current_app.response_class(
            response=json_str,
            status=200,
            mimetype='application/json'
        )
    else:
        print(f"[GAME] PROFILE NOT FOUND for user {user_id}, returning 503")
        return jsonify({"error": "Save data not found"}), 503

@game_bp.route('/rest/gamestate/<user_id>', methods=['POST'])
def save_gamestate(user_id):
    try:
        player_data = request.get_json(force=True, silent=True)
        if player_data is None:
            raise ValueError("No JSON data received or invalid JSON")
            
        print(f"[GAME] SAVING PROFILE for user {user_id} to database")
        # Update leaderboard and full data in database
        try:
            update_player_in_db(user_id, player_data)
        except Exception as e:
            print(f"[GAME] ERROR UPDATING DATABASE: {e}")
            raise
        
        return jsonify({"success": True})
    except Exception as e:
        print(f"[GAME] ERROR SAVING PROFILE: {e}")
        return jsonify({"success": False, "error": str(e)}), 500

@game_bp.route('/ping', methods=['GET'])
def ping():
    return jsonify({"status": "ok", "message": "Game blueprint is active"})

@game_bp.route('/api/leaderboard', methods=['GET'])
@game_bp.route('/api/leaderboard/', methods=['GET'])
def get_leaderboard():
    """
    Returns the top 10 players ranked by level then XP.
    Response format: { UID: { Level: X, TimePlayed: Y, Name: Z }, ... }
    Caches the result in leaderboard.json
    """
    from utils.db import LEADERBOARD_JSON_PATH, init_db, get_db_connection

    try:
        init_db() # Ensure DB exists
        conn = get_db_connection()
        players = conn.execute('''
            SELECT uid, PlayerLevel as level, xp, Time_played as playtime, name 
            FROM players 
            ORDER BY PlayerLevel DESC, xp DESC 
            LIMIT 10
        ''').fetchall()
        conn.close()
        
        leaderboard = {}
        for p in players:
            leaderboard[p['uid']] = {
                "Level": p['level'],
                "time played": p['playtime'],
                "Name": p['name']
            }
        
        # Save to leaderboard.json
        try:
            with open(LEADERBOARD_JSON_PATH, 'w') as f:
                json.dump(leaderboard, f, indent=2)
        except Exception as e:
            print(f"[GAME] ERROR SAVING leaderboard.json: {e}")

        return jsonify(leaderboard)
    except Exception as e:
        print(f"[GAME] ERROR FETCHING LEADERBOARD: {e}")
        return jsonify({"error": str(e)}), 500

@game_bp.route('/api/<uid>/icon.png', methods=['GET'])
def get_player_icon(uid):
    """
    Redirects to the player's Discord avatar if available,
    otherwise redirects to a placeholder avatar.
    """
    from utils.db import get_player_data, resolve_master_uid
    
    # Resolve internal UID if necessary
    master_uid = resolve_master_uid(uid) or uid
    profile = get_player_data(master_uid)
    
    if profile and profile.get('DISCORD'):
        try:
            # DISCORD field is stored as a JSON string
            discord_data = profile['DISCORD']
            if isinstance(discord_data, str):
                discord_data = json.loads(discord_data)
            
            discord_id = discord_data.get('id')
            avatar_hash = discord_data.get('avatar')
            
            if discord_id and avatar_hash:
                return redirect(f"https://cdn.discordapp.com/avatars/{discord_id}/{avatar_hash}.png")
        except Exception as e:
            print(f"[GAME] Error parsing Discord data for {uid}: {e}")
            
    # Fallback: UI Avatars with the player's name
    name = profile.get('name', 'Player') if profile else 'Player'
    return redirect(f"https://ui-avatars.com/api/?name={name}&background=random")

@game_bp.route('/rest/gamestate/<user_id>/reset', methods=['POST'])

def reset_gamestate(user_id):
    player_file = os.path.join(SERVER_DIR, 'player_data', f'{user_id}.json')
    if os.path.exists(player_file):
        os.remove(player_file)
        print(f"[GAME] RESET PROFILE for user {user_id}")
    return jsonify({"success": True})
@game_bp.route('/contents/featured', methods=['GET'])
def get_featured_contents():
    print("[GAME] REQUESTING FEATURED CONTENTS", flush=True)
    return jsonify({
        "id": 1,
        "title": "Featured Content",
        "description": "Featured content for DCN",
        "type": "featured",
        "mime_type": "text/html",
        "created_at": "2026-03-16T17:00:00Z",
        "updated_at": "2026-03-16T17:00:00Z",
        "expires_in": "2026-03-17T17:00:00Z",
        "urls": {
            "html5": "https://www.google.com" # Dummy URL, but must be present and non-empty
        },
        "featured": True
    })

# --- SWRVE MOCKS ---
@game_bp.route('/api/1/user_resources_and_campaigns', methods=['GET'])
def swrve_resources_and_campaigns():
    """
    Returns a dummy Swrve campaign that triggers on orderboard completion.
    """
    return jsonify({
        "version": 1,
        "cdn_root": "http://localhost:44733/assets/",
        "game_data": {
            "1": { "app_store_url": "http://localhost" }
        },
        "rules": {
            "delay_first_message": 0,
            "max_messages_per_session": 99999,
            "min_delay_between_messages": 0
        },
        "campaigns": [
            {
                "id": 12345,
                "name": "Mock Social Quest Message",
                "rules": {
                    "show_at_session_start": False,
                    "triggers": [
                        { "event_name": "gameplay.orderboard.none.completed" },
                        { "event_name": "gameplay.orderboard.completed" }
                    ]
                },
                "messages": [
                    {
                        "id": 1,
                        "name": "SocialQuestMessage",
                        "priority": 1,
                        "formats": [
                            {
                                "name": "landscape",
                                "orientation": "landscape",
                                "size": {"x": 1024, "y": 768},
                                "images": [],
                                "buttons": [
                                    {
                                        "id": 1,
                                        "name": "close",
                                        "type": "dismiss",
                                        "rect": {"x": 0, "y": 0, "w": 50, "h": 50}
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        ]
    })

@game_bp.route('/api/1/user_resources_diff', methods=['GET'])
def swrve_resources_diff():
    return jsonify([])

@game_bp.route('/1/batch', methods=['POST'])
def swrve_batch():
    return "", 200
