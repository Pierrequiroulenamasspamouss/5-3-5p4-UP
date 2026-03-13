namespace strange.extensions.injector.impl
{
	public class InjectionException : global::System.Exception
	{
		public global::strange.extensions.injector.api.InjectionExceptionType type { get; set; }

		public InjectionException()
		{
		}

		public InjectionException(string message, global::strange.extensions.injector.api.InjectionExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
