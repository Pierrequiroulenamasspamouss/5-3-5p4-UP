# Data Formats

This document describes the structure of the JSON-based data systems used by the Kampai Game Server.

---

## 1. `definitions.json` (The Data Dictionary)

This is the central repository for all game-side metadata. It contains an object where each key represents a specific category of definitions.

### Major Definition Categories

#### `buildingDefinitions` (Array)
Describes all structures placed in the game world.
- **`id`**: Unique integer ID for the building.
- **`localizedKey`**: Translation key for the building name.
- **`constructionTime`**: Time (in seconds) to build.
- **`type`**: Broad category (`RESOURCE`, `CRAFTING`, `DECORATION`, etc.).
- **`itemId`**: The underlying item ID this building "represents" in inventory.
- **`recipeDefinitions`**: (For `CRAFTING`) List of item IDs this building can produce.

#### `itemDefinitions` (Array)
Describes all collectable or usable items (coconuts, bananas, tools, etc.).
- **`id`**: Unique integer ID.
- **`type`**: `CURRENCY`, `RESOURCE`, `CRAFTABLE`, etc.
- **`localizedKey`**: Translation key.

#### `salePackDefinitions` (Array)
Definitions for shop bundles.
- **`id`**: Unique integer ID.
- **`UTCSTARTDATE`/`UTCENDDATE`**: (Filtered by server) Availability window.
- **`CANBUYTHISMANYTIMES`**: Purchase limit.
- **`UNLOCKLEVEL`**: Level requirement.

#### `currencyStoreDefinition` (Object)
Defines the shop layout, categories, and individual SKUs.

---

## 2. `empty_player.json` (Player Profile)

The player profile is the main data structure representing a user's progress and inventory.

### Core Fields
- **`ID`**: The unique numeric account ID.
- **`version`**: Schema version (incremented on save).
- **`nextId`**: The ID to be assigned to the next unique instance of a minion or building.

### Inventory System
The `inventory` field is an array of objects. Each object can represent different things based on its `Definition` ID:

#### Currencies (Fixed Definitions)
- **ID 0**: Soft Currency (Coins)
- **ID 1**: Premium Currency (Gems/Sand)
- **ID 2**: XP
- **Structure**: `{ "ID": 1, "Definition": 1, "Quantity": 20 }`

#### Buildings & Minions (Instance-Based)
These entries represent a specific placement or character.
- **Common Fields**: `ID` (Instance ID), `Definition` (Type ID).
- **Building Fields**: `Location` (x, y), `State` (Construction, Active), `BuildingNumber`.
- **Minion Fields**: `Name`, `State` (Assigned, Idle).

---

## 3. `config.json` (Server Configuration)

Controls server-wide behaviors and client-side parameters.

- **`allConfigs`**: Root object.
- **`anyDeviceType`**: Default settings for all clients.
  - **`definitions`**: URL to the active `definitions.json`.
  - **`dlcManifests`**: Dictionary of URLs for various performance levels (`low`, `med`, `high`).
  - **`maxRPS`**: Rate limit for API requests.
  - **`msHeartbeat`**: Interval (in milliseconds) for client heartbeats.
  - **`minimumVersion`**: Force upgrade threshold.
  - **`killSwitches`**: List of features to disable dynamically.
