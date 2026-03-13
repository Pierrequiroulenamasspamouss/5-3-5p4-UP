from flask import Blueprint, request, jsonify, send_file, current_app
import os
import json
from utils.profile import generate_new_player_profile

game_bp = Blueprint('game', __name__)

BASE_HOST = "http://localhost"
SERVER_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
VIDEO_PATH = os.path.join(SERVER_DIR, "assets", "video.mp4")
DEFINITIONS_PATH = os.path.join(SERVER_DIR, "definitions.json")
CONFIG_PATH = os.path.join(SERVER_DIR, "config.json")
MANIFEST_PATH = os.path.join(SERVER_DIR, "DLC_Manifest.json")
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

@game_bp.route('/configs/<path:path>', methods=['GET'])
@game_bp.route('/rest/config/<path:path>', methods=['GET'])
def get_config(path):
    if os.path.exists(CONFIG_PATH):
        return send_file(CONFIG_PATH, mimetype='application/json')
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
        print(f"[GAME] SERVING REAL DEFINITIONS from {DEFINITIONS_PATH}", flush=True)
        return send_file(DEFINITIONS_PATH, mimetype='application/json')
    print(f"[GAME] WARNING: DEFINITIONS NOT FOUND at {DEFINITIONS_PATH}", flush=True)
    return jsonify({})

@game_bp.route('/rest/gamestate/<user_id>', methods=['GET'])
def get_gamestate(user_id):
    player_data_dir = os.path.join(SERVER_DIR, 'player_data')
    os.makedirs(player_data_dir, exist_ok=True)
    player_file = os.path.join(player_data_dir, f'{user_id}.json')
    
    if os.path.exists(player_file):
        print(f"[GAME] LOADING EXISTING PROFILE for user {user_id}")
        try:
            with open(player_file, 'r') as f:
                profile = json.load(f)
        except Exception as e:
            print(f"[GAME] ERROR LOADING PROFILE: {e}, generating new one")
            profile = generate_new_player_profile(user_id)
    else:
        print(f"[GAME] GENERATING NEW PROFILE for user {user_id}")
        profile = generate_new_player_profile(user_id)
        # Save immediately so the next load works correctly
        try:
            with open(player_file, 'w') as f:
                json.dump(profile, f, indent=2)
        except Exception as e:
            print(f"[GAME] ERROR SAVING NEW PROFILE: {e}")
    
    json_str = json.dumps(profile, ensure_ascii=False)
    return current_app.response_class(
        response=json_str,
        status=200,
        mimetype='application/json'
    )

@game_bp.route('/rest/gamestate/<user_id>', methods=['POST'])
def save_gamestate(user_id):
    try:
        player_data = request.get_json()
        player_data_dir = os.path.join(SERVER_DIR, 'player_data')
        os.makedirs(player_data_dir, exist_ok=True)
        player_file = os.path.join(player_data_dir, f'{user_id}.json')
        
        with open(player_file, 'w') as f:
            json.dump(player_data, f, indent=2)
        
        return jsonify({"success": True})
    except Exception as e:
        print(f"[GAME] ERROR SAVING PROFILE: {e}")
        return jsonify({"success": False, "error": str(e)}), 500

@game_bp.route('/rest/gamestate/<user_id>/reset', methods=['POST'])
def reset_gamestate(user_id):
    player_file = os.path.join(SERVER_DIR, 'player_data', f'{user_id}.json')
    if os.path.exists(player_file):
        os.remove(player_file)
        print(f"[GAME] RESET PROFILE for user {user_id}")
    return jsonify({"success": True})
