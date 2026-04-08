import re
import os

# Paths to the prefab and script metadata
PREFAB_PATH = r'c:\Users\domin\Downloads\Unityproject\OpenMP\Minionsparadise\Assets\Resources\content\shared\shared_ui\ui_prefabs\ui_common\hud\screen_HUD_Panel_Settings_Menu.prefab'
SCRIPTS_ROOT = r'c:\Users\domin\Downloads\Unityproject\OpenMP\Minionsparadise\Assets\Scripts'

def get_guid_mapping():
    mapping = {}
    for root, dirs, files in os.walk(SCRIPTS_ROOT):
        for file in files:
            if file.endswith('.cs.meta'):
                path = os.path.join(root, file)
                with open(path, 'r', encoding='utf-8') as f:
                    content = f.read()
                    guid = re.search(r'guid:\s*([a-f0-9]+)', content)
                    if guid:
                        mapping[guid.group(1)] = file.replace('.cs.meta', '.cs')
    return mapping

def scan_prefab():
    print(f"Scanning prefab: {PREFAB_PATH}\n")
    guid_to_name = get_guid_mapping()
    
    with open(PREFAB_PATH, 'r', encoding='utf-8') as f:
        content = f.read()

    # Split into YAML documents
    docs = re.split(r'^--- !u!\d+ &(\d+)', content, flags=re.MULTILINE)
    id_to_doc = {}
    for i in range(1, len(docs)-1, 2):
        id_to_doc[docs[i]] = docs[i+1]

    # Map GO IDs to names
    go_names = {}
    for fid, body in id_to_doc.items():
        if body.strip().startswith('GameObject:'):
            nm = re.search(r'm_Name:\s*(.+)', body)
            if nm: go_names[fid] = nm.group(1).strip()

    # Find MonoBehaviours and their parents
    found_views = {}
    
    for fid, body in id_to_doc.items():
        if body.strip().startswith('MonoBehaviour:'):
            guid_match = re.search(r'm_Script:\s*\{fileID:\s*\d+,\s*guid:\s*([a-f0-9]+)', body)
            go_ref = re.search(r'm_GameObject:\s*\{fileID:\s*(\d+)\}', body)
            
            if guid_match and go_ref:
                guid = guid_match.group(1)
                go_id = go_ref.group(1)
                script_name = guid_to_name.get(guid, f"Unknown (guid: {guid})")
                go_name = go_names.get(go_id, f"Unknown (id: {go_id})")
                
                if "View" in script_name or "Mediator" in script_name:
                    if script_name not in found_views: found_views[script_name] = []
                    found_views[script_name].append(go_name)

    print("=== SUMMARY OF VIEWS/MEDIATORS IN PREFAB ===")
    for script, objects in found_views.items():
        count = len(objects)
        status = "OK" if count == 1 else "!!! MULTIPLE DETECTED !!!"
        print(f"[{status}] {script}: attached to {count} objects: {', '.join(objects)}")

if __name__ == "__main__":
    scan_prefab()
