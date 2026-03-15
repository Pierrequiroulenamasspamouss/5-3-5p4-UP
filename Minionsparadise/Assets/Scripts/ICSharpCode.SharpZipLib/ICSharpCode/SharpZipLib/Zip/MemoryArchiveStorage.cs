namespace ICSharpCode.SharpZipLib.Zip
{
	public class MemoryArchiveStorage : global::ICSharpCode.SharpZipLib.Zip.BaseArchiveStorage
	{
		private global::System.IO.MemoryStream temporaryStream_;

		private global::System.IO.MemoryStream finalStream_;

		public global::System.IO.MemoryStream FinalStream
		{
			get
			{
				return finalStream_;
			}
		}

		public MemoryArchiveStorage()
			: base(global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode.Direct)
		{
		}

		public MemoryArchiveStorage(global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode updateMode)
			: base(updateMode)
		{
		}

		public override global::System.IO.Stream GetTemporaryOutput()
		{
			temporaryStream_ = new global::System.IO.MemoryStream();
			return temporaryStream_;
		}

		public override global::System.IO.Stream ConvertTemporaryToFinal()
		{
			if (temporaryStream_ == null)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("No temporary stream has been created");
			}
			finalStream_ = new global::System.IO.MemoryStream(temporaryStream_.ToArray());
			return finalStream_;
		}

		public override global::System.IO.Stream MakeTemporaryCopy(global::System.IO.Stream stream)
		{
			temporaryStream_ = new global::System.IO.MemoryStream();
			stream.Position = 0L;
			global::ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(stream, temporaryStream_, new byte[4096]);
			return temporaryStream_;
		}

		public override global::System.IO.Stream OpenForDirectUpdate(global::System.IO.Stream stream)
		{
			global::System.IO.Stream stream2;
			if (stream == null || !stream.CanWrite)
			{
				stream2 = new global::System.IO.MemoryStream();
				if (stream != null)
				{
					stream.Position = 0L;
					global::ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(stream, stream2, new byte[4096]);
					stream.Close();
				}
			}
			else
			{
				stream2 = stream;
			}
			return stream2;
		}

		public override void Dispose()
		{
			if (temporaryStream_ != null)
			{
				temporaryStream_.Close();
			}
		}
	}
}
