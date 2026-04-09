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
- [ ] **OFFLINE MODE**: 
Offline mode should consist in a button in the ModsPanelView that allows the player to switch to a local configuration. For that it need to pass different steps: 
    - When logging in, and saving the player's progress, it needs to do also a local copy of the progress on the machine, in streamingAssets I guess, so that Android compatibility remains. For that the game also needs a copy of the configuration, and the definitions inside the game's internal files, that could be compiled with the game (I believe there is already a definitions.json inside the game's resources.) so that on the first launch it doesn't break for definitions missing. Once a first communication with the server has been made, the game will attempt to register to the server, as a new player, and upload the save with the player's progress up to now ( simple json object that contains the player's inventory and data.). That means the server has to handle that properly. It also will retrieve the config.json from the server (which is different from the config.json that is built inside the game) and store it inside the streaming assets. Same for the definitions, so that the player will always have the latest definitions possible. 
    - If a login fails when launching the game the game falls back to the local resources (config, definitions, player save) for whatever is the latest version of it. That depends of if the player already has contacted the server once or not. 
    - The screen showing there is no connection would have a button called Play offline (ANTIGRAVITY: ButtonView.OnClickEvent triggered on btn_playOfflineUnityEngine.Debug:Log(Object)Kampai.UI.View.ButtonView:OnClickEvent() (at Assets/Scripts/Assembly-CSharp/Kampai/UI/View/ButtonView.cs:16)UnityEngine.EventSystems.EventSystem:Update() the UI is already implemented in game. the logic is missing. )
    - Once the game is in offline mode, the game will not attempt to connect to the server and use the local resources, that means the local version of the definitions, the local version of the server's config.json, and the local version of the player's data. 
    - The first launch of the game is a little different since the player never had access to the server, the game has never connected to the server previously, the game HAS to rely on local defintions/config, and save newly generated data on device. Also since the user ID is defined by the server at registration I believe, there has changes to be made to the server for creating specific register requests, with a specific user ID. After the first launch of the game, the game will not need to rely on the builtin config/definitions and should prioritize the latest config and definitions that have been pulled from the server, AND been stored on the device. 
    - The fix has to be implemented for Windows right now, but keeping compatibility with Android ina future date in time. 
    - To sum up : 
        - First-time playing the game : register -> failure -> create custom UID that may be replaced in the future.
                    -> use builtin config and definitions, and load player data successfuly
                 -> success of the registration, proceed as right now. 
                 -> always saves a copy on device of the player's data when uploading the save to the server.  
                 -> also saves a copy of the config and the definitions locally each time it fetches that data off the server. 
        - Not first time playing the game (has already once downloaded config and definitions from the server, success to reach the server) -> failure to login ->  loads those instead, and loads the local game save. 
            -> success to login : compares the player data off the server and off the device, and uses the one with the most recent timestamp. Loads that one and deletes the old one (renames it to .old and deletes the previous .old save file. so that it cycles.)
        - First time reaching the server after offline first setup : register to the server with specifying a precise UID, the server COULD respond with a different UID if the one is already taken. else it will accept the UID, and respond as a valid registration. The server will create the entry in the database for that player. login after that SHOULD go smoothly. Saving the player data too, once the player is properly in the database. 

- The ModsPanelView should be able to have a toggle for the preference of the player to remember, by default the game SHOULD try to play online, but if the toggle is set to offline in the mods, the game will not display the message of the device being offline again, since it would check the preference before loading the screen, and if the preference is set to online_pref = false, the game will directly launch the game offline without the "game offline popup screen". Note that the preference is different that the current game state. The preference indicates which default configuration is set, and the state whether the game is played online or offline is volatile and defined at each launch by either the preference, either the option to "play offline" on the popup screen.
- Functions that use online functionality ( marketplace, socialEvents, etc...) should be able to handle null responses and show empty screens or "you are playing offline. This is not available" messages for example.

        









- [x] **Fix Shop Crashes**: Resolve client-side crash when the server returns filtered or empty sales definitions for restricted users. (Fixed: Server now returns permanent fallback items)
- [x] **Robust Sales Engine**: Refine the `sales.py` logic to allow better dynamic offer management without UI side-effects. (Optimized and localized config)
- [ ] **Android 8+ crash**: Self explanatory, I assume there is an issue with ARM7 libraries for FMOD 
- [ ] **REBUILD FMOD PROJECT TO ADD CUSTOM AUDIO LOOPS**: Currently using raw banks extracted from the game, it is difficult to add new audio to the game
- [ ] **Adding debug command to remove limited time offers**: Self explanatory 
- [ ] **Events and server-related issues**: Social events not automatically restarted when a week has passed for example, or definitions not working properly
- [ ] **Loading time very long on Android**: Maybe I should add a bundle mechanic back, if I have a way to rebuild the bundles. 
- [ ] **Android Discord login doesn't work**: Add media playing and discord login 
- [ ] **Video doesn't always play for new user.**: Self explanatory, depending on the platform the intro video should play, thoo it doesn't  
- [ ] **Some users get stuck at 30% loading on Android**: adding some telemetry back MIGHT be useful, some simple one that will send the logs to the server and then upload them in a Google drive folder
- [ ] **Softlocks in early-game**: pretty common stuff, lua quests issues, I should fix the commands to unlock yourself 
- [ ] **TeamOrderBoard no audio**: Self explanatory 
- [ ] **Grayed out minion button softlocking the game**: Could be fixed by removing the party prerequisite ?
- [ ] **Permissions issues on later Android versions**: Self explanatory 
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
