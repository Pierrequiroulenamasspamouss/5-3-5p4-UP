import json

with open(r'c:\Users\domin\Downloads\Unityproject\OpenMP\SERVER\definitions.json', 'r') as f:
    data = json.load(f)
    chars = data.get('namedCharacterDefinitions', [])
    phil = next((c for c in chars if c.get('id') == 70000), None)
    print(json.dumps(phil, indent=2))
