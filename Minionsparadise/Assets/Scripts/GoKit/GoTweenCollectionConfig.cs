public class GoTweenCollectionConfig
{
	public int id;

	public int iterations = 1;

	public GoLoopType loopType = Go.defaultLoopType;

	public GoUpdateType propertyUpdateType = Go.defaultUpdateType;

	public global::System.Action<AbstractGoTween> onInitHandler;

	public global::System.Action<AbstractGoTween> onBeginHandler;

	public global::System.Action<AbstractGoTween> onIterationStartHandler;

	public global::System.Action<AbstractGoTween> onUpdateHandler;

	public global::System.Action<AbstractGoTween> onIterationEndHandler;

	public global::System.Action<AbstractGoTween> onCompleteHandler;

	public GoTweenCollectionConfig setIterations(int iterations)
	{
		this.iterations = iterations;
		return this;
	}

	public GoTweenCollectionConfig setIterations(int iterations, GoLoopType loopType)
	{
		this.iterations = iterations;
		this.loopType = loopType;
		return this;
	}

	public GoTweenCollectionConfig setUpdateType(GoUpdateType setUpdateType)
	{
		propertyUpdateType = setUpdateType;
		return this;
	}

	public GoTweenCollectionConfig onInit(global::System.Action<AbstractGoTween> onInit)
	{
		onInitHandler = onInit;
		return this;
	}

	public GoTweenCollectionConfig onBegin(global::System.Action<AbstractGoTween> onBegin)
	{
		onBeginHandler = onBegin;
		return this;
	}

	public GoTweenCollectionConfig onIterationStart(global::System.Action<AbstractGoTween> onIterationStart)
	{
		onIterationStartHandler = onIterationStart;
		return this;
	}

	public GoTweenCollectionConfig onUpdate(global::System.Action<AbstractGoTween> onUpdate)
	{
		onUpdateHandler = onUpdate;
		return this;
	}

	public GoTweenCollectionConfig onIterationEnd(global::System.Action<AbstractGoTween> onIterationEnd)
	{
		onIterationEndHandler = onIterationEnd;
		return this;
	}

	public GoTweenCollectionConfig onComplete(global::System.Action<AbstractGoTween> onComplete)
	{
		onCompleteHandler = onComplete;
		return this;
	}

	public GoTweenCollectionConfig setId(int id)
	{
		this.id = id;
		return this;
	}
}
