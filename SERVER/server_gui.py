import tkinter as tk
from tkinter import ttk, messagebox, scrolledtext
import os
import json
import time
import glob
from threading import Thread
import subprocess

# Paths (Relative to the server directory)
SERVER_DIR = os.path.dirname(os.path.abspath(__file__))
PLAYER_DATA_DIR = os.path.join(SERVER_DIR, 'player_data')
DEFINITIONS_PATH = os.path.join(SERVER_DIR, 'definitions.json')
DEV_DEFINITIONS_PATH = os.path.join(SERVER_DIR, 'dev_definitions.json')

class ServerGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("Kampai Server Manager")
        self.root.geometry("1100x850")
        self.root.configure(bg="#2c3e50")

        self.style = ttk.Style()
        self.style.theme_use('clam')
        self.style.configure("TFrame", background="#2c3e50")
        self.style.configure("TLabel", background="#2c3e50", foreground="#ecf0f1", font=("Segoe UI", 10))
        self.style.configure("Header.TLabel", font=("Segoe UI", 14, "bold"), foreground="#3498db")
        self.style.configure("TButton", font=("Segoe UI", 10))
        self.style.configure("TEntry", font=("Consolas", 10))
        self.style.configure("TNotebook", background="#2c3e50")
        self.style.configure("TNotebook.Tab", font=("Segoe UI", 10, "bold"))

        self.setup_tabs()
        
        # Periodic Refresh
        self.refresh_stats()
        self.auto_refresh()

    def setup_tabs(self):
        self.notebook = ttk.Notebook(self.root)
        self.notebook.pack(fill="both", expand=True, padx=10, pady=10)

        # Tab 1: Dashboard
        self.dash_tab = ttk.Frame(self.notebook)
        self.notebook.add(self.dash_tab, text=" 📊 Dashboard ")
        self.setup_dashboard()

        # Tab 2: Player Editor
        self.editor_tab = ttk.Frame(self.notebook)
        self.notebook.add(self.editor_tab, text=" 👤 Player Editor ")
        self.setup_player_editor()

        # Tab 3: Terminal
        self.terminal_tab = ttk.Frame(self.notebook)
        self.notebook.add(self.terminal_tab, text=" 💻 Terminal ")
        self.setup_terminal()

    def setup_dashboard(self):
        frame = ttk.Frame(self.dash_tab, padding=20)
        frame.pack(fill="both", expand=True)

        ttk.Label(frame, text="Server Statistics", style="Header.TLabel").grid(row=0, column=0, sticky="w", pady=10)

        self.online_label = ttk.Label(frame, text="Online/Active Players (5m): 0", font=("Segoe UI", 12))
        self.online_label.grid(row=1, column=0, sticky="w", pady=5)

        self.total_label = ttk.Label(frame, text="Total Registered Players: 0", font=("Segoe UI", 12))
        self.total_label.grid(row=2, column=0, sticky="w", pady=5)

        ttk.Label(frame, text="Recent Logins (Activity):", style="Header.TLabel").grid(row=3, column=0, sticky="w", pady=10)

        self.login_listbox = tk.Listbox(frame, width=80, height=20, bg="#34495e", fg="#ecf0f1", font=("Consolas", 10), borderwidth=0, highlightthickness=1, highlightbackground="#3498db")
        self.login_listbox.grid(row=4, column=0, sticky="nsew", pady=5)

        # Server Control
        ctrl_frame = ttk.LabelFrame(frame, text=" Server Control ", padding=10)
        ctrl_frame.grid(row=0, column=1, rowspan=5, sticky="ne", padx=20)

        self.server_status_var = tk.StringVar(value="Server: Stopped")
        ttk.Label(ctrl_frame, textvariable=self.server_status_var, font=("Segoe UI", 12, "bold"), foreground="#e67e22").pack(pady=10)
        
        ttk.Button(ctrl_frame, text="Start Server", command=self.start_server_process, width=20).pack(pady=5)
        ttk.Button(ctrl_frame, text="Stop Server", command=self.stop_server_process, width=20).pack(pady=5)
        ttk.Separator(ctrl_frame, orient="horizontal").pack(fill="x", pady=10)
        ttk.Button(ctrl_frame, text="Refresh Dashboard", command=self.refresh_stats, width=20).pack(pady=5)

    def setup_player_editor(self):
        frame = ttk.Frame(self.editor_tab, padding=20)
        frame.pack(fill="both", expand=True)

        # Selection Panel
        sel_frame = ttk.Frame(frame)
        sel_frame.pack(fill="x", pady=5)

        ttk.Label(sel_frame, text="Select Player ID:").pack(side="left", padx=5)
        self.player_selector = ttk.Combobox(sel_frame, width=30)
        self.player_selector.pack(side="left", padx=5)
        
        load_btn = ttk.Button(sel_frame, text="Load Data", command=self.load_player_data)
        load_btn.pack(side="left", padx=5)

        refresh_list_btn = ttk.Button(sel_frame, text="Refresh List", command=self.refresh_player_list)
        refresh_list_btn.pack(side="left", padx=5)

        # Editor Panel
        self.edit_frame = ttk.LabelFrame(frame, text=" Player Attributes ", padding=20)
        self.edit_frame.pack(fill="both", expand=True, pady=10)

        # Variables for fields
        self.player_fields = {}
        self.create_field("Player ID", "id")
        self.create_field("XP Level (highestFtueLevel)", "highestFtueLevel")
        self.create_field("Currency 0 (Sand Dollars)", "currency0")
        self.create_field("Currency 1 (Doubloons)", "currency1")

        save_btn = ttk.Button(frame, text="Save Changes", command=self.save_player_data, width=30)
        save_btn.pack(pady=10)

        self.refresh_player_list()

    def create_field(self, label, key):
        row = len(self.player_fields)
        ttk.Label(self.edit_frame, text=f"{label}:").grid(row=row, column=0, sticky="e", padx=5, pady=10)
        var = tk.StringVar()
        entry = ttk.Entry(self.edit_frame, textvariable=var, width=60)
        entry.grid(row=row, column=1, sticky="w", padx=5, pady=10)
        self.player_fields[key] = (var, entry)

    def setup_terminal(self):
        frame = ttk.Frame(self.terminal_tab, padding=20)
        frame.pack(fill="both", expand=True)

        self.term_output = scrolledtext.ScrolledText(frame, bg="#1e272e", fg="#d2dae2", font=("Consolas", 10), insertbackground="white")
        self.term_output.pack(fill="both", expand=True, pady=5)

        entry_frame = ttk.Frame(frame)
        entry_frame.pack(fill="x", pady=5)

        ttk.Label(entry_frame, text=">").pack(side="left", padx=5)
        self.term_input = ttk.Entry(entry_frame, font=("Consolas", 10))
        self.term_input.pack(side="left", fill="x", expand=True, padx=5)
        self.term_input.bind("<Return>", self.execute_command)

        send_btn = ttk.Button(entry_frame, text="Execute", command=self.execute_command)
        send_btn.pack(side="left", padx=5)

        self.log_terminal("Kampai Server Manager GUI v1.0 Initialized.")
        self.log_terminal("Read-to-use. Click 'Start Server' to begin monitoring.")

    # --- Server Process Management ---
    def start_server_process(self):
        if hasattr(self, 'server_proc') and self.server_proc.poll() is None:
            self.log_terminal("Server is already running.")
            return
        
        def run():
            try:
                # Use the virtual environment if available
                python_exe = os.path.join(SERVER_DIR, "venv", "Scripts", "python.exe")
                if not os.path.exists(python_exe):
                    python_exe = "python"
                
                self.server_proc = subprocess.Popen(
                    [python_exe, "kampai_server.py"],
                    cwd=SERVER_DIR,
                    stdout=subprocess.PIPE,
                    stderr=subprocess.STDOUT,
                    text=True,
                    bufsize=1,
                    creationflags=subprocess.CREATE_NO_WINDOW if os.name == 'nt' else 0
                )
                self.server_status_var.set("Server: Running")
                self.log_terminal(">>> Server process started.")
                
                for line in iter(self.server_proc.stdout.readline, ""):
                    if line:
                        self.log_terminal(f"[SERVER] {line.strip()}")
                
                self.server_proc.stdout.close()
                self.server_proc.wait()
                self.server_status_var.set("Server: Stopped")
                self.log_terminal(">>> Server process terminated.")
            except Exception as e:
                self.log_terminal(f"Error starting server: {e}")
                self.server_status_var.set("Server: Error")

        Thread(target=run, daemon=True).start()

    def stop_server_process(self):
        if hasattr(self, 'server_proc') and self.server_proc.poll() is None:
            # On Windows, taskkill might be cleaner for Flask with subprocesses
            subprocess.run(["taskkill", "/F", "/T", "/PID", str(self.server_proc.pid)], capture_output=True)
            self.log_terminal("Server stopped via taskkill.")
        else:
            self.log_terminal("Server is not running.")

    # --- Logic Helpers ---
    def auto_refresh(self):
        self.refresh_stats()
        self.root.after(30000, self.auto_refresh) # Every 30 seconds

    def log_terminal(self, msg):
        self.term_output.insert(tk.END, f"[{time.strftime('%H:%M:%S')}] {msg}\n")
        self.term_output.see(tk.END)

    def refresh_stats(self):
        if not os.path.exists(PLAYER_DATA_DIR):
            return

        files = glob.glob(os.path.join(PLAYER_DATA_DIR, "*.json"))
        self.total_label.config(text=f"Total Registered Players: {len(files)}")

        now = time.time()
        active_count = 0
        recent_logins = []

        for f in files:
            mtime = os.path.getmtime(f)
            if now - mtime < 300: # 5 minutes
                active_count += 1
            
            player_id = os.path.basename(f).replace(".json", "")
            recent_logins.append((mtime, player_id))

        self.online_label.config(text=f"Online/Active Players (5m): {active_count}")

        recent_logins.sort(reverse=True)
        self.login_listbox.delete(0, tk.END)
        for mtime, pid in recent_logins[:50]:
            timestr = time.strftime('%H:%M:%S', time.localtime(mtime))
            self.login_listbox.insert(tk.END, f"[{timestr}] Player ID: {pid}")

    def refresh_player_list(self):
        if not os.path.exists(PLAYER_DATA_DIR):
            return
        files = glob.glob(os.path.join(PLAYER_DATA_DIR, "*.json"))
        player_ids = [os.path.basename(f).replace(".json", "") for f in files]
        self.player_selector['values'] = sorted(player_ids, key=lambda x: int(x) if x.isdigit() else 0)

    def load_player_data(self):
        pid = self.player_selector.get()
        if not pid: return
        
        path = os.path.join(PLAYER_DATA_DIR, f"{pid}.json")
        try:
            with open(path, 'r', encoding='utf-8') as f:
                data = json.load(f)
            
            self.player_fields['id'][0].set(data.get('id', data.get('ID', pid)))
            self.player_fields['highestFtueLevel'][0].set(str(data.get('highestFtueLevel', '0')))
            
            inv = data.get('inventory', [])
            cur0, cur1 = 0, 0
            for item in inv:
                d_id = item.get('def', item.get('Definition', -1))
                if d_id == 0: cur0 = item.get('quantity', item.get('Quantity', 0))
                elif d_id == 1: cur1 = item.get('quantity', item.get('Quantity', 0))
            
            self.player_fields['currency0'][0].set(str(cur0))
            self.player_fields['currency1'][0].set(str(cur1))
            self.current_player_data = data
            self.log_terminal(f"Loaded player {pid}")
        except Exception as e:
            messagebox.showerror("Error", f"Load failed: {e}")

    def save_player_data(self):
        if not hasattr(self, 'current_player_data'):
            messagebox.showwarning("Warning", "Load a player first.")
            return
        
        pid = self.player_selector.get()
        path = os.path.join(PLAYER_DATA_DIR, f"{pid}.json")
        
        try:
            new_level = int(self.player_fields['highestFtueLevel'][0].get())
            new_cur0 = int(self.player_fields['currency0'][0].get())
            new_cur1 = int(self.player_fields['currency1'][0].get())
            
            self.current_player_data['highestFtueLevel'] = new_level
            inv = self.current_player_data.get('inventory', [])
            
            def update_item(def_id, qty):
                for item in inv:
                    if item.get('def') == def_id or item.get('Definition') == def_id:
                        if 'quantity' in item: item['quantity'] = qty
                        if 'Quantity' in item: item['Quantity'] = qty
                        return True
                return False

            if not update_item(0, new_cur0): inv.append({"id": 0, "def": 0, "quantity": new_cur0})
            if not update_item(1, new_cur1): inv.append({"id": 1, "def": 1, "quantity": new_cur1})
            
            with open(path, 'w', encoding='utf-8') as f:
                json.dump(self.current_player_data, f, indent=2)
            
            messagebox.showinfo("Success", f"Player {pid} updated.")
            self.log_terminal(f"Saved changes for {pid}")
            self.refresh_stats()
        except Exception as e:
            messagebox.showerror("Error", f"Save failed: {e}")

    def execute_command(self, event=None):
        cmd = self.term_input.get().strip()
        if not cmd: return
        self.log_terminal(f"> {cmd}")
        self.term_input.delete(0, tk.END)
        if cmd == "help": self.log_terminal("Commands: help, status, clear, ls-players")
        elif cmd == "clear": self.term_output.delete('1.0', tk.END)
        elif cmd == "status": self.refresh_stats(); self.log_terminal("Dashboard refreshed.")
        elif cmd == "ls-players": 
            files = glob.glob(os.path.join(PLAYER_DATA_DIR, "*.json"))
            self.log_terminal(f"Players: {[os.path.basename(f).replace('.json','') for f in files]}")
        else: self.log_terminal(f"Unknown command: {cmd}")

if __name__ == "__main__":
    root = tk.Tk()
    app = ServerGUI(root)
    root.mainloop()
