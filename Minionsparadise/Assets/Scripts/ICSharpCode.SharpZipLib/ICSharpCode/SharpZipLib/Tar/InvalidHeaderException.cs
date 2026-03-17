namespace ICSharpCode.SharpZipLib.Tar
{
	[global::System.Serializable]
	public class InvalidHeaderException : global::ICSharpCode.SharpZipLib.Tar.TarException
	{
		protected InvalidHeaderException(global::System.Runtime.Serialization.SerializationInfo information, global::System.Runtime.Serialization.StreamingContext context)
			: base(information, context)
		{
		}

		public InvalidHeaderException()
		{
		}

		public InvalidHeaderException(string message)
			: base(message)
		{
		}

		public InvalidHeaderException(string message, global::System.Exception exception)
			: base(message, exception)
		{
		}
	}
}
