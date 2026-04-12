public class ColorTweenProperty : AbstractMaterialColorTweenProperty, IGenericProperty
{
	private global::System.Action<global::UnityEngine.Color> _setter;

	public string propertyName { get; private set; }

	public ColorTweenProperty(string propertyName, global::UnityEngine.Color endValue, bool isRelative = false)
		: base(endValue, isRelative)
	{
		this.propertyName = propertyName;
	}

	public override bool validateTarget(object target)
	{
		_setter = GoTweenUtils.setterForProperty<global::System.Action<global::UnityEngine.Color>>(target, propertyName);
		return _setter != null;
	}

	public override void prepareForUse()
	{
		global::System.Func<global::UnityEngine.Color> func = GoTweenUtils.getterForProperty<global::System.Func<global::UnityEngine.Color>>(_ownerTween.target, propertyName);
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
		base.prepareForUse();
	}

	public override void tick(float totalElapsedTime)
	{
		float value = _easeFunction(totalElapsedTime, 0f, 1f, _ownerTween.duration);
		global::UnityEngine.Color obj = GoTweenUtils.unclampedColorLerp(_startValue, _diffValue, value);
		_setter(obj);
	}
}
