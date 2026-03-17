namespace ICSharpCode.SharpZipLib.Zip
{
	public class NTTaggedData : global::ICSharpCode.SharpZipLib.Zip.ITaggedData
	{
		private global::System.DateTime _lastAccessTime = global::System.DateTime.FromFileTime(0L);

		private global::System.DateTime _lastModificationTime = global::System.DateTime.FromFileTime(0L);

		private global::System.DateTime _createTime = global::System.DateTime.FromFileTime(0L);

		public short TagID
		{
			get
			{
				return 10;
			}
		}

		public global::System.DateTime LastModificationTime
		{
			get
			{
				return _lastModificationTime;
			}
			set
			{
				if (!IsValidValue(value))
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_lastModificationTime = value;
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
				_createTime = value;
			}
		}

		public global::System.DateTime LastAccessTime
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
				_lastAccessTime = value;
			}
		}

		public void SetData(byte[] data, int index, int count)
		{
			using (global::System.IO.MemoryStream stream = new global::System.IO.MemoryStream(data, index, count, false))
			{
				using (global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(stream))
				{
					zipHelperStream.ReadLEInt();
					while (zipHelperStream.Position < zipHelperStream.Length)
					{
						int num = zipHelperStream.ReadLEShort();
						int num2 = zipHelperStream.ReadLEShort();
						if (num == 1)
						{
							if (num2 >= 24)
							{
								long fileTime = zipHelperStream.ReadLELong();
								_lastModificationTime = global::System.DateTime.FromFileTime(fileTime);
								long fileTime2 = zipHelperStream.ReadLELong();
								_lastAccessTime = global::System.DateTime.FromFileTime(fileTime2);
								long fileTime3 = zipHelperStream.ReadLELong();
								_createTime = global::System.DateTime.FromFileTime(fileTime3);
							}
							break;
						}
						zipHelperStream.Seek(num2, global::System.IO.SeekOrigin.Current);
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
					zipHelperStream.WriteLEInt(0);
					zipHelperStream.WriteLEShort(1);
					zipHelperStream.WriteLEShort(24);
					zipHelperStream.WriteLELong(_lastModificationTime.ToFileTime());
					zipHelperStream.WriteLELong(_lastAccessTime.ToFileTime());
					zipHelperStream.WriteLELong(_createTime.ToFileTime());
					return memoryStream.ToArray();
				}
			}
		}

		public static bool IsValidValue(global::System.DateTime value)
		{
			bool result = true;
			try
			{
				value.ToFileTimeUtc();
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
