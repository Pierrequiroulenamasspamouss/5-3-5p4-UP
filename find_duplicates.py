import re
import collections

def find_duplicate_keys(file_path):
    with open(file_path, 'r') as f:
        content = f.read()
    
    # Simple regex to find keys. This is not perfect but should work for well-formatted JSON.
    key_pattern = re.compile(r'\"(\w+)\"\s*:')
    
    # We need to track the scope of objects.
    # A better way is to use a custom decoder.
    
    import json
    
    class DuplicateCheckDecoder(json.JSONDecoder):
        def __init__(self, *args, **kwargs):
            super().__init__(object_pairs_hook=self.dict_with_duplicate_check, *args, **kwargs)
        
        def dict_with_duplicate_check(self, pairs):
            d = {}
            for k, v in pairs:
                k_lower = k.lower()
                if k_lower in d:
                    print(f"Duplicate key found (case-insensitive): '{k}' vs '{d[k_lower][0]}' with values {v} vs {d[k_lower][1]}")
                d[k_lower] = (k, v)
            return d

    print(f"Analyzing {file_path} for case-insensitive duplicate keys...")
    try:
        json.loads(content, cls=DuplicateCheckDecoder)
    except Exception as e:
        print(f"Error parsing JSON: {e}")

find_duplicate_keys(r'c:\Unity\LATEST\SERVER\definitions.json')
