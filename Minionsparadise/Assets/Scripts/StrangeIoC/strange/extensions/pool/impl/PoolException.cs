namespace strange.extensions.pool.impl
{
	public class PoolException : global::System.Exception
	{
		public global::strange.extensions.pool.api.PoolExceptionType type { get; set; }

		public PoolException()
		{
		}

		public PoolException(string message, global::strange.extensions.pool.api.PoolExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
