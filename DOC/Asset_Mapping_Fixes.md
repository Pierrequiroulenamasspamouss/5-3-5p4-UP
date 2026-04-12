# Asset Mapping & Missing Texture Investigation

## Summary
Investigated reports of "missing textures" for specific buildings: "Minion Shrugged" (Atlas Statue) and "Minion Bath" (Hot Tub). 

## Findings

### 1. Minion Shrugged Statue
- **Prefab Name**: `Decor_StatueAtlas_Prefab`
- **Location**: `Assets/Resources/content/dlc/decor_statueatlas/model/`
- **Materials**:
    - `Decor_StatueAtlas_D.mat` (Main texture: `Decor_StatueAtlas_D.png`)
    - `Decor_StatueAtlas_Water_Mat.mat` (Uses `WaterFlow.shader`)
    - `Decor_StatueAtlas_Shadow_D.mat`
    - `Decor_StatueAtlasGlobe_D.mat`
- **Status**: All assets were found in the source directory. The "missing" appearance may be due to the asset not being registered in `KampaiAssetManifest.json`, preventing correct loading via `KampaiResources.Load()`.

### 2. Minion Bath (Hot Tub)
- **Prefab Name**: `Leisure_HotTub_Prefab`
- **Location**: `Assets/Resources/content/dlc/leisure_hottub/model/`
- **Materials**:
    - `Leisure_HotTub_D.mat`
    - `Leisure_HotTub_Water_D.mat` (Uses `WaterFlow.shader`)
    - `Leisure_HotTub_Shadow_D.mat`
- **Status**: All assets found in source directory. Verified that the water texture `Water+Waves.png` is located in `shared/shared_materials/`.

## Resolution / Next Steps
The assets themselves are present and their material/texture links (GUIDs) are correct. To fix the "missing" issue in the game:
1. Ensure these prefabs are added to `KampaiAssetManifest.json`.
2. Verify that the `KampaiResources` system is scanning the `content/dlc/` subdirectories during initialization.
3. If the assets appear pink, ensure that `WaterFlow.shader` and `Unlit.shader` are compiled correctly for the target platform.

## Shader Fixes (Night Mode)
The following shaders have been verified or updated to support `ApplyKampaiNight`:
- `AnimVert_wScroll.shader`
- `WaterSlide.shader`
- `WaterFlow.shader`
