file_path = r"c:\Unity\LATEST\Minionsparadise\Assets\Shader\AnimVert_wDistort.shader"
with open(file_path, 'r') as f:
    content = f.read()

# Pattern for the fragment return
old_block_1 = """                half4 textureColor = texBlue * i.color;
                
                // --- Mélange final avec l'Alpha dynamique ---
                half blendAlpha = i.color.a * _FadeAlpha;
                half4 finalColor = lerp(baseColor, textureColor, blendAlpha);
                finalColor.rgb = ApplyKampaiNight(finalColor.rgb, _NightGlow);
                return finalColor;"""

new_block_1 = """                half4 textureColor = texBlue * i.color;
                
                half blendAlpha = i.color.a * _FadeAlpha;
                
                // --- Night Mode Injection (Prior to blend) ---
                textureColor.rgb = ApplyKampaiNight(textureColor.rgb, _NightGlow);
                
                return lerp(baseColor, textureColor, blendAlpha);"""

content = content.replace(old_block_1, new_block_1)

# Check if there's another one (the second subshader was already partially fixed by multi_replace maybe)
# Actually, I'll just check for any remaining 'finalColor.rgb = ApplyKampaiNight(finalColor.rgb, _NightGlow);'

with open(file_path, 'w') as f:
    f.write(content)
print("Updated AnimVert_wDistort.shader via script")
