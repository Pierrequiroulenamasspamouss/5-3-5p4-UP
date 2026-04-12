namespace ICSharpCode.SharpZipLib.Zip
{
	public class ZipEntryFactory : global::ICSharpCode.SharpZipLib.Zip.IEntryFactory
	{
		public enum TimeSetting
		{
			LastWriteTime = 0,
			LastWriteTimeUtc = 1,
			CreateTime = 2,
			CreateTimeUtc = 3,
			LastAccessTime = 4,
			LastAccessTimeUtc = 5,
			Fixed = 6
		}

		private global::ICSharpCode.SharpZipLib.Core.INameTransform nameTransform_;

		private global::System.DateTime fixedDateTime_ = global::System.DateTime.Now;

		private global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting timeSetting_;

		private bool isUnicodeText_;

		private int getAttributes_ = -1;

		private int setAttributes_;

		public global::ICSharpCode.SharpZipLib.Core.INameTransform NameTransform
		{
			get
			{
				return nameTransform_;
			}
			set
			{
				if (value == null)
				{
					nameTransform_ = new global::ICSharpCode.SharpZipLib.Zip.ZipNameTransform();
				}
				else
				{
					nameTransform_ = value;
				}
			}
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting Setting
		{
			get
			{
				return timeSetting_;
			}
			set
			{
				timeSetting_ = value;
			}
		}

		public global::System.DateTime FixedDateTime
		{
			get
			{
				return fixedDateTime_;
			}
			set
			{
				if (value.Year < 1970)
				{
					throw new global::System.ArgumentException("Value is too old to be valid", "value");
				}
				fixedDateTime_ = value;
			}
		}

		public int GetAttributes
		{
			get
			{
				return getAttributes_;
			}
			set
			{
				getAttributes_ = value;
			}
		}

		public int SetAttributes
		{
			get
			{
				return setAttributes_;
			}
			set
			{
				setAttributes_ = value;
			}
		}

		public bool IsUnicodeText
		{
			get
			{
				return isUnicodeText_;
			}
			set
			{
				isUnicodeText_ = value;
			}
		}

		public ZipEntryFactory()
		{
			nameTransform_ = new global::ICSharpCode.SharpZipLib.Zip.ZipNameTransform();
		}

		public ZipEntryFactory(global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting timeSetting)
		{
			timeSetting_ = timeSetting;
			nameTransform_ = new global::ICSharpCode.SharpZipLib.Zip.ZipNameTransform();
		}

		public ZipEntryFactory(global::System.DateTime time)
		{
			timeSetting_ = global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.Fixed;
			FixedDateTime = time;
			nameTransform_ = new global::ICSharpCode.SharpZipLib.Zip.ZipNameTransform();
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeFileEntry(string fileName)
		{
			return MakeFileEntry(fileName, null, true);
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeFileEntry(string fileName, bool useFileSystem)
		{
			return MakeFileEntry(fileName, null, useFileSystem);
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeFileEntry(string fileName, string entryName, bool useFileSystem)
		{
			global::ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry(nameTransform_.TransformFile((entryName != null && entryName.Length > 0) ? entryName : fileName));
			zipEntry.IsUnicodeText = isUnicodeText_;
			int num = 0;
			bool flag = setAttributes_ != 0;
			global::System.IO.FileInfo fileInfo = null;
#if !UNITY_WEBPLAYER
			if (useFileSystem)
			{
				fileInfo = new global::System.IO.FileInfo(fileName);
			}
			if (fileInfo != null && fileInfo.Exists)
			{
				switch (timeSetting_)
				{
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.CreateTime:
					zipEntry.DateTime = fileInfo.CreationTime;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.CreateTimeUtc:
					zipEntry.DateTime = fileInfo.CreationTimeUtc;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.LastAccessTime:
					zipEntry.DateTime = fileInfo.LastAccessTime;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.LastAccessTimeUtc:
					zipEntry.DateTime = fileInfo.LastAccessTimeUtc;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.LastWriteTime:
					zipEntry.DateTime = fileInfo.LastWriteTime;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.LastWriteTimeUtc:
					zipEntry.DateTime = fileInfo.LastWriteTimeUtc;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.Fixed:
					zipEntry.DateTime = fixedDateTime_;
					break;
				default:
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Unhandled time setting in MakeFileEntry");
				}
				zipEntry.Size = fileInfo.Length;
				flag = true;
				num = (int)fileInfo.Attributes & getAttributes_;
			}
#else
			if (timeSetting_ == global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.Fixed)
			{
				zipEntry.DateTime = fixedDateTime_;
			}
#endif
			else if (timeSetting_ == global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.Fixed)
			{
				zipEntry.DateTime = fixedDateTime_;
			}
			if (flag)
			{
				num |= setAttributes_;
				zipEntry.ExternalFileAttributes = num;
			}
			return zipEntry;
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeDirectoryEntry(string directoryName)
		{
			return MakeDirectoryEntry(directoryName, true);
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipEntry MakeDirectoryEntry(string directoryName, bool useFileSystem)
		{
			global::ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry(nameTransform_.TransformDirectory(directoryName));
			zipEntry.IsUnicodeText = isUnicodeText_;
			zipEntry.Size = 0L;
			int num = 0;
			global::System.IO.DirectoryInfo directoryInfo = null;
#if !UNITY_WEBPLAYER
			if (useFileSystem)
			{
				directoryInfo = new global::System.IO.DirectoryInfo(directoryName);
			}
			if (directoryInfo != null && directoryInfo.Exists)
			{
				switch (timeSetting_)
				{
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.CreateTime:
					zipEntry.DateTime = directoryInfo.CreationTime;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.CreateTimeUtc:
					zipEntry.DateTime = directoryInfo.CreationTimeUtc;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.LastAccessTime:
					zipEntry.DateTime = directoryInfo.LastAccessTime;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.LastAccessTimeUtc:
					zipEntry.DateTime = directoryInfo.LastAccessTimeUtc;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.LastWriteTime:
					zipEntry.DateTime = directoryInfo.LastWriteTime;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.LastWriteTimeUtc:
					zipEntry.DateTime = directoryInfo.LastWriteTimeUtc;
					break;
				case global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.Fixed:
					zipEntry.DateTime = fixedDateTime_;
					break;
				default:
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Unhandled time setting in MakeDirectoryEntry");
				}
				num = (int)directoryInfo.Attributes & getAttributes_;
			}
#else
			if (timeSetting_ == global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.Fixed)
			{
				zipEntry.DateTime = fixedDateTime_;
			}
#endif
			else if (timeSetting_ == global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory.TimeSetting.Fixed)
			{
				zipEntry.DateTime = fixedDateTime_;
			}
			num |= setAttributes_ | 0x10;
			zipEntry.ExternalFileAttributes = num;
			return zipEntry;
		}
	}
}
