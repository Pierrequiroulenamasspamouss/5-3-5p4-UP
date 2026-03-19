import sqlite3
import os
import sys

# Define database path relative to this script
DB_PATH = os.path.join(os.path.dirname(__file__), '..', 'player_data', 'players.db')

def cleanup_players(min_seconds):
    """
    Removes players from the database who have played for less than min_seconds.
    """
    if not os.path.exists(DB_PATH):
        print(f"Error: Database not found at {DB_PATH}")
        return

    conn = sqlite3.connect(DB_PATH)
    conn.row_factory = sqlite3.Row
    
    try:
        # 1. Count how many will be deleted
        count_query = "SELECT COUNT(*) as cnt FROM players WHERE totalAccumulatedGameplayDuration < ?"
        count = conn.execute(count_query, (min_seconds,)).fetchone()['cnt']
        
        if count == 0:
            print(f"No players found with less than {min_seconds} seconds of playtime.")
        else:
            print(f"Found {count} players with less than {min_seconds} seconds of playtime.")
            
            # 2. Perform deletion
            delete_query = "DELETE FROM players WHERE totalAccumulatedGameplayDuration < ?"
            conn.execute(delete_query, (min_seconds,))
            conn.commit()
            print(f"Successfully deleted {count} players.")
            
    except Exception as e:
        print(f"An error occurred: {e}")
    finally:
        conn.close()

if __name__ == "__main__":
    # 3 minutes = 180 seconds
    limit_seconds = 180
    print(f"Running cleanup for players with less than {limit_seconds} seconds (3 minutes) of playtime...")
    cleanup_players(limit_seconds)
