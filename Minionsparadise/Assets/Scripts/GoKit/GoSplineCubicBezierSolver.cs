public class GoSplineCubicBezierSolver : AbstractGoSplineSolver
{
	public GoSplineCubicBezierSolver(global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes)
	{
		_nodes = nodes;
	}

	public override void closePath()
	{
	}

	public override global::UnityEngine.Vector3 getPoint(float t)
	{
		float num = 1f - t;
		return num * num * num * _nodes[0] + 3f * num * num * t * _nodes[1] + 3f * num * t * t * _nodes[2] + t * t * t * _nodes[3];
	}

	public override void drawGizmos()
	{
		global::UnityEngine.Color color = global::UnityEngine.Gizmos.color;
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.red;
		global::UnityEngine.Gizmos.DrawLine(_nodes[0], _nodes[1]);
		global::UnityEngine.Gizmos.DrawLine(_nodes[2], _nodes[3]);
		global::UnityEngine.Gizmos.color = color;
	}
}
