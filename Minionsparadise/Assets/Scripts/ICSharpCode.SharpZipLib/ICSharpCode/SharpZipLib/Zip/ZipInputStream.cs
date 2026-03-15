namespace ICSharpCode.SharpZipLib.Zip
{
	public class ZipInputStream : global::ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream
	{
		private delegate int ReadDataHandler(byte[] b, int offset, int length);

		private global::ICSharpCode.SharpZipLib.Zip.ZipInputStream.ReadDataHandler internalReader;

		private global::ICSharpCode.SharpZipLib.Checksums.Crc32 crc = new global::ICSharpCode.SharpZipLib.Checksums.Crc32();

		private global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry;

		private long size;

		private int method;

		private int flags;

		private string password;

		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				password = value;
			}
		}

		public bool CanDecompressEntry
		{
			get
			{
				if (entry != null)
				{
					return entry.CanDecompress;
				}
				return false;
			}
		}

		public override int Available
		{
			get
			{
				if (entry == null)
				{
					return 0;
				}
				return 1;
			}
		}

		public override long Length
		{
			get
			{
				if (entry != null)
				{
					if (entry.Size >= 0)
					{
						return entry.Size;
					}
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Length not available for the current entry");
				}
				throw new global::System.InvalidOperationException("No current entry");
			}
		}

		public ZipInputStream(global::System.IO.Stream baseInputStream)
			: base(baseInputStream, new global::ICSharpCode.SharpZipLib.Zip.Compression.Inflater(true))
		{
			internalReader = ReadingNotAvailable;
		}

		public ZipInputStream(global::System.IO.Stream baseInputStream, int bufferSize)
			: base(baseInputStream, new global::ICSharpCode.SharpZipLib.Zip.Compression.Inflater(true), bufferSize)
		{
			internalReader = ReadingNotAvailable;
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipEntry GetNextEntry()
		{
			if (crc == null)
			{
				throw new global::System.InvalidOperationException("Closed.");
			}
			if (entry != null)
			{
				CloseEntry();
			}
			int num = inputBuffer.ReadLeInt();
			switch (num)
			{
			case 33639248:
			case 84233040:
			case 101010256:
			case 101075792:
			case 117853008:
				Close();
				return null;
			case 134695760:
			case 808471376:
				num = inputBuffer.ReadLeInt();
				break;
			}
			if (num != 67324752)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Wrong Local header signature: 0x" + string.Format("{0:X}", num));
			}
			short versionRequiredToExtract = (short)inputBuffer.ReadLeShort();
			flags = inputBuffer.ReadLeShort();
			method = inputBuffer.ReadLeShort();
			uint num2 = (uint)inputBuffer.ReadLeInt();
			int num3 = inputBuffer.ReadLeInt();
			csize = inputBuffer.ReadLeInt();
			size = inputBuffer.ReadLeInt();
			int num4 = inputBuffer.ReadLeShort();
			int num5 = inputBuffer.ReadLeShort();
			bool flag = (flags & 1) == 1;
			byte[] array = new byte[num4];
			inputBuffer.ReadRawBuffer(array);
			string name = global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToStringExt(flags, array);
			entry = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry(name, versionRequiredToExtract);
			entry.Flags = flags;
			entry.CompressionMethod = (global::ICSharpCode.SharpZipLib.Zip.CompressionMethod)method;
			if ((flags & 8) == 0)
			{
				entry.Crc = num3 & 0xFFFFFFFFu;
				entry.Size = size & 0xFFFFFFFFu;
				entry.CompressedSize = csize & 0xFFFFFFFFu;
				entry.CryptoCheckValue = (byte)((num3 >> 24) & 0xFF);
			}
			else
			{
				if (num3 != 0)
				{
					entry.Crc = num3 & 0xFFFFFFFFu;
				}
				if (size != 0)
				{
					entry.Size = size & 0xFFFFFFFFu;
				}
				if (csize != 0)
				{
					entry.CompressedSize = csize & 0xFFFFFFFFu;
				}
				entry.CryptoCheckValue = (byte)((num2 >> 8) & 0xFF);
			}
			entry.DosTime = num2;
			if (num5 > 0)
			{
				byte[] array2 = new byte[num5];
				inputBuffer.ReadRawBuffer(array2);
				entry.ExtraData = array2;
			}
			entry.ProcessExtraData(true);
			if (entry.CompressedSize >= 0)
			{
				csize = entry.CompressedSize;
			}
			if (entry.Size >= 0)
			{
				size = entry.Size;
			}
			if (method == 0 && ((!flag && csize != size) || (flag && csize - 12 != size)))
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Stored, but compressed != uncompressed");
			}
			if (entry.IsCompressionMethodSupported())
			{
				internalReader = InitialRead;
			}
			else
			{
				internalReader = ReadingNotSupported;
			}
			return entry;
		}

		private void ReadDataDescriptor()
		{
			if (inputBuffer.ReadLeInt() != 134695760)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Data descriptor signature not found");
			}
			entry.Crc = inputBuffer.ReadLeInt() & 0xFFFFFFFFu;
			if (entry.LocalHeaderRequiresZip64)
			{
				csize = inputBuffer.ReadLeLong();
				size = inputBuffer.ReadLeLong();
			}
			else
			{
				csize = inputBuffer.ReadLeInt();
				size = inputBuffer.ReadLeInt();
			}
			entry.CompressedSize = csize;
			entry.Size = size;
		}

		private void CompleteCloseEntry(bool testCrc)
		{
			StopDecrypting();
			if ((flags & 8) != 0)
			{
				ReadDataDescriptor();
			}
			size = 0L;
			if (testCrc && (crc.Value & 0xFFFFFFFFu) != entry.Crc && entry.Crc != -1)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("CRC mismatch");
			}
			crc.Reset();
			if (method == 8)
			{
				inf.Reset();
			}
			entry = null;
		}

		public void CloseEntry()
		{
			if (crc == null)
			{
				throw new global::System.InvalidOperationException("Closed");
			}
			if (entry == null)
			{
				return;
			}
			if (method == 8)
			{
				if ((flags & 8) != 0)
				{
					byte[] array = new byte[4096];
					while (Read(array, 0, array.Length) > 0)
					{
					}
					return;
				}
				csize -= inf.TotalIn;
				inputBuffer.Available += inf.RemainingInput;
			}
			if (inputBuffer.Available > csize && csize >= 0)
			{
				inputBuffer.Available = (int)(inputBuffer.Available - csize);
			}
			else
			{
				csize -= inputBuffer.Available;
				inputBuffer.Available = 0;
				while (csize != 0)
				{
					long num = Skip(csize);
					if (num <= 0)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Zip archive ends early.");
					}
					csize -= num;
				}
			}
			CompleteCloseEntry(false);
		}

		public override int ReadByte()
		{
			byte[] array = new byte[1];
			if (Read(array, 0, 1) <= 0)
			{
				return -1;
			}
			return array[0] & 0xFF;
		}

		private int ReadingNotAvailable(byte[] destination, int offset, int count)
		{
			throw new global::System.InvalidOperationException("Unable to read from this stream");
		}

		private int ReadingNotSupported(byte[] destination, int offset, int count)
		{
			throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("The compression method for this entry is not supported");
		}

		private int InitialRead(byte[] destination, int offset, int count)
		{
			if (!CanDecompressEntry)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Library cannot extract this entry. Version required is (" + entry.Version + ")");
			}
			if (entry.IsCrypted)
			{
				if (password == null)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("No password set.");
				}
				global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged pkzipClassicManaged = new global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged();
				byte[] rgbKey = global::ICSharpCode.SharpZipLib.Encryption.PkzipClassic.GenerateKeys(global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToArray(password));
				inputBuffer.CryptoTransform = pkzipClassicManaged.CreateDecryptor(rgbKey, null);
				byte[] array = new byte[12];
				inputBuffer.ReadClearTextBuffer(array, 0, 12);
				if (array[11] != entry.CryptoCheckValue)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Invalid password");
				}
				if (csize >= 12)
				{
					csize -= 12L;
				}
				else if ((entry.Flags & 8) == 0)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Entry compressed size {0} too small for encryption", csize));
				}
			}
			else
			{
				inputBuffer.CryptoTransform = null;
			}
			if (csize > 0 || (flags & 8) != 0)
			{
				if (method == 8 && inputBuffer.Available > 0)
				{
					inputBuffer.SetInflaterInput(inf);
				}
				internalReader = BodyRead;
				return BodyRead(destination, offset, count);
			}
			internalReader = ReadingNotAvailable;
			return 0;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new global::System.ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new global::System.ArgumentOutOfRangeException("offset", "Cannot be negative");
			}
			if (count < 0)
			{
				throw new global::System.ArgumentOutOfRangeException("count", "Cannot be negative");
			}
			if (buffer.Length - offset < count)
			{
				throw new global::System.ArgumentException("Invalid offset/count combination");
			}
			return internalReader(buffer, offset, count);
		}

		private int BodyRead(byte[] buffer, int offset, int count)
		{
			if (crc == null)
			{
				throw new global::System.InvalidOperationException("Closed");
			}
			if (entry == null || count <= 0)
			{
				return 0;
			}
			if (offset + count > buffer.Length)
			{
				throw new global::System.ArgumentException("Offset + count exceeds buffer size");
			}
			bool flag = false;
			switch (method)
			{
			case 8:
				count = base.Read(buffer, offset, count);
				if (count <= 0)
				{
					if (!inf.IsFinished)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Inflater not finished!");
					}
					inputBuffer.Available = inf.RemainingInput;
					if ((flags & 8) == 0 && ((inf.TotalIn != csize && csize != uint.MaxValue && csize != -1) || inf.TotalOut != size))
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Size mismatch: " + csize + ";" + size + " <-> " + inf.TotalIn + ";" + inf.TotalOut);
					}
					inf.Reset();
					flag = true;
				}
				break;
			case 0:
				if (count > csize && csize >= 0)
				{
					count = (int)csize;
				}
				if (count > 0)
				{
					count = inputBuffer.ReadClearTextBuffer(buffer, offset, count);
					if (count > 0)
					{
						csize -= count;
						size -= count;
					}
				}
				if (csize == 0)
				{
					flag = true;
				}
				else if (count < 0)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("EOF in stored block");
				}
				break;
			}
			if (count > 0)
			{
				crc.Update(buffer, offset, count);
			}
			if (flag)
			{
				CompleteCloseEntry(true);
			}
			return count;
		}

		public override void Close()
		{
			internalReader = ReadingNotAvailable;
			crc = null;
			entry = null;
			base.Close();
		}
	}
}
