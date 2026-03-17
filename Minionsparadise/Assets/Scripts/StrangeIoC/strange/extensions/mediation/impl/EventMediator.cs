namespace strange.extensions.mediation.impl
{
	public class EventMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_DISPATCHER)]
		public global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher dispatcher { get; set; }
	}
}
