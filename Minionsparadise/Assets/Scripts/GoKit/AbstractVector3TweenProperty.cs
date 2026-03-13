public abstract class AbstractVector3TweenProperty : AbstractTweenProperty
{
	protected global::UnityEngine.Transform _target;

	protected global::UnityEngine.Vector3 _originalEndValue;

	protected global::UnityEngine.Vector3 _startValue;

	protected global::UnityEngine.Vector3 _endValue;

	protected global::UnityEngine.Vector3 _diffValue;

	public AbstractVector3TweenProperty()
	{
	}

	public AbstractVector3TweenProperty(global::UnityEngine.Vector3 endValue, bool isRelative = false)
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
			_diffValue = _endValue;
		}
		else
		{
			_diffValue = _endValue - _startValue;
		}
	}

	public void resetWithNewEndValue(global::UnityEngine.Vector3 endValue)
	{
		_originalEndValue = endValue;
		prepareForUse();
	}
}
