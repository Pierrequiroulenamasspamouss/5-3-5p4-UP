namespace strange.extensions.dispatcher.eventdispatcher.api
{
	public interface IEventBinding : global::strange.framework.api.IBinding
	{
		global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType TypeForCallback(global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback);

		global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType TypeForCallback(global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback);

		new global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding Bind(object key);

		global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding To(global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback);

		global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding To(global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback);
	}
}
