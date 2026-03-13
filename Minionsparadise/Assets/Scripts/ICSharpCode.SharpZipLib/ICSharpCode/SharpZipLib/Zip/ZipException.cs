namespace ICSharpCode.SharpZipLib.Zip
{
	[global::System.Serializable]
	public class ZipException : global::ICSharpCode.SharpZipLib.SharpZipBaseException
	{
		protected ZipException(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}

		public ZipException()
		{
		}

		public ZipException(string message)
			: base(message)
		{
		}

		public ZipException(string message, global::System.Exception exception)
			: base(message, exception)
		{
		}
	}
}
