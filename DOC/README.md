# Kampai Game Server Documentation

Welcome to the official documentation for the **Kampai Game Server**. This server is a Python-based backend designed to support the *Minions Paradise* (Kampai) game client. It provides essential services such as user authentication, profile persistence, game metadata (definitions), and shop management.

## Tech Stack

- **Language**: Python 3
- **Web Framework**: Flask
- **Concurrency**: Threaded, with a dual-port listener setup.
- **Persistence**: 
  - **Primary**: SQLite database (managed via `utils/db.py`).
  - **Legacy**: Older deployments used flat JSON files in `player_data/`. The server automatically migrates these to the DB on startup.
- **JSON Handling**: Configured to preserve key order (`JSON_SORT_KEYS = False`) to maintain compatibility with legacy client parsers.

## Server Architecture

The server runs on two ports simultaneously:
1. **Main Port (44733)**: Handles the majority of game traffic and supports auto-reload (debug mode).
2. **Secondary Port (44732)**: Handles secondary requests to prevent blocking the main thread.

### Blueprint Structure
The API is divided into several logical modules (Blueprints):

- **User Blueprint (`routes/user.py`)**: Authentication, registration, Discord identity linking, and social events (TSE).
- **Game Blueprint (`routes/game.py`)**: Core game state management, serving static definitions, config files, and leaderboards.
- **Sales Blueprint (`routes/sales.py`)**: In-game shop logic, sale pack filtering, and price management.
- **Metrics Blueprint (`routes/metrics.py`)**: Telemetry and analytics collection.

## Navigation

- [API Reference](API_Reference.md): Detailed documentation of all REST endpoints.
- [Data Formats](Data_Formats.md): Information on `definitions.json`, `config.json`, and player profiles.
- [Admin Guide](Admin_Guide.md): Instructions for using the server's CLI and GUI management tools.
