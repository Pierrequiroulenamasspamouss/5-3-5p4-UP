import sys
import os
import json
import sqlite3

# Add parent dir to path to import utils
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))

from utils.db import get_db_connection, link_discord_to_player, resolve_master_uid

def cleanup_orphans():
    print("--- Cleaning up existing orphaned entries ---")
    conn = get_db_connection()
    
    # Find all rows that have a Discord ID
    rows = conn.execute("SELECT uid, DISCORD FROM players WHERE DISCORD IS NOT NULL AND DISCORD != ''").fetchall()
    
    discord_to_uids = {}
    for row in rows:
        try:
            d_data = json.loads(row['DISCORD'])
            d_id = d_data.get('id')
            if d_id:
                if d_id not in discord_to_uids:
                    discord_to_uids[d_id] = {"profile": d_data, "uids": set()}
                discord_to_uids[d_id]["uids"].add(row['uid'])
                discord_to_uids[d_id]["uids"].update(d_data.get('uids', []))
        except:
            continue
            
    conn.close()
    
    for d_id, data in discord_to_uids.items():
        uids = list(data["uids"])
        if len(uids) > 0:
            print(f"Consolidating Discord {d_id} with UIDs: {uids}")
            # link_discord_to_player now handles deletion of slaves
            link_discord_to_player(uids[0], data["profile"])
            
    print("Cleanup complete. Check your database browser now.")

if __name__ == "__main__":
    cleanup_orphans()
