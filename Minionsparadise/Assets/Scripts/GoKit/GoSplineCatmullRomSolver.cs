public class GoSplineCatmullRomSolver : AbstractGoSplineSolver
{
	public GoSplineCatmullRomSolver(global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes)
	{
		_nodes = nodes;
	}

	public override void closePath()
	{
		_nodes.RemoveAt(0);
		_nodes.RemoveAt(_nodes.Count - 1);
		if (_nodes[0] != _nodes[_nodes.Count - 1])
		{
			_nodes.Add(_nodes[0]);
		}
		float num = global::UnityEngine.Vector3.Distance(_nodes[0], _nodes[1]);
		float num2 = global::UnityEngine.Vector3.Distance(_nodes[0], _nodes[_nodes.Count - 2]);
		float num3 = num2 / global::UnityEngine.Vector3.Distance(_nodes[1], _nodes[0]);
		global::UnityEngine.Vector3 item = _nodes[0] + (_nodes[1] - _nodes[0]) * num3;
		float num4 = num / global::UnityEngine.Vector3.Distance(_nodes[_nodes.Count - 2], _nodes[0]);
		global::UnityEngine.Vector3 item2 = _nodes[0] + (_nodes[_nodes.Count - 2] - _nodes[0]) * num4;
		_nodes.Insert(0, item2);
		_nodes.Add(item);
	}

	public override global::UnityEngine.Vector3 getPoint(float t)
	{
		int num = _nodes.Count - 3;
		int num2 = global::UnityEngine.Mathf.Min(global::UnityEngine.Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		global::UnityEngine.Vector3 vector = _nodes[num2];
		global::UnityEngine.Vector3 vector2 = _nodes[num2 + 1];
		global::UnityEngine.Vector3 vector3 = _nodes[num2 + 2];
		global::UnityEngine.Vector3 vector4 = _nodes[num2 + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
	}

	public override void drawGizmos()
	{
		if (_nodes.Count >= 2)
		{
			global::UnityEngine.Color color = global::UnityEngine.Gizmos.color;
			global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(1f, 1f, 1f, 0.3f);
			global::UnityEngine.Gizmos.DrawLine(_nodes[0], _nodes[1]);
			global::UnityEngine.Gizmos.DrawLine(_nodes[_nodes.Count - 1], _nodes[_nodes.Count - 2]);
			global::UnityEngine.Gizmos.color = color;
		}
	}
}
