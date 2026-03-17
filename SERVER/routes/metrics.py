from flask import Blueprint, jsonify

metrics_bp = Blueprint('metrics', __name__)

@metrics_bp.route('/persistence', methods=['POST', 'GET'])
def persistence_fallback():
    return jsonify({"status": "success", "digest": "mock"})

@metrics_bp.route('/rest/telemetry', methods=['POST'])
@metrics_bp.route('/metrics', methods=['POST'])
def telemetry():
    return jsonify({"status": "ok"})

@metrics_bp.route('/rest/healthMetrics/', defaults={'subpath': ''}, methods=['POST', 'GET'])
@metrics_bp.route('/rest/healthMetrics/<path:subpath>', methods=['POST', 'GET'])
def health_metrics(subpath):
    return jsonify({"status": "ok"})

@metrics_bp.route('/rest/social/', defaults={'subpath': ''}, methods=['GET', 'POST'])
@metrics_bp.route('/rest/social/<path:subpath>', methods=['GET', 'POST'])
def social_stub(subpath):
    return jsonify({"status": "ok", "data": []})
