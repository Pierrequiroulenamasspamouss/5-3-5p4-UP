namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	public class EventDispatcherException : global::System.Exception
	{
		public global::strange.extensions.dispatcher.eventdispatcher.api.EventDispatcherExceptionType type { get; set; }

		public EventDispatcherException()
		{
		}

		public EventDispatcherException(string message, global::strange.extensions.dispatcher.eventdispatcher.api.EventDispatcherExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
