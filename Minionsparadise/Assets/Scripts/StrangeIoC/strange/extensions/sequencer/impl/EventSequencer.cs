namespace strange.extensions.sequencer.impl
{
	public class EventSequencer : global::strange.extensions.sequencer.impl.Sequencer
	{
		protected override global::strange.extensions.sequencer.api.ISequenceCommand createCommand(object cmd, object data)
		{
			base.injectionBinder.Bind<global::strange.extensions.sequencer.api.ISequenceCommand>().To(cmd);
			if (data is global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)
			{
				base.injectionBinder.Bind<global::strange.extensions.dispatcher.eventdispatcher.api.IEvent>().ToValue(data).ToInject(false);
			}
			global::strange.extensions.sequencer.api.ISequenceCommand instance = base.injectionBinder.GetInstance<global::strange.extensions.sequencer.api.ISequenceCommand>();
			instance.data = data;
			if (data is global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)
			{
				base.injectionBinder.Unbind<global::strange.extensions.dispatcher.eventdispatcher.api.IEvent>();
			}
			base.injectionBinder.Unbind<global::strange.extensions.sequencer.api.ISequenceCommand>();
			return instance;
		}
	}
}
