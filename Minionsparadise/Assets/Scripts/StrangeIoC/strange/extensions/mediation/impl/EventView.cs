namespace strange.extensions.mediation.impl
{
	public class EventView : global::strange.extensions.mediation.impl.View
	{
		[Inject]
		public global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher dispatcher { get; set; }
	}
}
