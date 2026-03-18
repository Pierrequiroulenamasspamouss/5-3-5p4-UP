from flask import Flask
import threading
import logging

from routes.user import user_bp
from routes.game import game_bp
from routes.metrics import metrics_bp
from routes.sales import sales_bp

# Disable verbose logs
log = logging.getLogger('werkzeug')
log.setLevel(logging.ERROR)

def create_app(port):
    app = Flask(f"App_{port}")
    
    # CRITICAL: Keep JSON order intact
    app.config['JSON_SORT_KEYS'] = False

    # Register Blueprints
    app.register_blueprint(user_bp)
    app.register_blueprint(game_bp)
    app.register_blueprint(metrics_bp)
    app.register_blueprint(sales_bp)

    from flask import request
    @app.before_request
    def log_request_info():
        print(f"[HTTP IN] {request.method} {request.url}", flush=True)

    return app

def run_server(port):
    app = create_app(port)
    print(f">>> Server started on port {port}", flush=True)
    app.run(host='0.0.0.0', port=port, debug=False, threaded=True)

if __name__ == '__main__':
    # Using a single run for the main port with debug=True for auto-reload
    # and a separate thread for the other port. 
    # Flask's reloader only works well in the main thread.
    
    def run_secondary():
        # Secondary port without reloader to avoid conflicts
        app_sec = create_app(44732)
        print(">>> Secondary Server started on port 44732", flush=True)
        app_sec.run(host='0.0.0.0', port=44732, debug=False, threaded=True, use_reloader=False)

    t2 = threading.Thread(target=run_secondary, daemon=True)
    t2.start()

    # Main port with reloader
    app_main = create_app(44733)
    print(">>> Main Server started on port 44733 (with auto-reload)", flush=True)
    app_main.run(host='0.0.0.0', port=44733, debug=True, threaded=True, use_reloader=True)
# trigger reload 1773787109.4464662
