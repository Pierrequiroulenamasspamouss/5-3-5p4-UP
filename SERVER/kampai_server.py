from flask import Flask
import threading
import logging

from routes.user import user_bp
from routes.game import game_bp
from routes.metrics import metrics_bp

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
    t1 = threading.Thread(target=run_server, args=(44733,))
    t2 = threading.Thread(target=run_server, args=(44732,))
    t1.start()
    t2.start()
    t1.join()
    t2.join()