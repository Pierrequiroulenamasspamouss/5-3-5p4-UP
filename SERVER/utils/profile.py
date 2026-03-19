import time
import json
import os

def generate_new_player_profile(user_id_str):
    server_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    empty_player_path = os.path.join(server_dir, "empty_player.json")
    
    if os.path.exists(empty_player_path):
        try:
            with open(empty_player_path, 'r') as f:
                profile = json.load(f)
            
            # Update the ID to the one requested
            profile["ID"] = str(user_id_str)
            print(f"[PROFILE] Generated new profile for {user_id_str} using empty_player.json")
            return profile
        except Exception as e:
            print(f"[PROFILE] Error loading empty_player.json: {e}")

    # Fallback if empty_player.json is missing or corrupt
    numeric_id = 1001
    try:
        numeric_id = int(str(user_id_str))
    except ValueError:
        numeric_id = 1001

    profile = {
        "version": "3",
        "ID": str(numeric_id),
        "nextId": 1000,
        
        "inventory": [],
        
        "villainQueue": [],
        "pendingTransactions": [],
        "unlocks": [],
        "socialRewards": [],
        "PlatformStoreTransactionIDs": [],
        
        "highestFtueLevel": 999,
        "lastLevelUpTime": 0,
        "lastGameStartTime": 0,
        "totalGameplayDurationSinceLastLevelUp": 0,
        "targetExpansionID": 0,
        "freezeTime": 0
    }
    
    return profile
