# API Reference

This document provides a detailed technical reference for the Kampai Game Server API.

## Base URLs
- **Main Server**: `http://localhost:44733`
- **Secondary Server**: `http://localhost:44732`

---

## User & Authentication (`user_bp`)

### Login / Session
- **Route**: `POST /rest/user/login` or `POST /rest/user/session`
- **Request Body**:
  ```json
  {
    "userId": "123456789"
  }
  ```
- **Response**:
  ```json
  {
    "userId": "123456789",
    "sessionId": "uuid-v4-string",
    "synergyId": "syn_123456789",
    "isNewUser": false,
    "isTester": true,
    "country": "US",
    "tosVersion": "1.0",
    "privacyVersion": "1.0"
  }
  ```

### Registration
- **Route**: `POST /rest/user/register`
- **Response**: Generates a new random numeric UserID and returns a session.

### Identity Management (Discord)
- **Link Identity**: `POST /rest/v2/user/<user_id>/identity`
  - Links an external ID (Discord) to the local game account.
- **Unlink Discord**: `POST /rest/v2/user/<user_id>/discord/unlink`
  - Removes Discord association while keeping player progress.
- **Conflict Resolution**:
  - `POST /rest/v2/user/<user_id>/identity/<anon_id>`: Forward re-link (Take over the other account).
  - `POST /rest/v2/user/<user_id>/identity/<anon_id>/reverseLink`: Keep current account and stay unlinked.

### Timed Social Events (TSE)
- **Get/Create Team**: `GET|POST /rest/tse/event/<event_id>/team/user/<user_id>`
- **Team Actions**: `POST /rest/tse/event/<event_id>/team/<team_id>/user/<user_id>/<action>`
  - Actions: `join`, `order` (complete task), `reward` (claim rewards).

---

## Game State & Resources (`game_bp`)

### Profile Persistence
- **Get Game State**: `GET /rest/gamestate/<user_id>`
  - Returns the full player JSON object. If the player is new, initializes it using `empty_player.json`.
- **Save Game State**: `POST /rest/gamestate/<user_id>`
  - Persists the provided JSON object to the database.
- **Reset Game State**: `POST /rest/gamestate/<user_id>/reset`
  - Deletes the player data (Admin/Debug use only).

### Static Data
- **Definitions**: `GET /rest/definitions/<filename>`
  - Serves the `definitions.json` file.
- **Configuration**: `GET /rest/config/<path>`
  - Serves the `config.json` file.
- **DLC Manifest**: `GET /rest/dlc/manifests/<filename>`
  - Serves the `DLC_Manifest.json`.

### Social & Leaderboard
- **Leaderboard**: `GET /api/leaderboard`
  - Returns top 10 players ranked by Level and XP.
- **Player Icon**: `GET /api/<uid>/icon.png`
  - Redirects to the player's Discord avatar or a generated placeholder.

---

## Shop & Sales (`sales_bp`)

### Market Prices
- **Route**: `GET /rest/market_prices`
- **Response**: Returns a mapping of SKU IDs to display prices (e.g., `{"com.ea.gp.minions.coins1": "$0.99"}`).

### Sales (Shop Inventory)
- **Route**: `GET /rest/sales/<user_id>/v2`
- **Logic**: Filters `salePackDefinitions` from `definitions.json` based on the `ShopSchedule.json` and the player's level.
- **Fields Filtered**: `UTCSTARTDATE`, `UTCENDDATE`, `CANBUYTHISMANYTIMES`, `UNLOCKLEVEL`.
