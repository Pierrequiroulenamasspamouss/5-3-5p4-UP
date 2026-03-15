namespace strange.framework.impl
{
	public class BinderException : global::System.Exception
	{
		public global::strange.framework.api.BinderExceptionType type { get; set; }

		public BinderException()
		{
		}

		public BinderException(string message, global::strange.framework.api.BinderExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
