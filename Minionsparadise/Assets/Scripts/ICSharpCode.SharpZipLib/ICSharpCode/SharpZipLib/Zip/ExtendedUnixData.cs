namespace ICSharpCode.SharpZipLib.Zip
{
	public class ExtendedUnixData : global::ICSharpCode.SharpZipLib.Zip.ITaggedData
	{
		[global::System.Flags]
		public enum Flags : byte
		{
			ModificationTime = 1,
			AccessTime = 2,
			CreateTime = 4
		}

		private global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags _flags;

		private global::System.DateTime _modificationTime = new global::System.DateTime(1970, 1, 1);

		private global::System.DateTime _lastAccessTime = new global::System.DateTime(1970, 1, 1);

		private global::System.DateTime _createTime = new global::System.DateTime(1970, 1, 1);

		public short TagID
		{
			get
			{
				return 21589;
			}
		}

		public global::System.DateTime ModificationTime
		{
			get
			{
				return _modificationTime;
			}
			set
			{
				if (!IsValidValue(value))
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_flags |= global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags.ModificationTime;
				_modificationTime = value;
			}
		}

		public global::System.DateTime AccessTime
		{
			get
			{
				return _lastAccessTime;
			}
			set
			{
				if (!IsValidValue(value))
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_flags |= global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags.AccessTime;
				_lastAccessTime = value;
			}
		}

		public global::System.DateTime CreateTime
		{
			get
			{
				return _createTime;
			}
			set
			{
				if (!IsValidValue(value))
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_flags |= global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags.CreateTime;
				_createTime = value;
			}
		}

		private global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags Include
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}

		public void SetData(byte[] data, int index, int count)
		{
			using (global::System.IO.MemoryStream stream = new global::System.IO.MemoryStream(data, index, count, false))
			{
				using (global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(stream))
				{
					_flags = (global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags)zipHelperStream.ReadByte();
					if ((_flags & global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags.ModificationTime) != 0 && count >= 5)
					{
						int seconds = zipHelperStream.ReadLEInt();
						_modificationTime = (new global::System.DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new global::System.TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
					}
					if ((_flags & global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags.AccessTime) != 0)
					{
						int seconds2 = zipHelperStream.ReadLEInt();
						_lastAccessTime = (new global::System.DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new global::System.TimeSpan(0, 0, 0, seconds2, 0)).ToLocalTime();
					}
					if ((_flags & global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags.CreateTime) != 0)
					{
						int seconds3 = zipHelperStream.ReadLEInt();
						_createTime = (new global::System.DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new global::System.TimeSpan(0, 0, 0, seconds3, 0)).ToLocalTime();
					}
				}
			}
		}

		public byte[] GetData()
		{
			using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream())
			{
				using (global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(memoryStream))
				{
					zipHelperStream.IsStreamOwner = false;
					zipHelperStream.WriteByte((byte)_flags);
					if ((_flags & global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags.ModificationTime) != 0)
					{
						int value = (int)(_modificationTime.ToUniversalTime() - new global::System.DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value);
					}
					if ((_flags & global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags.AccessTime) != 0)
					{
						int value2 = (int)(_lastAccessTime.ToUniversalTime() - new global::System.DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value2);
					}
					if ((_flags & global::ICSharpCode.SharpZipLib.Zip.ExtendedUnixData.Flags.CreateTime) != 0)
					{
						int value3 = (int)(_createTime.ToUniversalTime() - new global::System.DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value3);
					}
					return memoryStream.ToArray();
				}
			}
		}

		public static bool IsValidValue(global::System.DateTime value)
		{
			if (!(value >= new global::System.DateTime(1901, 12, 13, 20, 45, 52)))
			{
				return value <= new global::System.DateTime(2038, 1, 19, 3, 14, 7);
			}
			return true;
		}
	}
}
