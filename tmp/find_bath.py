import json

with open(r'c:\Users\domin\Downloads\Unityproject\OpenMP\SERVER\definitions.json', 'r', encoding='utf-8') as f:
    data = json.load(f)
    buildings = data.get('buildingDefinitions', [])
    for b in buildings:
        prefab = str(b.get('prefab'))
        key = str(b.get('localizedKey'))
        if 'hot' in prefab.lower() or 'tub' in prefab.lower() or 'bath' in prefab.lower() or 'hot' in key.lower() or 'tub' in key.lower() or 'bath' in key.lower():
            print(f"ID: {b.get('id')} prefab: {prefab} key: {key}")
