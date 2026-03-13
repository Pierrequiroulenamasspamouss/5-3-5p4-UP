public class EulerAnglesTweenProperty : AbstractVector3TweenProperty
{
	private bool _useLocalEulers;

	public bool useLocalEulers
	{
		get
		{
			return _useLocalEulers;
		}
	}

	public EulerAnglesTweenProperty(global::UnityEngine.Vector3 endValue, bool isRelative = false, bool useLocalEulers = false)
		: base(endValue, isRelative)
	{
		_useLocalEulers = useLocalEulers;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (base.Equals(obj))
		{
			return _useLocalEulers == ((EulerAnglesTweenProperty)obj)._useLocalEulers;
		}
		RotationTweenProperty rotationTweenProperty = obj as RotationTweenProperty;
		if (rotationTweenProperty != null)
		{
			return _useLocalEulers == rotationTweenProperty.useLocalRotation;
		}
		return false;
	}

	public override void prepareForUse()
	{
		_target = _ownerTween.target as global::UnityEngine.Transform;
		_endValue = _originalEndValue;
		if (_ownerTween.isFrom)
		{
			_startValue = _endValue;
			if (_useLocalEulers)
			{
				_endValue = _target.localEulerAngles;
			}
			else
			{
				_endValue = _target.eulerAngles;
			}
		}
		else if (_useLocalEulers)
		{
			_startValue = _target.localEulerAngles;
		}
		else
		{
			_startValue = _target.eulerAngles;
		}
		base.prepareForUse();
	}

	public override void tick(float totalElapsedTime)
	{
		float value = _easeFunction(totalElapsedTime, 0f, 1f, _ownerTween.duration);
		global::UnityEngine.Vector3 vector = GoTweenUtils.unclampedVector3Lerp(_startValue, _diffValue, value);
		if (_useLocalEulers)
		{
			_target.localEulerAngles = vector;
		}
		else
		{
			_target.eulerAngles = vector;
		}
	}
}
