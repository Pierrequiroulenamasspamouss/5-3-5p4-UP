namespace ICSharpCode.SharpZipLib.Core
{
	[global::System.Obsolete("Use ExtendedPathFilter instead")]
	public class NameAndSizeFilter : global::ICSharpCode.SharpZipLib.Core.PathFilter
	{
		private long minSize_;

		private long maxSize_ = long.MaxValue;

		public long MinSize
		{
			get
			{
				return minSize_;
			}
			set
			{
				if (value < 0 || maxSize_ < value)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				minSize_ = value;
			}
		}

		public long MaxSize
		{
			get
			{
				return maxSize_;
			}
			set
			{
				if (value < 0 || minSize_ > value)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				maxSize_ = value;
			}
		}

		public NameAndSizeFilter(string filter, long minSize, long maxSize)
			: base(filter)
		{
			MinSize = minSize;
			MaxSize = maxSize;
		}

		public override bool IsMatch(string name)
		{
			bool flag = base.IsMatch(name);
			if (flag)
			{
#if !UNITY_WEBPLAYER
				global::System.IO.FileInfo fileInfo = new global::System.IO.FileInfo(name);
				long length = fileInfo.Length;
				flag = MinSize <= length && MaxSize >= length;
#endif
			}
			return flag;
		}
	}
}
