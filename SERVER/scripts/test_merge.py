import sys
import os
import json
import sqlite3

# Add parent dir to path to import utils
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))

from utils.db import init_db, get_db_connection, update_player_in_db, link_discord_to_player, get_player_data

def test_playtime_guard():
    print("--- Starting High Playtime Guard Test ---")
    init_db()
    
    # 1. Setup Veteran Player
    uid_vet = "VETERAN_PLAYER"
    print(f"Creating {uid_vet} with 9999 playtime...")
    update_player_in_db(uid_vet, {"ID": uid_vet, "totalAccumulatedGameplayDuration": 9999})
    
    # 2. Setup New Player
    uid_new = "NEW_PLAYER"
    print(f"Creating {uid_new} with 0 playtime...")
    update_player_in_db(uid_new, {"ID": uid_new, "totalAccumulatedGameplayDuration": 0})
    
    # 3. Link them to same Discord
    discord_id = "DISCORD_GUARD_TEST"
    print("Linking both to same Discord account...")
    link_discord_to_player(uid_vet, {"id": discord_id})
    link_discord_to_player(uid_new, {"id": discord_id})
    
    # 4. Simulate the "Stale Update" (New Player client sends its 0-playtime save)
    print(f"Simulating STALE update from {uid_new} (playtime 0)...")
    update_player_in_db(uid_new, {
        "ID": uid_new, 
        "totalAccumulatedGameplayDuration": 0, 
        "lastPlayedTime": 12345678,
        "inventory": [{"Definition": 0, "Quantity": 0}] # Trying to zero out XP
    })
    
    # 5. Verify data is PROTECTED
    data = get_player_data(uid_new)
    print(f"Resulting Playtime for {uid_new}: {data['totalAccumulatedGameplayDuration']}")
    
    if data['totalAccumulatedGameplayDuration'] == 9999:
        print("SUCCESS: High Playtime data was PROTECTED.")
    else:
        print(f"FAILURE: High Playtime data was OVERWRITTEN with {data['totalAccumulatedGameplayDuration']}.")

if __name__ == "__main__":
    test_playtime_guard()
