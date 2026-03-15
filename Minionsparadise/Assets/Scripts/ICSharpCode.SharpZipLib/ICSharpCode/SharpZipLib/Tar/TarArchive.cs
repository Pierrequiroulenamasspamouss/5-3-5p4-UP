namespace ICSharpCode.SharpZipLib.Tar
{
	public class TarArchive : global::System.IDisposable
	{
		private bool keepOldFiles;

		private bool asciiTranslate;

		private int userId;

		private string userName = string.Empty;

		private int groupId;

		private string groupName = string.Empty;

		private string rootPath;

		private string pathPrefix;

		private bool applyUserInfoOverrides;

		private global::ICSharpCode.SharpZipLib.Tar.TarInputStream tarIn;

		private global::ICSharpCode.SharpZipLib.Tar.TarOutputStream tarOut;

		private bool isDisposed;

		public bool AsciiTranslate
		{
			get
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				return asciiTranslate;
			}
			set
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				asciiTranslate = value;
			}
		}

		public string PathPrefix
		{
			get
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				return pathPrefix;
			}
			set
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				pathPrefix = value;
			}
		}

		public string RootPath
		{
			get
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				return rootPath;
			}
			set
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				rootPath = value.Replace('\\', '/').TrimEnd('/');
			}
		}

		public bool ApplyUserInfoOverrides
		{
			get
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				return applyUserInfoOverrides;
			}
			set
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				applyUserInfoOverrides = value;
			}
		}

		public int UserId
		{
			get
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				return userId;
			}
		}

		public string UserName
		{
			get
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				return userName;
			}
		}

		public int GroupId
		{
			get
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				return groupId;
			}
		}

		public string GroupName
		{
			get
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				return groupName;
			}
		}

		public int RecordSize
		{
			get
			{
				if (isDisposed)
				{
					throw new global::System.ObjectDisposedException("TarArchive");
				}
				if (tarIn != null)
				{
					return tarIn.RecordSize;
				}
				if (tarOut != null)
				{
					return tarOut.RecordSize;
				}
				return 10240;
			}
		}

		public bool IsStreamOwner
		{
			set
			{
				if (tarIn != null)
				{
					tarIn.IsStreamOwner = value;
				}
				else
				{
					tarOut.IsStreamOwner = value;
				}
			}
		}

		public event global::ICSharpCode.SharpZipLib.Tar.ProgressMessageHandler ProgressMessageEvent;

		protected virtual void OnProgressMessageEvent(global::ICSharpCode.SharpZipLib.Tar.TarEntry entry, string message)
		{
			global::ICSharpCode.SharpZipLib.Tar.ProgressMessageHandler progressMessageEvent = this.ProgressMessageEvent;
			if (progressMessageEvent != null)
			{
				progressMessageEvent(this, entry, message);
			}
		}

		protected TarArchive()
		{
		}

		protected TarArchive(global::ICSharpCode.SharpZipLib.Tar.TarInputStream stream)
		{
			if (stream == null)
			{
				throw new global::System.ArgumentNullException("stream");
			}
			tarIn = stream;
		}

		protected TarArchive(global::ICSharpCode.SharpZipLib.Tar.TarOutputStream stream)
		{
			if (stream == null)
			{
				throw new global::System.ArgumentNullException("stream");
			}
			tarOut = stream;
		}

		public static global::ICSharpCode.SharpZipLib.Tar.TarArchive CreateInputTarArchive(global::System.IO.Stream inputStream)
		{
			if (inputStream == null)
			{
				throw new global::System.ArgumentNullException("inputStream");
			}
			global::ICSharpCode.SharpZipLib.Tar.TarInputStream tarInputStream = inputStream as global::ICSharpCode.SharpZipLib.Tar.TarInputStream;
			if (tarInputStream != null)
			{
				return new global::ICSharpCode.SharpZipLib.Tar.TarArchive(tarInputStream);
			}
			return CreateInputTarArchive(inputStream, 20);
		}

		public static global::ICSharpCode.SharpZipLib.Tar.TarArchive CreateInputTarArchive(global::System.IO.Stream inputStream, int blockFactor)
		{
			if (inputStream == null)
			{
				throw new global::System.ArgumentNullException("inputStream");
			}
			if (inputStream is global::ICSharpCode.SharpZipLib.Tar.TarInputStream)
			{
				throw new global::System.ArgumentException("TarInputStream not valid");
			}
			return new global::ICSharpCode.SharpZipLib.Tar.TarArchive(new global::ICSharpCode.SharpZipLib.Tar.TarInputStream(inputStream, blockFactor));
		}

		public static global::ICSharpCode.SharpZipLib.Tar.TarArchive CreateOutputTarArchive(global::System.IO.Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new global::System.ArgumentNullException("outputStream");
			}
			global::ICSharpCode.SharpZipLib.Tar.TarOutputStream tarOutputStream = outputStream as global::ICSharpCode.SharpZipLib.Tar.TarOutputStream;
			if (tarOutputStream != null)
			{
				return new global::ICSharpCode.SharpZipLib.Tar.TarArchive(tarOutputStream);
			}
			return CreateOutputTarArchive(outputStream, 20);
		}

		public static global::ICSharpCode.SharpZipLib.Tar.TarArchive CreateOutputTarArchive(global::System.IO.Stream outputStream, int blockFactor)
		{
			if (outputStream == null)
			{
				throw new global::System.ArgumentNullException("outputStream");
			}
			if (outputStream is global::ICSharpCode.SharpZipLib.Tar.TarOutputStream)
			{
				throw new global::System.ArgumentException("TarOutputStream is not valid");
			}
			return new global::ICSharpCode.SharpZipLib.Tar.TarArchive(new global::ICSharpCode.SharpZipLib.Tar.TarOutputStream(outputStream, blockFactor));
		}

		public void SetKeepOldFiles(bool keepExistingFiles)
		{
			if (isDisposed)
			{
				throw new global::System.ObjectDisposedException("TarArchive");
			}
			keepOldFiles = keepExistingFiles;
		}

		[global::System.Obsolete("Use the AsciiTranslate property")]
		public void SetAsciiTranslation(bool translateAsciiFiles)
		{
			if (isDisposed)
			{
				throw new global::System.ObjectDisposedException("TarArchive");
			}
			asciiTranslate = translateAsciiFiles;
		}

		public void SetUserInfo(int userId, string userName, int groupId, string groupName)
		{
			if (isDisposed)
			{
				throw new global::System.ObjectDisposedException("TarArchive");
			}
			this.userId = userId;
			this.userName = userName;
			this.groupId = groupId;
			this.groupName = groupName;
			applyUserInfoOverrides = true;
		}

		[global::System.Obsolete("Use Close instead")]
		public void CloseArchive()
		{
			Close();
		}

		public void ListContents()
		{
			if (isDisposed)
			{
				throw new global::System.ObjectDisposedException("TarArchive");
			}
			while (true)
			{
				global::ICSharpCode.SharpZipLib.Tar.TarEntry nextEntry = tarIn.GetNextEntry();
				if (nextEntry == null)
				{
					break;
				}
				OnProgressMessageEvent(nextEntry, null);
			}
		}

		public void ExtractContents(string destinationDirectory)
		{
			if (isDisposed)
			{
				throw new global::System.ObjectDisposedException("TarArchive");
			}
			while (true)
			{
				global::ICSharpCode.SharpZipLib.Tar.TarEntry nextEntry = tarIn.GetNextEntry();
				if (nextEntry == null)
				{
					break;
				}
				ExtractEntry(destinationDirectory, nextEntry);
			}
		}

		private void ExtractEntry(string destDir, global::ICSharpCode.SharpZipLib.Tar.TarEntry entry)
		{
			OnProgressMessageEvent(entry, null);
			string text = entry.Name;
			if (global::System.IO.Path.IsPathRooted(text))
			{
				text = text.Substring(global::System.IO.Path.GetPathRoot(text).Length);
			}
			text = text.Replace('/', global::System.IO.Path.DirectorySeparatorChar);
			string text2 = global::System.IO.Path.Combine(destDir, text);
			if (entry.IsDirectory)
			{
				EnsureDirectoryExists(text2);
				return;
			}
			string directoryName = global::System.IO.Path.GetDirectoryName(text2);
			EnsureDirectoryExists(directoryName);
			bool flag = true;
#if !UNITY_WEBPLAYER
			global::System.IO.FileInfo fileInfo = new global::System.IO.FileInfo(text2);
			if (fileInfo.Exists)
			{
				if (keepOldFiles)
				{
					OnProgressMessageEvent(entry, "Destination file already exists");
					flag = false;
				}
				else if ((fileInfo.Attributes & global::System.IO.FileAttributes.ReadOnly) != 0)
				{
					OnProgressMessageEvent(entry, "Destination file already exists, and is read-only");
					flag = false;
				}
			}
#endif
			if (!flag)
			{
				return;
			}
			bool flag2 = false;
			global::System.IO.Stream stream = global::System.IO.File.Create(text2);
			if (asciiTranslate)
			{
				flag2 = !IsBinary(text2);
			}
			global::System.IO.StreamWriter streamWriter = null;
			if (flag2)
			{
				streamWriter = new global::System.IO.StreamWriter(stream);
			}
			byte[] array = new byte[32768];
			while (true)
			{
				int num = tarIn.Read(array, 0, array.Length);
				if (num <= 0)
				{
					break;
				}
				if (flag2)
				{
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						if (array[i] == 10)
						{
							string value = global::System.Text.Encoding.ASCII.GetString(array, num2, i - num2);
							streamWriter.WriteLine(value);
							num2 = i + 1;
						}
					}
				}
				else
				{
					stream.Write(array, 0, num);
				}
			}
			if (flag2)
			{
				streamWriter.Close();
			}
			else
			{
				stream.Close();
			}
		}

		public void WriteEntry(global::ICSharpCode.SharpZipLib.Tar.TarEntry sourceEntry, bool recurse)
		{
			if (sourceEntry == null)
			{
				throw new global::System.ArgumentNullException("sourceEntry");
			}
			if (isDisposed)
			{
				throw new global::System.ObjectDisposedException("TarArchive");
			}
			try
			{
				if (recurse)
				{
					global::ICSharpCode.SharpZipLib.Tar.TarHeader.SetValueDefaults(sourceEntry.UserId, sourceEntry.UserName, sourceEntry.GroupId, sourceEntry.GroupName);
				}
				WriteEntryCore(sourceEntry, recurse);
			}
			finally
			{
				if (recurse)
				{
					global::ICSharpCode.SharpZipLib.Tar.TarHeader.RestoreSetValues();
				}
			}
		}

		private void WriteEntryCore(global::ICSharpCode.SharpZipLib.Tar.TarEntry sourceEntry, bool recurse)
		{
			string text = null;
			string text2 = sourceEntry.File;
			global::ICSharpCode.SharpZipLib.Tar.TarEntry tarEntry = (global::ICSharpCode.SharpZipLib.Tar.TarEntry)sourceEntry.Clone();
			if (applyUserInfoOverrides)
			{
				tarEntry.GroupId = groupId;
				tarEntry.GroupName = groupName;
				tarEntry.UserId = userId;
				tarEntry.UserName = userName;
			}
			OnProgressMessageEvent(tarEntry, null);
			if (asciiTranslate && !tarEntry.IsDirectory && !IsBinary(text2))
			{
				text = global::System.IO.Path.GetTempFileName();
#if !UNITY_WEBPLAYER
				using (global::System.IO.StreamReader streamReader = global::System.IO.File.OpenText(text2))
				{
					using (global::System.IO.Stream stream = global::System.IO.File.Create(text))
					{
						while (true)
						{
							string text3 = streamReader.ReadLine();
							if (text3 == null)
							{
								break;
							}
							byte[] bytes = global::System.Text.Encoding.ASCII.GetBytes(text3);
							stream.Write(bytes, 0, bytes.Length);
							stream.WriteByte(10);
						}
						stream.Flush();
					}
				}
				tarEntry.Size = new global::System.IO.FileInfo(text).Length;
#else
				tarEntry.Size = 0L;
#endif
				text2 = text;
			}
			string text4 = null;
			if (rootPath != null && tarEntry.Name.StartsWith(rootPath, global::System.StringComparison.OrdinalIgnoreCase))
			{
				text4 = tarEntry.Name.Substring(rootPath.Length + 1);
			}
			if (pathPrefix != null)
			{
				text4 = ((text4 == null) ? (pathPrefix + "/" + tarEntry.Name) : (pathPrefix + "/" + text4));
			}
			if (text4 != null)
			{
				tarEntry.Name = text4;
			}
			tarOut.PutNextEntry(tarEntry);
			if (tarEntry.IsDirectory)
			{
				if (recurse)
				{
					global::ICSharpCode.SharpZipLib.Tar.TarEntry[] directoryEntries = tarEntry.GetDirectoryEntries();
					for (int i = 0; i < directoryEntries.Length; i++)
					{
						WriteEntryCore(directoryEntries[i], recurse);
					}
				}
				return;
			}
			using (global::System.IO.Stream stream2 = global::System.IO.File.OpenRead(text2))
			{
				byte[] array = new byte[32768];
				while (true)
				{
					int num = stream2.Read(array, 0, array.Length);
					if (num > 0)
					{
						tarOut.Write(array, 0, num);
						continue;
					}
					break;
				}
			}
			if (text != null && text.Length > 0)
			{
				global::System.IO.File.Delete(text);
			}
			tarOut.CloseEntry();
		}

		public void Dispose()
		{
			Dispose(true);
			global::System.GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed)
			{
				return;
			}
			isDisposed = true;
			if (disposing)
			{
				if (tarOut != null)
				{
					tarOut.Flush();
					tarOut.Close();
				}
				if (tarIn != null)
				{
					tarIn.Close();
				}
			}
		}

		public virtual void Close()
		{
			Dispose(true);
		}

		~TarArchive()
		{
			Dispose(false);
		}

		private static void EnsureDirectoryExists(string directoryName)
		{
#if !UNITY_WEBPLAYER
			if (!global::System.IO.Directory.Exists(directoryName))
			{
				try
				{
					global::System.IO.Directory.CreateDirectory(directoryName);
				}
				catch (global::System.Exception ex)
				{
					throw new global::ICSharpCode.SharpZipLib.Tar.TarException("Exception creating directory '" + directoryName + "', " + ex.Message, ex);
				}
			}
#endif
		}

		private static bool IsBinary(string filename)
		{
#if !UNITY_WEBPLAYER
			using (global::System.IO.FileStream fileStream = global::System.IO.File.OpenRead(filename))
			{
				int num = global::System.Math.Min(4096, (int)fileStream.Length);
				byte[] array = new byte[num];
				int num2 = fileStream.Read(array, 0, num);
				for (int i = 0; i < num2; i++)
				{
					byte b = array[i];
					if (b < 8 || (b > 13 && b < 32) || b == byte.MaxValue)
					{
						return true;
					}
				}
			}
#endif
			return false;
		}
	}
}
