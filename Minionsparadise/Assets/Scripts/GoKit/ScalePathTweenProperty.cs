public class ScalePathTweenProperty : AbstractTweenProperty
{
	protected global::UnityEngine.Transform _target;

	protected global::UnityEngine.Vector3 _startValue;

	private GoSpline _path;

	public ScalePathTweenProperty(GoSpline path, bool isRelative = false)
		: base(isRelative)
	{
		_path = path;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (base.Equals(obj))
		{
			return true;
		}
		return obj.GetType() == typeof(ScaleTweenProperty);
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
		else
		{
			_startValue = _target.localScale;
		}
	}

	public override void tick(float totalElapsedTime)
	{
		float t = _easeFunction(totalElapsedTime, 0f, 1f, _ownerTween.duration);
		global::UnityEngine.Vector3 pointOnPath = _path.getPointOnPath(t);
		if (_isRelative)
		{
			pointOnPath += _startValue;
		}
		_target.localScale = pointOnPath;
	}
}
