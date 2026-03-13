namespace strange.extensions.command.impl
{
	public class EventCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_DISPATCHER)]
		public global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher dispatcher { get; set; }

		[Inject]
		public global::strange.extensions.dispatcher.eventdispatcher.api.IEvent evt { get; set; }

		public override void Retain()
		{
			base.Retain();
		}

		public override void Release()
		{
			base.Release();
		}
	}
}
