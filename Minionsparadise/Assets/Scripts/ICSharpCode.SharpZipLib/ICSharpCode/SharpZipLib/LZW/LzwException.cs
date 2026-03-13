namespace ICSharpCode.SharpZipLib.LZW
{
	[global::System.Serializable]
	public class LzwException : global::ICSharpCode.SharpZipLib.SharpZipBaseException
	{
		protected LzwException(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}

		public LzwException()
		{
		}

		public LzwException(string message)
			: base(message)
		{
		}

		public LzwException(string message, global::System.Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
