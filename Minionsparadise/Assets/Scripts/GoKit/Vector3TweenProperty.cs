public class Vector3TweenProperty : AbstractVector3TweenProperty, IGenericProperty
{
	private global::System.Action<global::UnityEngine.Vector3> _setter;

	public string propertyName { get; private set; }

	public Vector3TweenProperty(string propertyName, global::UnityEngine.Vector3 endValue, bool isRelative = false)
		: base(endValue, isRelative)
	{
		this.propertyName = propertyName;
	}

	public override bool validateTarget(object target)
	{
		_setter = GoTweenUtils.setterForProperty<global::System.Action<global::UnityEngine.Vector3>>(target, propertyName);
		return _setter != null;
	}

	public override void prepareForUse()
	{
		global::System.Func<global::UnityEngine.Vector3> func = GoTweenUtils.getterForProperty<global::System.Func<global::UnityEngine.Vector3>>(_ownerTween.target, propertyName);
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
		global::UnityEngine.Vector3 obj = GoTweenUtils.unclampedVector3Lerp(_startValue, _diffValue, value);
		_setter(obj);
	}
}
