public class IntTweenProperty : AbstractTweenProperty, IGenericProperty
{
	private global::System.Action<int> _setter;

	protected int _originalEndValue;

	protected int _startValue;

	protected int _endValue;

	protected int _diffValue;

	public string propertyName { get; private set; }

	public IntTweenProperty(string propertyName, int endValue, bool isRelative = false)
		: base(isRelative)
	{
		this.propertyName = propertyName;
		_originalEndValue = endValue;
	}

	public override bool validateTarget(object target)
	{
		_setter = GoTweenUtils.setterForProperty<global::System.Action<int>>(target, propertyName);
		return _setter != null;
	}

	public override void prepareForUse()
	{
		global::System.Func<int> func = GoTweenUtils.getterForProperty<global::System.Func<int>>(_ownerTween.target, propertyName);
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
		float num = _easeFunction(totalElapsedTime, _startValue, _diffValue, _ownerTween.duration);
		_setter((int)global::System.Math.Round(num));
	}
}
