import os

materials = [
    r"Minionsparadise/Assets/Resources/content/dlc/decor_lighttiki01/model/materials/Decor_LightTiki01_D.mat",
    r"Minionsparadise/Assets/Resources/content/dlc/decor_lighttiki02/model/materials/Decor_LightTiki02_D.mat",
    r"Minionsparadise/Assets/Resources/content/dlc/decor_lighttikired/model/materials/Decor_LightTikiRed_D.mat",
    r"Minionsparadise/Assets/Resources/content/dlc/decor_lighttikistarcircle/model/materials/Decor_LightTikiStarCircle_D.mat",
    r"Minionsparadise/Assets/Resources/content/dlc/decor_lighttikistarwrap/model/materials/Decor_LightTikiStarWrap_D.mat",
    r"Minionsparadise/Assets/Resources/content/dlc/decor_lighttikiwhite/model/materials/Decor_LightTikiWhite_D.mat"
]

base_path = r"c:\Unity\LATEST"

glow_entry = """      data:
        first:
          name: _NightGlow
        second: 1
"""

for mat_rel in materials:
    mat_path = os.path.join(base_path, mat_rel)
    if os.path.exists(mat_path):
        with open(mat_path, 'r') as f:
            lines = f.readlines()
        
        # Find m_Floats section
        insertion_point = -1
        for i, line in enumerate(lines):
            if 'm_Floats:' in line:
                insertion_point = i + 1
                break
        
        if insertion_point != -1:
            # Check if _NightGlow already exists
            already_exists = False
            for line in lines[insertion_point:]:
                if 'name: _NightGlow' in line:
                    already_exists = True
                    break
            
            if not already_exists:
                lines.insert(insertion_point, glow_entry)
                with open(mat_path, 'w') as f:
                    f.writelines(lines)
                print(f"Updated {mat_rel}")
            else:
                print(f"Skipping {mat_rel}, already has _NightGlow")
    else:
        print(f"File not found: {mat_path}")
