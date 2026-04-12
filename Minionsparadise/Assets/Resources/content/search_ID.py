import os
import json

# --- CONFIGURATION ---
# Dossier racine où se trouvent tes fichiers JSON (Assets/Contents par exemple)
SEARCH_PATH = r'C:\Unity\LATEST\Minionsparadise\Assets' 
SEARCH_VALUES = ["1080", "70000", "phil"]

def search_in_json_files(directory, targets):
    print(f"--- Recherche de {targets} dans {directory} ---")
    results_found = 0
    
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.json'):
                file_path = os.path.join(root, file)
                try:
                    with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
                        content = f.read()
                        content_lower = content.lower()
                        
                        for target in targets:
                            if target.lower() in content_lower:
                                print(f"[FOUND] '{target}' trouvé dans : {file_path}")
                                # Optionnel : afficher la ligne
                                # lines = content.splitlines()
                                # for i, line in enumerate(lines):
                                #    if target.lower() in line.lower():
                                #        print(f"   Ligne {i+1}: {line.strip()}")
                                results_found += 1
                except Exception as e:
                    print(f"[ERROR] Impossible de lire {file_path}: {e}")

    print(f"\nRecherche terminée. {results_found} occurrences trouvées.")

if __name__ == "__main__":
    if os.path.exists(SEARCH_PATH):
        search_in_json_files(SEARCH_PATH, SEARCH_VALUES)
    else:
        print(f"Erreur : Le dossier {SEARCH_PATH} n'existe pas.")