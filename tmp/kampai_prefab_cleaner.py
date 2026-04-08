import re
import os

PREFAB_PATH = r'c:\Users\domin\Downloads\Unityproject\OpenMP\Minionsparadise\Assets\Resources\content\shared\shared_ui\ui_prefabs\ui_common\hud\screen_HUD_Panel_Settings_Menu.prefab'
SCRIPTS_ROOT = r'c:\Users\domin\Downloads\Unityproject\OpenMP\Minionsparadise\Assets\Scripts'

def get_mediator_guids():
    mediators = {}
    for root, dirs, files in os.walk(SCRIPTS_ROOT):
        for file in files:
            if "Mediator.cs" in file and file.endswith(".meta"):
                path = os.path.join(root, file)
                with open(path, 'r', encoding='utf-8') as f:
                    guid = re.search(r'guid:\s*([a-f0-9]+)', f.read())
                    if guid:
                        mediators[guid.group(1)] = file.replace('.meta', '')
    return mediators

def clean_prefab():
    print(f"--- Kampai Prefab Mediator Stripper ---")
    mediator_guids = get_mediator_guids()
    print(f"Found {len(mediator_guids)} mediator scripts in project.")

    with open(PREFAB_PATH, 'r', encoding='utf-8') as f:
        content = f.read()

    # 1. Identify all MonoBehaviour chunks that are Mediators
    # Standard Unity YAML delimiter
    docs = re.split(r'^--- !u!', content, flags=re.MULTILINE)
    header = docs[0]
    
    new_docs = []
    mediator_fids = set()
    
    for doc_body in docs[1:]:
        # Extract id and type
        # Format: 114 &12345
        match = re.search(r'^(\d+) &(\d+)', doc_body)
        if not match:
            new_docs.append(doc_body)
            continue
            
        doc_type = match.group(1)
        doc_id = match.group(2)
        
        if doc_type == '114': # MonoBehaviour
            guid_match = re.search(r'guid:\s*([a-f0-9]+)', doc_body)
            if guid_match and guid_match.group(1) in mediator_guids:
                print(f"Removing Mediator ID {doc_id} ({mediator_guids[guid_match.group(1)]})")
                mediator_fids.add(doc_id)
                continue
        
        new_docs.append(doc_body)

    # 2. Cleanup component references in GameObjects
    final_content = header
    for doc_body in new_docs:
        header_match = re.search(r'^(\d+) &(\d+)', doc_body)
        doc_type = header_match.group(1) if header_match else ""
        
        if doc_type == '1': # GameObject
            updated_body = doc_body
            for fid in mediator_fids:
                # Remove lines like: - component: {fileID: 12345}
                updated_body = re.sub(r'^\s*-\s*component:\s*\{fileID:\s*' + fid + r'\}\s*\n', '', updated_body, flags=re.MULTILINE)
            final_content += "--- !u!" + updated_body
        else:
            final_content += "--- !u!" + doc_body

    with open(PREFAB_PATH, 'w', encoding='utf-8', newline='\n') as f:
        f.write(final_content)
    
    print(f"\nSuccessfully removed {len(mediator_fids)} mediator instances from prefab.")
    print("Your Settings Menu should now open without crashing!")

if __name__ == "__main__":
    clean_prefab()
