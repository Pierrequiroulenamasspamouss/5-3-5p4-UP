import json
import collections

def analyze_definitions(file_path):
    with open(file_path, 'r') as f:
        data = json.load(f)
    
    buildings = data.get('buildingDefinitions', [])
    building_ids = [b.get('id') for b in buildings]
    
    # Check for duplicates in buildingDefinitions
    id_counts = collections.Counter(building_ids)
    duplicates = {id: count for id, count in id_counts.items() if count > 1}
    
    # Check for other categories
    categories = list(data.keys())
    
    return {
        'building_ids': set(building_ids),
        'duplicates': duplicates,
        'categories': categories
    }

current_path = r'c:\Unity\LATEST\SERVER\definitions.json'
archive_path = r'c:\Unity\LATEST\archive\SERVER\definitions.json'

current_info = analyze_definitions(current_path)
archive_info = analyze_definitions(archive_path)

missing_buildings = archive_info['building_ids'] - current_info['building_ids']
extra_buildings = current_info['building_ids'] - archive_info['building_ids']

print(f"Current Categories: {current_info['categories']}")
print(f"Archive Categories: {archive_info['categories']}")
print(f"Missing Building IDs (in archive but not in current): {missing_buildings}")
print(f"Duplicate Building IDs in current: {current_info['duplicates']}")

# Search for missing buildings in other categories in current
with open(current_path, 'r') as f:
    current_data = json.load(f)

for bid in missing_buildings:
    found_in = []
    for cat, items in current_data.items():
        if isinstance(items, list):
            for item in items:
                if isinstance(item, dict) and item.get('id') == bid:
                    found_in.append(cat)
    if found_in:
        print(f"Building ID {bid} found in other categories: {found_in}")
    else:
        print(f"Building ID {bid} NOT FOUND in current definitions.json")

# Find duplicate fields within objects
def find_duplicate_keys_in_file(path):
    duplicates = []
    with open(path, 'r') as f:
        content = f.read()
    
    # This is hard to do with standard json lib as it overwrites.
    # We can try a manual regex or a custom decoder.
    return "Use manual check for now"

print(find_duplicate_keys_in_file(current_path))
