import os

shaders = [
    r"c:\Unity\LATEST\Minionsparadise\Assets\Shader\Unlit_minusOffset1.shader",
    r"c:\Unity\LATEST\Minionsparadise\Assets\Shader\Unlit_minusOffset10.shader",
    r"c:\Unity\LATEST\Minionsparadise\Assets\Shader\Unlit_plusOffset1.shader",
    r"c:\Unity\LATEST\Minionsparadise\Assets\Shader\Unlit_plusOffset10.shader"
]

def update_offset_shader(file_path):
    with open(file_path, 'r') as f:
        lines = f.readlines()
    
    modified = False
    
    # 1. Add _NightGlow property
    prop_found = False
    for i, line in enumerate(lines):
        if '_NightGlow' in line:
            prop_found = True
            break
        if '_TransparencyLM' in line:
            lines.insert(i + 1, '        _NightGlow ("Night Glow", Range(0,1)) = 0\n')
            modified = True
            prop_found = True
            break
            
    # 2. Add include
    include_found = False
    for i, line in enumerate(lines):
        if 'KampaiNight.cginc' in line:
            include_found = True
            break
        if '#include "AutoLight.cginc"' in line:
            lines.insert(i + 1, '            #include "KampaiNight.cginc"\n')
            modified = True
            include_found = True
            break
            
    # 3. Add half _NightGlow
    var_found = False
    for i, line in enumerate(lines):
        if 'half _NightGlow;' in line:
            var_found = True
            break
        if 'half _VertexColor;' in line:
            lines.insert(i + 1, '            half _NightGlow;\n')
            modified = True
            var_found = True
            break
            
    # 4. Apply ApplyKampaiNight in frag
    frag_modified = False
    for i, line in enumerate(lines):
        if 'return half4(finalRGB, finalAlpha * _FadeAlpha);' in line:
            # Check if already applied
            if 'ApplyKampaiNight' in lines[i-1] or 'ApplyKampaiNight' in lines[i-2]:
                frag_modified = True
                break
            lines.insert(i, '\n                // --- Night Mode Injection ---\n')
            lines.insert(i + 1, '                finalRGB = ApplyKampaiNight(finalRGB, _NightGlow);\n')
            modified = True
            frag_modified = True
            break

    if modified:
        with open(file_path, 'w') as f:
            f.writelines(lines)
        print(f"Updated {file_path}")
    else:
        print(f"No changes for {file_path}")

for s in shaders:
    update_offset_shader(s)
