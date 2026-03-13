namespace strange.extensions.sequencer.impl
{
	public class EventSequenceCommand : global::strange.extensions.sequencer.impl.SequenceCommand
	{
		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_DISPATCHER)]
		public global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher dispatcher { get; set; }

		[Inject]
		public global::strange.extensions.dispatcher.eventdispatcher.api.IEvent evt { get; set; }
	}
}
