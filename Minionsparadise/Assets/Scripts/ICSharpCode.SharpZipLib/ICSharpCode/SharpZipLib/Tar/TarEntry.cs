namespace ICSharpCode.SharpZipLib.Tar
{
	public class TarEntry : global::System.ICloneable
	{
		private string file;

		private global::ICSharpCode.SharpZipLib.Tar.TarHeader header;

		public global::ICSharpCode.SharpZipLib.Tar.TarHeader TarHeader
		{
			get
			{
				return header;
			}
		}

		public string Name
		{
			get
			{
				return header.Name;
			}
			set
			{
				header.Name = value;
			}
		}

		public int UserId
		{
			get
			{
				return header.UserId;
			}
			set
			{
				header.UserId = value;
			}
		}

		public int GroupId
		{
			get
			{
				return header.GroupId;
			}
			set
			{
				header.GroupId = value;
			}
		}

		public string UserName
		{
			get
			{
				return header.UserName;
			}
			set
			{
				header.UserName = value;
			}
		}

		public string GroupName
		{
			get
			{
				return header.GroupName;
			}
			set
			{
				header.GroupName = value;
			}
		}

		public global::System.DateTime ModTime
		{
			get
			{
				return header.ModTime;
			}
			set
			{
				header.ModTime = value;
			}
		}

		public string File
		{
			get
			{
				return file;
			}
		}

		public long Size
		{
			get
			{
				return header.Size;
			}
			set
			{
				header.Size = value;
			}
		}

		public bool IsDirectory
		{
			get
			{
#if !UNITY_WEBPLAYER
				if (file != null)
				{
					return global::System.IO.Directory.Exists(file);
				}
#endif
				if (header != null && (header.TypeFlag == 53 || Name.EndsWith("/")))
				{
					return true;
				}
				return false;
			}
		}

		private TarEntry()
		{
			header = new global::ICSharpCode.SharpZipLib.Tar.TarHeader();
		}

		public TarEntry(byte[] headerBuffer)
		{
			header = new global::ICSharpCode.SharpZipLib.Tar.TarHeader();
			header.ParseBuffer(headerBuffer);
		}

		public TarEntry(global::ICSharpCode.SharpZipLib.Tar.TarHeader header)
		{
			if (header == null)
			{
				throw new global::System.ArgumentNullException("header");
			}
			this.header = (global::ICSharpCode.SharpZipLib.Tar.TarHeader)header.Clone();
		}

		public object Clone()
		{
			global::ICSharpCode.SharpZipLib.Tar.TarEntry tarEntry = new global::ICSharpCode.SharpZipLib.Tar.TarEntry();
			tarEntry.file = file;
			tarEntry.header = (global::ICSharpCode.SharpZipLib.Tar.TarHeader)header.Clone();
			tarEntry.Name = Name;
			return tarEntry;
		}

		public static global::ICSharpCode.SharpZipLib.Tar.TarEntry CreateTarEntry(string name)
		{
			global::ICSharpCode.SharpZipLib.Tar.TarEntry tarEntry = new global::ICSharpCode.SharpZipLib.Tar.TarEntry();
			NameTarHeader(tarEntry.header, name);
			return tarEntry;
		}

		public static global::ICSharpCode.SharpZipLib.Tar.TarEntry CreateEntryFromFile(string fileName)
		{
			global::ICSharpCode.SharpZipLib.Tar.TarEntry tarEntry = new global::ICSharpCode.SharpZipLib.Tar.TarEntry();
			tarEntry.GetFileTarHeader(tarEntry.header, fileName);
			return tarEntry;
		}

		public override bool Equals(object obj)
		{
			global::ICSharpCode.SharpZipLib.Tar.TarEntry tarEntry = obj as global::ICSharpCode.SharpZipLib.Tar.TarEntry;
			if (tarEntry != null)
			{
				return Name.Equals(tarEntry.Name);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public bool IsDescendent(global::ICSharpCode.SharpZipLib.Tar.TarEntry toTest)
		{
			if (toTest == null)
			{
				throw new global::System.ArgumentNullException("toTest");
			}
			return toTest.Name.StartsWith(Name);
		}

		public void SetIds(int userId, int groupId)
		{
			UserId = userId;
			GroupId = groupId;
		}

		public void SetNames(string userName, string groupName)
		{
			UserName = userName;
			GroupName = groupName;
		}

		public void GetFileTarHeader(global::ICSharpCode.SharpZipLib.Tar.TarHeader header, string file)
		{
			if (header == null)
			{
				throw new global::System.ArgumentNullException("header");
			}
			if (file == null)
			{
				throw new global::System.ArgumentNullException("file");
			}
			this.file = file;
			string text = file;
			if (text.IndexOf(global::System.Environment.CurrentDirectory) == 0)
			{
				text = text.Substring(global::System.Environment.CurrentDirectory.Length);
			}
			text = text.Replace(global::System.IO.Path.DirectorySeparatorChar, '/');
			while (text.StartsWith("/"))
			{
				text = text.Substring(1);
			}
			header.LinkName = string.Empty;
			header.Name = text;
#if !UNITY_WEBPLAYER
			if (global::System.IO.Directory.Exists(file))
			{
				header.Mode = 1003;
				header.TypeFlag = 53;
				if (header.Name.Length == 0 || header.Name[header.Name.Length - 1] != '/')
				{
					header.Name += "/";
				}
				header.Size = 0L;
			}
			else
#endif
			{
				header.Mode = 33216;
				header.TypeFlag = 48;
#if !UNITY_WEBPLAYER
				header.Size = new global::System.IO.FileInfo(file.Replace('/', global::System.IO.Path.DirectorySeparatorChar)).Length;
#else
				header.Size = 0L;
#endif
			}
#if !UNITY_WEBPLAYER
			header.ModTime = global::System.IO.File.GetLastWriteTime(file.Replace('/', global::System.IO.Path.DirectorySeparatorChar)).ToUniversalTime();
#else
			header.ModTime = global::System.DateTime.UtcNow;
#endif
			header.DevMajor = 0;
			header.DevMinor = 0;
		}

		public global::ICSharpCode.SharpZipLib.Tar.TarEntry[] GetDirectoryEntries()
		{
#if !UNITY_WEBPLAYER
			if (file == null || !global::System.IO.Directory.Exists(file))
			{
				return new global::ICSharpCode.SharpZipLib.Tar.TarEntry[0];
			}
			string[] fileSystemEntries = global::System.IO.Directory.GetFileSystemEntries(file);
			global::ICSharpCode.SharpZipLib.Tar.TarEntry[] array = new global::ICSharpCode.SharpZipLib.Tar.TarEntry[fileSystemEntries.Length];
			for (int i = 0; i < fileSystemEntries.Length; i++)
			{
				array[i] = CreateEntryFromFile(fileSystemEntries[i]);
			}
			return array;
#else
			return new global::ICSharpCode.SharpZipLib.Tar.TarEntry[0];
#endif
		}

		public void WriteEntryHeader(byte[] outBuffer)
		{
			header.WriteHeader(outBuffer);
		}

		public static void AdjustEntryName(byte[] buffer, string newName)
		{
			global::ICSharpCode.SharpZipLib.Tar.TarHeader.GetNameBytes(newName, buffer, 0, 100);
		}

		public static void NameTarHeader(global::ICSharpCode.SharpZipLib.Tar.TarHeader header, string name)
		{
			if (header == null)
			{
				throw new global::System.ArgumentNullException("header");
			}
			if (name == null)
			{
				throw new global::System.ArgumentNullException("name");
			}
			bool flag = name.EndsWith("/");
			header.Name = name;
			header.Mode = (flag ? 1003 : 33216);
			header.UserId = 0;
			header.GroupId = 0;
			header.Size = 0L;
			header.ModTime = global::System.DateTime.UtcNow;
			header.TypeFlag = (byte)(flag ? 53 : 48);
			header.LinkName = string.Empty;
			header.UserName = string.Empty;
			header.GroupName = string.Empty;
			header.DevMajor = 0;
			header.DevMinor = 0;
		}
	}
}
