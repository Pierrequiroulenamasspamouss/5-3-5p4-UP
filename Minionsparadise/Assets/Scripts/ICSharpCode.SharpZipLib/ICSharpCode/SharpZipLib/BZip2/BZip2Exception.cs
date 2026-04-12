namespace ICSharpCode.SharpZipLib.BZip2
{
	[global::System.Serializable]
	public class BZip2Exception : global::ICSharpCode.SharpZipLib.SharpZipBaseException
	{
		protected BZip2Exception(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}

		public BZip2Exception()
		{
		}

		public BZip2Exception(string message)
			: base(message)
		{
		}

		public BZip2Exception(string message, global::System.Exception exception)
			: base(message, exception)
		{
		}
	}
}
