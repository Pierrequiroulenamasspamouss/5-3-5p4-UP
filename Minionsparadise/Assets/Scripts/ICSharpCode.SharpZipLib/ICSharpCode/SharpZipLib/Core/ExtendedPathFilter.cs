namespace ICSharpCode.SharpZipLib.Core
{
	public class ExtendedPathFilter : global::ICSharpCode.SharpZipLib.Core.PathFilter
	{
		private long minSize_;

		private long maxSize_ = long.MaxValue;

		private global::System.DateTime minDate_ = global::System.DateTime.MinValue;

		private global::System.DateTime maxDate_ = global::System.DateTime.MaxValue;

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

		public global::System.DateTime MinDate
		{
			get
			{
				return minDate_;
			}
			set
			{
				if (value > maxDate_)
				{
					throw new global::System.ArgumentOutOfRangeException("value", "Exceeds MaxDate");
				}
				minDate_ = value;
			}
		}

		public global::System.DateTime MaxDate
		{
			get
			{
				return maxDate_;
			}
			set
			{
				if (minDate_ > value)
				{
					throw new global::System.ArgumentOutOfRangeException("value", "Exceeds MinDate");
				}
				maxDate_ = value;
			}
		}

		public ExtendedPathFilter(string filter, long minSize, long maxSize)
			: base(filter)
		{
			MinSize = minSize;
			MaxSize = maxSize;
		}

		public ExtendedPathFilter(string filter, global::System.DateTime minDate, global::System.DateTime maxDate)
			: base(filter)
		{
			MinDate = minDate;
			MaxDate = maxDate;
		}

		public ExtendedPathFilter(string filter, long minSize, long maxSize, global::System.DateTime minDate, global::System.DateTime maxDate)
			: base(filter)
		{
			MinSize = minSize;
			MaxSize = maxSize;
			MinDate = minDate;
			MaxDate = maxDate;
		}

		public override bool IsMatch(string name)
		{
			bool flag = base.IsMatch(name);
			if (flag)
			{
#if !UNITY_WEBPLAYER
				global::System.IO.FileInfo fileInfo = new global::System.IO.FileInfo(name);
				flag = MinSize <= fileInfo.Length && MaxSize >= fileInfo.Length && MinDate <= fileInfo.LastWriteTime && MaxDate >= fileInfo.LastWriteTime;
#endif
			}
			return flag;
		}
	}
}
