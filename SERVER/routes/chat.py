from flask import Blueprint, request, jsonify
from utils.db import add_chat_message, get_chat_messages

chat_bp = Blueprint('chat', __name__)

@chat_bp.route('/chat', methods=['GET', 'POST'])
@chat_bp.route('/api/globalchat', methods=['GET', 'POST'])
def global_chat():
    """
    Unified chat endpoint. Handles GET (polling) and POST (sending).
    Supports both JSON and Form data for Unity compatibility.
    """
    if request.method == 'POST':
        # 1. Try JSON
        data = request.get_json(force=True, silent=True)
        
        # 2. Fallback to Form Data (Unity WWWForm)
        if not data:
            data = request.form
            
        user_id = data.get('userId') or data.get('user')
        message = data.get('message') or data.get('text')
        
        if not user_id or not message:
            return jsonify({"success": False, "error": "Missing user or message"}), 400
        
        if len(message) > 400: # Slightly more generous than 200
            return jsonify({"success": False, "error": "Message too long"}), 400
            
        try:
            add_chat_message(user_id, message)
            return jsonify({"success": True})
        except Exception as e:
            print(f"[CHAT] Post Error: {e}")
            return jsonify({"success": False, "error": str(e)}), 500

    # GET logic (Polling)
    try:
        limit = request.args.get('limit', default=100, type=int)
        raw_messages = get_chat_messages(limit)
        
        # Map DB keys to Unity ChatMessage keys (user, text)
        formatted_messages = []
        for m in raw_messages:
            formatted_messages.append({
                "user": m.get("username", "Unknown"),
                "text": m.get("message", ""),
                "timestamp": m.get("timestamp", "")
            })
            
        # Reverse list so newest is at the bottom for the client
        formatted_messages.reverse()
        
        return jsonify({"messages": formatted_messages})
    except Exception as e:
        print(f"[CHAT] Get Error: {e}")
        return jsonify({"messages": [], "error": str(e)}), 500
