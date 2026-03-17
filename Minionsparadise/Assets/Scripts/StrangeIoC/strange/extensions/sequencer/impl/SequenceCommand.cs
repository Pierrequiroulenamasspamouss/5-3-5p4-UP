namespace strange.extensions.sequencer.impl
{
	public class SequenceCommand : global::strange.extensions.command.impl.Command, global::strange.extensions.sequencer.api.ISequenceCommand, global::strange.extensions.command.api.ICommand
	{
		[Inject]
		public global::strange.extensions.sequencer.api.ISequencer sequencer { get; set; }

		public new void Fail()
		{
			if (sequencer != null)
			{
				sequencer.Stop(this);
			}
		}

		public new virtual void Execute()
		{
			throw new global::strange.extensions.sequencer.impl.SequencerException("You must override the Execute method in every SequenceCommand", global::strange.extensions.sequencer.api.SequencerExceptionType.EXECUTE_OVERRIDE);
		}

		public new void Release()
		{
			base.retain = false;
			if (sequencer != null)
			{
				sequencer.ReleaseCommand(this);
			}
		}
	}
}
