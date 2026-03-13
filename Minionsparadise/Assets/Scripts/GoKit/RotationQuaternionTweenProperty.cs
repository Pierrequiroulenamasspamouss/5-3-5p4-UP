public class RotationQuaternionTweenProperty : AbstractQuaternionTweenProperty
{
	private bool _useLocalRotation;

	public bool useLocalRotation
	{
		get
		{
			return _useLocalRotation;
		}
	}

	public RotationQuaternionTweenProperty(global::UnityEngine.Quaternion endValue, bool isRelative = false, bool useLocalRotation = false)
		: base(endValue, isRelative)
	{
		_useLocalRotation = useLocalRotation;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (base.Equals(obj))
		{
			return _useLocalRotation == ((RotationQuaternionTweenProperty)obj)._useLocalRotation;
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
			if (_useLocalRotation)
			{
				_endValue = _target.localRotation;
			}
			else
			{
				_endValue = _target.rotation;
			}
		}
		else if (_useLocalRotation)
		{
			_startValue = _target.localRotation;
		}
		else
		{
			_startValue = _target.rotation;
		}
		base.prepareForUse();
	}

	public override void tick(float totalElapsedTime)
	{
		float t = _easeFunction(totalElapsedTime, 0f, 1f, _ownerTween.duration);
		global::UnityEngine.Quaternion quaternion = global::UnityEngine.Quaternion.Slerp(_startValue, _endValue, t);
		if (_useLocalRotation)
		{
			_target.localRotation = quaternion;
		}
		else
		{
			_target.rotation = quaternion;
		}
	}
}
