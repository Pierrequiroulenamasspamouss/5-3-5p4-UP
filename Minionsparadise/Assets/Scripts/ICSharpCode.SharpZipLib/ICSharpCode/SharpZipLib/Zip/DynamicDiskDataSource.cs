namespace ICSharpCode.SharpZipLib.Zip
{
	public class DynamicDiskDataSource : global::ICSharpCode.SharpZipLib.Zip.IDynamicDataSource
	{
		public global::System.IO.Stream GetSource(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry, string name)
		{
			global::System.IO.Stream result = null;
			if (name != null)
			{
				result = global::System.IO.File.Open(name, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.Read);
			}
			return result;
		}
	}
}
