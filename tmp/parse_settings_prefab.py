import re, sys

prefab_path = r'c:\Users\domin\Downloads\Unityproject\OpenMP\Minionsparadise\Assets\Resources\content\shared\shared_ui\ui_prefabs\ui_common\hud\screen_HUD_Panel_Settings_Menu.prefab'

with open(prefab_path, 'r', encoding='utf-8') as f:
    content = f.read()

# Parse into YAML documents separated by --- !u!
docs = re.split(r'^--- !u!\d+ &(\d+)', content, flags=re.MULTILINE)

# Build ID -> doc mapping
id_to_doc = {}
for i in range(1, len(docs)-1, 2):
    file_id = docs[i]
    body = docs[i+1]
    id_to_doc[file_id] = body

# Find all GameObjects (type 1) and extract their names and m_Component lists
go_names = {}  # fileID -> name
go_components = {}  # fileID -> list of component fileIDs

for fid, body in id_to_doc.items():
    if body.strip().startswith('GameObject:'):
        name_match = re.search(r'm_Name:\s*(.+)', body)
        if name_match:
            go_names[fid] = name_match.group(1).strip()
        # Extract component references
        comps = re.findall(r'component:\s*\{fileID:\s*(\d+)\}', body)
        go_components[fid] = comps

# For each component, find its owning GameObject
comp_to_go = {}
for go_fid, comp_list in go_components.items():
    for c in comp_list:
        comp_to_go[c] = go_fid

# Now find RectTransform components and show data for interesting ones
interesting = ['btn_dlc', 'btn_notification', 'btn_languageselector', 'nighttoggle', 
               'toggle_doubleconfirm', 'panel_options', 'panel_settings', 'panels',
               'screen_hud_panel_settings_menu']

for fid, body in id_to_doc.items():
    if 'RectTransform:' in body[:30]:
        # Which GO owns this?
        go_fid = comp_to_go.get(fid)
        if go_fid:
            go_name = go_names.get(go_fid, '???')
        else:
            # Try m_GameObject reference
            go_ref = re.search(r'm_GameObject:\s*\{fileID:\s*(\d+)\}', body)
            if go_ref:
                go_fid = go_ref.group(1)
                go_name = go_names.get(go_fid, '???')
            else:
                go_name = '???'
        
        if any(k in go_name.lower() for k in interesting):
            anchor_min = re.search(r'm_AnchorMin:\s*\{(.+?)\}', body)
            anchor_max = re.search(r'm_AnchorMax:\s*\{(.+?)\}', body)
            anchored_pos = re.search(r'm_AnchoredPosition:\s*\{(.+?)\}', body)
            size = re.search(r'm_SizeDelta:\s*\{(.+?)\}', body)
            pivot = re.search(r'm_Pivot:\s*\{(.+?)\}', body)
            print(f'=== {go_name} (RT id={fid}) ===')
            if anchor_min: print(f'  AnchorMin: {anchor_min.group(1)}')
            if anchor_max: print(f'  AnchorMax: {anchor_max.group(1)}')
            if anchored_pos: print(f'  AnchoredPos: {anchored_pos.group(1)}')
            if size: print(f'  SizeDelta: {size.group(1)}')
            if pivot: print(f'  Pivot: {pivot.group(1)}')
            print()
