namespace ICSharpCode.SharpZipLib.Tar
{
	[global::System.Serializable]
	public class TarException : global::ICSharpCode.SharpZipLib.SharpZipBaseException
	{
		protected TarException(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}

		public TarException()
		{
		}

		public TarException(string message)
			: base(message)
		{
		}

		public TarException(string message, global::System.Exception exception)
			: base(message, exception)
		{
		}
	}
}
