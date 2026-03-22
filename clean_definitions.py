import json
import collections

def clean_definitions(current_path, archive_path, output_path):
    with open(current_path, 'r') as f:
        current_data = json.load(f)
    
    with open(archive_path, 'r') as f:
        archive_data = json.load(f)

    # Function to clean an object from duplicate keys (case-insensitive)
    # Since json.load already overwrote them, we can't find them here.
    # We need to parse it differently or just rely on the fact that 
    # we can RE-GENERATE it without duplicates.
    
    # Wait, if json.load overwrote them, we already LOST the non-zero values 
    # if the zero value came last.
    
    # So we MUST parse it manually to catch the duplicate keys BEFORE they are lost.
    
    from json import JSONDecoder

    class DuplicateCheckDecoder(JSONDecoder):
        def __init__(self, *args, **kwargs):
            super().__init__(object_pairs_hook=self.dict_with_cleaning, *args, **kwargs)
        
        def dict_with_cleaning(self, pairs):
            d = {}
            for k, v in pairs:
                k_lower = k.lower()
                if k_lower in d:
                    # Duplicate found!
                    existing_k, existing_v = d[k_lower]
                    if k_lower == 'unlocklevel':
                        # Prefer non-zero value
                        if existing_v == 0 and v != 0:
                            d[k_lower] = (k, v)
                        elif v == 0 and existing_v != 0:
                            # Keep existing
                            pass
                        else:
                            # Just keep the last one or whatever
                            d[k_lower] = (k, v)
                    else:
                        # For other keys, just keep the last one (standard behavior)
                        d[k_lower] = (k, v)
                else:
                    d[k_lower] = (k, v)
            
            # Convert back to regular dict with proper casing
            return {k: v for k, v in d.values()}

    with open(current_path, 'r') as f:
        content = f.read()
    
    cleaned_data = json.loads(content, cls=DuplicateCheckDecoder)
    
    # Ensure buildings from archive's buildingDefinitions are in current buildingDefinitions
    archive_buildings = {b['id']: b for b in archive_data.get('buildingDefinitions', [])}
    current_buildings = {b['id']: b for b in cleaned_data.get('buildingDefinitions', [])}
    
    for aid, abuilding in archive_buildings.items():
        if aid not in current_buildings:
            found_in = []
            for cat, items in cleaned_data.items():
                if isinstance(items, list):
                    for item in items:
                        if isinstance(item, dict) and item.get('id') == aid:
                            found_in.append(cat)
            
            if found_in:
                print(f"Building ID {aid} moved to other categories: {found_in}! Moving back to buildingDefinitions...")
                # Remove from other categories
                for cat in found_in:
                    cleaned_data[cat] = [i for i in cleaned_data[cat] if i.get('id') != aid]
                # Add back to buildingDefinitions
                cleaned_data['buildingDefinitions'].append(abuilding)
            else:
                print(f"Building ID {aid} missing from current definitions.json! Restoring from archive...")
                cleaned_data['buildingDefinitions'].append(abuilding)

    # Write cleaned data
    with open(output_path, 'w') as f:
        json.dump(cleaned_data, f, indent=2, sort_keys=True)
    
    print(f"Cleaned definitions saved to {output_path}")

current_file = r'c:\Unity\LATEST\SERVER\definitions.json'
archive_file = r'c:\Unity\LATEST\archive\SERVER\definitions.json'
output_file = r'c:\Unity\LATEST\SERVER\definitions_cleaned.json'

clean_definitions(current_file, archive_file, output_file)
