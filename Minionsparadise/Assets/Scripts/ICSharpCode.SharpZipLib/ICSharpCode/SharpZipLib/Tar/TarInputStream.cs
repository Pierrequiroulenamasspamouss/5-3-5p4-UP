namespace ICSharpCode.SharpZipLib.Tar
{
	public class TarInputStream : global::System.IO.Stream
	{
		public interface IEntryFactory
		{
			global::ICSharpCode.SharpZipLib.Tar.TarEntry CreateEntry(string name);

			global::ICSharpCode.SharpZipLib.Tar.TarEntry CreateEntryFromFile(string fileName);

			global::ICSharpCode.SharpZipLib.Tar.TarEntry CreateEntry(byte[] headerBuffer);
		}

		public class EntryFactoryAdapter : global::ICSharpCode.SharpZipLib.Tar.TarInputStream.IEntryFactory
		{
			public global::ICSharpCode.SharpZipLib.Tar.TarEntry CreateEntry(string name)
			{
				return global::ICSharpCode.SharpZipLib.Tar.TarEntry.CreateTarEntry(name);
			}

			public global::ICSharpCode.SharpZipLib.Tar.TarEntry CreateEntryFromFile(string fileName)
			{
				return global::ICSharpCode.SharpZipLib.Tar.TarEntry.CreateEntryFromFile(fileName);
			}

			public global::ICSharpCode.SharpZipLib.Tar.TarEntry CreateEntry(byte[] headerBuffer)
			{
				return new global::ICSharpCode.SharpZipLib.Tar.TarEntry(headerBuffer);
			}
		}

		protected bool hasHitEOF;

		protected long entrySize;

		protected long entryOffset;

		protected byte[] readBuffer;

		protected global::ICSharpCode.SharpZipLib.Tar.TarBuffer tarBuffer;

		private global::ICSharpCode.SharpZipLib.Tar.TarEntry currentEntry;

		protected global::ICSharpCode.SharpZipLib.Tar.TarInputStream.IEntryFactory entryFactory;

		private readonly global::System.IO.Stream inputStream;

		public bool IsStreamOwner
		{
			get
			{
				return tarBuffer.IsStreamOwner;
			}
			set
			{
				tarBuffer.IsStreamOwner = value;
			}
		}

		public override bool CanRead
		{
			get
			{
				return inputStream.CanRead;
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
				return inputStream.Length;
			}
		}

		public override long Position
		{
			get
			{
				return inputStream.Position;
			}
			set
			{
				throw new global::System.NotSupportedException("TarInputStream Seek not supported");
			}
		}

		public int RecordSize
		{
			get
			{
				return tarBuffer.RecordSize;
			}
		}

		public long Available
		{
			get
			{
				return entrySize - entryOffset;
			}
		}

		public bool IsMarkSupported
		{
			get
			{
				return false;
			}
		}

		public TarInputStream(global::System.IO.Stream inputStream)
			: this(inputStream, 20)
		{
		}

		public TarInputStream(global::System.IO.Stream inputStream, int blockFactor)
		{
			this.inputStream = inputStream;
			tarBuffer = global::ICSharpCode.SharpZipLib.Tar.TarBuffer.CreateInputTarBuffer(inputStream, blockFactor);
		}

		public override void Flush()
		{
			inputStream.Flush();
		}

		public override long Seek(long offset, global::System.IO.SeekOrigin origin)
		{
			throw new global::System.NotSupportedException("TarInputStream Seek not supported");
		}

		public override void SetLength(long value)
		{
			throw new global::System.NotSupportedException("TarInputStream SetLength not supported");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new global::System.NotSupportedException("TarInputStream Write not supported");
		}

		public override void WriteByte(byte value)
		{
			throw new global::System.NotSupportedException("TarInputStream WriteByte not supported");
		}

		public override int ReadByte()
		{
			byte[] array = new byte[1];
			int num = Read(array, 0, 1);
			if (num <= 0)
			{
				return -1;
			}
			return array[0];
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new global::System.ArgumentNullException("buffer");
			}
			int num = 0;
			if (entryOffset >= entrySize)
			{
				return 0;
			}
			long num2 = count;
			if (num2 + entryOffset > entrySize)
			{
				num2 = entrySize - entryOffset;
			}
			if (readBuffer != null)
			{
				int num3 = (int)((num2 > readBuffer.Length) ? readBuffer.Length : num2);
				global::System.Array.Copy(readBuffer, 0, buffer, offset, num3);
				if (num3 >= readBuffer.Length)
				{
					readBuffer = null;
				}
				else
				{
					int num4 = readBuffer.Length - num3;
					byte[] destinationArray = new byte[num4];
					global::System.Array.Copy(readBuffer, num3, destinationArray, 0, num4);
					readBuffer = destinationArray;
				}
				num += num3;
				num2 -= num3;
				offset += num3;
			}
			while (num2 > 0)
			{
				byte[] array = tarBuffer.ReadBlock();
				if (array == null)
				{
					throw new global::ICSharpCode.SharpZipLib.Tar.TarException("unexpected EOF with " + num2 + " bytes unread");
				}
				int num5 = (int)num2;
				int num6 = array.Length;
				if (num6 > num5)
				{
					global::System.Array.Copy(array, 0, buffer, offset, num5);
					readBuffer = new byte[num6 - num5];
					global::System.Array.Copy(array, num5, readBuffer, 0, num6 - num5);
				}
				else
				{
					num5 = num6;
					global::System.Array.Copy(array, 0, buffer, offset, num6);
				}
				num += num5;
				num2 -= num5;
				offset += num5;
			}
			entryOffset += num;
			return num;
		}

		public override void Close()
		{
			tarBuffer.Close();
		}

		public void SetEntryFactory(global::ICSharpCode.SharpZipLib.Tar.TarInputStream.IEntryFactory factory)
		{
			entryFactory = factory;
		}

		[global::System.Obsolete("Use RecordSize property instead")]
		public int GetRecordSize()
		{
			return tarBuffer.RecordSize;
		}

		public void Skip(long skipCount)
		{
			byte[] array = new byte[8192];
			long num = skipCount;
			while (num > 0)
			{
				int count = (int)((num > array.Length) ? array.Length : num);
				int num2 = Read(array, 0, count);
				if (num2 == -1)
				{
					break;
				}
				num -= num2;
			}
		}

		public void Mark(int markLimit)
		{
		}

		public void Reset()
		{
		}

		public global::ICSharpCode.SharpZipLib.Tar.TarEntry GetNextEntry()
		{
			if (hasHitEOF)
			{
				return null;
			}
			if (currentEntry != null)
			{
				SkipToNextEntry();
			}
			byte[] array = tarBuffer.ReadBlock();
			if (array == null)
			{
				hasHitEOF = true;
			}
			else if (global::ICSharpCode.SharpZipLib.Tar.TarBuffer.IsEndOfArchiveBlock(array))
			{
				hasHitEOF = true;
			}
			if (hasHitEOF)
			{
				currentEntry = null;
			}
			else
			{
				try
				{
					global::ICSharpCode.SharpZipLib.Tar.TarHeader tarHeader = new global::ICSharpCode.SharpZipLib.Tar.TarHeader();
					tarHeader.ParseBuffer(array);
					if (!tarHeader.IsChecksumValid)
					{
						throw new global::ICSharpCode.SharpZipLib.Tar.TarException("Header checksum is invalid");
					}
					entryOffset = 0L;
					entrySize = tarHeader.Size;
					global::System.Text.StringBuilder stringBuilder = null;
					if (tarHeader.TypeFlag == 76)
					{
						byte[] array2 = new byte[512];
						long num = entrySize;
						stringBuilder = new global::System.Text.StringBuilder();
						while (num > 0)
						{
							int num2 = Read(array2, 0, (int)((num > array2.Length) ? array2.Length : num));
							if (num2 == -1)
							{
								throw new global::ICSharpCode.SharpZipLib.Tar.InvalidHeaderException("Failed to read long name entry");
							}
							stringBuilder.Append(global::ICSharpCode.SharpZipLib.Tar.TarHeader.ParseName(array2, 0, num2).ToString());
							num -= num2;
						}
						SkipToNextEntry();
						array = tarBuffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag == 103)
					{
						SkipToNextEntry();
						array = tarBuffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag == 120)
					{
						SkipToNextEntry();
						array = tarBuffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag == 86)
					{
						SkipToNextEntry();
						array = tarBuffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag != 48 && tarHeader.TypeFlag != 0 && tarHeader.TypeFlag != 53)
					{
						SkipToNextEntry();
						array = tarBuffer.ReadBlock();
					}
					if (entryFactory == null)
					{
						currentEntry = new global::ICSharpCode.SharpZipLib.Tar.TarEntry(array);
						if (stringBuilder != null)
						{
							currentEntry.Name = stringBuilder.ToString();
						}
					}
					else
					{
						currentEntry = entryFactory.CreateEntry(array);
					}
					entryOffset = 0L;
					entrySize = currentEntry.Size;
				}
				catch (global::ICSharpCode.SharpZipLib.Tar.InvalidHeaderException ex)
				{
					entrySize = 0L;
					entryOffset = 0L;
					currentEntry = null;
					string message = string.Format("Bad header in record {0} block {1} {2}", tarBuffer.CurrentRecord, tarBuffer.CurrentBlock, ex.Message);
					throw new global::ICSharpCode.SharpZipLib.Tar.InvalidHeaderException(message);
				}
			}
			return currentEntry;
		}

		public void CopyEntryContents(global::System.IO.Stream outputStream)
		{
			byte[] array = new byte[32768];
			while (true)
			{
				int num = Read(array, 0, array.Length);
				if (num <= 0)
				{
					break;
				}
				outputStream.Write(array, 0, num);
			}
		}

		private void SkipToNextEntry()
		{
			long num = entrySize - entryOffset;
			if (num > 0)
			{
				Skip(num);
			}
			readBuffer = null;
		}
	}
}
