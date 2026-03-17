namespace strange.extensions.context.api
{
	public interface ICrossContextCapable
	{
		global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder { get; set; }

		global::strange.extensions.dispatcher.api.IDispatcher crossContextDispatcher { get; set; }

		void AssignCrossContext(global::strange.extensions.context.api.ICrossContextCapable childContext);

		void RemoveCrossContext(global::strange.extensions.context.api.ICrossContextCapable childContext);

		object GetComponent<T>();

		object GetComponent<T>(object name);
	}
}
