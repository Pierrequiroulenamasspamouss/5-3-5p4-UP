namespace strange.extensions.sequencer.impl
{
	public class SequencerException : global::System.Exception
	{
		public global::strange.extensions.sequencer.api.SequencerExceptionType type { get; set; }

		public SequencerException()
		{
		}

		public SequencerException(string message, global::strange.extensions.sequencer.api.SequencerExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
