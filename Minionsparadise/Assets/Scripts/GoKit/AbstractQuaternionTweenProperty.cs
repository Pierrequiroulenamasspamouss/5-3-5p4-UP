public abstract class AbstractQuaternionTweenProperty : AbstractTweenProperty
{
	protected global::UnityEngine.Transform _target;

	protected global::UnityEngine.Quaternion _originalEndValue;

	protected global::UnityEngine.Quaternion _startValue;

	protected global::UnityEngine.Quaternion _endValue;

	public AbstractQuaternionTweenProperty()
	{
	}

	public AbstractQuaternionTweenProperty(global::UnityEngine.Quaternion endValue, bool isRelative = false)
		: base(isRelative)
	{
		_originalEndValue = endValue;
	}

	public override bool validateTarget(object target)
	{
		return target is global::UnityEngine.Transform;
	}

	public override void prepareForUse()
	{
		if (_isRelative && !_ownerTween.isFrom)
		{
			_endValue = _startValue * _endValue;
		}
	}
}
