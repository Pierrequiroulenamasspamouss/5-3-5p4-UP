namespace strange.extensions.dispatcher.eventdispatcher.api
{
	public interface IEventDispatcher : global::strange.extensions.dispatcher.api.IDispatcher
	{
		global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding Bind(object key);

		void AddListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback);

		void AddListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback);

		void RemoveListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback);

		void RemoveListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback);

		bool HasListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback);

		bool HasListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback);

		void UpdateListener(bool toAdd, object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback);

		void UpdateListener(bool toAdd, object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback);

		void ReleaseEvent(global::strange.extensions.dispatcher.eventdispatcher.api.IEvent evt);
	}
}
