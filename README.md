# Minions Paradise — Open Paradise

Welcome to the **Minions Paradise** preservation and modification project. This repository contains the client-side code and server-side infrastructure required to run a custom version of EA's *Minions Paradise* (v5.3.x).

## Project Overview

This project aims to revitalize the game by removing its original reliance on external EA servers and `.unity3d` asset bundles. The client has been modified to load assets directly from local storage, and a custom Flask server provides all necessary backend services.

### Key Modifications
- **Local Asset Loading**: Assets are now loaded from `Minionsparadise/Assets/resources/content`, bypassing the need for remote bundle downloads.
- **Custom Backend**: A Python/Flask private server replaces the original EA backend, offering persistent saves, economy management, and social features.
- **Platform Agnostic**: Designed to run on modern development environments with a flexible reverse-proxy setup for mobile testing.

---

## Repository Structure

The project is organized into three main components:

### 🏝️ [Minionsparadise]
The core Unity 5.3 project.
- **Assets**: Contains all scripts, UI definitions, and game resources.
- **Scripts**: Extensive C# modifications to support local I/O and custom server communication.
- **FMOD**: Integrated audio project for game sounds and music.

### 🖥️ [SERVER]
A Flask-based private server implementations.
- **routes/**: API endpoints for game state, user profiles, sales/marketplace, and TSE events.
- **utils/**: Database handlers (SQLite), profile generation, and logging utilities.
- **marketplace/**: Definitions for in-game shop items and promotional offers.
- **nopromousers.txt**: A list of UIDs for which promotional offers are restricted.

### 📚 [DOC]
Technical documentation and guides.
- **API_Reference.md**: Documentation of the internal server endpoints.
- **Admin_Guide.md**: Instructions for server maintenance and user management.
- **IO_Replacements.md**: Details on how the Unity asset loading was retrofitted.

### 🛠️ Utilities
- **config-switcher.py**: A GUI utility to toggle between local and public server configurations for both the Unity client and the Flask server.

---

## 🛠️ TODO List


### High Priority



- Config file should be easily editable without a rebuild of the game. Right now, the game will load the config.json file from the game resources, when I would need it to be not in the resources, but in the StreamedAssets, so that it can be modified by the user and not be "untouchable" in the blob of the Unity resources. For that I need to have a script to : 
    - Load the internal config.json(Minionsparadise\Assets\Resources\config.json) in the persistent data path of the device (either Android, windows or potentially iOS), save it as a "local-config.json", and that the game itself uses the local-config.json from the streamingAssets to fetch the server's URL

- [x] **loadDefinitionsCommand.cs categorization/reset issues** [SOLVED]: Fixed categorization and inventory reset logic.
- [x] **Currency Shop prices messed up when online** [SOLVED]: Fixed `DebugCurrencyService` to merge server prices and preserve local fallbacks if the server returns incomplete data.
- [x] **No Level Cap** [SOLVED]: Removed the soft-maximum level by clamping XP requirements and rewards to the last defined level, allowing for infinite leveling.
- [x] **Optimizations breaking Villain Lair assets on Android** [SOLVED]: Corrected path sanitization in `KampaiResources.LoadAsync` to handle build-specific path prefixes.
- [x] **Discord login on Android** [SOLVED]: Enabled `CanOpenURL` in Android native implementation to allow browser-based sign-up.
- [x] **Video media not playing/launching** [SOLVED]: Integrated `IVideoService` in `LoadPlayerCommand` to play the intro video for new users on all platforms.
- [x] **Events and server-related issues (TSE)** [SOLVED]: Implemented automatic cycle looping in `TimedSocialEventService` to ensure events restart indefinitely.
- [x] **Stuck at 30% loading on Android** [SOLVED]: Restored telemetry funnel in `MainStartCommand` and added `SimpleLogTelemetrySender` for remote debugging.
- [x] **Missing language keys tool** [SOLVED]: Created `LocalizationSyncTool.cs` (Editor/Kampai/Tools) to identify missing localization entries.
- [ ] **REBUILD FMOD PROJECT TO ADD CUSTOM AUDIO LOOPS**: Currently using raw banks extracted from the game.
- [ ] **TeamOrderBoard no audio**: Stuart performance after order completion is missing sound (FMOD event missing).


### Features & Infrastructure
- [ ] **TSE Logic Expansion**: Improve The Social Event (TSE) team systems and invitation reliability.
- [ ] **Global Chat Polish**: Enhance the in-game chat interface and server-side message history management.

### Maintenance
- [ ] Improve server-side logging for better debugging of client networking issues.


---

## Getting Started

1. **Server**: Navigate to the `SERVER/` directory and run `python kampai_server.py`.
2. **Client**: Open the `Minionsparadise/` folder in Unity 5.3.5p4. Use the custom build pipeline to prepare the assets. Note that there are different branches for different versions of the game. There is a Unity-2017 branch for later Unity versions, as well as a development branch where I put the latest development progress. 
3. **Deployment**: For mobile testing, use the `reverse-proxy-client.bat` to route traffic from your device to the local server. (frp fast reverse proxy)

---

*Note: This project is for educational and preservation purposes only.*
