public class Vector3XTweenProperty : AbstractVector3TweenProperty, IGenericProperty
{
	protected global::System.Action<global::UnityEngine.Vector3> _setter;

	protected global::System.Func<global::UnityEngine.Vector3> _getter;

	protected new float _originalEndValue;

	protected new float _startValue;

	protected new float _endValue;

	protected new float _diffValue;

	public string propertyName { get; private set; }

	public Vector3XTweenProperty(string propertyName, float endValue, bool isRelative = false)
	{
		this.propertyName = propertyName;
		_isRelative = isRelative;
		_originalEndValue = endValue;
	}

	public override bool validateTarget(object target)
	{
		_setter = GoTweenUtils.setterForProperty<global::System.Action<global::UnityEngine.Vector3>>(target, propertyName);
		return _setter != null;
	}

	public override void prepareForUse()
	{
		_getter = GoTweenUtils.getterForProperty<global::System.Func<global::UnityEngine.Vector3>>(_ownerTween.target, propertyName);
		_endValue = _originalEndValue;
		if (_ownerTween.isFrom)
		{
			_startValue = _endValue;
			_endValue = _getter().x;
		}
		else
		{
			_startValue = _getter().x;
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
		obj.x = _easeFunction(totalElapsedTime, _startValue, _diffValue, _ownerTween.duration);
		_setter(obj);
	}
}
