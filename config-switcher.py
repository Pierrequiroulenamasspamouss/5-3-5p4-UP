import os
import shutil
import tkinter as tk
from tkinter import messagebox

# === PATHS ===
BASE_DIR = os.path.dirname(os.path.abspath(__file__))

PATHS = [
    {
        "target": os.path.join(BASE_DIR, "Minionsparadise", "Assets", "Resources", "config.json"),
        "local": os.path.join(BASE_DIR, "Minionsparadise", "Assets", "Resources", "config.json.local"),
        "public": os.path.join(BASE_DIR, "Minionsparadise", "Assets", "Resources", "config.json.public"),
    },
    {
        "target": os.path.join(BASE_DIR, "SERVER", "config.json"),
        "local": os.path.join(BASE_DIR, "SERVER", "config.json.local"),
        "public": os.path.join(BASE_DIR, "SERVER", "config.json.public"),
    }
]

def replace_configs(mode):
    try:
        for path in PATHS:
            source = path[mode]
            target = path["target"]

            if not os.path.exists(source):
                raise FileNotFoundError(f"Missing file: {source}")

            # Remove existing config.json
            if os.path.exists(target):
                os.remove(target)

            # Copy selected version to config.json
            shutil.copy(source, target)

        messagebox.showinfo("Success", f"Switched to {mode.upper()} configuration!")

    except Exception as e:
        messagebox.showerror("Error", str(e))


# === GUI ===
root = tk.Tk()
root.title("Config Switcher")
root.geometry("300x150")

label = tk.Label(root, text="Select configuration:", font=("Arial", 12))
label.pack(pady=15)

btn_local = tk.Button(root, text="Local", width=15, command=lambda: replace_configs("local"))
btn_local.pack(pady=5)

btn_public = tk.Button(root, text="Public", width=15, command=lambda: replace_configs("public"))
btn_public.pack(pady=5)

root.mainloop()