public class GoSplineQuadraticBezierSolver : AbstractGoSplineSolver
{
	public GoSplineQuadraticBezierSolver(global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes)
	{
		_nodes = nodes;
	}

	protected float quadBezierLength(global::UnityEngine.Vector3 startPoint, global::UnityEngine.Vector3 controlPoint, global::UnityEngine.Vector3 endPoint)
	{
		global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[2]
		{
			controlPoint - startPoint,
			startPoint - 2f * controlPoint + endPoint
		};
		if (array[1] != global::UnityEngine.Vector3.zero)
		{
			float num = 4f * global::UnityEngine.Vector3.Dot(array[1], array[1]);
			float num2 = 8f * global::UnityEngine.Vector3.Dot(array[0], array[1]);
			float num3 = 4f * global::UnityEngine.Vector3.Dot(array[0], array[0]);
			float num4 = 4f * num3 * num - num2 * num2;
			float num5 = 2f * num + num2;
			float num6 = num + num2 + num3;
			float num7 = 0.25f / num;
			float num8 = num4 / (8f * global::UnityEngine.Mathf.Pow(num, 1.5f));
			return num7 * (num5 * global::UnityEngine.Mathf.Sqrt(num6) - num2 * global::UnityEngine.Mathf.Sqrt(num3)) + num8 * (global::UnityEngine.Mathf.Log(2f * global::UnityEngine.Mathf.Sqrt(num * num6) + num5) - global::UnityEngine.Mathf.Log(2f * global::UnityEngine.Mathf.Sqrt(num * num3) + num2));
		}
		return 2f * array[0].magnitude;
	}

	public override void closePath()
	{
	}

	public override global::UnityEngine.Vector3 getPoint(float t)
	{
		float num = 1f - t;
		return num * num * _nodes[0] + 2f * num * t * _nodes[1] + t * t * _nodes[2];
	}

	public override void drawGizmos()
	{
		global::UnityEngine.Color color = global::UnityEngine.Gizmos.color;
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.red;
		global::UnityEngine.Gizmos.DrawLine(_nodes[0], _nodes[1]);
		global::UnityEngine.Gizmos.DrawLine(_nodes[1], _nodes[2]);
		global::UnityEngine.Gizmos.color = color;
	}
}
