import re

prefab_path = r'c:\Users\domin\Downloads\Unityproject\OpenMP\Minionsparadise\Assets\Resources\content\shared\shared_ui\ui_prefabs\ui_common\hud\screen_HUD_Panel_Settings_Menu.prefab'
with open(prefab_path, 'r', encoding='utf-8') as f:
    content = f.read()

docs = re.split(r'^--- !u!\d+ &(\d+)', content, flags=re.MULTILINE)
id_to_doc = {}
for i in range(1, len(docs)-1, 2):
    id_to_doc[docs[i]] = docs[i+1]

go_names = {}
for fid, body in id_to_doc.items():
    if body.strip().startswith('GameObject:'):
        name_match = re.search(r'm_Name:\s*(.+)', body)
        if name_match:
            go_names[fid] = name_match.group(1).strip()

comp_to_go = {}
for fid, body in id_to_doc.items():
    if body.strip().startswith('GameObject:'):
        comps = re.findall(r'fileID: (\d+)', body)
        for c in comps:
            comp_to_go[c] = fid

# Find RectTransforms for tab-related objects
targets = ['btn_Settings', 'btn_Help', 'panel_Settings', 'panel_Help', 'panel_About', 'panels']
for fid, body in id_to_doc.items():
    if 'RectTransform:' in body[:30]:
        go_ref = re.search(r'm_GameObject:\s*\{fileID:\s*(\d+)\}', body)
        if go_ref:
            go_fid = go_ref.group(1)
            go_name = go_names.get(go_fid, '???')
            if go_name in targets:
                anchor_min = re.search(r'm_AnchorMin:\s*\{(.+?)\}', body)
                anchor_max = re.search(r'm_AnchorMax:\s*\{(.+?)\}', body)
                anchored_pos = re.search(r'm_AnchoredPosition:\s*\{(.+?)\}', body)
                size = re.search(r'm_SizeDelta:\s*\{(.+?)\}', body)
                parent = re.search(r'm_Father:\s*\{fileID:\s*(\d+)\}', body)
                parent_go = '???'
                if parent:
                    p_fid = parent.group(1)
                    p_go_ref = comp_to_go.get(p_fid)
                    if p_go_ref:
                        parent_go = go_names.get(p_go_ref, '???')
                    else:
                        # Try the reverse - the parent RT's GO
                        if p_fid in id_to_doc:
                            p_body = id_to_doc[p_fid]
                            p_go_ref2 = re.search(r'm_GameObject:\s*\{fileID:\s*(\d+)\}', p_body)
                            if p_go_ref2:
                                parent_go = go_names.get(p_go_ref2.group(1), '???')

                amin = anchor_min.group(1) if anchor_min else '?'
                amax = anchor_max.group(1) if anchor_max else '?'
                apos = anchored_pos.group(1) if anchored_pos else '?'
                sz = size.group(1) if size else '?'
                print("=== {} (RT id={}) ===".format(go_name, fid))
                print("  Parent: {}".format(parent_go))
                print("  AnchorMin: {}".format(amin))
                print("  AnchorMax: {}".format(amax))
                print("  AnchoredPos: {}".format(apos))
                print("  SizeDelta: {}".format(sz))
                print()
