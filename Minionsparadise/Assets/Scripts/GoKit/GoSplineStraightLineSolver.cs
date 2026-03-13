public class GoSplineStraightLineSolver : AbstractGoSplineSolver
{
	private global::System.Collections.Generic.Dictionary<int, float> _segmentStartLocations;

	private global::System.Collections.Generic.Dictionary<int, float> _segmentDistances;

	private int _currentSegment;

	public GoSplineStraightLineSolver(global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes)
	{
		_nodes = nodes;
	}

	public override void buildPath()
	{
		if (_nodes.Count >= 3)
		{
			_segmentStartLocations = new global::System.Collections.Generic.Dictionary<int, float>(_nodes.Count - 2);
			_segmentDistances = new global::System.Collections.Generic.Dictionary<int, float>(_nodes.Count - 1);
			for (int i = 0; i < _nodes.Count - 1; i++)
			{
				float num = global::UnityEngine.Vector3.Distance(_nodes[i], _nodes[i + 1]);
				_segmentDistances.Add(i, num);
				_pathLength += num;
			}
			float num2 = 0f;
			for (int j = 0; j < _segmentDistances.Count - 1; j++)
			{
				num2 += _segmentDistances[j];
				_segmentStartLocations.Add(j + 1, num2 / _pathLength);
			}
		}
	}

	public override void closePath()
	{
		if (_nodes[0] != _nodes[_nodes.Count - 1])
		{
			_nodes.Add(_nodes[0]);
		}
	}

	public override global::UnityEngine.Vector3 getPoint(float t)
	{
		return getPointOnPath(t);
	}

	public override global::UnityEngine.Vector3 getPointOnPath(float t)
	{
		if (_nodes.Count < 3)
		{
			return global::UnityEngine.Vector3.Lerp(_nodes[0], _nodes[1], t);
		}
		int[] array = new int[_segmentStartLocations.Keys.Count];
		_segmentStartLocations.Keys.CopyTo(array, 0);
		_currentSegment = 0;
		foreach (int num in array)
		{
			float num2 = _segmentStartLocations[num];
			if (!(num2 < t))
			{
				break;
			}
			_currentSegment = num;
		}
		float num3 = t * _pathLength;
		for (int num4 = _currentSegment - 1; num4 >= 0; num4--)
		{
			num3 -= _segmentDistances[num4];
		}
		return global::UnityEngine.Vector3.Lerp(_nodes[_currentSegment], _nodes[_currentSegment + 1], num3 / _segmentDistances[_currentSegment]);
	}
}
