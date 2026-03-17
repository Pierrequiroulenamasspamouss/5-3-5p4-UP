import os
import shutil

# --- CONFIGURATION ---
# Remplace par le chemin absolu vers ton dossier Assets/Resources (ou autre)
TARGET_DIR = r'C:\Unity\LATEST\Minionsparadise\Assets\content' 

SUFFIX_SOURCE = "_MED"
SUFFIXES_TARGET = ["_LOW", "_VERYLOW"]
EXTENSION = ".prefab"
META_EXTENSION = ".prefab.meta"

def fix_missing_resolutions(root_folder):
    count = 0
    
    for root, dirs, files in os.walk(root_folder):
        for file in files:
            # On cherche uniquement les fichiers MED
            if file.endswith(SUFFIX_SOURCE + EXTENSION):
                base_name = file.replace(SUFFIX_SOURCE + EXTENSION, "")
                source_path = os.path.join(root, file)
                source_meta = source_path + ".meta"
                
                for suffix in SUFFIXES_TARGET:
                    target_file = base_name + suffix + EXTENSION
                    target_path = os.path.join(root, target_file)
                    target_meta = target_path + ".meta"
                    
                    # Si le prefab n'existe pas, on le crée
                    if not os.path.exists(target_path):
                        print(f"[COPY] Creating {target_file} from MED...")
                        shutil.copy2(source_path, target_path)
                        
                        # On copie aussi le .meta s'il existe pour éviter les erreurs d'import
                        if os.path.exists(source_meta):
                            shutil.copy2(source_meta, target_meta)
                        
                        count += 1
    
    print(f"\nTerminé ! {count} fichiers générés.")

if __name__ == "__main__":
    if os.path.exists(TARGET_DIR):
        fix_missing_resolutions(TARGET_DIR)
    else:
        print(f"Erreur : Le dossier {TARGET_DIR} est introuvable.")