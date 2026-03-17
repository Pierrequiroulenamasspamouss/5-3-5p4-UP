public class FloatTweenProperty : AbstractTweenProperty, IGenericProperty
{
	private global::System.Action<float> _setter;

	protected float _originalEndValue;

	protected float _startValue;

	protected float _endValue;

	protected float _diffValue;

	public string propertyName { get; private set; }

	public FloatTweenProperty(string propertyName, float endValue, bool isRelative = false)
		: base(isRelative)
	{
		this.propertyName = propertyName;
		_originalEndValue = endValue;
	}

	public override bool validateTarget(object target)
	{
		_setter = GoTweenUtils.setterForProperty<global::System.Action<float>>(target, propertyName);
		return _setter != null;
	}

	public override void prepareForUse()
	{
		global::System.Func<float> func = GoTweenUtils.getterForProperty<global::System.Func<float>>(_ownerTween.target, propertyName);
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
		float obj = _easeFunction(totalElapsedTime, _startValue, _diffValue, _ownerTween.duration);
		_setter(obj);
	}
}
