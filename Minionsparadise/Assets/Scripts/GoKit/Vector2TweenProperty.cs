public class Vector2TweenProperty : AbstractTweenProperty, IGenericProperty
{
	private global::System.Action<global::UnityEngine.Vector2> _setter;

	protected global::UnityEngine.Vector2 _originalEndValue;

	protected global::UnityEngine.Vector2 _startValue;

	protected global::UnityEngine.Vector2 _endValue;

	protected global::UnityEngine.Vector2 _diffValue;

	public string propertyName { get; private set; }

	public Vector2TweenProperty(string propertyName, global::UnityEngine.Vector2 endValue, bool isRelative = false)
		: base(isRelative)
	{
		this.propertyName = propertyName;
		_originalEndValue = endValue;
	}

	public override bool validateTarget(object target)
	{
		_setter = GoTweenUtils.setterForProperty<global::System.Action<global::UnityEngine.Vector2>>(target, propertyName);
		return _setter != null;
	}

	public override void prepareForUse()
	{
		global::System.Func<global::UnityEngine.Vector2> func = GoTweenUtils.getterForProperty<global::System.Func<global::UnityEngine.Vector2>>(_ownerTween.target, propertyName);
		_endValue = _originalEndValue;
		if (_ownerTween.isFrom)
		{
			_startValue = _endValue;
			_endValue = func();
		}
		else
		{
			_startValue = func();
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
		float value = _easeFunction(totalElapsedTime, 0f, 1f, _ownerTween.duration);
		global::UnityEngine.Vector2 obj = GoTweenUtils.unclampedVector2Lerp(_startValue, _diffValue, value);
		_setter(obj);
	}
}
