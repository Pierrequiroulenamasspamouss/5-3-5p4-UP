public class AbstractGoTweenCollection : AbstractGoTween
{
	protected class TweenFlowItem
	{
		public float startTime;

		public float duration;

		public AbstractGoTween tween;

		public float endTime
		{
			get
			{
				return startTime + duration;
			}
		}

		public TweenFlowItem(float startTime, AbstractGoTween tween)
		{
			this.tween = tween;
			this.startTime = startTime;
			duration = tween.totalDuration;
		}

		public TweenFlowItem(float startTime, float duration)
		{
			this.duration = duration;
			this.startTime = startTime;
		}
	}

	protected global::System.Collections.Generic.List<AbstractGoTweenCollection.TweenFlowItem> _tweenFlows = new global::System.Collections.Generic.List<AbstractGoTweenCollection.TweenFlowItem>();

	public AbstractGoTweenCollection(GoTweenCollectionConfig config)
	{
		base.allowEvents = true;
		_didInit = false;
		_didBegin = false;
		_fireIterationStart = true;
		base.id = config.id;
		base.loopType = config.loopType;
		base.iterations = config.iterations;
		base.updateType = config.propertyUpdateType;
		base.timeScale = 1f;
		base.state = GoTweenState.Paused;
		_onInit = config.onInitHandler;
		_onBegin = config.onBeginHandler;
		_onIterationStart = config.onIterationStartHandler;
		_onUpdate = config.onUpdateHandler;
		_onIterationEnd = config.onIterationEndHandler;
		_onComplete = config.onCompleteHandler;
		Go.addTween(this);
	}

	public global::System.Collections.Generic.List<GoTween> tweensWithTarget(object target)
	{
		global::System.Collections.Generic.List<GoTween> list = new global::System.Collections.Generic.List<GoTween>();
		for (int i = 0; i < _tweenFlows.Count; i++)
		{
			AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = _tweenFlows[i];
			if (tweenFlowItem.tween == null)
			{
				continue;
			}
			GoTween goTween = tweenFlowItem.tween as GoTween;
			if (goTween != null && goTween.target == target)
			{
				list.Add(goTween);
			}
			if (goTween != null)
			{
				continue;
			}
			AbstractGoTweenCollection abstractGoTweenCollection = tweenFlowItem.tween as AbstractGoTweenCollection;
			if (abstractGoTweenCollection != null)
			{
				global::System.Collections.Generic.List<GoTween> list2 = abstractGoTweenCollection.tweensWithTarget(target);
				if (list2.Count > 0)
				{
					list.AddRange(list2);
				}
			}
		}
		return list;
	}

	public override bool removeTweenProperty(AbstractTweenProperty property)
	{
		for (int i = 0; i < _tweenFlows.Count; i++)
		{
			AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = _tweenFlows[i];
			if (tweenFlowItem.tween != null && tweenFlowItem.tween.removeTweenProperty(property))
			{
				return true;
			}
		}
		return false;
	}

	public override bool containsTweenProperty(AbstractTweenProperty property)
	{
		for (int i = 0; i < _tweenFlows.Count; i++)
		{
			AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = _tweenFlows[i];
			if (tweenFlowItem.tween != null && tweenFlowItem.tween.containsTweenProperty(property))
			{
				return true;
			}
		}
		return false;
	}

	public override global::System.Collections.Generic.List<AbstractTweenProperty> allTweenProperties()
	{
		global::System.Collections.Generic.List<AbstractTweenProperty> list = new global::System.Collections.Generic.List<AbstractTweenProperty>();
		for (int i = 0; i < _tweenFlows.Count; i++)
		{
			AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = _tweenFlows[i];
			if (tweenFlowItem.tween != null)
			{
				list.AddRange(tweenFlowItem.tween.allTweenProperties());
			}
		}
		return list;
	}

	public override bool isValid()
	{
		return true;
	}

	public override void play()
	{
		base.play();
		for (int i = 0; i < _tweenFlows.Count; i++)
		{
			AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = _tweenFlows[i];
			if (tweenFlowItem.tween != null)
			{
				tweenFlowItem.tween.play();
			}
		}
	}

	public override void pause()
	{
		base.pause();
		for (int i = 0; i < _tweenFlows.Count; i++)
		{
			AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = _tweenFlows[i];
			if (tweenFlowItem.tween != null)
			{
				tweenFlowItem.tween.pause();
			}
		}
	}

	public override bool update(float deltaTime)
	{
		if (!_didInit)
		{
			onInit();
		}
		if (!_didBegin)
		{
			onBegin();
		}
		if (_fireIterationStart)
		{
			onIterationStart();
		}
		base.update(deltaTime);
		float num = (_isLoopingBackOnPingPong ? (base.duration - _elapsedTime) : _elapsedTime);
		AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = null;
		if (_didIterateLastFrame && base.loopType == GoLoopType.RestartFromBeginning)
		{
			if (base.isReversed || _isLoopingBackOnPingPong)
			{
				for (int i = 0; i < _tweenFlows.Count; i++)
				{
					tweenFlowItem = _tweenFlows[i];
					if (tweenFlowItem.tween != null)
					{
						bool flag = tweenFlowItem.tween.allowEvents;
						tweenFlowItem.tween.allowEvents = false;
						tweenFlowItem.tween.restart();
						tweenFlowItem.tween.allowEvents = flag;
					}
				}
			}
			else
			{
				for (int num2 = _tweenFlows.Count - 1; num2 >= 0; num2--)
				{
					tweenFlowItem = _tweenFlows[num2];
					if (tweenFlowItem.tween != null)
					{
						bool flag2 = tweenFlowItem.tween.allowEvents;
						tweenFlowItem.tween.allowEvents = false;
						tweenFlowItem.tween.restart();
						tweenFlowItem.tween.allowEvents = flag2;
					}
				}
			}
		}
		else if ((base.isReversed && !_isLoopingBackOnPingPong) || (!base.isReversed && _isLoopingBackOnPingPong))
		{
			for (int num3 = _tweenFlows.Count - 1; num3 >= 0; num3--)
			{
				tweenFlowItem = _tweenFlows[num3];
				if (tweenFlowItem.tween != null)
				{
					if (_didIterateLastFrame && base.state != GoTweenState.Complete)
					{
						if (!tweenFlowItem.tween.isReversed)
						{
							tweenFlowItem.tween.reverse();
						}
						tweenFlowItem.tween.play();
					}
					if (tweenFlowItem.tween.state == GoTweenState.Running && tweenFlowItem.endTime >= num)
					{
						float deltaTime2 = global::UnityEngine.Mathf.Abs(num - tweenFlowItem.startTime - tweenFlowItem.tween.totalElapsedTime);
						tweenFlowItem.tween.update(deltaTime2);
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < _tweenFlows.Count; j++)
			{
				tweenFlowItem = _tweenFlows[j];
				if (tweenFlowItem.tween == null)
				{
					continue;
				}
				if (_didIterateLastFrame && base.state != GoTweenState.Complete)
				{
					if (tweenFlowItem.tween.isReversed)
					{
						tweenFlowItem.tween.reverse();
					}
					tweenFlowItem.tween.play();
				}
				if (tweenFlowItem.tween.state == GoTweenState.Running && tweenFlowItem.startTime <= num)
				{
					float deltaTime3 = num - tweenFlowItem.startTime - tweenFlowItem.tween.totalElapsedTime;
					tweenFlowItem.tween.update(deltaTime3);
				}
			}
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

	public override void reverse()
	{
		base.reverse();
		float num = (_isLoopingBackOnPingPong ? (base.duration - _elapsedTime) : _elapsedTime);
		for (int i = 0; i < _tweenFlows.Count; i++)
		{
			AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = _tweenFlows[i];
			if (tweenFlowItem.tween == null)
			{
				continue;
			}
			if (base.isReversed != tweenFlowItem.tween.isReversed)
			{
				tweenFlowItem.tween.reverse();
			}
			tweenFlowItem.tween.pause();
			if (base.isReversed || _isLoopingBackOnPingPong)
			{
				if (tweenFlowItem.startTime <= num)
				{
					tweenFlowItem.tween.play();
				}
			}
			else if (tweenFlowItem.endTime >= num)
			{
				tweenFlowItem.tween.play();
			}
		}
	}

	public override void goTo(float time, bool skipDelay)
	{
		time = global::UnityEngine.Mathf.Clamp(time, 0f, base.totalDuration);
		if (time == _totalElapsedTime)
		{
			return;
		}
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
		base.update(0f);
		float num = (_isLoopingBackOnPingPong ? (base.duration - _elapsedTime) : _elapsedTime);
		AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = null;
		if (base.isReversed || _isLoopingBackOnPingPong)
		{
			for (int i = 0; i < _tweenFlows.Count; i++)
			{
				tweenFlowItem = _tweenFlows[i];
				if (tweenFlowItem != null)
				{
					if (tweenFlowItem.endTime >= num)
					{
						break;
					}
					changeTimeForFlowItem(tweenFlowItem, num);
				}
			}
			for (int num2 = _tweenFlows.Count - 1; num2 >= 0; num2--)
			{
				tweenFlowItem = _tweenFlows[num2];
				if (tweenFlowItem != null)
				{
					if (tweenFlowItem.endTime < num)
					{
						break;
					}
					changeTimeForFlowItem(tweenFlowItem, num);
				}
			}
			return;
		}
		for (int num3 = _tweenFlows.Count - 1; num3 >= 0; num3--)
		{
			tweenFlowItem = _tweenFlows[num3];
			if (tweenFlowItem != null)
			{
				if (tweenFlowItem.startTime <= num)
				{
					break;
				}
				changeTimeForFlowItem(tweenFlowItem, num);
			}
		}
		for (int j = 0; j < _tweenFlows.Count; j++)
		{
			tweenFlowItem = _tweenFlows[j];
			if (tweenFlowItem != null)
			{
				if (tweenFlowItem.startTime > num)
				{
					break;
				}
				changeTimeForFlowItem(tweenFlowItem, num);
			}
		}
	}

	private void changeTimeForFlowItem(AbstractGoTweenCollection.TweenFlowItem flowItem, float time)
	{
		if (flowItem != null && flowItem.tween != null)
		{
			if (flowItem.tween.isReversed != (base.isReversed || _isLoopingBackOnPingPong))
			{
				flowItem.tween.reverse();
			}
			float time2 = global::UnityEngine.Mathf.Clamp(time - flowItem.startTime, 0f, flowItem.endTime);
			if (flowItem.startTime <= time && flowItem.endTime >= time)
			{
				flowItem.tween.goToAndPlay(time2);
				return;
			}
			flowItem.tween.goTo(time2);
			flowItem.tween.pause();
		}
	}
}
