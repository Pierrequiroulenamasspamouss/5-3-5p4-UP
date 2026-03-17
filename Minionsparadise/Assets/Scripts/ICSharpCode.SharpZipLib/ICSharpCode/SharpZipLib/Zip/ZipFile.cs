namespace ICSharpCode.SharpZipLib.Zip
{
	public class ZipFile : global::System.Collections.IEnumerable, global::System.IDisposable
	{
		public delegate void KeysRequiredEventHandler(object sender, global::ICSharpCode.SharpZipLib.Zip.KeysRequiredEventArgs e);

		[global::System.Flags]
		private enum HeaderTest
		{
			Extract = 1,
			Header = 2
		}

		private enum UpdateCommand
		{
			Copy = 0,
			Modify = 1,
			Add = 2
		}

		private class UpdateComparer : global::System.Collections.IComparer
		{
			public int Compare(object x, object y)
			{
				global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate zipUpdate = x as global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate;
				global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate zipUpdate2 = y as global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate;
				int num;
				if (zipUpdate == null)
				{
					num = ((zipUpdate2 != null) ? (-1) : 0);
				}
				else if (zipUpdate2 == null)
				{
					num = 1;
				}
				else
				{
					int num2 = ((zipUpdate.Command != global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Copy && zipUpdate.Command != global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Modify) ? 1 : 0);
					int num3 = ((zipUpdate2.Command != global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Copy && zipUpdate2.Command != global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Modify) ? 1 : 0);
					num = num2 - num3;
					if (num == 0)
					{
						long num4 = zipUpdate.Entry.Offset - zipUpdate2.Entry.Offset;
						num = ((num4 < 0) ? (-1) : ((num4 != 0) ? 1 : 0));
					}
				}
				return num;
			}
		}

		private class ZipUpdate
		{
			private global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry_;

			private global::ICSharpCode.SharpZipLib.Zip.ZipEntry outEntry_;

			private global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand command_;

			private global::ICSharpCode.SharpZipLib.Zip.IStaticDataSource dataSource_;

			private string filename_;

			private long sizePatchOffset_ = -1L;

			private long crcPatchOffset_ = -1L;

			private long _offsetBasedSize = -1L;

			public global::ICSharpCode.SharpZipLib.Zip.ZipEntry Entry
			{
				get
				{
					return entry_;
				}
			}

			public global::ICSharpCode.SharpZipLib.Zip.ZipEntry OutEntry
			{
				get
				{
					if (outEntry_ == null)
					{
						outEntry_ = (global::ICSharpCode.SharpZipLib.Zip.ZipEntry)entry_.Clone();
					}
					return outEntry_;
				}
			}

			public global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand Command
			{
				get
				{
					return command_;
				}
			}

			public string Filename
			{
				get
				{
					return filename_;
				}
			}

			public long SizePatchOffset
			{
				get
				{
					return sizePatchOffset_;
				}
				set
				{
					sizePatchOffset_ = value;
				}
			}

			public long CrcPatchOffset
			{
				get
				{
					return crcPatchOffset_;
				}
				set
				{
					crcPatchOffset_ = value;
				}
			}

			public long OffsetBasedSize
			{
				get
				{
					return _offsetBasedSize;
				}
				set
				{
					_offsetBasedSize = value;
				}
			}

			public ZipUpdate(string fileName, global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
			{
				command_ = global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Add;
				entry_ = entry;
				filename_ = fileName;
			}

			[global::System.Obsolete]
			public ZipUpdate(string fileName, string entryName, global::ICSharpCode.SharpZipLib.Zip.CompressionMethod compressionMethod)
			{
				command_ = global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Add;
				entry_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry(entryName);
				entry_.CompressionMethod = compressionMethod;
				filename_ = fileName;
			}

			[global::System.Obsolete]
			public ZipUpdate(string fileName, string entryName)
				: this(fileName, entryName, global::ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated)
			{
			}

			[global::System.Obsolete]
			public ZipUpdate(global::ICSharpCode.SharpZipLib.Zip.IStaticDataSource dataSource, string entryName, global::ICSharpCode.SharpZipLib.Zip.CompressionMethod compressionMethod)
			{
				command_ = global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Add;
				entry_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry(entryName);
				entry_.CompressionMethod = compressionMethod;
				dataSource_ = dataSource;
			}

			public ZipUpdate(global::ICSharpCode.SharpZipLib.Zip.IStaticDataSource dataSource, global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
			{
				command_ = global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Add;
				entry_ = entry;
				dataSource_ = dataSource;
			}

			public ZipUpdate(global::ICSharpCode.SharpZipLib.Zip.ZipEntry original, global::ICSharpCode.SharpZipLib.Zip.ZipEntry updated)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Modify not currently supported");
			}

			public ZipUpdate(global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand command, global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
			{
				command_ = command;
				entry_ = (global::ICSharpCode.SharpZipLib.Zip.ZipEntry)entry.Clone();
			}

			public ZipUpdate(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
				: this(global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Copy, entry)
			{
			}

			public global::System.IO.Stream GetSource()
			{
				global::System.IO.Stream result = null;
				if (dataSource_ != null)
				{
					result = dataSource_.GetSource();
				}
				return result;
			}
		}

		private class ZipString
		{
			private string comment_;

			private byte[] rawComment_;

			private bool isSourceString_;

			public bool IsSourceString
			{
				get
				{
					return isSourceString_;
				}
			}

			public int RawLength
			{
				get
				{
					MakeBytesAvailable();
					return rawComment_.Length;
				}
			}

			public byte[] RawComment
			{
				get
				{
					MakeBytesAvailable();
					return (byte[])rawComment_.Clone();
				}
			}

			public ZipString(string comment)
			{
				comment_ = comment;
				isSourceString_ = true;
			}

			public ZipString(byte[] rawString)
			{
				rawComment_ = rawString;
			}

			public void Reset()
			{
				if (isSourceString_)
				{
					rawComment_ = null;
				}
				else
				{
					comment_ = null;
				}
			}

			private void MakeTextAvailable()
			{
				if (comment_ == null)
				{
					comment_ = global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToString(rawComment_);
				}
			}

			private void MakeBytesAvailable()
			{
				if (rawComment_ == null)
				{
					rawComment_ = global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToArray(comment_);
				}
			}

			public static implicit operator string(global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipString zipString)
			{
				zipString.MakeTextAvailable();
				return zipString.comment_;
			}
		}

		private class ZipEntryEnumerator : global::System.Collections.IEnumerator
		{
			private global::ICSharpCode.SharpZipLib.Zip.ZipEntry[] array;

			private int index = -1;

			public object Current
			{
				get
				{
					return array[index];
				}
			}

			public ZipEntryEnumerator(global::ICSharpCode.SharpZipLib.Zip.ZipEntry[] entries)
			{
				array = entries;
			}

			public void Reset()
			{
				index = -1;
			}

			public bool MoveNext()
			{
				return ++index < array.Length;
			}
		}

		private class UncompressedStream : global::System.IO.Stream
		{
			private global::System.IO.Stream baseStream_;

			public override bool CanRead
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
					return baseStream_.CanWrite;
				}
			}

			public override bool CanSeek
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
					return 0L;
				}
			}

			public override long Position
			{
				get
				{
					return baseStream_.Position;
				}
				set
				{
				}
			}

			public UncompressedStream(global::System.IO.Stream baseStream)
			{
				baseStream_ = baseStream;
			}

			public override void Close()
			{
			}

			public override void Flush()
			{
				baseStream_.Flush();
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				return 0;
			}

			public override long Seek(long offset, global::System.IO.SeekOrigin origin)
			{
				return 0L;
			}

			public override void SetLength(long value)
			{
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				baseStream_.Write(buffer, offset, count);
			}
		}

		private class PartialInputStream : global::System.IO.Stream
		{
			private global::ICSharpCode.SharpZipLib.Zip.ZipFile zipFile_;

			private global::System.IO.Stream baseStream_;

			private long start_;

			private long length_;

			private long readPos_;

			private long end_;

			public override long Position
			{
				get
				{
					return readPos_ - start_;
				}
				set
				{
					long num = start_ + value;
					if (num < start_)
					{
						throw new global::System.ArgumentException("Negative position is invalid");
					}
					if (num >= end_)
					{
						throw new global::System.InvalidOperationException("Cannot seek past end");
					}
					readPos_ = num;
				}
			}

			public override long Length
			{
				get
				{
					return length_;
				}
			}

			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			public override bool CanTimeout
			{
				get
				{
					return baseStream_.CanTimeout;
				}
			}

			public PartialInputStream(global::ICSharpCode.SharpZipLib.Zip.ZipFile zipFile, long start, long length)
			{
				start_ = start;
				length_ = length;
				zipFile_ = zipFile;
				baseStream_ = zipFile_.baseStream_;
				readPos_ = start;
				end_ = start + length;
			}

			public override int ReadByte()
			{
				if (readPos_ >= end_)
				{
					return -1;
				}
				lock (baseStream_)
				{
					baseStream_.Seek(readPos_++, global::System.IO.SeekOrigin.Begin);
					return baseStream_.ReadByte();
				}
			}

			public override void Close()
			{
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				lock (baseStream_)
				{
					if (count > end_ - readPos_)
					{
						count = (int)(end_ - readPos_);
						if (count == 0)
						{
							return 0;
						}
					}
					baseStream_.Seek(readPos_, global::System.IO.SeekOrigin.Begin);
					int num = baseStream_.Read(buffer, offset, count);
					if (num > 0)
					{
						readPos_ += num;
					}
					return num;
				}
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new global::System.NotSupportedException();
			}

			public override void SetLength(long value)
			{
				throw new global::System.NotSupportedException();
			}

			public override long Seek(long offset, global::System.IO.SeekOrigin origin)
			{
				long num = readPos_;
				switch (origin)
				{
				case global::System.IO.SeekOrigin.Begin:
					num = start_ + offset;
					break;
				case global::System.IO.SeekOrigin.Current:
					num = readPos_ + offset;
					break;
				case global::System.IO.SeekOrigin.End:
					num = end_ + offset;
					break;
				}
				if (num < start_)
				{
					throw new global::System.ArgumentException("Negative position is invalid");
				}
				if (num >= end_)
				{
					throw new global::System.IO.IOException("Cannot seek past end");
				}
				readPos_ = num;
				return readPos_;
			}

			public override void Flush()
			{
			}
		}

		private const int DefaultBufferSize = 4096;

		public global::ICSharpCode.SharpZipLib.Zip.ZipFile.KeysRequiredEventHandler KeysRequired;

		private bool isDisposed_;

		private string name_;

		private string comment_;

		private string rawPassword_;

		private global::System.IO.Stream baseStream_;

		private bool isStreamOwner;

		private long offsetOfFirstEntry;

		private global::ICSharpCode.SharpZipLib.Zip.ZipEntry[] entries_;

		private byte[] key;

		private bool isNewArchive_;

		private global::ICSharpCode.SharpZipLib.Zip.UseZip64 useZip64_ = global::ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;

		private global::System.Collections.ArrayList updates_;

		private long updateCount_;

		private global::System.Collections.Hashtable updateIndex_;

		private global::ICSharpCode.SharpZipLib.Zip.IArchiveStorage archiveStorage_;

		private global::ICSharpCode.SharpZipLib.Zip.IDynamicDataSource updateDataSource_;

		private bool contentsEdited_;

		private int bufferSize_ = 4096;

		private byte[] copyBuffer_;

		private global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipString newComment_;

		private bool commentEdited_;

		private global::ICSharpCode.SharpZipLib.Zip.IEntryFactory updateEntryFactory_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory();

		private byte[] Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		public string Password
		{
			set
			{
				if (value == null || value.Length == 0)
				{
					key = null;
					return;
				}
				rawPassword_ = value;
				key = global::ICSharpCode.SharpZipLib.Encryption.PkzipClassic.GenerateKeys(global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToArray(value));
			}
		}

		private bool HaveKeys
		{
			get
			{
				return key != null;
			}
		}

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

		public bool IsEmbeddedArchive
		{
			get
			{
				return offsetOfFirstEntry > 0;
			}
		}

		public bool IsNewArchive
		{
			get
			{
				return isNewArchive_;
			}
		}

		public string ZipFileComment
		{
			get
			{
				return comment_;
			}
		}

		public string Name
		{
			get
			{
				return name_;
			}
		}

		[global::System.Obsolete("Use the Count property instead")]
		public int Size
		{
			get
			{
				return entries_.Length;
			}
		}

		public long Count
		{
			get
			{
				return entries_.Length;
			}
		}

		[global::System.Runtime.CompilerServices.IndexerName("EntryByIndex")]
		public global::ICSharpCode.SharpZipLib.Zip.ZipEntry this[int index]
		{
			get
			{
				return (global::ICSharpCode.SharpZipLib.Zip.ZipEntry)entries_[index].Clone();
			}
		}

		public global::ICSharpCode.SharpZipLib.Core.INameTransform NameTransform
		{
			get
			{
				return updateEntryFactory_.NameTransform;
			}
			set
			{
				updateEntryFactory_.NameTransform = value;
			}
		}

		public global::ICSharpCode.SharpZipLib.Zip.IEntryFactory EntryFactory
		{
			get
			{
				return updateEntryFactory_;
			}
			set
			{
				if (value == null)
				{
					updateEntryFactory_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory();
				}
				else
				{
					updateEntryFactory_ = value;
				}
			}
		}

		public int BufferSize
		{
			get
			{
				return bufferSize_;
			}
			set
			{
				if (value < 1024)
				{
					throw new global::System.ArgumentOutOfRangeException("value", "cannot be below 1024");
				}
				if (bufferSize_ != value)
				{
					bufferSize_ = value;
					copyBuffer_ = null;
				}
			}
		}

		public bool IsUpdating
		{
			get
			{
				return updates_ != null;
			}
		}

		public global::ICSharpCode.SharpZipLib.Zip.UseZip64 UseZip64
		{
			get
			{
				return useZip64_;
			}
			set
			{
				useZip64_ = value;
			}
		}

		private void OnKeysRequired(string fileName)
		{
			if (KeysRequired != null)
			{
				global::ICSharpCode.SharpZipLib.Zip.KeysRequiredEventArgs e = new global::ICSharpCode.SharpZipLib.Zip.KeysRequiredEventArgs(fileName, key);
				KeysRequired(this, e);
				key = e.Key;
			}
		}

		public ZipFile(string name)
		{
			if (name == null)
			{
				throw new global::System.ArgumentNullException("name");
			}
			name_ = name;
			baseStream_ = global::System.IO.File.Open(name, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.Read);
			isStreamOwner = true;
			try
			{
				ReadEntries();
			}
			catch
			{
				DisposeInternal(true);
				throw;
			}
		}

		public ZipFile(global::System.IO.FileStream file)
		{
			if (file == null)
			{
				throw new global::System.ArgumentNullException("file");
			}
			if (!file.CanSeek)
			{
				throw new global::System.ArgumentException("Stream is not seekable", "file");
			}
			baseStream_ = file;
			name_ = file.Name;
			isStreamOwner = true;
			try
			{
				ReadEntries();
			}
			catch
			{
				DisposeInternal(true);
				throw;
			}
		}

		public ZipFile(global::System.IO.Stream stream)
		{
			if (stream == null)
			{
				throw new global::System.ArgumentNullException("stream");
			}
			if (!stream.CanSeek)
			{
				throw new global::System.ArgumentException("Stream is not seekable", "stream");
			}
			baseStream_ = stream;
			isStreamOwner = true;
			if (baseStream_.Length > 0)
			{
				try
				{
					ReadEntries();
					return;
				}
				catch
				{
					DisposeInternal(true);
					throw;
				}
			}
			entries_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry[0];
			isNewArchive_ = true;
		}

		internal ZipFile()
		{
			entries_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry[0];
			isNewArchive_ = true;
		}

		~ZipFile()
		{
			Dispose(false);
		}

		public void Close()
		{
			DisposeInternal(true);
			global::System.GC.SuppressFinalize(this);
		}

		public static global::ICSharpCode.SharpZipLib.Zip.ZipFile Create(string fileName)
		{
			if (fileName == null)
			{
				throw new global::System.ArgumentNullException("fileName");
			}
#if !UNITY_WEBPLAYER
			global::System.IO.FileStream fileStream = global::System.IO.File.Create(fileName);
			global::ICSharpCode.SharpZipLib.Zip.ZipFile zipFile = new global::ICSharpCode.SharpZipLib.Zip.ZipFile();
			zipFile.name_ = fileName;
			zipFile.baseStream_ = fileStream;
			zipFile.isStreamOwner = true;
			return zipFile;
#else
			return null;
#endif
		}

		public static global::ICSharpCode.SharpZipLib.Zip.ZipFile Create(global::System.IO.Stream outStream)
		{
			if (outStream == null)
			{
				throw new global::System.ArgumentNullException("outStream");
			}
			if (!outStream.CanWrite)
			{
				throw new global::System.ArgumentException("Stream is not writeable", "outStream");
			}
			if (!outStream.CanSeek)
			{
				throw new global::System.ArgumentException("Stream is not seekable", "outStream");
			}
			global::ICSharpCode.SharpZipLib.Zip.ZipFile zipFile = new global::ICSharpCode.SharpZipLib.Zip.ZipFile();
			zipFile.baseStream_ = outStream;
			return zipFile;
		}

		public global::System.Collections.IEnumerator GetEnumerator()
		{
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			return new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipEntryEnumerator(entries_);
		}

		public int FindEntry(string name, bool ignoreCase)
		{
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			for (int i = 0; i < entries_.Length; i++)
			{
				if (string.Compare(name, entries_[i].Name, ignoreCase, global::System.Globalization.CultureInfo.InvariantCulture) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		public global::ICSharpCode.SharpZipLib.Zip.ZipEntry GetEntry(string name)
		{
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			int num = FindEntry(name, true);
			if (num < 0)
			{
				return null;
			}
			return (global::ICSharpCode.SharpZipLib.Zip.ZipEntry)entries_[num].Clone();
		}

		public global::System.IO.Stream GetInputStream(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			if (entry == null)
			{
				throw new global::System.ArgumentNullException("entry");
			}
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			long num = entry.ZipFileIndex;
			if (num < 0 || num >= entries_.Length || entries_[num].Name != entry.Name)
			{
				num = FindEntry(entry.Name, true);
				if (num < 0)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Entry cannot be found");
				}
			}
			return GetInputStream(num);
		}

		public global::System.IO.Stream GetInputStream(long entryIndex)
		{
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			long start = LocateEntry(entries_[entryIndex]);
			global::ICSharpCode.SharpZipLib.Zip.CompressionMethod compressionMethod = entries_[entryIndex].CompressionMethod;
			global::System.IO.Stream stream = new global::ICSharpCode.SharpZipLib.Zip.ZipFile.PartialInputStream(this, start, entries_[entryIndex].CompressedSize);
			if (entries_[entryIndex].IsCrypted)
			{
				stream = CreateAndInitDecryptionStream(stream, entries_[entryIndex]);
				if (stream == null)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Unable to decrypt this entry");
				}
			}
			switch (compressionMethod)
			{
			case global::ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated:
				stream = new global::ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream(stream, new global::ICSharpCode.SharpZipLib.Zip.Compression.Inflater(true));
				break;
			default:
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Unsupported compression method " + compressionMethod);
			case global::ICSharpCode.SharpZipLib.Zip.CompressionMethod.Stored:
				break;
			}
			return stream;
		}

		public bool TestArchive(bool testData)
		{
			return TestArchive(testData, global::ICSharpCode.SharpZipLib.Zip.TestStrategy.FindFirstError, null);
		}

		public bool TestArchive(bool testData, global::ICSharpCode.SharpZipLib.Zip.TestStrategy strategy, global::ICSharpCode.SharpZipLib.Zip.ZipTestResultHandler resultHandler)
		{
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			global::ICSharpCode.SharpZipLib.Zip.TestStatus testStatus = new global::ICSharpCode.SharpZipLib.Zip.TestStatus(this);
			if (resultHandler != null)
			{
				resultHandler(testStatus, null);
			}
			global::ICSharpCode.SharpZipLib.Zip.ZipFile.HeaderTest tests = (testData ? (global::ICSharpCode.SharpZipLib.Zip.ZipFile.HeaderTest.Extract | global::ICSharpCode.SharpZipLib.Zip.ZipFile.HeaderTest.Header) : global::ICSharpCode.SharpZipLib.Zip.ZipFile.HeaderTest.Header);
			bool flag = true;
			try
			{
				int num = 0;
				while (flag && num < Count)
				{
					if (resultHandler != null)
					{
						testStatus.SetEntry(this[num]);
						testStatus.SetOperation(global::ICSharpCode.SharpZipLib.Zip.TestOperation.EntryHeader);
						resultHandler(testStatus, null);
					}
					try
					{
						TestLocalHeader(this[num], tests);
					}
					catch (global::ICSharpCode.SharpZipLib.Zip.ZipException ex)
					{
						testStatus.AddError();
						if (resultHandler != null)
						{
							resultHandler(testStatus, string.Format("Exception during test - '{0}'", ex.Message));
						}
						if (strategy == global::ICSharpCode.SharpZipLib.Zip.TestStrategy.FindFirstError)
						{
							flag = false;
						}
					}
					if (flag && testData && this[num].IsFile)
					{
						if (resultHandler != null)
						{
							testStatus.SetOperation(global::ICSharpCode.SharpZipLib.Zip.TestOperation.EntryData);
							resultHandler(testStatus, null);
						}
						global::ICSharpCode.SharpZipLib.Checksums.Crc32 crc = new global::ICSharpCode.SharpZipLib.Checksums.Crc32();
						using (global::System.IO.Stream stream = GetInputStream(this[num]))
						{
							byte[] array = new byte[4096];
							long num2 = 0L;
							int num3;
							while ((num3 = stream.Read(array, 0, array.Length)) > 0)
							{
								crc.Update(array, 0, num3);
								if (resultHandler != null)
								{
									num2 += num3;
									testStatus.SetBytesTested(num2);
									resultHandler(testStatus, null);
								}
							}
						}
						if (this[num].Crc != crc.Value)
						{
							testStatus.AddError();
							if (resultHandler != null)
							{
								resultHandler(testStatus, "CRC mismatch");
							}
							if (strategy == global::ICSharpCode.SharpZipLib.Zip.TestStrategy.FindFirstError)
							{
								flag = false;
							}
						}
						if ((this[num].Flags & 8) != 0)
						{
							global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(baseStream_);
							global::ICSharpCode.SharpZipLib.Zip.DescriptorData descriptorData = new global::ICSharpCode.SharpZipLib.Zip.DescriptorData();
							zipHelperStream.ReadDataDescriptor(this[num].LocalHeaderRequiresZip64, descriptorData);
							if (this[num].Crc != descriptorData.Crc)
							{
								testStatus.AddError();
							}
							if (this[num].CompressedSize != descriptorData.CompressedSize)
							{
								testStatus.AddError();
							}
							if (this[num].Size != descriptorData.Size)
							{
								testStatus.AddError();
							}
						}
					}
					if (resultHandler != null)
					{
						testStatus.SetOperation(global::ICSharpCode.SharpZipLib.Zip.TestOperation.EntryComplete);
						resultHandler(testStatus, null);
					}
					num++;
				}
				if (resultHandler != null)
				{
					testStatus.SetOperation(global::ICSharpCode.SharpZipLib.Zip.TestOperation.MiscellaneousTests);
					resultHandler(testStatus, null);
				}
			}
			catch (global::System.Exception ex2)
			{
				testStatus.AddError();
				if (resultHandler != null)
				{
					resultHandler(testStatus, string.Format("Exception during test - '{0}'", ex2.Message));
				}
			}
			if (resultHandler != null)
			{
				testStatus.SetOperation(global::ICSharpCode.SharpZipLib.Zip.TestOperation.Complete);
				testStatus.SetEntry(null);
				resultHandler(testStatus, null);
			}
			return testStatus.ErrorCount == 0;
		}

		private long TestLocalHeader(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry, global::ICSharpCode.SharpZipLib.Zip.ZipFile.HeaderTest tests)
		{
			lock (baseStream_)
			{
				bool flag = (tests & global::ICSharpCode.SharpZipLib.Zip.ZipFile.HeaderTest.Header) != 0;
				bool flag2 = (tests & global::ICSharpCode.SharpZipLib.Zip.ZipFile.HeaderTest.Extract) != 0;
				baseStream_.Seek(offsetOfFirstEntry + entry.Offset, global::System.IO.SeekOrigin.Begin);
				if (ReadLEUint() != 67324752)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Wrong local header signature @{0:X}", offsetOfFirstEntry + entry.Offset));
				}
				short num = (short)(ReadLEUshort() & 0xFF);
				short num2 = (short)ReadLEUshort();
				short num3 = (short)ReadLEUshort();
				short num4 = (short)ReadLEUshort();
				short num5 = (short)ReadLEUshort();
				uint num6 = ReadLEUint();
				long num7 = ReadLEUint();
				long num8 = ReadLEUint();
				int num9 = ReadLEUshort();
				int num10 = ReadLEUshort();
				byte[] array = new byte[num9];
				global::ICSharpCode.SharpZipLib.Core.StreamUtils.ReadFully(baseStream_, array);
				byte[] array2 = new byte[num10];
				global::ICSharpCode.SharpZipLib.Core.StreamUtils.ReadFully(baseStream_, array2);
				global::ICSharpCode.SharpZipLib.Zip.ZipExtraData zipExtraData = new global::ICSharpCode.SharpZipLib.Zip.ZipExtraData(array2);
				if (zipExtraData.Find(1))
				{
					num8 = zipExtraData.ReadLong();
					num7 = zipExtraData.ReadLong();
					if ((num2 & 8) != 0)
					{
						if (num8 != -1 && num8 != entry.Size)
						{
							throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Size invalid for descriptor");
						}
						if (num7 != -1 && num7 != entry.CompressedSize)
						{
							throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Compressed size invalid for descriptor");
						}
					}
				}
				else if (num >= 45 && ((int)num8 == -1 || (int)num7 == -1))
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Required Zip64 extended information missing");
				}
				if (flag2 && entry.IsFile)
				{
					if (!entry.IsCompressionMethodSupported())
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Compression method not supported");
					}
					if (num > 51 || (num > 20 && num < 45))
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Version required to extract this entry not supported ({0})", num));
					}
					if ((num2 & 0x3060) != 0)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("The library does not support the zip version required to extract this entry");
					}
				}
				if (flag)
				{
					if (num <= 63 && num != 10 && num != 11 && num != 20 && num != 21 && num != 25 && num != 27 && num != 45 && num != 46 && num != 50 && num != 51 && num != 52 && num != 61 && num != 62 && num != 63)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Version required to extract this entry is invalid ({0})", num));
					}
					if ((num2 & 0xC010) != 0)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Reserved bit flags cannot be set.");
					}
					if ((num2 & 1) != 0 && num < 20)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Version required to extract this entry is too low for encryption ({0})", num));
					}
					if ((num2 & 0x40) != 0)
					{
						if ((num2 & 1) == 0)
						{
							throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Strong encryption flag set but encryption flag is not set");
						}
						if (num < 50)
						{
							throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Version required to extract this entry is too low for encryption ({0})", num));
						}
					}
					if ((num2 & 0x20) != 0 && num < 27)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Patched data requires higher version than ({0})", num));
					}
					if (num2 != entry.Flags)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Central header/local header flags mismatch");
					}
					if (entry.CompressionMethod != (global::ICSharpCode.SharpZipLib.Zip.CompressionMethod)num3)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Central header/local header compression method mismatch");
					}
					if (entry.Version != num)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Extract version mismatch");
					}
					if ((num2 & 0x40) != 0 && num < 62)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Strong encryption flag set but version not high enough");
					}
					if ((num2 & 0x2000) != 0 && (num4 != 0 || num5 != 0))
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Header masked set but date/time values non-zero");
					}
					if ((num2 & 8) == 0 && num6 != (uint)entry.Crc)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Central header/local header crc mismatch");
					}
					if (num8 == 0 && num7 == 0 && num6 != 0)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Invalid CRC for empty entry");
					}
					if (entry.Name.Length > num9)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("File name length mismatch");
					}
					string text = global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToStringExt(num2, array);
					if (text != entry.Name)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Central header and local header file name mismatch");
					}
					if (entry.IsDirectory)
					{
						if (num8 > 0)
						{
							throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Directory cannot have size");
						}
						if (entry.IsCrypted)
						{
							if (num7 > 14)
							{
								throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Directory compressed size invalid");
							}
						}
						else if (num7 > 2)
						{
							throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Directory compressed size invalid");
						}
					}
					if (!global::ICSharpCode.SharpZipLib.Zip.ZipNameTransform.IsValidName(text, true))
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Name is invalid");
					}
				}
				if ((num2 & 8) == 0 || num8 > 0 || num7 > 0)
				{
					if (num8 != entry.Size)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Size mismatch between central header({0}) and local header({1})", entry.Size, num8));
					}
					if (num7 != entry.CompressedSize && num7 != uint.MaxValue && num7 != -1)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Compressed size mismatch between central header({0}) and local header({1})", entry.CompressedSize, num7));
					}
				}
				int num11 = num9 + num10;
				return offsetOfFirstEntry + entry.Offset + 30 + num11;
			}
		}

		public void BeginUpdate(global::ICSharpCode.SharpZipLib.Zip.IArchiveStorage archiveStorage, global::ICSharpCode.SharpZipLib.Zip.IDynamicDataSource dataSource)
		{
			if (archiveStorage == null)
			{
				throw new global::System.ArgumentNullException("archiveStorage");
			}
			if (dataSource == null)
			{
				throw new global::System.ArgumentNullException("dataSource");
			}
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			if (IsEmbeddedArchive)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Cannot update embedded/SFX archives");
			}
			archiveStorage_ = archiveStorage;
			updateDataSource_ = dataSource;
			updateIndex_ = new global::System.Collections.Hashtable();
			updates_ = new global::System.Collections.ArrayList(entries_.Length);
			global::ICSharpCode.SharpZipLib.Zip.ZipEntry[] array = entries_;
			foreach (global::ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry in array)
			{
				int num = updates_.Add(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(zipEntry));
				updateIndex_.Add(zipEntry.Name, num);
			}
			updates_.Sort(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateComparer());
			int num2 = 0;
			foreach (global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate item in updates_)
			{
				if (num2 != updates_.Count - 1)
				{
					item.OffsetBasedSize = ((global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate)updates_[num2 + 1]).Entry.Offset - item.Entry.Offset;
					num2++;
					continue;
				}
				break;
			}
			updateCount_ = updates_.Count;
			contentsEdited_ = false;
			commentEdited_ = false;
			newComment_ = null;
		}

		public void BeginUpdate(global::ICSharpCode.SharpZipLib.Zip.IArchiveStorage archiveStorage)
		{
			BeginUpdate(archiveStorage, new global::ICSharpCode.SharpZipLib.Zip.DynamicDiskDataSource());
		}

		public void BeginUpdate()
		{
			if (Name == null)
			{
				BeginUpdate(new global::ICSharpCode.SharpZipLib.Zip.MemoryArchiveStorage(), new global::ICSharpCode.SharpZipLib.Zip.DynamicDiskDataSource());
			}
			else
			{
				BeginUpdate(new global::ICSharpCode.SharpZipLib.Zip.DiskArchiveStorage(this), new global::ICSharpCode.SharpZipLib.Zip.DynamicDiskDataSource());
			}
		}

		public void CommitUpdate()
		{
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			CheckUpdating();
			try
			{
				updateIndex_.Clear();
				updateIndex_ = null;
				if (contentsEdited_)
				{
					RunUpdates();
				}
				else if (commentEdited_)
				{
					UpdateCommentOnly();
				}
				else if (entries_.Length == 0)
				{
					byte[] comment = ((newComment_ != null) ? newComment_.RawComment : global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToArray(comment_));
					using (global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(baseStream_))
					{
						zipHelperStream.WriteEndOfCentralDirectory(0L, 0L, 0L, comment);
						return;
					}
				}
			}
			finally
			{
				PostUpdateCleanup();
			}
		}

		public void AbortUpdate()
		{
			PostUpdateCleanup();
		}

		public void SetComment(string comment)
		{
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			CheckUpdating();
			newComment_ = new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipString(comment);
			if (newComment_.RawLength > 65535)
			{
				newComment_ = null;
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Comment length exceeds maximum - 65535");
			}
			commentEdited_ = true;
		}

		private void AddUpdate(global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update)
		{
			contentsEdited_ = true;
			int num = FindExistingUpdate(update.Entry.Name);
			if (num >= 0)
			{
				if (updates_[num] == null)
				{
					updateCount_++;
				}
				updates_[num] = update;
			}
			else
			{
				num = updates_.Add(update);
				updateCount_++;
				updateIndex_.Add(update.Entry.Name, num);
			}
		}

		public void Add(string fileName, global::ICSharpCode.SharpZipLib.Zip.CompressionMethod compressionMethod, bool useUnicodeText)
		{
			if (fileName == null)
			{
				throw new global::System.ArgumentNullException("fileName");
			}
			if (isDisposed_)
			{
				throw new global::System.ObjectDisposedException("ZipFile");
			}
			if (!global::ICSharpCode.SharpZipLib.Zip.ZipEntry.IsCompressionMethodSupported(compressionMethod))
			{
				throw new global::System.ArgumentOutOfRangeException("compressionMethod");
			}
			CheckUpdating();
			contentsEdited_ = true;
			global::ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = EntryFactory.MakeFileEntry(fileName);
			zipEntry.IsUnicodeText = useUnicodeText;
			zipEntry.CompressionMethod = compressionMethod;
			AddUpdate(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(fileName, zipEntry));
		}

		public void Add(string fileName, global::ICSharpCode.SharpZipLib.Zip.CompressionMethod compressionMethod)
		{
			if (fileName == null)
			{
				throw new global::System.ArgumentNullException("fileName");
			}
			if (!global::ICSharpCode.SharpZipLib.Zip.ZipEntry.IsCompressionMethodSupported(compressionMethod))
			{
				throw new global::System.ArgumentOutOfRangeException("compressionMethod");
			}
			CheckUpdating();
			contentsEdited_ = true;
			global::ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = EntryFactory.MakeFileEntry(fileName);
			zipEntry.CompressionMethod = compressionMethod;
			AddUpdate(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(fileName, zipEntry));
		}

		public void Add(string fileName)
		{
			if (fileName == null)
			{
				throw new global::System.ArgumentNullException("fileName");
			}
			CheckUpdating();
			AddUpdate(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(fileName, EntryFactory.MakeFileEntry(fileName)));
		}

		public void Add(string fileName, string entryName)
		{
			if (fileName == null)
			{
				throw new global::System.ArgumentNullException("fileName");
			}
			if (entryName == null)
			{
				throw new global::System.ArgumentNullException("entryName");
			}
			CheckUpdating();
			AddUpdate(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(fileName, EntryFactory.MakeFileEntry(fileName, entryName, true)));
		}

		public void Add(global::ICSharpCode.SharpZipLib.Zip.IStaticDataSource dataSource, string entryName)
		{
			if (dataSource == null)
			{
				throw new global::System.ArgumentNullException("dataSource");
			}
			if (entryName == null)
			{
				throw new global::System.ArgumentNullException("entryName");
			}
			CheckUpdating();
			AddUpdate(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(dataSource, EntryFactory.MakeFileEntry(entryName, false)));
		}

		public void Add(global::ICSharpCode.SharpZipLib.Zip.IStaticDataSource dataSource, string entryName, global::ICSharpCode.SharpZipLib.Zip.CompressionMethod compressionMethod)
		{
			if (dataSource == null)
			{
				throw new global::System.ArgumentNullException("dataSource");
			}
			if (entryName == null)
			{
				throw new global::System.ArgumentNullException("entryName");
			}
			CheckUpdating();
			global::ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = EntryFactory.MakeFileEntry(entryName, false);
			zipEntry.CompressionMethod = compressionMethod;
			AddUpdate(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(dataSource, zipEntry));
		}

		public void Add(global::ICSharpCode.SharpZipLib.Zip.IStaticDataSource dataSource, string entryName, global::ICSharpCode.SharpZipLib.Zip.CompressionMethod compressionMethod, bool useUnicodeText)
		{
			if (dataSource == null)
			{
				throw new global::System.ArgumentNullException("dataSource");
			}
			if (entryName == null)
			{
				throw new global::System.ArgumentNullException("entryName");
			}
			CheckUpdating();
			global::ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = EntryFactory.MakeFileEntry(entryName, false);
			zipEntry.IsUnicodeText = useUnicodeText;
			zipEntry.CompressionMethod = compressionMethod;
			AddUpdate(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(dataSource, zipEntry));
		}

		public void Add(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			if (entry == null)
			{
				throw new global::System.ArgumentNullException("entry");
			}
			CheckUpdating();
			if (entry.Size != 0 || entry.CompressedSize != 0)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Entry cannot have any data");
			}
			AddUpdate(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Add, entry));
		}

		public void AddDirectory(string directoryName)
		{
			if (directoryName == null)
			{
				throw new global::System.ArgumentNullException("directoryName");
			}
			CheckUpdating();
			global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry = EntryFactory.MakeDirectoryEntry(directoryName);
			AddUpdate(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate(global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Add, entry));
		}

		public bool Delete(string fileName)
		{
			if (fileName == null)
			{
				throw new global::System.ArgumentNullException("fileName");
			}
			CheckUpdating();
			bool flag = false;
			int num = FindExistingUpdate(fileName);
			if (num >= 0 && updates_[num] != null)
			{
				flag = true;
				contentsEdited_ = true;
				updates_[num] = null;
				updateCount_--;
				return flag;
			}
			throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Cannot find entry to delete");
		}

		public void Delete(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			if (entry == null)
			{
				throw new global::System.ArgumentNullException("entry");
			}
			CheckUpdating();
			int num = FindExistingUpdate(entry);
			if (num >= 0)
			{
				contentsEdited_ = true;
				updates_[num] = null;
				updateCount_--;
				return;
			}
			throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Cannot find entry to delete");
		}

		private void WriteLEShort(int value)
		{
			baseStream_.WriteByte((byte)(value & 0xFF));
			baseStream_.WriteByte((byte)((value >> 8) & 0xFF));
		}

		private void WriteLEUshort(ushort value)
		{
			baseStream_.WriteByte((byte)(value & 0xFF));
			baseStream_.WriteByte((byte)(value >> 8));
		}

		private void WriteLEInt(int value)
		{
			WriteLEShort(value & 0xFFFF);
			WriteLEShort(value >> 16);
		}

		private void WriteLEUint(uint value)
		{
			WriteLEUshort((ushort)(value & 0xFFFF));
			WriteLEUshort((ushort)(value >> 16));
		}

		private void WriteLeLong(long value)
		{
			WriteLEInt((int)(value & 0xFFFFFFFFu));
			WriteLEInt((int)(value >> 32));
		}

		private void WriteLEUlong(ulong value)
		{
			WriteLEUint((uint)(value & 0xFFFFFFFFu));
			WriteLEUint((uint)(value >> 32));
		}

		private void WriteLocalEntryHeader(global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update)
		{
			global::ICSharpCode.SharpZipLib.Zip.ZipEntry outEntry = update.OutEntry;
			outEntry.Offset = baseStream_.Position;
			if (update.Command != global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Copy)
			{
				if (outEntry.CompressionMethod == global::ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated)
				{
					if (outEntry.Size == 0)
					{
						outEntry.CompressedSize = outEntry.Size;
						outEntry.Crc = 0L;
						outEntry.CompressionMethod = global::ICSharpCode.SharpZipLib.Zip.CompressionMethod.Stored;
					}
				}
				else if (outEntry.CompressionMethod == global::ICSharpCode.SharpZipLib.Zip.CompressionMethod.Stored)
				{
					outEntry.Flags &= -9;
				}
				if (HaveKeys)
				{
					outEntry.IsCrypted = true;
					if (outEntry.Crc < 0)
					{
						outEntry.Flags |= 8;
					}
				}
				else
				{
					outEntry.IsCrypted = false;
				}
				switch (useZip64_)
				{
				case global::ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic:
					if (outEntry.Size < 0)
					{
						outEntry.ForceZip64();
					}
					break;
				case global::ICSharpCode.SharpZipLib.Zip.UseZip64.On:
					outEntry.ForceZip64();
					break;
				}
			}
			WriteLEInt(67324752);
			WriteLEShort(outEntry.Version);
			WriteLEShort(outEntry.Flags);
			WriteLEShort((byte)outEntry.CompressionMethod);
			WriteLEInt((int)outEntry.DosTime);
			if (!outEntry.HasCrc)
			{
				update.CrcPatchOffset = baseStream_.Position;
				WriteLEInt(0);
			}
			else
			{
				WriteLEInt((int)outEntry.Crc);
			}
			if (outEntry.LocalHeaderRequiresZip64)
			{
				WriteLEInt(-1);
				WriteLEInt(-1);
			}
			else
			{
				if (outEntry.CompressedSize < 0 || outEntry.Size < 0)
				{
					update.SizePatchOffset = baseStream_.Position;
				}
				WriteLEInt((int)outEntry.CompressedSize);
				WriteLEInt((int)outEntry.Size);
			}
			byte[] array = global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToArray(outEntry.Flags, outEntry.Name);
			if (array.Length > 65535)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Entry name too long.");
			}
			global::ICSharpCode.SharpZipLib.Zip.ZipExtraData zipExtraData = new global::ICSharpCode.SharpZipLib.Zip.ZipExtraData(outEntry.ExtraData);
			if (outEntry.LocalHeaderRequiresZip64)
			{
				zipExtraData.StartNewEntry();
				zipExtraData.AddLeLong(outEntry.Size);
				zipExtraData.AddLeLong(outEntry.CompressedSize);
				zipExtraData.AddNewEntry(1);
			}
			else
			{
				zipExtraData.Delete(1);
			}
			outEntry.ExtraData = zipExtraData.GetEntryData();
			WriteLEShort(array.Length);
			WriteLEShort(outEntry.ExtraData.Length);
			if (array.Length > 0)
			{
				baseStream_.Write(array, 0, array.Length);
			}
			if (outEntry.LocalHeaderRequiresZip64)
			{
				if (!zipExtraData.Find(1))
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Internal error cannot find extra data");
				}
				update.SizePatchOffset = baseStream_.Position + zipExtraData.CurrentReadIndex;
			}
			if (outEntry.ExtraData.Length > 0)
			{
				baseStream_.Write(outEntry.ExtraData, 0, outEntry.ExtraData.Length);
			}
		}

		private int WriteCentralDirectoryHeader(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			if (entry.CompressedSize < 0)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Attempt to write central directory entry with unknown csize");
			}
			if (entry.Size < 0)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Attempt to write central directory entry with unknown size");
			}
			if (entry.Crc < 0)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Attempt to write central directory entry with unknown crc");
			}
			WriteLEInt(33639248);
			WriteLEShort(51);
			WriteLEShort(entry.Version);
			WriteLEShort(entry.Flags);
			WriteLEShort((byte)entry.CompressionMethod);
			WriteLEInt((int)entry.DosTime);
			WriteLEInt((int)entry.Crc);
			if (entry.IsZip64Forced() || entry.CompressedSize >= uint.MaxValue)
			{
				WriteLEInt(-1);
			}
			else
			{
				WriteLEInt((int)(entry.CompressedSize & 0xFFFFFFFFu));
			}
			if (entry.IsZip64Forced() || entry.Size >= uint.MaxValue)
			{
				WriteLEInt(-1);
			}
			else
			{
				WriteLEInt((int)entry.Size);
			}
			byte[] array = global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToArray(entry.Flags, entry.Name);
			if (array.Length > 65535)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Entry name is too long.");
			}
			WriteLEShort(array.Length);
			global::ICSharpCode.SharpZipLib.Zip.ZipExtraData zipExtraData = new global::ICSharpCode.SharpZipLib.Zip.ZipExtraData(entry.ExtraData);
			if (entry.CentralHeaderRequiresZip64)
			{
				zipExtraData.StartNewEntry();
				if (entry.Size >= uint.MaxValue || useZip64_ == global::ICSharpCode.SharpZipLib.Zip.UseZip64.On)
				{
					zipExtraData.AddLeLong(entry.Size);
				}
				if (entry.CompressedSize >= uint.MaxValue || useZip64_ == global::ICSharpCode.SharpZipLib.Zip.UseZip64.On)
				{
					zipExtraData.AddLeLong(entry.CompressedSize);
				}
				if (entry.Offset >= uint.MaxValue)
				{
					zipExtraData.AddLeLong(entry.Offset);
				}
				zipExtraData.AddNewEntry(1);
			}
			else
			{
				zipExtraData.Delete(1);
			}
			byte[] entryData = zipExtraData.GetEntryData();
			WriteLEShort(entryData.Length);
			WriteLEShort((entry.Comment != null) ? entry.Comment.Length : 0);
			WriteLEShort(0);
			WriteLEShort(0);
			if (entry.ExternalFileAttributes != -1)
			{
				WriteLEInt(entry.ExternalFileAttributes);
			}
			else if (entry.IsDirectory)
			{
				WriteLEUint(16u);
			}
			else
			{
				WriteLEUint(0u);
			}
			if (entry.Offset >= uint.MaxValue)
			{
				WriteLEUint(uint.MaxValue);
			}
			else
			{
				WriteLEUint((uint)entry.Offset);
			}
			if (array.Length > 0)
			{
				baseStream_.Write(array, 0, array.Length);
			}
			if (entryData.Length > 0)
			{
				baseStream_.Write(entryData, 0, entryData.Length);
			}
			byte[] array2 = ((entry.Comment != null) ? global::System.Text.Encoding.ASCII.GetBytes(entry.Comment) : new byte[0]);
			if (array2.Length > 0)
			{
				baseStream_.Write(array2, 0, array2.Length);
			}
			return 46 + array.Length + entryData.Length + array2.Length;
		}

		private void PostUpdateCleanup()
		{
			updateDataSource_ = null;
			updates_ = null;
			updateIndex_ = null;
			if (archiveStorage_ != null)
			{
				archiveStorage_.Dispose();
				archiveStorage_ = null;
			}
		}

		private string GetTransformedFileName(string name)
		{
			global::ICSharpCode.SharpZipLib.Core.INameTransform nameTransform = NameTransform;
			if (nameTransform == null)
			{
				return name;
			}
			return nameTransform.TransformFile(name);
		}

		private string GetTransformedDirectoryName(string name)
		{
			global::ICSharpCode.SharpZipLib.Core.INameTransform nameTransform = NameTransform;
			if (nameTransform == null)
			{
				return name;
			}
			return nameTransform.TransformDirectory(name);
		}

		private byte[] GetBuffer()
		{
			if (copyBuffer_ == null)
			{
				copyBuffer_ = new byte[bufferSize_];
			}
			return copyBuffer_;
		}

		private void CopyDescriptorBytes(global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update, global::System.IO.Stream dest, global::System.IO.Stream source)
		{
			int num = GetDescriptorSize(update);
			if (num <= 0)
			{
				return;
			}
			byte[] buffer = GetBuffer();
			while (num > 0)
			{
				int count = global::System.Math.Min(buffer.Length, num);
				int num2 = source.Read(buffer, 0, count);
				if (num2 > 0)
				{
					dest.Write(buffer, 0, num2);
					num -= num2;
					continue;
				}
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Unxpected end of stream");
			}
		}

		private void CopyBytes(global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update, global::System.IO.Stream destination, global::System.IO.Stream source, long bytesToCopy, bool updateCrc)
		{
			if (destination == source)
			{
				throw new global::System.InvalidOperationException("Destination and source are the same");
			}
			global::ICSharpCode.SharpZipLib.Checksums.Crc32 crc = new global::ICSharpCode.SharpZipLib.Checksums.Crc32();
			byte[] buffer = GetBuffer();
			long num = bytesToCopy;
			long num2 = 0L;
			int num4;
			do
			{
				int num3 = buffer.Length;
				if (bytesToCopy < num3)
				{
					num3 = (int)bytesToCopy;
				}
				num4 = source.Read(buffer, 0, num3);
				if (num4 > 0)
				{
					if (updateCrc)
					{
						crc.Update(buffer, 0, num4);
					}
					destination.Write(buffer, 0, num4);
					bytesToCopy -= num4;
					num2 += num4;
				}
			}
			while (num4 > 0 && bytesToCopy > 0);
			if (num2 != num)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Failed to copy bytes expected {0} read {1}", num, num2));
			}
			if (updateCrc)
			{
				update.OutEntry.Crc = crc.Value;
			}
		}

		private int GetDescriptorSize(global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update)
		{
			int result = 0;
			if ((update.Entry.Flags & 8) != 0)
			{
				result = 12;
				if (update.Entry.LocalHeaderRequiresZip64)
				{
					result = 20;
				}
			}
			return result;
		}

		private void CopyDescriptorBytesDirect(global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update, global::System.IO.Stream stream, ref long destinationPosition, long sourcePosition)
		{
			int num = GetDescriptorSize(update);
			while (num > 0)
			{
				int count = num;
				byte[] buffer = GetBuffer();
				stream.Position = sourcePosition;
				int num2 = stream.Read(buffer, 0, count);
				if (num2 > 0)
				{
					stream.Position = destinationPosition;
					stream.Write(buffer, 0, num2);
					num -= num2;
					destinationPosition += num2;
					sourcePosition += num2;
					continue;
				}
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Unxpected end of stream");
			}
		}

		private void CopyEntryDataDirect(global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update, global::System.IO.Stream stream, bool updateCrc, ref long destinationPosition, ref long sourcePosition)
		{
			long num = update.Entry.CompressedSize;
			global::ICSharpCode.SharpZipLib.Checksums.Crc32 crc = new global::ICSharpCode.SharpZipLib.Checksums.Crc32();
			byte[] buffer = GetBuffer();
			long num2 = num;
			long num3 = 0L;
			int num5;
			do
			{
				int num4 = buffer.Length;
				if (num < num4)
				{
					num4 = (int)num;
				}
				stream.Position = sourcePosition;
				num5 = stream.Read(buffer, 0, num4);
				if (num5 > 0)
				{
					if (updateCrc)
					{
						crc.Update(buffer, 0, num5);
					}
					stream.Position = destinationPosition;
					stream.Write(buffer, 0, num5);
					destinationPosition += num5;
					sourcePosition += num5;
					num -= num5;
					num3 += num5;
				}
			}
			while (num5 > 0 && num > 0);
			if (num3 != num2)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Failed to copy bytes expected {0} read {1}", num2, num3));
			}
			if (updateCrc)
			{
				update.OutEntry.Crc = crc.Value;
			}
		}

		private int FindExistingUpdate(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			int result = -1;
			string transformedFileName = GetTransformedFileName(entry.Name);
			if (updateIndex_.ContainsKey(transformedFileName))
			{
				result = (int)updateIndex_[transformedFileName];
			}
			return result;
		}

		private int FindExistingUpdate(string fileName)
		{
			int result = -1;
			string transformedFileName = GetTransformedFileName(fileName);
			if (updateIndex_.ContainsKey(transformedFileName))
			{
				result = (int)updateIndex_[transformedFileName];
			}
			return result;
		}

		private global::System.IO.Stream GetOutputStream(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			global::System.IO.Stream stream = baseStream_;
			if (entry.IsCrypted)
			{
				stream = CreateAndInitEncryptionStream(stream, entry);
			}
			switch (entry.CompressionMethod)
			{
			case global::ICSharpCode.SharpZipLib.Zip.CompressionMethod.Stored:
				return new global::ICSharpCode.SharpZipLib.Zip.ZipFile.UncompressedStream(stream);
			case global::ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated:
			{
				global::ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream deflaterOutputStream = new global::ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream(stream, new global::ICSharpCode.SharpZipLib.Zip.Compression.Deflater(9, true));
				deflaterOutputStream.IsStreamOwner = false;
				return deflaterOutputStream;
			}
			default:
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Unknown compression method " + entry.CompressionMethod);
			}
		}

		private void AddEntry(global::ICSharpCode.SharpZipLib.Zip.ZipFile workFile, global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update)
		{
			global::System.IO.Stream stream = null;
			if (update.Entry.IsFile)
			{
				stream = update.GetSource();
				if (stream == null)
				{
					stream = updateDataSource_.GetSource(update.Entry, update.Filename);
				}
			}
			if (stream != null)
			{
				using (stream)
				{
					long length = stream.Length;
					if (update.OutEntry.Size < 0)
					{
						update.OutEntry.Size = length;
					}
					else if (update.OutEntry.Size != length)
					{
						throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Entry size/stream size mismatch");
					}
					workFile.WriteLocalEntryHeader(update);
					long position = workFile.baseStream_.Position;
					using (global::System.IO.Stream destination = workFile.GetOutputStream(update.OutEntry))
					{
						CopyBytes(update, destination, stream, length, true);
					}
					long position2 = workFile.baseStream_.Position;
					update.OutEntry.CompressedSize = position2 - position;
					if ((update.OutEntry.Flags & 8) == 8)
					{
						global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(workFile.baseStream_);
						zipHelperStream.WriteDataDescriptor(update.OutEntry);
					}
					return;
				}
			}
			workFile.WriteLocalEntryHeader(update);
			update.OutEntry.CompressedSize = 0L;
		}

		private void ModifyEntry(global::ICSharpCode.SharpZipLib.Zip.ZipFile workFile, global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update)
		{
			workFile.WriteLocalEntryHeader(update);
			long position = workFile.baseStream_.Position;
			if (update.Entry.IsFile && update.Filename != null)
			{
				using (global::System.IO.Stream destination = workFile.GetOutputStream(update.OutEntry))
				{
					using (global::System.IO.Stream stream = GetInputStream(update.Entry))
					{
						CopyBytes(update, destination, stream, stream.Length, true);
					}
				}
			}
			long position2 = workFile.baseStream_.Position;
			update.Entry.CompressedSize = position2 - position;
		}

		private void CopyEntryDirect(global::ICSharpCode.SharpZipLib.Zip.ZipFile workFile, global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update, ref long destinationPosition)
		{
			bool flag = false;
			if (update.Entry.Offset == destinationPosition)
			{
				flag = true;
			}
			if (!flag)
			{
				baseStream_.Position = destinationPosition;
				workFile.WriteLocalEntryHeader(update);
				destinationPosition = baseStream_.Position;
			}
			long num = 0L;
			long num2 = update.Entry.Offset + 26;
			baseStream_.Seek(num2, global::System.IO.SeekOrigin.Begin);
			uint num3 = ReadLEUshort();
			uint num4 = ReadLEUshort();
			num = baseStream_.Position + num3 + num4;
			if (flag)
			{
				if (update.OffsetBasedSize != -1)
				{
					destinationPosition += update.OffsetBasedSize;
				}
				else
				{
					destinationPosition += num - num2 + 26 + update.Entry.CompressedSize + GetDescriptorSize(update);
				}
				return;
			}
			if (update.Entry.CompressedSize > 0)
			{
				CopyEntryDataDirect(update, baseStream_, false, ref destinationPosition, ref num);
			}
			CopyDescriptorBytesDirect(update, baseStream_, ref destinationPosition, num);
		}

		private void CopyEntry(global::ICSharpCode.SharpZipLib.Zip.ZipFile workFile, global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate update)
		{
			workFile.WriteLocalEntryHeader(update);
			if (update.Entry.CompressedSize > 0)
			{
				long offset = update.Entry.Offset + 26;
				baseStream_.Seek(offset, global::System.IO.SeekOrigin.Begin);
				uint num = ReadLEUshort();
				uint num2 = ReadLEUshort();
				baseStream_.Seek(num + num2, global::System.IO.SeekOrigin.Current);
				CopyBytes(update, workFile.baseStream_, baseStream_, update.Entry.CompressedSize, false);
			}
			CopyDescriptorBytes(update, workFile.baseStream_, baseStream_);
		}

		private void Reopen(global::System.IO.Stream source)
		{
			if (source == null)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Failed to reopen archive - no source");
			}
			isNewArchive_ = false;
			baseStream_ = source;
			ReadEntries();
		}

		private void Reopen()
		{
			if (Name == null)
			{
				throw new global::System.InvalidOperationException("Name is not known cannot Reopen");
			}
#if !UNITY_WEBPLAYER
			Reopen(global::System.IO.File.Open(Name, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.Read));
#endif
		}

		private void UpdateCommentOnly()
		{
			long length = baseStream_.Length;
			global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream zipHelperStream = null;
			if (archiveStorage_.UpdateMode == global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode.Safe)
			{
				global::System.IO.Stream stream = archiveStorage_.MakeTemporaryCopy(baseStream_);
				zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(stream);
				zipHelperStream.IsStreamOwner = true;
				baseStream_.Close();
				baseStream_ = null;
			}
			else if (archiveStorage_.UpdateMode == global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode.Direct)
			{
				baseStream_ = archiveStorage_.OpenForDirectUpdate(baseStream_);
				zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(baseStream_);
			}
			else
			{
				baseStream_.Close();
				baseStream_ = null;
				zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(Name);
			}
			using (zipHelperStream)
			{
				long num = zipHelperStream.LocateBlockWithSignature(101010256, length, 22, 65535);
				if (num < 0)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Cannot find central directory");
				}
				zipHelperStream.Position += 16L;
				byte[] rawComment = newComment_.RawComment;
				zipHelperStream.WriteLEShort(rawComment.Length);
				zipHelperStream.Write(rawComment, 0, rawComment.Length);
				zipHelperStream.SetLength(zipHelperStream.Position);
			}
			if (archiveStorage_.UpdateMode == global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode.Safe)
			{
				Reopen(archiveStorage_.ConvertTemporaryToFinal());
			}
			else
			{
				ReadEntries();
			}
		}

		private void RunUpdates()
		{
			long num = 0L;
			long num2 = 0L;
			bool flag = false;
			long destinationPosition = 0L;
			global::ICSharpCode.SharpZipLib.Zip.ZipFile zipFile;
			if (IsNewArchive)
			{
				zipFile = this;
				zipFile.baseStream_.Position = 0L;
				flag = true;
			}
			else if (archiveStorage_.UpdateMode == global::ICSharpCode.SharpZipLib.Zip.FileUpdateMode.Direct)
			{
				zipFile = this;
				zipFile.baseStream_.Position = 0L;
				flag = true;
				updates_.Sort(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateComparer());
			}
			else
			{
				zipFile = Create(archiveStorage_.GetTemporaryOutput());
				zipFile.UseZip64 = UseZip64;
				if (key != null)
				{
					zipFile.key = (byte[])key.Clone();
				}
			}
			try
			{
				foreach (global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate item in updates_)
				{
					if (item == null)
					{
						continue;
					}
					switch (item.Command)
					{
					case global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Copy:
						if (flag)
						{
							CopyEntryDirect(zipFile, item, ref destinationPosition);
						}
						else
						{
							CopyEntry(zipFile, item);
						}
						break;
					case global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Modify:
						ModifyEntry(zipFile, item);
						break;
					case global::ICSharpCode.SharpZipLib.Zip.ZipFile.UpdateCommand.Add:
						if (!IsNewArchive && flag)
						{
							zipFile.baseStream_.Position = destinationPosition;
						}
						AddEntry(zipFile, item);
						if (flag)
						{
							destinationPosition = zipFile.baseStream_.Position;
						}
						break;
					}
				}
				if (!IsNewArchive && flag)
				{
					zipFile.baseStream_.Position = destinationPosition;
				}
				long position = zipFile.baseStream_.Position;
				foreach (global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate item2 in updates_)
				{
					if (item2 != null)
					{
						num += zipFile.WriteCentralDirectoryHeader(item2.OutEntry);
					}
				}
				byte[] comment = ((newComment_ != null) ? newComment_.RawComment : global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToArray(comment_));
				using (global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(zipFile.baseStream_))
				{
					zipHelperStream.WriteEndOfCentralDirectory(updateCount_, num, position, comment);
				}
				num2 = zipFile.baseStream_.Position;
				foreach (global::ICSharpCode.SharpZipLib.Zip.ZipFile.ZipUpdate item3 in updates_)
				{
					if (item3 == null)
					{
						continue;
					}
					if (item3.CrcPatchOffset > 0 && item3.OutEntry.CompressedSize > 0)
					{
						zipFile.baseStream_.Position = item3.CrcPatchOffset;
						zipFile.WriteLEInt((int)item3.OutEntry.Crc);
					}
					if (item3.SizePatchOffset > 0)
					{
						zipFile.baseStream_.Position = item3.SizePatchOffset;
						if (item3.OutEntry.LocalHeaderRequiresZip64)
						{
							zipFile.WriteLeLong(item3.OutEntry.Size);
							zipFile.WriteLeLong(item3.OutEntry.CompressedSize);
						}
						else
						{
							zipFile.WriteLEInt((int)item3.OutEntry.CompressedSize);
							zipFile.WriteLEInt((int)item3.OutEntry.Size);
						}
					}
				}
			}
			catch
			{
				zipFile.Close();
				if (!flag && zipFile.Name != null)
				{
#if !UNITY_WEBPLAYER
					global::System.IO.File.Delete(zipFile.Name);
#endif
				}
				throw;
			}
			if (flag)
			{
				zipFile.baseStream_.SetLength(num2);
				zipFile.baseStream_.Flush();
				isNewArchive_ = false;
				ReadEntries();
			}
			else
			{
				baseStream_.Close();
				Reopen(archiveStorage_.ConvertTemporaryToFinal());
			}
		}

		private void CheckUpdating()
		{
			if (updates_ == null)
			{
				throw new global::System.InvalidOperationException("BeginUpdate has not been called");
			}
		}

		void global::System.IDisposable.Dispose()
		{
			Close();
		}

		private void DisposeInternal(bool disposing)
		{
			if (isDisposed_)
			{
				return;
			}
			isDisposed_ = true;
			entries_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry[0];
			if (IsStreamOwner && baseStream_ != null)
			{
				lock (baseStream_)
				{
					baseStream_.Close();
				}
			}
			PostUpdateCleanup();
		}

		protected virtual void Dispose(bool disposing)
		{
			DisposeInternal(disposing);
		}

		private ushort ReadLEUshort()
		{
			int num = baseStream_.ReadByte();
			if (num < 0)
			{
				throw new global::System.IO.EndOfStreamException("End of stream");
			}
			int num2 = baseStream_.ReadByte();
			if (num2 < 0)
			{
				throw new global::System.IO.EndOfStreamException("End of stream");
			}
			return (ushort)((ushort)num | (ushort)(num2 << 8));
		}

		private uint ReadLEUint()
		{
			return (uint)(ReadLEUshort() | (ReadLEUshort() << 16));
		}

		private ulong ReadLEUlong()
		{
			return ReadLEUint() | ((ulong)ReadLEUint() << 32);
		}

		private long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
		{
			using (global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream zipHelperStream = new global::ICSharpCode.SharpZipLib.Zip.ZipHelperStream(baseStream_))
			{
				return zipHelperStream.LocateBlockWithSignature(signature, endLocation, minimumBlockSize, maximumVariableData);
			}
		}

		private void ReadEntries()
		{
			if (!baseStream_.CanSeek)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("ZipFile stream must be seekable");
			}
			long num = LocateBlockWithSignature(101010256, baseStream_.Length, 22, 65535);
			if (num < 0)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Cannot find central directory");
			}
			ushort num2 = ReadLEUshort();
			ushort num3 = ReadLEUshort();
			ulong num4 = ReadLEUshort();
			ulong num5 = ReadLEUshort();
			ulong num6 = ReadLEUint();
			long num7 = ReadLEUint();
			uint num8 = ReadLEUshort();
			if (num8 != 0)
			{
				byte[] array = new byte[num8];
				global::ICSharpCode.SharpZipLib.Core.StreamUtils.ReadFully(baseStream_, array);
				comment_ = global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToString(array);
			}
			else
			{
				comment_ = string.Empty;
			}
			bool flag = false;
			if (num2 == ushort.MaxValue || num3 == ushort.MaxValue || num4 == 65535 || num5 == 65535 || num6 == uint.MaxValue || num7 == uint.MaxValue)
			{
				flag = true;
				long num9 = LocateBlockWithSignature(117853008, num, 0, 4096);
				if (num9 < 0)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Cannot find Zip64 locator");
				}
				ReadLEUint();
				ulong num10 = ReadLEUlong();
				ReadLEUint();
				baseStream_.Position = (long)num10;
				long num11 = ReadLEUint();
				if (num11 != 101075792)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException(string.Format("Invalid Zip64 Central directory signature at {0:X}", num10));
				}
				ReadLEUlong();
				ReadLEUshort();
				ReadLEUshort();
				ReadLEUint();
				ReadLEUint();
				num4 = ReadLEUlong();
				num5 = ReadLEUlong();
				num6 = ReadLEUlong();
				num7 = (long)ReadLEUlong();
			}
			entries_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry[num4];
			if (!flag && num7 < num - (long)(4 + num6))
			{
				offsetOfFirstEntry = num - ((long)(4 + num6) + num7);
				if (offsetOfFirstEntry <= 0)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Invalid embedded zip archive");
				}
			}
			baseStream_.Seek(offsetOfFirstEntry + num7, global::System.IO.SeekOrigin.Begin);
			for (ulong num12 = 0uL; num12 < num4; num12++)
			{
				if (ReadLEUint() != 33639248)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Wrong Central Directory signature");
				}
				int madeByInfo = ReadLEUshort();
				int versionRequiredToExtract = ReadLEUshort();
				int num13 = ReadLEUshort();
				int method = ReadLEUshort();
				uint num14 = ReadLEUint();
				uint num15 = ReadLEUint();
				long num16 = ReadLEUint();
				long num17 = ReadLEUint();
				int num18 = ReadLEUshort();
				int num19 = ReadLEUshort();
				int num20 = ReadLEUshort();
				ReadLEUshort();
				ReadLEUshort();
				uint externalFileAttributes = ReadLEUint();
				long offset = ReadLEUint();
				byte[] array2 = new byte[global::System.Math.Max(num18, num20)];
				global::ICSharpCode.SharpZipLib.Core.StreamUtils.ReadFully(baseStream_, array2, 0, num18);
				string name = global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToStringExt(num13, array2, num18);
				global::ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = new global::ICSharpCode.SharpZipLib.Zip.ZipEntry(name, versionRequiredToExtract, madeByInfo, (global::ICSharpCode.SharpZipLib.Zip.CompressionMethod)method);
				zipEntry.Crc = (long)num15 & 0xFFFFFFFFL;
				zipEntry.Size = num17 & 0xFFFFFFFFu;
				zipEntry.CompressedSize = num16 & 0xFFFFFFFFu;
				zipEntry.Flags = num13;
				zipEntry.DosTime = num14;
				zipEntry.ZipFileIndex = (long)num12;
				zipEntry.Offset = offset;
				zipEntry.ExternalFileAttributes = (int)externalFileAttributes;
				if ((num13 & 8) == 0)
				{
					zipEntry.CryptoCheckValue = (byte)(num15 >> 24);
				}
				else
				{
					zipEntry.CryptoCheckValue = (byte)((num14 >> 8) & 0xFF);
				}
				if (num19 > 0)
				{
					byte[] array3 = new byte[num19];
					global::ICSharpCode.SharpZipLib.Core.StreamUtils.ReadFully(baseStream_, array3);
					zipEntry.ExtraData = array3;
				}
				zipEntry.ProcessExtraData(false);
				if (num20 > 0)
				{
					global::ICSharpCode.SharpZipLib.Core.StreamUtils.ReadFully(baseStream_, array2, 0, num20);
					zipEntry.Comment = global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToStringExt(num13, array2, num20);
				}
				entries_[num12] = zipEntry;
			}
		}

		private long LocateEntry(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			return TestLocalHeader(entry, global::ICSharpCode.SharpZipLib.Zip.ZipFile.HeaderTest.Extract);
		}

		private global::System.IO.Stream CreateAndInitDecryptionStream(global::System.IO.Stream baseStream, global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			global::System.Security.Cryptography.CryptoStream cryptoStream = null;
			if (entry.Version < 50 || (entry.Flags & 0x40) == 0)
			{
				global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged pkzipClassicManaged = new global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged();
				OnKeysRequired(entry.Name);
				if (!HaveKeys)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("No password available for encrypted stream");
				}
				cryptoStream = new global::System.Security.Cryptography.CryptoStream(baseStream, pkzipClassicManaged.CreateDecryptor(key, null), global::System.Security.Cryptography.CryptoStreamMode.Read);
				CheckClassicPassword(cryptoStream, entry);
			}
			else
			{
				if (entry.Version != 51)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Decryption method not supported");
				}
				OnKeysRequired(entry.Name);
				if (!HaveKeys)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("No password available for AES encrypted stream");
				}
				int aESSaltLen = entry.AESSaltLen;
				byte[] array = new byte[aESSaltLen];
				int num = baseStream.Read(array, 0, aESSaltLen);
				if (num != aESSaltLen)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("AES Salt expected " + aESSaltLen + " got " + num);
				}
				byte[] array2 = new byte[2];
				baseStream.Read(array2, 0, 2);
				int blockSize = entry.AESKeySize / 8;
				global::ICSharpCode.SharpZipLib.Encryption.ZipAESTransform zipAESTransform = new global::ICSharpCode.SharpZipLib.Encryption.ZipAESTransform(rawPassword_, array, blockSize, false);
				byte[] pwdVerifier = zipAESTransform.PwdVerifier;
				if (pwdVerifier[0] != array2[0] || pwdVerifier[1] != array2[1])
				{
					throw new global::System.Exception("Invalid password for AES");
				}
				cryptoStream = new global::ICSharpCode.SharpZipLib.Encryption.ZipAESStream(baseStream, zipAESTransform, global::System.Security.Cryptography.CryptoStreamMode.Read);
			}
			return cryptoStream;
		}

		private global::System.IO.Stream CreateAndInitEncryptionStream(global::System.IO.Stream baseStream, global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			global::System.Security.Cryptography.CryptoStream cryptoStream = null;
			if (entry.Version < 50 || (entry.Flags & 0x40) == 0)
			{
				global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged pkzipClassicManaged = new global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged();
				OnKeysRequired(entry.Name);
				if (!HaveKeys)
				{
					throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("No password available for encrypted stream");
				}
				cryptoStream = new global::System.Security.Cryptography.CryptoStream(new global::ICSharpCode.SharpZipLib.Zip.ZipFile.UncompressedStream(baseStream), pkzipClassicManaged.CreateEncryptor(key, null), global::System.Security.Cryptography.CryptoStreamMode.Write);
				if (entry.Crc < 0 || (entry.Flags & 8) != 0)
				{
					WriteEncryptionHeader(cryptoStream, entry.DosTime << 16);
				}
				else
				{
					WriteEncryptionHeader(cryptoStream, entry.Crc);
				}
			}
			return cryptoStream;
		}

		private static void CheckClassicPassword(global::System.Security.Cryptography.CryptoStream classicCryptoStream, global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			byte[] array = new byte[12];
			global::ICSharpCode.SharpZipLib.Core.StreamUtils.ReadFully(classicCryptoStream, array);
			if (array[11] != entry.CryptoCheckValue)
			{
				throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Invalid password");
			}
		}

		private static void WriteEncryptionHeader(global::System.IO.Stream stream, long crcValue)
		{
			byte[] array = new byte[12];
			global::System.Random random = new global::System.Random();
			random.NextBytes(array);
			array[11] = (byte)(crcValue >> 24);
			stream.Write(array, 0, array.Length);
		}
	}
}
