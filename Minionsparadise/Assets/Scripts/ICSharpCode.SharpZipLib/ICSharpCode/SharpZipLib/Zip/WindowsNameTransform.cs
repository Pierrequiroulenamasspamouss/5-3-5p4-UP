namespace ICSharpCode.SharpZipLib.Zip
{
	public class WindowsNameTransform : global::ICSharpCode.SharpZipLib.Core.INameTransform
	{
		private const int MaxPath = 260;

		private string _baseDirectory;

		private bool _trimIncomingPaths;

		private char _replacementChar = '_';

		private static readonly char[] InvalidEntryChars;

		public string BaseDirectory
		{
			get
			{
				return _baseDirectory;
			}
			set
			{
				if (value == null)
				{
					throw new global::System.ArgumentNullException("value");
				}
				_baseDirectory = global::System.IO.Path.GetFullPath(value);
			}
		}

		public bool TrimIncomingPaths
		{
			get
			{
				return _trimIncomingPaths;
			}
			set
			{
				_trimIncomingPaths = value;
			}
		}

		public char Replacement
		{
			get
			{
				return _replacementChar;
			}
			set
			{
				for (int i = 0; i < InvalidEntryChars.Length; i++)
				{
					if (InvalidEntryChars[i] == value)
					{
						throw new global::System.ArgumentException("invalid path character");
					}
				}
				if (value == global::System.IO.Path.DirectorySeparatorChar || value == global::System.IO.Path.AltDirectorySeparatorChar)
				{
					throw new global::System.ArgumentException("invalid replacement character");
				}
				_replacementChar = value;
			}
		}

		public WindowsNameTransform(string baseDirectory)
		{
			if (baseDirectory == null)
			{
				throw new global::System.ArgumentNullException("baseDirectory", "Directory name is invalid");
			}
			BaseDirectory = baseDirectory;
		}

		public WindowsNameTransform()
		{
		}

		public string TransformDirectory(string name)
		{
			name = TransformFile(name);
			if (name.Length > 0)
			{
				while (name.EndsWith(global::System.IO.Path.DirectorySeparatorChar.ToString()))
				{
					name = name.Remove(name.Length - 1, 1);
				}
				return name;
			}
			throw new global::ICSharpCode.SharpZipLib.Zip.ZipException("Cannot have an empty directory name");
		}

		public string TransformFile(string name)
		{
			if (name != null)
			{
				name = MakeValidName(name, _replacementChar);
				if (_trimIncomingPaths)
				{
					name = global::System.IO.Path.GetFileName(name);
				}
				if (_baseDirectory != null)
				{
					name = global::System.IO.Path.Combine(_baseDirectory, name);
				}
			}
			else
			{
				name = string.Empty;
			}
			return name;
		}

		public static bool IsValidName(string name)
		{
			return name != null && name.Length <= 260 && string.Compare(name, MakeValidName(name, '_')) == 0;
		}

		static WindowsNameTransform()
		{
			char[] invalidPathChars = global::System.IO.Path.GetInvalidPathChars();
			int num = invalidPathChars.Length + 3;
			InvalidEntryChars = new char[num];
			global::System.Array.Copy(invalidPathChars, 0, InvalidEntryChars, 0, invalidPathChars.Length);
			InvalidEntryChars[num - 1] = '*';
			InvalidEntryChars[num - 2] = '?';
			InvalidEntryChars[num - 3] = ':';
		}

		public static string MakeValidName(string name, char replacement)
		{
			if (name == null)
			{
				throw new global::System.ArgumentNullException("name");
			}
			name = global::ICSharpCode.SharpZipLib.Core.WindowsPathUtils.DropPathRoot(name.Replace("/", global::System.IO.Path.DirectorySeparatorChar.ToString()));
			while (name.Length > 0 && name[0] == global::System.IO.Path.DirectorySeparatorChar)
			{
				name = name.Remove(0, 1);
			}
			while (name.Length > 0 && name[name.Length - 1] == global::System.IO.Path.DirectorySeparatorChar)
			{
				name = name.Remove(name.Length - 1, 1);
			}
			int num;
			for (num = name.IndexOf(string.Format("{0}{0}", global::System.IO.Path.DirectorySeparatorChar)); num >= 0; num = name.IndexOf(global::System.IO.Path.DirectorySeparatorChar))
			{
				name = name.Remove(num, 1);
			}
			num = name.IndexOfAny(InvalidEntryChars);
			if (num >= 0)
			{
				global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder(name);
				while (num >= 0)
				{
					stringBuilder[num] = replacement;
					num = ((num < name.Length) ? name.IndexOfAny(InvalidEntryChars, num + 1) : (-1));
				}
				name = stringBuilder.ToString();
			}
			if (name.Length > 260)
			{
				throw new global::System.IO.PathTooLongException();
			}
			return name;
		}
	}
}
