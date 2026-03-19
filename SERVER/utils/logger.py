import os
import time

SERVER_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
ACTIVITY_LOG = os.path.join(SERVER_DIR, "activity.log")

def log_activity(user_id, action):
    """Logs user activity for the GUI to monitor."""
    with open(ACTIVITY_LOG, "a", encoding="utf-8") as f:
        timestamp = time.strftime('%Y-%m-%d %H:%M:%S')
        f.write(f"[{timestamp}] {user_id}: {action}\n")

def get_recent_activity(limit=50):
    """Reads recent activity from the log file."""
    if not os.path.exists(ACTIVITY_LOG):
        return []
    
    with open(ACTIVITY_LOG, "r", encoding="utf-8") as f:
        lines = f.readlines()
        return lines[-limit:]
