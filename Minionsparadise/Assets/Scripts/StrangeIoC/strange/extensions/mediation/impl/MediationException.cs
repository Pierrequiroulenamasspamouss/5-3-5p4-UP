namespace strange.extensions.mediation.impl
{
	public class MediationException : global::System.Exception
	{
		public global::strange.extensions.mediation.api.MediationExceptionType type { get; set; }

		public MediationException()
		{
		}

		public MediationException(string message, global::strange.extensions.mediation.api.MediationExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}
