namespace ICSharpCode.SharpZipLib.Zip
{
	public interface IEntryFactory
	{
		global::ICSharpCode.SharpZipLib.Core.INameTransform NameTransform { get; set; }

		global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeFileEntry(string fileName);

		global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeFileEntry(string fileName, bool useFileSystem);

		global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeFileEntry(string fileName, string entryName, bool useFileSystem);

		global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeDirectoryEntry(string directoryName);

		global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeDirectoryEntry(string directoryName, bool useFileSystem);
	}
}
