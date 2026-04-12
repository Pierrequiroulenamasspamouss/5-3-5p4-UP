import os
import json
import shutil

# Paths
SERVER_DIR = r"c:\Users\domin\Downloads\Unityproject\OpenMP\SERVER"
UNITY_PROJECT_DIR = r"c:\Users\domin\Downloads\Unityproject\OpenMP\Minionsparadise"
RESOURCES_DIR = os.path.join(UNITY_PROJECT_DIR, "Assets", "Resources")

# Files to sync
SYNC_FILES = [
    (os.path.join(SERVER_DIR, "config.json"), os.path.join(RESOURCES_DIR, "config_server.json")),
    (os.path.join(SERVER_DIR, "definitions.json"), os.path.join(RESOURCES_DIR, "definitions.json"))
]

def sync_data():
    print("--- Syncing Server Data ---")
    for src, dst in SYNC_FILES:
        if os.path.exists(src):
            shutil.copy2(src, dst)
            print(f"Copied: {os.path.basename(src)} -> {os.path.basename(dst)}")
        else:
            print(f"Warning: Source file not found: {src}")

def generate_manifest():
    print("\n--- Generating KampaiAssetManifest.json ---")
    map_data = {}
    data_path = os.path.join(UNITY_PROJECT_DIR, "Assets")
    search_dirs = ["content", "Shader", "Resources"]
    
    # We use Case-Insensitive keys as Unity does
    for search_dir in search_dirs:
        full_search_path = os.path.join(data_path, search_dir)
        if not os.path.exists(full_search_path):
            continue
            
        for root, dirs, files in os.walk(full_search_path):
            for file in files:
                if file.endswith(".meta"):
                    continue
                
                # Normalize path for JSON manifest (using forward slashes and starting from "Assets/")
                file_path = os.path.join(root, file)
                normalized_path = file_path.replace(os.path.sep, '/').replace('\\', '/')
                
                # Find "Assets/" part
                assets_idx = normalized_path.lower().find("/assets/")
                if assets_idx != -1:
                    relative_path = normalized_path[assets_idx + 1:]
                else:
                    # Fallback relative to project root
                    relative_path = os.path.relpath(file_path, UNITY_PROJECT_DIR).replace(os.path.sep, '/')
                
                file_name_no_ext = os.path.splitext(file)[0]
                
                if file_name_no_ext not in map_data:
                    map_data[file_name_no_ext] = relative_path

    manifest_path = os.path.join(RESOURCES_DIR, "KampaiAssetManifest.json")
    with open(manifest_path, "w", encoding="utf-8") as f:
        json.dump(map_data, f, indent=2)
        
    print(f"Generated manifest with {len(map_data)} entries at {manifest_path}")

if __name__ == "__main__":
    sync_data()
    generate_manifest()
    print("\n--- Complete! ---")
