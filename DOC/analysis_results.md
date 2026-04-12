# Analysis of Changes between OpenMP_OLD and OpenMP

After reviewing the full `git diff` and recent commit logs between the older repository (`OpenMP_OLD`) and the current project state (`OpenMP`), here is an extensive list of changes and modifications that have led to the current erratic behavior of the game.

## 1. StrangeIoC and UI Mediation Changes (The "90% of Buttons Don't React" Issue)

### Issue Description
The user noticed that 90% of buttons do not react, mediators do not start, and listeners are missing.

### Cause & Changes
1. **The `KampaiTools.cs` Utility:** A new Editor script was created (`KampaiTools.cs`), which includes a `RepairSettingsPrefab` function. This tool was used to strip `Mediator` components off prefabs manually. 
2. **Missing Injections / Context Failure:** In standard StrangeIoC, `MediationBinder` attaches Mediators dynamically onto GameObjects containing `View` instances at runtime. However, if the `UIContext` (where all mediators are mapped) fails to initialize properly—or if an `InjectionException` occurs due to missing dependencies—the `MediationBinder` aborts early and none of the subsequent views will get their mediators.
3. **Changes in `UIContext.cs`:** Several View-to-Mediator bindings were explicitly removed:
   - `MoveBuildingMenuView`
   - `COPPAAgeGatePanelView`
   - `TownHallDialogView`
   - `UserAgeForCOPPAReceivedSignal`
   If any code still tries to dispatch or rely on these removed bindings, it may trigger an uncaught exception, which cascades and prevents the rest of the UI context from completing its DI mapping, thereby killing all other UI buttons (since they rely on `ButtonView` signals bubbling up to parent mediators).
4. **Altered `MediationBinder.cs` Logic:** Modified the core StrangeIoC `MediationBinder` to output warnings instead of strict mapping paths, and modified injection loops which can mask underlying registration errors.

## 2. Offline Mode and Definitions Service (The "Messed Up UI / Currency Broken" Issue)

### Issue Description
Offline mode successfully loads, but the UI is heavily bugged: text bubbles don't update, currency amounts are mismatched/broken, and the shop is unusable.

### Cause & Changes
1. **`OfflineModeUtility.cs` Introduces JSON Cache Fallback:** The game was modified to fetch and caching definitions, configs, and player saves using `OfflineModeUtility.cs` which leverages `Application.persistentDataPath` to save `definitions.json`, `server_config.json`, and `player_save.json`.
2. **Definition Parsing Flaws:** When reading definitions from the cached `definitions.json` file (unlike the previously utilized binary cache which worked fine), some definitions are dropping. The changes in `DefinitionService.cs` indicate that the parser loops through definitions and ignores items marked as `Disabled`. If JSON deserialization maps default values incorrectly for certain fields (like `Disabled` resolving to `true` by default on some node types, or missing properties), critical definitions (such as Currencies, Store Categories, or Tasks) fail to register.
3. **Mismatched Timestamps:** The new save loading logic in `OfflineModeUtility.GetLatestSave` attempts to compare save timestamps using a hacky substring approach (`"lastPlayedTime":`) which may fail to parse properly if the JSON formatting differs slightly, resulting in corrupt initial states or rolling back to an incompletely-fetched offline state.

## 3. Settings Panel Modifications
1. **Layout Overhaul:** `SettingsPanelView.cs` had its entirely hardcoded layout calculations removed (the `LayoutButtons()` method was erased), breaking auto-alignment on non-standard resolutions.
2. **Toggle Logic Refactor:** `ButtonView` and `SettingsPanelMediator` saw extensive re-writes for the `doubleConfirmToggle` property to fix bugs with the checkmark sync. However, doing so added brittle `null` checks inside injected components.
3. **Missing Modules:** The `DLCButton` and `nightToggleButton` were stripped off the custom UI scaling logic.

## Summary of Fixes Required
1. **Restore UIContext Bindings** that were errantly deleted, restoring the `MVCS` mediation integrity and allowing Mediators to bubble and attach dynamically again without halting execution.
2. **Patch DefinitionService JSON Deserialization** to guarantee that Currency, Tasks, and Store objects do not get inappropriately flagged as `Disabled` when reading from the text/JSON cache.
3. **Reinstate Safe `ButtonView` Behavior** and evaluate the stripped scripts.
4. **Fix `OfflineModeUtility`** string-parsing vulnerabilities.
