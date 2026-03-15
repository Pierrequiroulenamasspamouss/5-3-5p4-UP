public class Vector3ZTweenProperty : Vector3XTweenProperty
{
	public Vector3ZTweenProperty(string propertyName, float endValue, bool isRelative = false)
		: base(propertyName, endValue, isRelative)
	{
	}

	public override void prepareForUse()
	{
		_getter = GoTweenUtils.getterForProperty<global::System.Func<global::UnityEngine.Vector3>>(_ownerTween.target, base.propertyName);
		_endValue = _originalEndValue;
		if (_ownerTween.isFrom)
		{
			_startValue = _endValue;
			_endValue = _getter().z;
		}
		else
		{
			_startValue = _getter().z;
		}
		if (_isRelative && !_ownerTween.isFrom)
		{
			_diffValue = _endValue;
		}
		else
		{
			_diffValue = _endValue - _startValue;
		}
	}

	public override void tick(float totalElapsedTime)
	{
		global::UnityEngine.Vector3 obj = _getter();
		obj.z = _easeFunction(totalElapsedTime, _startValue, _diffValue, _ownerTween.duration);
		_setter(obj);
	}
}
