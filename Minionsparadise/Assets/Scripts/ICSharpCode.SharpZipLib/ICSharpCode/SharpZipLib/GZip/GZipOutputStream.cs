namespace ICSharpCode.SharpZipLib.GZip
{
	public class GZipOutputStream : global::ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream
	{
		private enum OutputState
		{
			Header = 0,
			Footer = 1,
			Finished = 2,
			Closed = 3
		}

		protected global::ICSharpCode.SharpZipLib.Checksums.Crc32 crc = new global::ICSharpCode.SharpZipLib.Checksums.Crc32();

		private global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState state_;

		public GZipOutputStream(global::System.IO.Stream baseOutputStream)
			: this(baseOutputStream, 4096)
		{
		}

		public GZipOutputStream(global::System.IO.Stream baseOutputStream, int size)
			: base(baseOutputStream, new global::ICSharpCode.SharpZipLib.Zip.Compression.Deflater(-1, true), size)
		{
		}

		public void SetLevel(int level)
		{
			if (level < 1)
			{
				throw new global::System.ArgumentOutOfRangeException("level");
			}
			deflater_.SetLevel(level);
		}

		public int GetLevel()
		{
			return deflater_.GetLevel();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (state_ == global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState.Header)
			{
				WriteHeader();
			}
			if (state_ != global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState.Footer)
			{
				throw new global::System.InvalidOperationException("Write not permitted in current state");
			}
			crc.Update(buffer, offset, count);
			base.Write(buffer, offset, count);
		}

		public override void Close()
		{
			try
			{
				Finish();
			}
			finally
			{
				if (state_ != global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState.Closed)
				{
					state_ = global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState.Closed;
					if (base.IsStreamOwner)
					{
						baseOutputStream_.Close();
					}
				}
			}
		}

		public override void Finish()
		{
			if (state_ == global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState.Header)
			{
				WriteHeader();
			}
			if (state_ == global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState.Footer)
			{
				state_ = global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState.Finished;
				base.Finish();
				uint num = (uint)(deflater_.TotalIn & 0xFFFFFFFFu);
				uint num2 = (uint)(crc.Value & 0xFFFFFFFFu);
				byte[] array = new byte[8]
				{
					(byte)num2,
					(byte)(num2 >> 8),
					(byte)(num2 >> 16),
					(byte)(num2 >> 24),
					(byte)num,
					(byte)(num >> 8),
					(byte)(num >> 16),
					(byte)(num >> 24)
				};
				baseOutputStream_.Write(array, 0, array.Length);
			}
		}

		private void WriteHeader()
		{
			if (state_ == global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState.Header)
			{
				state_ = global::ICSharpCode.SharpZipLib.GZip.GZipOutputStream.OutputState.Footer;
				int num = (int)((global::System.DateTime.Now.Ticks - new global::System.DateTime(1970, 1, 1).Ticks) / 10000000);
				byte[] array = new byte[10] { 31, 139, 8, 0, 0, 0, 0, 0, 0, 255 };
				array[4] = (byte)num;
				array[5] = (byte)(num >> 8);
				array[6] = (byte)(num >> 16);
				array[7] = (byte)(num >> 24);
				byte[] array2 = array;
				baseOutputStream_.Write(array2, 0, array2.Length);
			}
		}
	}
}
