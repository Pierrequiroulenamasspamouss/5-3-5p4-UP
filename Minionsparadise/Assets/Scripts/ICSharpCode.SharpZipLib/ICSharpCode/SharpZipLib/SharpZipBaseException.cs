namespace ICSharpCode.SharpZipLib
{
	[global::System.Serializable]
	public class SharpZipBaseException : global::System.ApplicationException
	{
		protected SharpZipBaseException(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}

		public SharpZipBaseException()
		{
		}

		public SharpZipBaseException(string message)
			: base(message)
		{
		}

		public SharpZipBaseException(string message, global::System.Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
