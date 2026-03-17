namespace ICSharpCode.SharpZipLib.Zip
{
	public class StaticDiskDataSource : global::ICSharpCode.SharpZipLib.Zip.IStaticDataSource
	{
		private string fileName_;

		public StaticDiskDataSource(string fileName)
		{
			fileName_ = fileName;
		}

		public global::System.IO.Stream GetSource()
		{
			return global::System.IO.File.Open(fileName_, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.Read);
		}
	}
}
