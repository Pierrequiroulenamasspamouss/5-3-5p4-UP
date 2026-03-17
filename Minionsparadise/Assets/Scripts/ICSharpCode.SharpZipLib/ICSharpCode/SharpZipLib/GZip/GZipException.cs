namespace ICSharpCode.SharpZipLib.GZip
{
	[global::System.Serializable]
	public class GZipException : global::ICSharpCode.SharpZipLib.SharpZipBaseException
	{
		protected GZipException(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}

		public GZipException()
		{
		}

		public GZipException(string message)
			: base(message)
		{
		}

		public GZipException(string message, global::System.Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
