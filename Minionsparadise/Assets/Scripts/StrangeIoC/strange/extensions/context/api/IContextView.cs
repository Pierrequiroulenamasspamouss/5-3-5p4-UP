namespace strange.extensions.context.api
{
	public interface IContextView : global::strange.extensions.mediation.api.IView
	{
		global::strange.extensions.context.api.IContext context { get; set; }
	}
}
