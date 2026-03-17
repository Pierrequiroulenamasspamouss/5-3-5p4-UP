namespace ICSharpCode.SharpZipLib.Zip
{
	public abstract class BaseArchiveStorage : global::ICSharpCode.SharpZipLib.Zip.IArchiveStorage
	{
		private global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode updateMode_;

		public global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode UpdateMode
		{
			get
			{
				return updateMode_;
			}
		}

		protected BaseArchiveStorage(global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode updateMode)
		{
			updateMode_ = updateMode;
		}

		public abstract global::System.IO.Stream GetTemporaryOutput();

		public abstract global::System.IO.Stream ConvertTemporaryToFinal();

		public abstract global::System.IO.Stream MakeTemporaryCopy(global::System.IO.Stream stream);

		public abstract global::System.IO.Stream OpenForDirectUpdate(global::System.IO.Stream stream);

		public abstract void Dispose();
	}
}
