public abstract class AbstractMaterialFloatTweenProperty : AbstractTweenProperty
{
	protected global::UnityEngine.Material _target;

	protected float _originalEndValue;

	protected float _startValue;

	protected float _endValue;

	protected float _diffValue;

	public AbstractMaterialFloatTweenProperty(float endValue, bool isRelative)
		: base(isRelative)
	{
		_originalEndValue = endValue;
	}

	public override bool validateTarget(object target)
	{
		if (!(target is global::UnityEngine.Material) && !(target is global::UnityEngine.GameObject) && !(target is global::UnityEngine.Transform))
		{
			return target is global::UnityEngine.Renderer;
		}
		return true;
	}

	public override void init(GoTween owner)
	{
		if (owner.target is global::UnityEngine.Material)
		{
			_target = (global::UnityEngine.Material)owner.target;
		}
		else if (owner.target is global::UnityEngine.GameObject)
		{
			_target = ((global::UnityEngine.GameObject)owner.target).GetComponent<global::UnityEngine.Renderer>().material;
		}
		else if (owner.target is global::UnityEngine.Transform)
		{
			_target = ((global::UnityEngine.Transform)owner.target).GetComponent<global::UnityEngine.Renderer>().material;
		}
		else if (owner.target is global::UnityEngine.Renderer)
		{
			_target = ((global::UnityEngine.Renderer)owner.target).material;
		}
		base.init(owner);
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
}
