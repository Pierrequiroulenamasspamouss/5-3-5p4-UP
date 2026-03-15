public class PivotTweenProperty : AbstractTweenProperty
{
	protected global::UnityEngine.RectTransform _target;

	protected global::UnityEngine.Vector2 _originalEndValue;

	protected global::UnityEngine.Vector2 _startValue;

	protected global::UnityEngine.Vector2 _endValue;

	protected global::UnityEngine.Vector2 _diffValue;

	public PivotTweenProperty(global::UnityEngine.Vector2 endValue, bool isRelative = false)
		: base(isRelative)
	{
		_originalEndValue = endValue;
	}

	public override bool validateTarget(object target)
	{
		return target is global::UnityEngine.RectTransform;
	}

	public override void prepareForUse()
	{
		_target = _ownerTween.target as global::UnityEngine.RectTransform;
		_endValue = _originalEndValue;
		if (_ownerTween.isFrom)
		{
			_startValue = (_isRelative ? (_endValue + _target.pivot) : _endValue);
			_endValue = _target.pivot;
		}
		else
		{
			_startValue = _target.pivot;
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
		global::UnityEngine.Vector2 pivot = GoTweenUtils.unclampedVector2Lerp(_startValue, _diffValue, value);
		_target.pivot = pivot;
	}

	public void resetWithNewEndValue(global::UnityEngine.Vector2 endValue)
	{
		_originalEndValue = endValue;
		prepareForUse();
	}
}
