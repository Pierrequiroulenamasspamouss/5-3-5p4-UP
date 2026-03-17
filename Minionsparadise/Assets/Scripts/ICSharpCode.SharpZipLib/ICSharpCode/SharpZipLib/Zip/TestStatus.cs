namespace ICSharpCode.SharpZipLib.Zip
{
	public class TestStatus
	{
		private global::ICSharpCode.SharpZipLib.Zip.ZipFile file_;

		private global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry_;

		private bool entryValid_;

		private int errorCount_;

		private long bytesTested_;

		private global::ICSharpCode.SharpZipLib.Zip.TestOperation operation_;

		public global::ICSharpCode.SharpZipLib.Zip.TestOperation Operation
		{
			get
			{
				return operation_;
			}
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipFile File
		{
			get
			{
				return file_;
			}
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipEntry Entry
		{
			get
			{
				return entry_;
			}
		}

		public int ErrorCount
		{
			get
			{
				return errorCount_;
			}
		}

		public long BytesTested
		{
			get
			{
				return bytesTested_;
			}
		}

		public bool EntryValid
		{
			get
			{
				return entryValid_;
			}
		}

		public TestStatus(global::ICSharpCode.SharpZipLib.Zip.ZipFile file)
		{
			file_ = file;
		}

		internal void AddError()
		{
			errorCount_++;
			entryValid_ = false;
		}

		internal void SetOperation(global::ICSharpCode.SharpZipLib.Zip.TestOperation operation)
		{
			operation_ = operation;
		}

		internal void SetEntry(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			entry_ = entry;
			entryValid_ = true;
			bytesTested_ = 0L;
		}

		internal void SetBytesTested(long value)
		{
			bytesTested_ = value;
		}
	}
}
