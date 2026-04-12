public class GoTween : AbstractGoTween
{
	private float _elapsedDelay;

	private bool _delayComplete;

	private global::System.Collections.Generic.List<AbstractTweenProperty> _tweenPropertyList = new global::System.Collections.Generic.List<AbstractTweenProperty>(2);

	private global::System.Type targetType;

	private GoEaseType _easeType;

	public object target { get; private set; }

	public float delay { get; private set; }

	public bool isFrom { get; private set; }

	public GoEaseType easeType
	{
		get
		{
			return _easeType;
		}
		set
		{
			_easeType = value;
			for (int i = 0; i < _tweenPropertyList.Count; i++)
			{
				AbstractTweenProperty abstractTweenProperty = _tweenPropertyList[i];
				abstractTweenProperty.setEaseType(value);
			}
		}
	}

	public global::UnityEngine.AnimationCurve easeCurve { get; set; }

	public GoTween(object target, float duration, GoTweenConfig config, global::System.Action<AbstractGoTween> onComplete = null)
	{
		if (duration <= 0f)
		{
			duration = float.Epsilon;
			global::UnityEngine.Debug.LogError("tween duration must be greater than 0. coerced to float.Epsilon.");
		}
		base.autoRemoveOnComplete = true;
		base.allowEvents = true;
		_didInit = false;
		_didBegin = false;
		_fireIterationStart = true;
		this.target = target;
		targetType = target.GetType();
		base.duration = duration;
		base.id = config.id;
		delay = config.delay;
		base.loopType = config.loopType;
		base.iterations = config.iterations;
		_easeType = config.easeType;
		easeCurve = config.easeCurve;
		base.updateType = config.propertyUpdateType;
		isFrom = config.isFrom;
		base.timeScale = config.timeScale;
		_onInit = config.onInitHandler;
		_onBegin = config.onBeginHandler;
		_onIterationStart = config.onIterationStartHandler;
		_onUpdate = config.onUpdateHandler;
		_onIterationEnd = config.onIterationEndHandler;
		_onComplete = config.onCompleteHandler;
		if (config.isPaused)
		{
			base.state = GoTweenState.Paused;
		}
		if (onComplete != null)
		{
			_onComplete = onComplete;
		}
		for (int i = 0; i < config.tweenProperties.Count; i++)
		{
			AbstractTweenProperty abstractTweenProperty = config.tweenProperties[i];
			if (abstractTweenProperty.isInitialized)
			{
				abstractTweenProperty = abstractTweenProperty.clone();
			}
			addTweenProperty(abstractTweenProperty);
		}
		if (base.iterations < 0)
		{
			base.totalDuration = float.PositiveInfinity;
		}
		else
		{
			base.totalDuration = (float)base.iterations * duration;
		}
	}

	public override bool update(float deltaTime)
	{
		if (!_didInit)
		{
			onInit();
		}
		if (Go.validateTargetObjectsEachTick && (target == null || target.Equals(null)))
		{
			Go.warn("target validation failed. destroying the tween to avoid errors. Target type: " + targetType);
			base.autoRemoveOnComplete = true;
			return true;
		}
		if (!_didBegin)
		{
			onBegin();
		}
		if (!_delayComplete && _elapsedDelay < delay)
		{
			if (base.timeScale != 0f)
			{
				_elapsedDelay += deltaTime / base.timeScale;
			}
			if (_elapsedDelay >= delay)
			{
				_delayComplete = true;
			}
			return false;
		}
		if (_fireIterationStart)
		{
			onIterationStart();
		}
		base.update(deltaTime);
		float num = (_isLoopingBackOnPingPong ? (base.duration - _elapsedTime) : _elapsedTime);
		for (int i = 0; i < _tweenPropertyList.Count; i++)
		{
			_tweenPropertyList[i].tick(num);
		}
		onUpdate();
		if (_fireIterationEnd)
		{
			onIterationEnd();
		}
		if (base.state == GoTweenState.Complete)
		{
			onComplete();
			return true;
		}
		return false;
	}

	public override bool isValid()
	{
		return target != null;
	}

	public void addTweenProperty(AbstractTweenProperty tweenProp)
	{
		if (tweenProp.validateTarget(target))
		{
			if (_tweenPropertyList.Contains(tweenProp))
			{
				global::UnityEngine.Debug.Log("not adding tween property because one already exists: " + tweenProp);
				return;
			}
			_tweenPropertyList.Add(tweenProp);
			tweenProp.init(this);
		}
		else
		{
			global::UnityEngine.Debug.Log("tween failed to validate target: " + tweenProp);
		}
	}

	public override bool removeTweenProperty(AbstractTweenProperty property)
	{
		if (_tweenPropertyList.Contains(property))
		{
			_tweenPropertyList.Remove(property);
			return true;
		}
		return false;
	}

	public override bool containsTweenProperty(AbstractTweenProperty property)
	{
		return _tweenPropertyList.Contains(property);
	}

	public void clearTweenProperties()
	{
		_tweenPropertyList.Clear();
	}

	public override global::System.Collections.Generic.List<AbstractTweenProperty> allTweenProperties()
	{
		return _tweenPropertyList;
	}

	protected override void onInit()
	{
		base.onInit();
		for (int i = 0; i < _tweenPropertyList.Count; i++)
		{
			_tweenPropertyList[i].prepareForUse();
		}
	}

	public override void destroy()
	{
		base.destroy();
		_tweenPropertyList.Clear();
		target = null;
	}

	public override void goTo(float time, bool skipDelay)
	{
		if (skipDelay)
		{
			_elapsedDelay = delay;
		}
		else
		{
			_elapsedDelay = global::UnityEngine.Mathf.Min(time, delay);
			time -= _elapsedDelay;
		}
		_delayComplete = _elapsedDelay >= delay;
		time = global::UnityEngine.Mathf.Clamp(time, 0f, base.totalDuration);
		if (time != _totalElapsedTime)
		{
			if ((base.isReversed && time == base.totalDuration) || (!base.isReversed && time == 0f))
			{
				_didBegin = false;
				_fireIterationStart = true;
			}
			else
			{
				_didBegin = true;
				_fireIterationStart = false;
			}
			_didIterateThisFrame = false;
			_totalElapsedTime = time;
			_completedIterations = (base.isReversed ? global::UnityEngine.Mathf.CeilToInt(_totalElapsedTime / base.duration) : global::UnityEngine.Mathf.FloorToInt(_totalElapsedTime / base.duration));
			update(0f);
		}
	}

	public override void complete()
	{
		if (base.iterations >= 0)
		{
			_delayComplete = true;
			base.complete();
		}
	}
}
