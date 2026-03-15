public class PositionPathTweenProperty : AbstractTweenProperty
{
	protected bool _useLocalPosition;

	protected global::UnityEngine.Transform _target;

	protected global::UnityEngine.Vector3 _startValue;

	private GoSpline _path;

	private GoLookAtType _lookAtType;

	private global::UnityEngine.Transform _lookTarget;

	private GoSmoothedQuaternion _smoothedRotation;

	public bool useLocalPosition
	{
		get
		{
			return _useLocalPosition;
		}
	}

	public PositionPathTweenProperty(GoSpline path, bool isRelative = false, bool useLocalPosition = false, GoLookAtType lookAtType = GoLookAtType.None, global::UnityEngine.Transform lookTarget = null)
		: base(isRelative)
	{
		_path = path;
		_useLocalPosition = useLocalPosition;
		_lookAtType = lookAtType;
		_lookTarget = lookTarget;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (base.Equals(obj))
		{
			return _useLocalPosition == ((PositionPathTweenProperty)obj)._useLocalPosition;
		}
		PositionTweenProperty positionTweenProperty = obj as PositionTweenProperty;
		if (positionTweenProperty != null)
		{
			return _useLocalPosition == positionTweenProperty.useLocalPosition;
		}
		return false;
	}

	public override void prepareForUse()
	{
		_target = _ownerTween.target as global::UnityEngine.Transform;
		if (_ownerTween.isFrom)
		{
			_path.reverseNodes();
		}
		else
		{
			_path.unreverseNodes();
		}
		_path.buildPath();
		if (_ownerTween.isFrom)
		{
			_startValue = _path.getLastNode();
		}
		else if (_useLocalPosition)
		{
			_startValue = _target.localPosition;
		}
		else
		{
			_startValue = _target.position;
		}
		if (_lookAtType == GoLookAtType.TargetTransform && _lookTarget == null)
		{
			_lookAtType = GoLookAtType.None;
		}
		_smoothedRotation = _target.rotation;
	}

	public override void tick(float totalElapsedTime)
	{
		float t = _easeFunction(totalElapsedTime, 0f, 1f, _ownerTween.duration);
		global::UnityEngine.Vector3 pointOnPath = _path.getPointOnPath(t);
		if (_isRelative)
		{
			pointOnPath += _startValue;
		}
		switch (_lookAtType)
		{
		case GoLookAtType.NextPathNode:
			_smoothedRotation.smoothValue = (pointOnPath.Equals(_target.position) ? global::UnityEngine.Quaternion.identity : global::UnityEngine.Quaternion.LookRotation(pointOnPath - _target.position));
			_target.rotation = _smoothedRotation.smoothValue;
			break;
		case GoLookAtType.TargetTransform:
			_target.LookAt(_lookTarget, global::UnityEngine.Vector3.up);
			break;
		}
		if (_useLocalPosition)
		{
			_target.localPosition = pointOnPath;
		}
		else
		{
			_target.position = pointOnPath;
		}
	}
}
