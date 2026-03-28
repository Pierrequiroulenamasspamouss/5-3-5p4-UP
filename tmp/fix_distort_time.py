file_path = r"c:\Unity\LATEST\Minionsparadise\Assets\Shader\AnimVert_wDistort.shader"
with open(file_path, 'r') as f:
    content = f.read()

# Pattern for faster animation and enabling night mode
content = content.replace("float timeVar = frac(_Time.x * 3.0);", "float timeVar = frac(_Time.y * 0.2);")
content = content.replace("// textureColor.rgb = ApplyKampaiNight(textureColor.rgb, _NightGlow);", "textureColor.rgb = ApplyKampaiNight(textureColor.rgb, _NightGlow);")

with open(file_path, 'w') as f:
    f.write(content)
print("Updated AnimVert_wDistort.shader to use _Time.y and night tint")
