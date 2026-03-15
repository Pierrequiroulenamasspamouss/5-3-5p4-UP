namespace strange.extensions.context.api
{
	public interface IContext : global::strange.framework.api.IBinder
	{
		global::strange.extensions.context.api.IContext Start();

		void Launch();

		global::strange.extensions.context.api.IContext AddContext(global::strange.extensions.context.api.IContext context);

		global::strange.extensions.context.api.IContext RemoveContext(global::strange.extensions.context.api.IContext context);

		void AddView(object view);

		void RemoveView(object view);

		object GetContextView();
	}
}
