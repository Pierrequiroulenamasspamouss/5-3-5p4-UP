namespace strange.extensions.dispatcher.eventdispatcher.api
{
	public interface IEvent
	{
		object type { get; set; }

		global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher target { get; set; }

		object data { get; set; }
	}
}
