public class GoTweenFlow : AbstractGoTweenCollection
{
	public GoTweenFlow()
		: this(new GoTweenCollectionConfig())
	{
	}

	public GoTweenFlow(GoTweenCollectionConfig config)
		: base(config)
	{
	}

	private void insert(AbstractGoTweenCollection.TweenFlowItem item)
	{
		if (item.tween != null && !item.tween.isValid())
		{
			return;
		}
		if (float.IsInfinity(item.duration))
		{
			global::UnityEngine.Debug.LogError("adding a Tween with infinite iterations to a TweenFlow is not permitted");
			return;
		}
		if (item.tween != null)
		{
			if (item.tween.isReversed != base.isReversed)
			{
				global::UnityEngine.Debug.LogError("adding a Tween that doesn't match the isReversed property of the TweenFlow is not permitted.");
				return;
			}
			Go.removeTween(item.tween);
			item.tween.play();
		}
		_tweenFlows.Add(item);
		_tweenFlows.Sort((AbstractGoTweenCollection.TweenFlowItem x, AbstractGoTweenCollection.TweenFlowItem y) => x.startTime.CompareTo(y.startTime));
		base.duration = global::UnityEngine.Mathf.Max(item.startTime + item.duration, base.duration);
		if (base.iterations < 0)
		{
			base.totalDuration = float.PositiveInfinity;
		}
		else
		{
			base.totalDuration = base.duration * (float)base.iterations;
		}
	}

	public GoTweenFlow insert(float startTime, AbstractGoTween tween)
	{
		AbstractGoTweenCollection.TweenFlowItem item = new AbstractGoTweenCollection.TweenFlowItem(startTime, tween);
		insert(item);
		return this;
	}
}
