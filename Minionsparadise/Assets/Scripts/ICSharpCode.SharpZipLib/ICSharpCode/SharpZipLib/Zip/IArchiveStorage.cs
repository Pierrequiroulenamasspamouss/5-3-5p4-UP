namespace ICSharpCode.SharpZipLib.Zip
{
	public interface IArchiveStorage
	{
		global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode UpdateMode { get; }

		global::System.IO.Stream GetTemporaryOutput();

		global::System.IO.Stream ConvertTemporaryToFinal();

		global::System.IO.Stream MakeTemporaryCopy(global::System.IO.Stream stream);

		global::System.IO.Stream OpenForDirectUpdate(global::System.IO.Stream stream);

		void Dispose();
	}
}
