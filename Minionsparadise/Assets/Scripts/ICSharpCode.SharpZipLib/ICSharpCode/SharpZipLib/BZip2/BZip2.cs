namespace ICSharpCode.SharpZipLib.BZip2
{
	public static class BZip2
	{
		public static void Decompress(global::System.IO.Stream inStream, global::System.IO.Stream outStream, bool isStreamOwner)
		{
			if (inStream == null || outStream == null)
			{
				throw new global::System.Exception("Null Stream");
			}
			try
			{
				using (global::ICSharpCode.SharpZipLib.BZip2.BZip2InputStream bZip2InputStream = new global::ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(inStream))
				{
					bZip2InputStream.IsStreamOwner = isStreamOwner;
					global::ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(bZip2InputStream, outStream, new byte[4096]);
				}
			}
			finally
			{
				if (isStreamOwner)
				{
					outStream.Close();
				}
			}
		}

		public static void Compress(global::System.IO.Stream inStream, global::System.IO.Stream outStream, bool isStreamOwner, int level)
		{
			if (inStream == null || outStream == null)
			{
				throw new global::System.Exception("Null Stream");
			}
			try
			{
				using (global::ICSharpCode.SharpZipLib.BZip2.BZip2OutputStream bZip2OutputStream = new global::ICSharpCode.SharpZipLib.BZip2.BZip2OutputStream(outStream, level))
				{
					bZip2OutputStream.IsStreamOwner = isStreamOwner;
					global::ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(inStream, bZip2OutputStream, new byte[4096]);
				}
			}
			finally
			{
				if (isStreamOwner)
				{
					inStream.Close();
				}
			}
		}
	}
}
