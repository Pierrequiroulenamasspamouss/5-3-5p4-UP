namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	public class InflaterInputStream : global::System.IO.Stream
	{
		protected global::ICSharpCode.SharpZipLib.Zip.Compression.Inflater inf;

		protected global::ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputBuffer inputBuffer;

		private global::System.IO.Stream baseInputStream;

		protected long csize;

		private bool isClosed;

		private bool isStreamOwner = true;

		public bool IsStreamOwner
		{
			get
			{
				return isStreamOwner;
			}
			set
			{
				isStreamOwner = value;
			}
		}

		public virtual int Available
		{
			get
			{
				if (!inf.IsFinished)
				{
					return 1;
				}
				return 0;
			}
		}

		public override bool CanRead
		{
			get
			{
				return baseInputStream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override long Length
		{
			get
			{
				return inputBuffer.RawLength;
			}
		}

		public override long Position
		{
			get
			{
				return baseInputStream.Position;
			}
			set
			{
				throw new global::System.NotSupportedException("InflaterInputStream Position not supported");
			}
		}

		public InflaterInputStream(global::System.IO.Stream baseInputStream)
			: this(baseInputStream, new global::ICSharpCode.SharpZipLib.Zip.Compression.Inflater(), 4096)
		{
		}

		public InflaterInputStream(global::System.IO.Stream baseInputStream, global::ICSharpCode.SharpZipLib.Zip.Compression.Inflater inf)
			: this(baseInputStream, inf, 4096)
		{
		}

		public InflaterInputStream(global::System.IO.Stream baseInputStream, global::ICSharpCode.SharpZipLib.Zip.Compression.Inflater inflater, int bufferSize)
		{
			if (baseInputStream == null)
			{
				throw new global::System.ArgumentNullException("baseInputStream");
			}
			if (inflater == null)
			{
				throw new global::System.ArgumentNullException("inflater");
			}
			if (bufferSize <= 0)
			{
				throw new global::System.ArgumentOutOfRangeException("bufferSize");
			}
			this.baseInputStream = baseInputStream;
			inf = inflater;
			inputBuffer = new global::ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputBuffer(baseInputStream, bufferSize);
		}

		public long Skip(long count)
		{
			if (count <= 0)
			{
				throw new global::System.ArgumentOutOfRangeException("count");
			}
			if (baseInputStream.CanSeek)
			{
				baseInputStream.Seek(count, global::System.IO.SeekOrigin.Current);
				return count;
			}
			int num = 2048;
			if (count < num)
			{
				num = (int)count;
			}
			byte[] buffer = new byte[num];
			int num2 = 1;
			long num3 = count;
			while (num3 > 0 && num2 > 0)
			{
				if (num3 < num)
				{
					num = (int)num3;
				}
				num2 = baseInputStream.Read(buffer, 0, num);
				num3 -= num2;
			}
			return count - num3;
		}

		protected void StopDecrypting()
		{
			inputBuffer.CryptoTransform = null;
		}

		protected void Fill()
		{
			if (inputBuffer.Available <= 0)
			{
				inputBuffer.Fill();
				if (inputBuffer.Available <= 0)
				{
					throw new global::ICSharpCode.SharpZipLib.SharpZipBaseException("Unexpected EOF");
				}
			}
			inputBuffer.SetInflaterInput(inf);
		}

		public override void Flush()
		{
			baseInputStream.Flush();
		}

		public override long Seek(long offset, global::System.IO.SeekOrigin origin)
		{
			throw new global::System.NotSupportedException("Seek not supported");
		}

		public override void SetLength(long value)
		{
			throw new global::System.NotSupportedException("InflaterInputStream SetLength not supported");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new global::System.NotSupportedException("InflaterInputStream Write not supported");
		}

		public override void WriteByte(byte value)
		{
			throw new global::System.NotSupportedException("InflaterInputStream WriteByte not supported");
		}

		public override global::System.IAsyncResult BeginWrite(byte[] buffer, int offset, int count, global::System.AsyncCallback callback, object state)
		{
			throw new global::System.NotSupportedException("InflaterInputStream BeginWrite not supported");
		}

		public override void Close()
		{
			if (!isClosed)
			{
				isClosed = true;
				if (isStreamOwner)
				{
					baseInputStream.Close();
				}
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (inf.IsNeedingDictionary)
			{
				throw new global::ICSharpCode.SharpZipLib.SharpZipBaseException("Need a dictionary");
			}
			int num = count;
			while (true)
			{
				int num2 = inf.Inflate(buffer, offset, num);
				offset += num2;
				num -= num2;
				if (num == 0 || inf.IsFinished)
				{
					break;
				}
				if (inf.IsNeedingInput)
				{
					Fill();
				}
				else if (num2 == 0)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Dont know what to do");
				}
			}
			return count - num;
		}
	}
}
