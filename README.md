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

- loadDefinitionsCommand.cs is very glitchy, I would need you to rollback to an old version of the definitions loading ( IDK how to have a history of only one specific file on Github, but I believe at the point of 50e5681cbd64cb8f947481aa9fc4067af16e5606 commit, the issue did not exist.). This causes strange issues like :
    - Buildings in the building view to not be displayed in the correct category of buildings. 
    - When resetting player's inventory it doesn't reset the unlocks the player has in the buildings shop ( NOT the microtransations currecyShop)

- On a similar note, the prices in the currencyShop (for real money microtransactions, not buying buildings ) is messed up, since the game fails to load the real prices when online and communicating with the server, but when offline and it loads the builtin prices from the game's resources (Minionsparadise\Assets\Resources\MarketPrices.json), there is no issue displaying the prices correctly. 

- There seems to be a soft-maximum level that the player can achieve, after a certain level ( around 50-60 I think). I need the game to have no "level cap". if you want to be level 10000 the game should be able to display a very big level.

- It seems some optimizations done will make the game fail to load Villain Lair assets on Android. 

- Clicking on "sign up with Discord" on Android doesn't open the browser or the application, like it does on Windows for example. 

- Video media play on a new game doesn't launch. Normally, a media player should play one video downloaded from the server, however the game seems to skip the video playing part, since I never implemented a media player in the game. I'd need to have that feature added back, both for Windows and Android. 

- [ ] **REBUILD FMOD PROJECT TO ADD CUSTOM AUDIO LOOPS**: Currently using raw banks extracted from the game, it is difficult to add new audio to the game
- [ ] **Events and server-related issues**: Social events not automatically restarted when a week has passed for example, or definitions not working properly

- [ ] **Video doesn't always play for new user.**: Self explanatory, depending on the platform the intro video should play, tho it doesn't.
- [ ] **Some users get stuck at 30% loading on Android**: adding some telemetry back MIGHT be useful, some simple one that will send the logs to the server and then upload them in a Google drive folder
- [ ] **TeamOrderBoard no audio**: When playing the game, it seems that Stuart doesn't make sound once the order board is completed 
- [ ] **Missing language keys** I need to make a list of the missing keys. I need a tool to sync all the languages, so I can take each key, and see if for exampole a language has keys missing


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
