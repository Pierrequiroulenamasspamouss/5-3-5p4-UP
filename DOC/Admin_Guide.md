# Admin & Management Guide

This document describes the administrative tools available for managing the Kampai Game Server.

---

## 1. Interactive GUI (`server_gui.py`)

The server includes a graphical interface for real-time monitoring and management. It is built using Python's `tkinter` library.

### Features
- **Server Controls**: Start/Stop the server threads.
- **Port Monitoring**: Visualization of traffic on ports 44732 and 44733.
- **Log Viewer**: Integrated terminal window showing HTTP requests and server-side events.
- **Player Search**: Search and view basic info for connected players.

**To Launch**:
```powershell
python c:\Unity\LATEST\SERVER\server_gui.py
```

---

## 2. Command-Line Administrator (`admin_cli.py`)

A powerful tool for modifying player data and server state directly from the terminal.

### Common Commands
- **Inventory Modification**:
  - `add_item <uid> <def_id> <quantity>`: Grant items or currency to a player.
- **Account Management**:
  - `reset_player <uid>`: Reset a player's progress to the `empty_player.json` state.
  - `link_discord <uid> <discord_id>`: Manually link an account.
- **System**:
  - `stats`: View total player count and basic server telemetry.

**To Use**:
```powershell
python c:\Unity\LATEST\SERVER\admin_cli.py
```

---

## 3. Database Maintenance

The server uses a SQLite database to persist player progress and social events.

- **File Location**: `c:\Unity\LATEST\SERVER\player_data\players.db`.
- **Detailed Schema**: For a full breakdown of tables and columns (including the complex JSON blobs), see the **[Database Schema](Database_Schema.md)**.
- **Migration**: On startup, the server automatically checks the `player_data/` directory for legacy `.json` files and migrates them into the SQLite database. Once migrated, the `.json` files are deleted.
- **Manual Editing**: Any standard SQLite browser (e.g., [DB Browser for SQLite](https://sqlitebrowser.org/)) can be used to view or edit the `players` table manually.
- **Backups**: Always back up the `players.db` file before performing manual edits or major server updates.
