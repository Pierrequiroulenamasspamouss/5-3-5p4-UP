public class AnchoredPosition3DTweenProperty : AbstractTweenProperty
{
	protected global::UnityEngine.RectTransform _target;

	protected global::UnityEngine.Vector3 _originalEndValue;

	protected global::UnityEngine.Vector3 _startValue;

	protected global::UnityEngine.Vector3 _endValue;

	protected global::UnityEngine.Vector3 _diffValue;

	public AnchoredPosition3DTweenProperty(global::UnityEngine.Vector3 endValue, bool isRelative = false)
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
			_startValue = (_isRelative ? (_endValue + _target.anchoredPosition3D) : _endValue);
			_endValue = _target.anchoredPosition3D;
		}
		else
		{
			_startValue = _target.anchoredPosition3D;
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
		global::UnityEngine.Vector3 anchoredPosition3D = GoTweenUtils.unclampedVector3Lerp(_startValue, _diffValue, value);
		_target.anchoredPosition3D = anchoredPosition3D;
	}

	public void resetWithNewEndValue(global::UnityEngine.Vector3 endValue)
	{
		_originalEndValue = endValue;
		prepareForUse();
	}
}
