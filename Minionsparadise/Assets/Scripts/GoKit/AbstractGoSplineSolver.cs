public abstract class AbstractGoSplineSolver
{
	protected struct Segment
	{
		public float time;

		public float distance;

		public Segment(float time, float distance)
		{
			this.time = time;
			this.distance = distance;
		}
	}

	protected global::System.Collections.Generic.List<global::UnityEngine.Vector3> _nodes;

	protected float _pathLength;

	protected int totalSubdivisionsPerNodeForLookupTable = 5;

	protected global::System.Collections.Generic.List<AbstractGoSplineSolver.Segment> segments;

	public global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes
	{
		get
		{
			return _nodes;
		}
	}

	public float pathLength
	{
		get
		{
			return _pathLength;
		}
	}

	public virtual void buildPath()
	{
		int num = _nodes.Count * totalSubdivisionsPerNodeForLookupTable;
		if (segments == null)
		{
			segments = new global::System.Collections.Generic.List<AbstractGoSplineSolver.Segment>(num);
		}
		else
		{
			segments.Clear();
			segments.Capacity = num;
		}
		_pathLength = 0f;
		float num2 = 1f / (float)num;
		global::UnityEngine.Vector3 b = getPoint(0f);
		for (int i = 1; i < num + 1; i++)
		{
			float num3 = num2 * (float)i;
			global::UnityEngine.Vector3 point = getPoint(num3);
			_pathLength += global::UnityEngine.Vector3.Distance(point, b);
			b = point;
			segments.Add(new AbstractGoSplineSolver.Segment(num3, _pathLength));
		}
	}

	public abstract void closePath();

	public abstract global::UnityEngine.Vector3 getPoint(float t);

	public virtual global::UnityEngine.Vector3 getPointOnPath(float t)
	{
		float num = _pathLength * t;
		int i;
		for (i = 0; i < segments.Count && !(segments[i].distance >= num); i++)
		{
		}
		AbstractGoSplineSolver.Segment segment = segments[i];
		if (i == 0)
		{
			t = num / segment.distance * segment.time;
		}
		else
		{
			AbstractGoSplineSolver.Segment segment2 = segments[i - 1];
			float num2 = segment.time - segment2.time;
			float num3 = segment.distance - segment2.distance;
			t = segment2.time + (num - segment2.distance) / num3 * num2;
		}
		return getPoint(t);
	}

	public void reverseNodes()
	{
		_nodes.Reverse();
	}

	public virtual void drawGizmos()
	{
	}
}
