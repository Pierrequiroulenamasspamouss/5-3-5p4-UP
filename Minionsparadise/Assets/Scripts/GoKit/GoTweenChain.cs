public class GoTweenChain : AbstractGoTweenCollection
{
	public GoTweenChain()
		: this(new GoTweenCollectionConfig())
	{
	}

	public GoTweenChain(GoTweenCollectionConfig config)
		: base(config)
	{
	}

	private void append(AbstractGoTweenCollection.TweenFlowItem item)
	{
		if (item.tween != null && !item.tween.isValid())
		{
			return;
		}
		if (float.IsInfinity(item.duration))
		{
			global::UnityEngine.Debug.LogError("adding a Tween with infinite iterations to a TweenChain is not permitted");
			return;
		}
		if (item.tween != null)
		{
			if (item.tween.isReversed != base.isReversed)
			{
				global::UnityEngine.Debug.LogError("adding a Tween that doesn't match the isReversed property of the TweenChain is not permitted.");
				return;
			}
			Go.removeTween(item.tween);
			item.tween.play();
		}
		_tweenFlows.Add(item);
		base.duration += item.duration;
		if (base.iterations < 0)
		{
			base.totalDuration = float.PositiveInfinity;
		}
		else
		{
			base.totalDuration = base.duration * (float)base.iterations;
		}
	}

	private void prepend(AbstractGoTweenCollection.TweenFlowItem item)
	{
		if (item.tween != null && !item.tween.isValid())
		{
			return;
		}
		if (float.IsInfinity(item.duration))
		{
			global::UnityEngine.Debug.LogError("adding a Tween with infinite iterations to a TweenChain is not permitted");
			return;
		}
		if (item.tween != null)
		{
			if (item.tween.isReversed != base.isReversed)
			{
				global::UnityEngine.Debug.LogError("adding a Tween that doesn't match the isReversed property of the TweenChain is not permitted.");
				return;
			}
			Go.removeTween(item.tween);
			item.tween.play();
		}
		for (int i = 0; i < _tweenFlows.Count; i++)
		{
			AbstractGoTweenCollection.TweenFlowItem tweenFlowItem = _tweenFlows[i];
			tweenFlowItem.startTime += item.duration;
		}
		_tweenFlows.Insert(0, item);
		base.duration += item.duration;
		if (base.iterations < 0)
		{
			base.totalDuration = float.PositiveInfinity;
		}
		else
		{
			base.totalDuration = base.duration * (float)base.iterations;
		}
	}

	public GoTweenChain append(AbstractGoTween tween)
	{
		AbstractGoTweenCollection.TweenFlowItem item = new AbstractGoTweenCollection.TweenFlowItem(base.duration, tween);
		append(item);
		return this;
	}

	public GoTweenChain appendDelay(float delay)
	{
		AbstractGoTweenCollection.TweenFlowItem item = new AbstractGoTweenCollection.TweenFlowItem(base.duration, delay);
		append(item);
		return this;
	}

	public GoTweenChain prepend(AbstractGoTween tween)
	{
		AbstractGoTweenCollection.TweenFlowItem item = new AbstractGoTweenCollection.TweenFlowItem(0f, tween);
		prepend(item);
		return this;
	}

	public GoTweenChain prependDelay(float delay)
	{
		AbstractGoTweenCollection.TweenFlowItem item = new AbstractGoTweenCollection.TweenFlowItem(0f, delay);
		prepend(item);
		return this;
	}
}
