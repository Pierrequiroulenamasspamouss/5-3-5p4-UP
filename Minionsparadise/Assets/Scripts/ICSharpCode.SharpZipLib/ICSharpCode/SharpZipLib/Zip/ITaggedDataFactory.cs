namespace ICSharpCode.SharpZipLib.Zip
{
	internal interface ITaggedDataFactory
	{
		global::ICSharpCode.SharpZipLib.Zip.ITaggedData Create(short tag, byte[] data, int offset, int count);
	}
}
