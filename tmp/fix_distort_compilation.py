file_path = r"c:\Unity\LATEST\Minionsparadise\Assets\Shader\AnimVert_wDistort.shader"
with open(file_path, 'r') as f:
    lines = f.readlines()

new_lines = []
skip = 0
for i, line in enumerate(lines):
    if skip > 0:
        skip -= 1
        continue
    
    # Identify the start of the fragment return logic
    if 'half4 baseColor = half4(0.5, 0.5, 0.5, 0.0);' in line:
        # Search ahead for the return
        j = i + 1
        found_return = False
        while j < len(lines) and not found_return:
            if 'return' in lines[j] and 'lerp' in lines[j]:
                found_return = True
                break
            j += 1
            
        if found_return:
            # We found the block. Re-generate it correctly.
            new_lines.append('                half4 baseColor = half4(0.5, 0.5, 0.5, 0.0);\n')
            new_lines.append('                half4 textureColor = texBlue * i.color;\n')
            new_lines.append('                half blendAlpha = i.color.a * _FadeAlpha;\n')
            new_lines.append('\n')
            new_lines.append('                // --- Night Mode Injection (Prior to blend) ---\n')
            new_lines.append('                textureColor.rgb = ApplyKampaiNight(textureColor.rgb, _NightGlow);\n')
            new_lines.append('\n')
            new_lines.append('                return lerp(baseColor, textureColor, blendAlpha);\n')
            skip = j - i
            continue

    new_lines.append(line)

with open(file_path, 'w') as f:
    f.writelines(new_lines)
print("Fixed both passes in AnimVert_wDistort.shader")
