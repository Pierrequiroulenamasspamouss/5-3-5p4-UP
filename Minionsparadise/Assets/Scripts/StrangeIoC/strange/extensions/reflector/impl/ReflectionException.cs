namespace strange.extensions.reflector.impl
{
	public class ReflectionException : global::System.Exception
	{
		public global::strange.extensions.reflector.api.ReflectionExceptionType type { get; set; }

		public ReflectionException()
		{
		}

		public ReflectionException(string message, global::strange.extensions.reflector.api.ReflectionExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
