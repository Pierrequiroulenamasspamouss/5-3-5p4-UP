public abstract class AbstractTweenProperty
{
	protected bool _isInitialized;

	protected bool _isRelative;

	protected GoTween _ownerTween;

	protected global::System.Func<float, float, float, float, float> _easeFunction;

	public bool isInitialized
	{
		get
		{
			return _isInitialized;
		}
	}

	public AbstractTweenProperty(bool isRelative = false)
	{
		_isRelative = isRelative;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this is IGenericProperty && obj is IGenericProperty)
		{
			return ((IGenericProperty)this).propertyName == ((IGenericProperty)obj).propertyName;
		}
		if (obj.GetType() == GetType())
		{
			return true;
		}
		return base.Equals(obj);
	}

	public virtual void init(GoTween owner)
	{
		_isInitialized = true;
		_ownerTween = owner;
		if (_easeFunction == null)
		{
			setEaseType(owner.easeType);
		}
	}

	public AbstractTweenProperty clone()
	{
		AbstractTweenProperty abstractTweenProperty = MemberwiseClone() as AbstractTweenProperty;
		abstractTweenProperty._ownerTween = null;
		abstractTweenProperty._isInitialized = false;
		abstractTweenProperty._easeFunction = null;
		return abstractTweenProperty;
	}

	public void setEaseType(GoEaseType easeType)
	{
		_easeFunction = GoTweenUtils.easeFunctionForType(easeType, _ownerTween);
	}

	public virtual bool validateTarget(object target)
	{
		return true;
	}

	public abstract void tick(float totalElapsedTime);

	public abstract void prepareForUse();
}
