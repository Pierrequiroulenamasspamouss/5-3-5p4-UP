# Offline Mode Implementation Walkthrough

We have implemented a robust Offline Mode for *Minions Paradise* that allows gameplay without a server connection, handles local data persistence, and provides a seamless transition back to online mode.

## Key Components

### 1. Data Infrastructure (`OfflineModeUtility.cs`)
- **Local Storage**: Saves player state, configurations, and definitions to `Application.persistentDataPath`.
- **File Rotation**: Before saving, the existing file is moved to `.old` to prevent data loss on crash.
- **Cache Fallback**: Methods to load from the local cache with built-in resource fallback for first-time offline launches.
- **Save Comparison**: A utility to compare `lastPlayedTime` between server and local saves to ensure the latest progress is kept during synchronization.

### 2. Loading Flow Retrofitting
- **`ConfigurationsService.cs`**: 
    - Automatically caches the server configuration JSON upon successful download.
    - Added `LoadLocalConfiguration()` to load from cache or built-in resources when offline.
    - Forced critical online features (Marketplace, Social, Facebook, Synergy) to be disabled via killswitches when in offline mode.
- **`FetchDefinitionsCommand.cs`**: 
    - Modified to check if the game is offline.
    - If offline, it bypasses the HTTP request and loads definitions from the local cache or `Resources/definitions.json`.
- **`LoadPlayerCommand.cs`**:
    - Modified the "remote" load case to automatically load from the local file if `IsOffline` is true.
- **`SavePlayerCommand.cs`**:
    - Updated to perform a parallel local save whenever a remote save is triggered, ensuring the local cache is always up-to-date.

### 3. Login Flow & Transitions
- **`TransitionToOfflineModeCommand.cs`**:
    - A new command that handles the logic of switching the game to an offline state.
    - Creates a temporary `OFFLINE_...` UID if no UserID exists.
    - Resumes the loading sequence by triggering configuration/player load.
- **`ShowOfflinePopupCommand.cs`**:
    - Modified to check for a new player preference `OfflineMode_Pref`.
    - If set to `true`, the "Lost Connectivity" popup is bypassed, and the game transitions to offline mode automatically.

### 4. UI Integration
- **`OfflineMediator.cs`**:
    - Hooked up the `playOfflineButton` (which was already in the view) to trigger the `TransitionToOfflineModeSignal`.
- **`ModsPanelView.cs` & `ModsPanelMediator.cs`**:
    - Added a new toggle in the Mods panel for the player's offline preference.
    - This allows users to "Mute" the offline warning and play offline by default.

### 5. Server-Side Support (`SERVER/routes/user.py`)
- **Custom UID Registration**: Updated the `/rest/user/register` endpoint to accept a client-provided `userId`.
- **Transition Support**: This allows the game to "Register" an offline-created UID once a connection is established, uploading the local progress to the server under that same ID.

## How to Test
1. **Initial Cache**: Launch the game online once to cache definitions and config.
2. **Manual Offline**: Disconnect the internet. When the "Lost Connectivity" popup appears, click "Play Offline".
3. **Automatic Offline**: Go to the Mods panel, toggle "Offline" to ON. Restart the game without internet. It should load directly into the game using local data.
4. **Sync**: Play offline, make progress (e.g., gain level). Reconnect internet and restart. The game should detect the local save is newer and upload it (or keep it as primary).

> [!IMPORTANT]
> Ensure that `definitions.json` and `config.json` are present in `Assets/Resources` for the very first launch in offline mode to work if the user hasn't downloaded them yet.
