namespace ICSharpCode.SharpZipLib.Zip
{
	public interface IDynamicDataSource
	{
		global::System.IO.Stream GetSource(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry, string name);
	}
}
