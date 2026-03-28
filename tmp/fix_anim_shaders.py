import os

shaders = [
    r"c:\Unity\LATEST\Minionsparadise\Assets\Shader\AnimVert_wDistort.shader",
    r"c:\Unity\LATEST\Minionsparadise\Assets\Shader\AnimVert_wScroll.shader"
]

def update_shader(file_path):
    with open(file_path, 'r') as f:
        lines = f.readlines()
    
    modified = False
    
    # 1. Properties
    for i, line in enumerate(lines):
        if '_NightGlow' in line:
            break
        if '}' in line and i < 30: # Look for end of Properties block
            lines.insert(i, '        _NightGlow ("Night Glow", Range(0,1)) = 0\n')
            modified = True
            break
            
    # 2. CGINCLUDE / Include
    found_include = False
    for i, line in enumerate(lines):
        if 'KampaiNight.cginc' in line:
            found_include = True
            break
        if '#include "UnityCG.cginc"' in line:
            lines.insert(i + 1, '    #include "KampaiNight.cginc"\n')
            modified = True
            found_include = True
            break
            
    # 3. uniform / half _NightGlow
    found_uniform = False
    for i, line in enumerate(lines):
        if 'half _NightGlow;' in line:
            found_uniform = True
            break
        if 'half _FadeAlpha;' in line:
            lines.insert(i + 1, '    half _NightGlow;\n')
            modified = True
            found_uniform = True
            break
            
    # 4. Fragment logic for wDistort
    if "AnimVert_wDistort" in file_path:
        for i, line in enumerate(lines):
            if 'return lerp(baseColor, textureColor, blendAlpha);' in line:
                lines[i] = '                half4 finalColor = lerp(baseColor, textureColor, blendAlpha);\n'
                lines.insert(i + 1, '                finalColor.rgb = ApplyKampaiNight(finalColor.rgb, _NightGlow);\n')
                lines.insert(i + 2, '                return finalColor;\n')
                modified = True
                
    # 5. Fragment logic for wScroll
    if "AnimVert_wScroll" in file_path:
        for i, line in enumerate(lines):
            if 'return half4(finalRGB, i.color.a * _FadeAlpha);' in line:
                lines.insert(i, '                finalRGB = ApplyKampaiNight(finalRGB, _NightGlow);\n')
                modified = True

    if modified:
        with open(file_path, 'w') as f:
            f.writelines(lines)
        print(f"Updated {file_path}")
    else:
        print(f"No changes for {file_path}")

for s in shaders:
    update_shader(s)
