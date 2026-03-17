namespace strange.extensions.sequencer.api
{
	public interface ISequencer : global::strange.extensions.command.api.ICommandBinder, global::strange.framework.api.IBinder
	{
		void ReleaseCommand(global::strange.extensions.sequencer.api.ISequenceCommand command);

		new global::strange.extensions.sequencer.api.ISequenceBinding Bind<T>();

		new global::strange.extensions.sequencer.api.ISequenceBinding Bind(object value);
	}
}
