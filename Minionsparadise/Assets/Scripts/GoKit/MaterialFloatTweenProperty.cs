public class MaterialFloatTweenProperty : AbstractMaterialFloatTweenProperty
{
	private string _materialPropertyName;

	public MaterialFloatTweenProperty(float endValue, string propertyName, bool isRelative = false)
		: base(endValue, isRelative)
	{
		_materialPropertyName = propertyName;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (base.Equals(obj))
		{
			return _materialPropertyName == ((MaterialFloatTweenProperty)obj)._materialPropertyName;
		}
		return false;
	}

	public override void prepareForUse()
	{
		_endValue = _originalEndValue;
		if (_ownerTween.isFrom)
		{
			_startValue = _endValue;
			_endValue = _target.GetFloat(_materialPropertyName);
		}
		else
		{
			_startValue = _target.GetFloat(_materialPropertyName);
		}
		base.prepareForUse();
	}

	public override void tick(float totalElapsedTime)
	{
		float t = _easeFunction(totalElapsedTime, 0f, 1f, _ownerTween.duration);
		float value = global::UnityEngine.Mathf.Lerp(_startValue, _diffValue, t);
		_target.SetFloat(_materialPropertyName, value);
	}
}
