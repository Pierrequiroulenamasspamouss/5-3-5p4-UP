public class ScaleTweenProperty : AbstractVector3TweenProperty
{
	public ScaleTweenProperty(global::UnityEngine.Vector3 endValue, bool isRelative = false)
		: base(endValue, isRelative)
	{
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (base.Equals(obj))
		{
			return true;
		}
		return obj.GetType() == typeof(ScalePathTweenProperty);
	}

	public override void prepareForUse()
	{
		_target = _ownerTween.target as global::UnityEngine.Transform;
		_endValue = _originalEndValue;
		if (_ownerTween.isFrom)
		{
			_startValue = _endValue;
			_endValue = _target.localScale;
		}
		else
		{
			_startValue = _target.localScale;
		}
		base.prepareForUse();
	}

	public override void tick(float totalElapsedTime)
	{
		float value = _easeFunction(totalElapsedTime, 0f, 1f, _ownerTween.duration);
		_target.localScale = GoTweenUtils.unclampedVector3Lerp(_startValue, _diffValue, value);
	}
}
