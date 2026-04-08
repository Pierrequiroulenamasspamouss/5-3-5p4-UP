import json

def find_phil(data):
    if isinstance(data, dict):
        if data.get('id') == 27000 or 'Phil' in str(data.get('name', '')):
            print(f"Found something: {data}")
        for k, v in data.items():
            if k == 'namedCharacterDefinitions' or k == 'characterDefinitions':
                print(f"Found {k}")
            find_phil(v)
    elif isinstance(data, list):
        for item in data:
            find_phil(item)

with open(r'c:\Users\domin\Downloads\Unityproject\OpenMP\SERVER\definitions.json', 'r') as f:
    try:
        data = json.load(f)
        for key in data:
            if 'Character' in key:
                print(f"Key: {key}")
        
        # Look for Phil specifically 
        # NamedCharacterType.PHIL is 2.
        # TaxonomyType.CHARACTER is often used.
        
        if 'namedCharacterDefinitions' in data:
            for char in data['namedCharacterDefinitions']:
                print(f"Named Char: {char.get('id')} - {char.get('name')}")
        
    except Exception as e:
        print(f"Error: {e}")
