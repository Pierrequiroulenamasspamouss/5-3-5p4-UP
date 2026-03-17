namespace ICSharpCode.SharpZipLib.Tar
{
	public class TarOutputStream : global::System.IO.Stream
	{
		private long currBytes;

		private int assemblyBufferLength;

		private bool isClosed;

		protected long currSize;

		protected byte[] blockBuffer;

		protected byte[] assemblyBuffer;

		protected global::ICSharpCode.SharpZipLib.Tar.TarBuffer buffer;

		protected global::System.IO.Stream outputStream;

		public bool IsStreamOwner
		{
			get
			{
				return buffer.IsStreamOwner;
			}
			set
			{
				buffer.IsStreamOwner = value;
			}
		}

		public override bool CanRead
		{
			get
			{
				return outputStream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return outputStream.CanSeek;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return outputStream.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				return outputStream.Length;
			}
		}

		public override long Position
		{
			get
			{
				return outputStream.Position;
			}
			set
			{
				outputStream.Position = value;
			}
		}

		public int RecordSize
		{
			get
			{
				return buffer.RecordSize;
			}
		}

		private bool IsEntryOpen
		{
			get
			{
				return currBytes < currSize;
			}
		}

		public TarOutputStream(global::System.IO.Stream outputStream)
			: this(outputStream, 20)
		{
		}

		public TarOutputStream(global::System.IO.Stream outputStream, int blockFactor)
		{
			if (outputStream == null)
			{
				throw new global::System.ArgumentNullException("outputStream");
			}
			this.outputStream = outputStream;
			buffer = global::ICSharpCode.SharpZipLib.Tar.TarBuffer.CreateOutputTarBuffer(outputStream, blockFactor);
			assemblyBuffer = new byte[512];
			blockBuffer = new byte[512];
		}

		public override long Seek(long offset, global::System.IO.SeekOrigin origin)
		{
			return outputStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			outputStream.SetLength(value);
		}

		public override int ReadByte()
		{
			return outputStream.ReadByte();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return outputStream.Read(buffer, offset, count);
		}

		public override void Flush()
		{
			outputStream.Flush();
		}

		public void Finish()
		{
			if (IsEntryOpen)
			{
				CloseEntry();
			}
			WriteEofBlock();
		}

		public override void Close()
		{
			if (!isClosed)
			{
				isClosed = true;
				Finish();
				buffer.Close();
			}
		}

		[global::System.Obsolete("Use RecordSize property instead")]
		public int GetRecordSize()
		{
			return buffer.RecordSize;
		}

		public void PutNextEntry(global::ICSharpCode.SharpZipLib.Tar.TarEntry entry)
		{
			if (entry == null)
			{
				throw new global::System.ArgumentNullException("entry");
			}
			if (entry.TarHeader.Name.Length >= 100)
			{
				global::ICSharpCode.SharpZipLib.Tar.TarHeader tarHeader = new global::ICSharpCode.SharpZipLib.Tar.TarHeader();
				tarHeader.TypeFlag = 76;
				tarHeader.Name += "././@LongLink";
				tarHeader.UserId = 0;
				tarHeader.GroupId = 0;
				tarHeader.GroupName = "";
				tarHeader.UserName = "";
				tarHeader.LinkName = "";
				tarHeader.Size = entry.TarHeader.Name.Length + 1;
				tarHeader.WriteHeader(blockBuffer);
				buffer.WriteBlock(blockBuffer);
				int num = 0;
				while (num < entry.TarHeader.Name.Length)
				{
					global::System.Array.Clear(blockBuffer, 0, blockBuffer.Length);
					global::ICSharpCode.SharpZipLib.Tar.TarHeader.GetAsciiBytes(entry.TarHeader.Name, num, blockBuffer, 0, 512);
					num += 512;
					buffer.WriteBlock(blockBuffer);
				}
			}
			entry.WriteEntryHeader(blockBuffer);
			buffer.WriteBlock(blockBuffer);
			currBytes = 0L;
			currSize = (entry.IsDirectory ? 0 : entry.Size);
		}

		public void CloseEntry()
		{
			if (assemblyBufferLength > 0)
			{
				global::System.Array.Clear(assemblyBuffer, assemblyBufferLength, assemblyBuffer.Length - assemblyBufferLength);
				buffer.WriteBlock(assemblyBuffer);
				currBytes += assemblyBufferLength;
				assemblyBufferLength = 0;
			}
			if (currBytes < currSize)
			{
				string message = string.Format("Entry closed at '{0}' before the '{1}' bytes specified in the header were written", currBytes, currSize);
				throw new global::ICSharpCode.SharpZipLib.Tar.TarException(message);
			}
		}

		public override void WriteByte(byte value)
		{
			Write(new byte[1] { value }, 0, 1);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new global::System.ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new global::System.ArgumentOutOfRangeException("offset", "Cannot be negative");
			}
			if (buffer.Length - offset < count)
			{
				throw new global::System.ArgumentException("offset and count combination is invalid");
			}
			if (count < 0)
			{
				throw new global::System.ArgumentOutOfRangeException("count", "Cannot be negative");
			}
			if (currBytes + count > currSize)
			{
				string message = string.Format("request to write '{0}' bytes exceeds size in header of '{1}' bytes", count, currSize);
				throw new global::System.ArgumentOutOfRangeException("count", message);
			}
			if (assemblyBufferLength > 0)
			{
				if (assemblyBufferLength + count >= blockBuffer.Length)
				{
					int num = blockBuffer.Length - assemblyBufferLength;
					global::System.Array.Copy(assemblyBuffer, 0, blockBuffer, 0, assemblyBufferLength);
					global::System.Array.Copy(buffer, offset, blockBuffer, assemblyBufferLength, num);
					this.buffer.WriteBlock(blockBuffer);
					currBytes += blockBuffer.Length;
					offset += num;
					count -= num;
					assemblyBufferLength = 0;
				}
				else
				{
					global::System.Array.Copy(buffer, offset, assemblyBuffer, assemblyBufferLength, count);
					offset += count;
					assemblyBufferLength += count;
					count -= count;
				}
			}
			while (count > 0)
			{
				if (count < blockBuffer.Length)
				{
					global::System.Array.Copy(buffer, offset, assemblyBuffer, assemblyBufferLength, count);
					assemblyBufferLength += count;
					break;
				}
				this.buffer.WriteBlock(buffer, offset);
				int num2 = blockBuffer.Length;
				currBytes += num2;
				count -= num2;
				offset += num2;
			}
		}

		private void WriteEofBlock()
		{
			global::System.Array.Clear(blockBuffer, 0, blockBuffer.Length);
			buffer.WriteBlock(blockBuffer);
		}
	}
}
