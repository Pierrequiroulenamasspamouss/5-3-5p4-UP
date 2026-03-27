from flask import Blueprint, request, jsonify
from utils.db import add_chat_message, get_chat_messages

chat_bp = Blueprint('chat', __name__)

@chat_bp.route('/api/globalchat', methods=['GET'])
def get_chat():
    """Retrieves the latest global chat messages."""
    limit = request.args.get('limit', default=50, type=int)
    messages = get_chat_messages(limit)
    return jsonify(messages)

@chat_bp.route('/api/globalchat', methods=['POST'])
def post_chat():
    """Submits a new message to the global chat."""
    data = request.get_json(force=True, silent=True) or {}
    user_id = data.get('userId')
    message = data.get('message')
    
    if not user_id or not message:
        return jsonify({"success": False, "error": "Missing userId or message"}), 400
    
    if len(message) > 200:
        return jsonify({"success": False, "error": "Message too long (max 200 chars)"}), 400
        
    try:
        add_chat_message(user_id, message)
        return jsonify({"success": True})
    except Exception as e:
        print(f"[CHAT] Error adding message: {e}")
        return jsonify({"success": False, "error": str(e)}), 500
