import json

with open(r'c:\Users\domin\Downloads\Unityproject\OpenMP\SERVER\definitions.json', 'r', encoding='utf-8') as f:
    data = json.load(f)
    buildings = data.get('buildingDefinitions', [])
    for b in buildings:
        print(f"ID: {b.get('id')} prefab: {b.get('prefab')} key: {b.get('localizedKey')}")
