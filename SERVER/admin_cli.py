import sqlite3
import json
import os
import sys

# Connect to the SQLite database
DB_PATH = os.path.join(os.path.dirname(__file__), "server.db")
EMPTY_PLAYER_PATH = os.path.join(os.path.dirname(__file__), "empty_player.json")

def get_connection():
    conn = sqlite3.connect(DB_PATH)
    conn.row_factory = sqlite3.Row
    return conn

def print_menu():
    print("\n" + "="*40)
    print("      MINIONS PARADISE ADMIN CLI")
    print("="*40)
    print("1. Add/Remove Sand Dollars (ID 0)")
    print("2. Add/Remove Doubloons (ID 1)")
    print("3. Reset Player (empty_player.json)")
    print("4. Remove Socials (Discord/Facebook)")
    print("5. Edit Player ID")
    print("0. Exit")
    print("="*40)

def find_player(conn):
    player_id = input("Enter player ID (UID or ID): ").strip()
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM players WHERE uid = ? OR ID = ?", (player_id, player_id))
    row = cursor.fetchone()
    if not row:
        print(f"[-] Player '{player_id}' not found.")
        return None
    print(f"[+] Found player: {row['uid']} (Name: {row['name']})")
    
    # Show current currencies
    state = json.loads(row['state']) if row['state'] else {}
    inventory = state.get('inventory', [])
    sand_dollars = 0
    doubloons = 0
    for item in inventory:
        if item.get('ID') == 0:
            sand_dollars = item.get('Quantity', 0)
        elif item.get('ID') == 1:
            doubloons = item.get('Quantity', 0)
            
    print(f"    Current Sand Dollars: {sand_dollars}")
    print(f"    Current Doubloons:    {doubloons}")
    return dict(row)

def update_currency(conn, player, currency_id, currency_name):
    print(f"\n--- Edit {currency_name} ---")
    state = json.loads(player['state'])
    inventory = state.get('inventory', [])
    
    current_amount = 0
    item_index = -1
    for i, item in enumerate(inventory):
        if item.get('ID') == currency_id:
            current_amount = item.get('Quantity', 0)
            item_index = i
            break
            
    print(f"Current amount: {current_amount}")
    
    try:
        change_str = input(f"Amount to add/remove (e.g. 500 or -200), or enter absolute new amount with '=' (e.g. =1000): ").strip()
        if not change_str:
            return
            
        new_amount = current_amount
        if change_str.startswith('='):
            new_amount = int(change_str[1:])
        else:
            new_amount += int(change_str)
            
        if new_amount < 0:
            new_amount = 0
            
        # Update inventory
        if item_index >= 0:
            inventory[item_index]['Quantity'] = new_amount
        else:
            inventory.append({"ID": currency_id, "Definition": currency_id, "Quantity": new_amount})
            
        state['inventory'] = inventory
        
        cursor = conn.cursor()
        cursor.execute("UPDATE players SET state = ? WHERE uid = ?", (json.dumps(state), player['uid']))
        conn.commit()
        print(f"[+] {currency_name} updated successfully! New amount: {new_amount}")
        
    except ValueError:
        print("[-] Invalid input. Please enter a valid number.")

def reset_player(conn, player):
    print("\n--- Reset Player ---")
    confirm = input(f"Are you sure you want to completely RESET player {player['uid']}? (y/n): ").strip().lower()
    if confirm != 'y':
        print("Aborted.")
        return
        
    try:
        with open(EMPTY_PLAYER_PATH, 'r') as f:
            empty_state = f.read()
            
        cursor = conn.cursor()
        cursor.execute("""
            UPDATE players 
            SET state = ?, 
                FB = '', 
                DISCORD = '', 
                discord_username = '', 
                discord_avatar = '',
                session_key = ''
            WHERE uid = ?
        """, (empty_state, player['uid']))
        conn.commit()
        print(f"[+] Player {player['uid']} has been reset using empty_player.json and socials removed.")
    except Exception as e:
        print(f"[-] Error resetting player: {e}")

def remove_socials(conn, player):
    print("\n--- Remove Socials ---")
    confirm = input(f"Remove Facebook and Discord links from player {player['uid']}? (y/n): ").strip().lower()
    if confirm != 'y':
        print("Aborted.")
        return
        
    cursor = conn.cursor()
    cursor.execute("""
        UPDATE players 
        SET FB = '', 
            DISCORD = '', 
            discord_username = '', 
            discord_avatar = ''
        WHERE uid = ?
    """, (player['uid'],))
    conn.commit()
    print(f"[+] Socials removed for player {player['uid']}.")

def edit_player_id(conn, player):
    print("\n--- Edit Player ID ---")
    print(f"Current UID: {player['uid']}")
    print(f"Current ID:  {player['ID']}")
    
    new_id = input("Enter new ID (leave blank to cancel): ").strip()
    if not new_id:
        print("Aborted.")
        return
        
    # Check if new ID already exists
    cursor = conn.cursor()
    cursor.execute("SELECT uid FROM players WHERE uid = ? OR ID = ?", (new_id, new_id))
    if cursor.fetchone():
        print(f"[-] A player with ID '{new_id}' already exists.")
        return
        
    confirm = input(f"Change ID from '{player['uid']}' to '{new_id}'? (y/n): ").strip().lower()
    if confirm != 'y':
        print("Aborted.")
        return
        
    try:
        cursor.execute("UPDATE players SET uid = ?, ID = ? WHERE uid = ?", (new_id, new_id, player['uid']))
        
        # Also update TSE tables if they exist
        try:
            cursor.execute("UPDATE tse_teams SET user_id = ? WHERE user_id = ?", (new_id, player['uid']))
            cursor.execute("UPDATE tse_members SET user_id = ? WHERE user_id = ?", (new_id, player['uid']))
        except sqlite3.OperationalError:
            pass # Tables might not exist in older DBs
            
        conn.commit()
        print(f"[+] Player ID successfully changed to '{new_id}'.")
    except Exception as e:
        print(f"[-] Error changing player ID: {e}")

def main():
    if not os.path.exists(DB_PATH):
        print(f"[-] Database not found at {DB_PATH}")
        sys.exit(1)
        
    conn = get_connection()
    
    while True:
        print_menu()
        choice = input("Select an option: ").strip()
        
        if choice == '0':
            print("Exiting...")
            break
            
        if choice in ['1', '2', '3', '4', '5']:
            player = find_player(conn)
            if not player:
                continue
                
            if choice == '1':
                update_currency(conn, player, 0, "Sand Dollars")
            elif choice == '2':
                update_currency(conn, player, 1, "Doubloons")
            elif choice == '3':
                reset_player(conn, player)
            elif choice == '4':
                remove_socials(conn, player)
            elif choice == '5':
                edit_player_id(conn, player)
        else:
            print("[-] Invalid option. Please select 0-5.")

if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        print("\nExiting...")
        sys.exit(0)
