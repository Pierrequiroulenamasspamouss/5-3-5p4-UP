namespace Elevation.Logging.Targets
{
#if !UNITY_WEBPLAYER
	public class FileTarget : global::Elevation.Logging.Targets.AsyncLoggingTarget
	{
		private global::System.IO.StreamWriter _writer;

		private readonly string _logFolder;

		private readonly string _filePrefix;

		private readonly int _maxFileSize;

		private readonly int _maxFileCount;

		protected global::System.IO.FileInfo _currentFileInfo;

		private global::System.IO.DirectoryInfo _logFolderInfo;

		private readonly global::System.Collections.Generic.HashSet<string> _previousFiles = new global::System.Collections.Generic.HashSet<string>();

		protected int CurrentFileSize
		{
			get
			{
				if (_writer != null)
				{
					return (int)_writer.BaseStream.Length;
				}
				return 0;
			}
		}

		private global::System.IO.DirectoryInfo LogFolderInfo
		{
			get
			{
				if (_logFolderInfo != null)
				{
					return _logFolderInfo;
				}
				_logFolderInfo = new global::System.IO.DirectoryInfo(_logFolder);
				if (!_logFolderInfo.Exists)
				{
					_logFolderInfo.Create();
				}
				return _logFolderInfo;
			}
		}

		private global::System.IO.FileInfo CurrentLogFile
		{
			get
			{
				if (_currentFileInfo != null)
				{
					return _currentFileInfo;
				}
				global::System.IO.FileInfo[] files = LogFolderInfo.GetFiles(string.Format("{0}*.log", _filePrefix));
				global::System.Array.Sort(files, (global::System.IO.FileInfo f1, global::System.IO.FileInfo f2) => string.Compare(f2.Name, f1.Name, global::System.StringComparison.Ordinal));
				global::System.IO.FileInfo fileInfo = null;
				for (int num = 0; num < files.Length; num++)
				{
					if (!_previousFiles.Contains(files[num].FullName))
					{
						fileInfo = files[num];
						break;
					}
				}
				if (fileInfo != null && fileInfo.Length <= _maxFileSize)
				{
					_currentFileInfo = fileInfo;
				}
				else
				{
					global::System.DateTime utcNow = global::System.DateTime.UtcNow;
					int num2 = 1;
					global::System.IO.FileInfo fileInfo2 = null;
					while (fileInfo2 == null)
					{
						string arg = string.Format("{0}/{1}_{2}-{3:00}-{4:00}T{5:00}-{6:00}-{7:00}", LogFolderInfo.FullName, _filePrefix, utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second);
						fileInfo2 = new global::System.IO.FileInfo(string.Format("{0}.log", arg));
						if (!fileInfo2.Exists)
						{
							break;
						}
						fileInfo2 = new global::System.IO.FileInfo(string.Format("{0}_{1:000}.log", arg, utcNow.Millisecond));
						if (!fileInfo2.Exists)
						{
							break;
						}
						fileInfo2 = new global::System.IO.FileInfo(string.Format("{0}_{1:000}_{2}.log", arg, utcNow.Millisecond, num2));
						if (!fileInfo2.Exists)
						{
							break;
						}
						fileInfo2 = null;
						num2++;
					}
					_currentFileInfo = fileInfo2;
					_previousFiles.Add(_currentFileInfo.FullName);
				}
				return _currentFileInfo;
			}
		}

		public FileTarget(global::Elevation.Logging.LogLevel level, string filePrefix, int maxFileSize, int maxFileCount, string logFolder = null, params global::Elevation.Logging.LogFilter[] filters)
			: base("File", level, filters)
		{
			_logFolder = ((logFolder != null) ? logFolder : string.Format("{0}/Logs", global::UnityEngine.Application.temporaryCachePath));
			_filePrefix = filePrefix;
			_maxFileSize = ((maxFileSize <= 0) ? 1048576 : maxFileSize);
			_maxFileCount = ((maxFileCount <= 0) ? 1 : maxFileCount);
		}

		public static global::Elevation.Logging.Targets.FileTarget Build(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			string logFolder = null;
			string filePrefix = "elevation";
			int maxFileSize = 5242880;
			int maxFileCount = 2;
			if (config.ContainsKey("logFolder"))
			{
				logFolder = config["logFolder"].ToString();
			}
			if (config.ContainsKey("filePrefix"))
			{
				filePrefix = config["filePrefix"].ToString();
			}
			if (config.ContainsKey("maxFileSize"))
			{
				maxFileSize = int.Parse(config["maxFileSize"].ToString());
			}
			if (config.ContainsKey("maxFileCount"))
			{
				maxFileCount = int.Parse(config["maxFileCount"].ToString());
			}
			global::Elevation.Logging.Targets.FileTarget fileTarget = new global::Elevation.Logging.Targets.FileTarget(global::Elevation.Logging.LogLevel.None, filePrefix, maxFileSize, maxFileCount, logFolder);
			fileTarget.UpdateConfig(config);
			return fileTarget;
		}

		protected override void Write(global::Elevation.Logging.LogEvent logEvent)
		{
			try
			{
				if (_writer == null)
				{
					_writer = new global::System.IO.StreamWriter(CurrentLogFile.FullName, true, global::System.Text.Encoding.UTF8, 32768)
					{
						AutoFlush = true,
						NewLine = "\n"
					};
				}
				_writer.WriteLine(FormattedLogEvent(logEvent));
				if (_writer.BaseStream.Length > _maxFileSize)
				{
					RollLogFiles();
				}
			}
			catch (global::System.Exception arg)
			{
				global::System.Console.Error.WriteLine("Unable to write to log file: {0}\n{1}", _currentFileInfo, arg);
				base.Level = global::Elevation.Logging.LogLevel.None;
			}
		}

		protected virtual void RollLogFiles()
		{
			if (_writer == null)
			{
				return;
			}
			_writer.Flush();
			_writer.Close();
			_writer = null;
			_currentFileInfo = null;
			global::System.IO.FileInfo[] files = LogFolderInfo.GetFiles(string.Format("{0}*.log", _filePrefix));
			if (files.Length >= _maxFileCount)
			{
				global::System.Array.Sort(files, (global::System.IO.FileInfo f1, global::System.IO.FileInfo f2) => string.Compare(f1.Name, f2.Name, global::System.StringComparison.Ordinal));
				for (int num = 0; num <= files.Length - _maxFileCount; num++)
				{
					_previousFiles.Remove(files[num].FullName);
					files[num].Delete();
				}
			}
		}
	}
#else
	public class FileTarget : global::Elevation.Logging.Targets.AsyncLoggingTarget
	{
		protected global::System.IO.FileInfo _currentFileInfo;

		protected int CurrentFileSize
		{
			get { return 0; }
		}

		public FileTarget(global::Elevation.Logging.LogLevel level, string filePrefix, int maxFileSize, int maxFileCount, string logFolder = null, params global::Elevation.Logging.LogFilter[] filters)
			: base("File", global::Elevation.Logging.LogLevel.None, filters)
		{
		}

		public static global::Elevation.Logging.Targets.FileTarget Build(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			return new global::Elevation.Logging.Targets.FileTarget(global::Elevation.Logging.LogLevel.None, string.Empty, 0, 0);
		}

		protected override void Write(global::Elevation.Logging.LogEvent logEvent)
		{
		}

		protected virtual void RollLogFiles()
		{
		}
	}
#endif
}
