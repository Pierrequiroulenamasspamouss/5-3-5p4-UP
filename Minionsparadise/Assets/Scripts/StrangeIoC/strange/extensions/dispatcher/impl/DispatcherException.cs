namespace strange.extensions.dispatcher.impl
{
	public class DispatcherException : global::System.Exception
	{
		public global::strange.extensions.dispatcher.api.DispatcherExceptionType type { get; set; }

		public DispatcherException()
		{
		}

		public DispatcherException(string message, global::strange.extensions.dispatcher.api.DispatcherExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
