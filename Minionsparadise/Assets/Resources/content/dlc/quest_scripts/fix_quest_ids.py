import os
import re

# Mapping of variable names and their hardcoded IDs to their correct definition IDs
# Format: { (variable_name_prefix, hardcoded_id): definition_id }
ID_MAPPING = {
    ('philId', '78'): '1080',
    ('tikiBarId', '313'): '3041',
    ('tikiBarID', '313'): '3041',
    ('tikiHutID', '313'): '3041',
    ('stageId', '370'): '3054',
    ('orderBoardId', '309'): '3022',
    ('coconutTreeID', '342'): '3142'
}

QUEST_SCRIPTS_DIR = r'c:\Unity\LATEST\Minionsparadise\Assets\content\dlc\quest_scripts'

def update_quest_scripts():
    files_processed = 0
    replacements_made = 0

    for filename in os.listdir(QUEST_SCRIPTS_DIR):
        if not filename.endswith('.txt'):
            continue
        
        filepath = os.path.join(QUEST_SCRIPTS_DIR, filename)
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
        
        new_content = content
        
        for (var_name, old_id), def_id in ID_MAPPING.items():
            # Pattern to match: varName = OldID or varName=OldID or local varName = OldID
            # We want to replace it with: varName = qs.getInstanceId(DefID)
            # Using a careful regex to not break other things
            pattern = rf'(\b{var_name}\s*=\s*){old_id}\b'
            replacement = rf'\1qs.getInstanceId({def_id})'
            
            if re.search(pattern, new_content):
                new_content = re.sub(pattern, replacement, new_content)
                print(f"Updated {var_name} in {filename}")
                replacements_made += 1
        
        if new_content != content:
            with open(filepath, 'w', encoding='utf-8') as f:
                f.write(new_content)
            files_processed += 1
    
    print(f"\nProcessing complete.")
    print(f"Files modified: {files_processed}")
    print(f"Total replacements made: {replacements_made}")

if __name__ == "__main__":
    update_quest_scripts()
