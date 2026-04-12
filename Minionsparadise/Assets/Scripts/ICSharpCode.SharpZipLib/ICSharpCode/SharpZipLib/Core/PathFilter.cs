namespace ICSharpCode.SharpZipLib.Core
{
	public class PathFilter : global::ICSharpCode.SharpZipLib.Core.IScanFilter
	{
		private global::ICSharpCode.SharpZipLib.Core.NameFilter nameFilter_;

		public PathFilter(string filter)
		{
			nameFilter_ = new global::ICSharpCode.SharpZipLib.Core.NameFilter(filter);
		}

		public virtual bool IsMatch(string name)
		{
			bool result = false;
			if (name != null)
			{
				string name2 = ((name.Length > 0) ? global::System.IO.Path.GetFullPath(name) : "");
				result = nameFilter_.IsMatch(name2);
			}
			return result;
		}
	}
}
