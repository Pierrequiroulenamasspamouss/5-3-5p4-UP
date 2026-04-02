# Database Schema

The Kampai Game Server uses a SQLite database to persist all player progress and social event data. The database file is located at `c:\Unity\LATEST\SERVER\player_data\players.db`.

---

## 1. Table: `players`

This is the primary table containing all persistent player data.

### Identification & Versioning
- **`uid`** (TEXT, PK): The "Master" User ID. When accounts are linked (e.g., via Discord), this field becomes a comma-separated list of all linked game IDs.
- **`ID`** (LONG): The internal numeric ID of the player. Often mirrors the `uid`.
- **`version`** (INTEGER): The data schema version of the player profile.
- **`nextId`** (INTEGER): Used by the game's instance-based inventory system to assign IDs to new buildings or minions.

### Game State (JSON Blobs)
These columns store complex game state as JSON strings:
- **`inventory`**: The player's full inventory (currencies, buildings, minions).
- **`unlocks`**: List of unlocked items or features.
- **`villainQueue`**: Current queue of pending villain actions.
- **`purchasedSales`**: History of IAP/Sales purchases.
- **`socialRewards`**: Claims for social-based rewards.
- **`triggers`**: State for game-side tutorial or event triggers.
- **`pendingTransactions`**: Tracked but unconfirmed transactions.
- **`PlatformStoreTransactionIDs`**: Store-side validation IDs.

### Progression & Metrics
- **`PlayerLevel`**: Current recalculated level of the player.
- **`xp`**: Current experience points.
- **`completedOrders`**: Total count of completed orderboard tasks.
- **`completedQuestsTotal`**: Total count of completed quests.
- **`highestFtueLevel`**: Highest completed level of the First-Time User Experience.
- **`Time_played`** / **`totalAccumulatedGameplayDuration`**: Total gameplay time in seconds.

### Identity & Social
- **`name`**: The display name of the player (Minion name or Discord username).
- **`DISCORD`**: JSON blob containing Discord profile info (`id`, `username`, `avatar`, and an array of linked `uids`).
- **`discord_username`**: Normalized Discord username for quick lookup.
- **`discord_avatar`**: Hash of the player's Discord avatar.
- **`FACEBOOK`** / **`GOOGLE_PLAY`**: Associated IDs for legacy or alternative login methods.

### Internal Tracking
- **`last_updated`**: Automatic TIMESTAMP of the last row modification.
- **`lastPlayedTime`**: Epoch timestamp of the last session.
- **`country`**: The alpha-2 ISO country code of the player.

---

## 2. Table: `tse_teams` (Timed Social Events)

Manages temporary teams created for collaborative social events.

- **`team_id`** (INTEGER, PK): Unique auto-incrementing ID.
- **`event_id`** (INTEGER): The ID of the social event from `definitions.json`.
- **`order_progress`** (TEXT): JSON array of completed orders, tracking which user completed which task.
- **`created_at`**: TIMESTAMP of team creation.

---

## 3. Table: `tse_members`

Maps players to their respective teams.

- **`team_id`** (INTEGER, FK): Reference to `tse_teams`.
- **`user_id`** (TEXT): The UID of the player.
- **`reward_claimed`** (INTEGER): Boolean flag (0 or 1) indicating if the player has claimed the event reward.

---

## Data Management Notes
- **JSON Serialization**: Columns labeled as "Blobs" or "JSON" are stored as strings. The server logic uses `json.loads()` and `json.dumps()` with `ensure_ascii=False`.
- **UID Consolidation**: When accounts are linked, the `uid` column is updated to include all associated IDs (e.g., `"1001, 1002"`). The server uses `resolve_master_uid` in `utils/db.py` to handle these lookups.
