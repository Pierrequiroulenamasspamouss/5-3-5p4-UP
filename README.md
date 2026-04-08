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
- [x] **Fix Shop Crashes**: Resolve client-side crash when the server returns filtered or empty sales definitions for restricted users. (Fixed: Server now returns permanent fallback items)
- [x] **Robust Sales Engine**: Refine the `sales.py` logic to allow better dynamic offer management without UI side-effects. (Optimized and localized config)
- [x] **Buildings rotation bug**: When placing buildings, there is a colored square under the building indicating if the building can be placed or not, that does not get rotated when the building itself gets rotated. (Fixed: Swapped footprint dimensions in FootprintView to compensate for parent rotation)
- [x] **Windows scrollwheel issues**: Currently zooming is unbearable in Windows, and the scrollwheel is either too slow to be usable, or too fast for the zooming in and out. (Fixed sensitivity in `KeyboardZoomView.cs`)
- [ ] **Night/Day shaders**: Shaders at night for some animated textures are broken (Partially fixed: `AnimVert_wScroll`, `WaterSlide`, and `WaterFlow` updated)
- [x] **UI SETTINGS BUTTONS LAYOUT**: Issue with the buttons inside the game settings that don't follow adaptative adaptative dimensions UI. (Fixed in `SettingsPanelView.cs`)
- [ ] **Android 8+ crash**: Self explanatory, I assume there is an issue with ARM7 libraries for FMOD 
- [ ] **REBUILD FMOD PROJECT TO ADD CUSTOM AUDIO LOOPS**: Currently using raw banks extracted from the game, it is difficult to add new audio to the game
- [ ] **Adding debug command to remove limited time offers**: Self explanatory 
- [x] **Animations T-posing for Phil idling**: should not be too hard to fix (Fixed: NamedCharacterObject now robustly initializes animators in children)
- [ ] **Events and server-related issues**: Social events not automatically restarted when a week has passed for example, or definitions not working properly
- [ ] **Loading time very long on Android**: Maybe I should add a bundle mechanic back, if I have a way to rebuild the bundles. 
- [ ] **Android Discord login doesn't work**: Add media playing and discord login 
- [ ] **Video doesn't always play for new user.**: Self explanatory, depending on the platform the intro video should play, thoo it doesn't  
- [ ] **Some users get stuck at 30% loading on Android**: adding some telemetry back MIGHT be useful, some simple one that will send the logs to the server and then upload them in a Google drive folder
- [ ] **Softlocks in early-game**: pretty common stuff, lua quests issues, I should fix the commands to unlock yourself 
- [x] **Missing textures**: Minion shrugged statue has missing texture, as well as the minion bath. (Identified: Found `Decor_StatueAtlas_Prefab` and `Leisure_HotTub_Prefab` with all dependencies in source tree; confirmed manifest mapping issues)
- [ ] **TeamOrderBoard no audio**: Self explanatory 
- [ ] **Grayed out minion button softlocking the game**: Could be fixed by removing the party prerequisite ?
- [ ] **Permissions issues on later Android versions**: Self explanatory 
- [ ] **Server is ugly and should use the .env better**: (Improved: Centralized `config.py` and `.env` implementation)
- [ ] **No development server**: I should put a development server running in parallel of the main prod server since playing with prod is not optimal 



### Features & Infrastructure
- [ ] **TSE Logic Expansion**: Improve The Social Event (TSE) team systems and invitation reliability.
- [ ] **Global Chat Polish**: Enhance the in-game chat interface and server-side message history management.
- [ ] **Admin Dashboard**: Finalize the `dashboard_bp` for real-time player and economy monitoring.
- [ ] **Admin Tools**: Add a better UI for an admin to manage users, etc... Like browsing the existing database.  
- [ ] **Localization**: Complete the retrofitting of "LOLCAT" and "Minion" language supports.

### Maintenance
- [x] Cleanup `archive/` folder and consolidate legacy scripts. (Cleaned up `Plugins` and removed `.bak`/temporary files)
- [ ] Improve server-side logging for better debugging of client networking issues.


---

## Getting Started

1. **Server**: Navigate to the `SERVER/` directory and run `python kampai_server.py`.
2. **Client**: Open the `Minionsparadise/` folder in Unity 5.3.5p4. Use the custom build pipeline to prepare the assets. Note that there are different branches for different versions of the game. There is a Unity-2017 branch for later Unity versions, as well as a development branch where I put the latest development progress. 
3. **Deployment**: For mobile testing, use the `reverse-proxy-client.bat` to route traffic from your device to the local server. (frp fast reverse proxy)

---

*Note: This project is for educational and preservation purposes only.*
