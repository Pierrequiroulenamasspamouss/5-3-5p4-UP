namespace strange.extensions.context.impl
{
	public class ContextException : global::System.Exception
	{
		public global::strange.extensions.context.api.ContextExceptionType type { get; set; }

		public ContextException()
		{
		}

		public ContextException(string message, global::strange.extensions.context.api.ContextExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
