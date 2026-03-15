namespace strange.extensions.signal.impl
{
	public class SignalException : global::System.Exception
	{
		public global::strange.extensions.signal.api.SignalExceptionType type { get; set; }

		public SignalException()
		{
		}

		public SignalException(string message, global::strange.extensions.signal.api.SignalExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
