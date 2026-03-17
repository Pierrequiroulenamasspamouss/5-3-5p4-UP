namespace ICSharpCode.SharpZipLib.Core
{
	public class FileSystemScanner
	{
		public global::ICSharpCode.SharpZipLib.Core.ProcessDirectoryHandler ProcessDirectory;

		public global::ICSharpCode.SharpZipLib.Core.ProcessFileHandler ProcessFile;

		public global::ICSharpCode.SharpZipLib.Core.CompletedFileHandler CompletedFile;

		public global::ICSharpCode.SharpZipLib.Core.DirectoryFailureHandler DirectoryFailure;

		public global::ICSharpCode.SharpZipLib.Core.FileFailureHandler FileFailure;

		private global::ICSharpCode.SharpZipLib.Core.IScanFilter fileFilter_;

		private global::ICSharpCode.SharpZipLib.Core.IScanFilter directoryFilter_;

		private bool alive_;

		public FileSystemScanner(string filter)
		{
			fileFilter_ = new global::ICSharpCode.SharpZipLib.Core.PathFilter(filter);
		}

		public FileSystemScanner(string fileFilter, string directoryFilter)
		{
			fileFilter_ = new global::ICSharpCode.SharpZipLib.Core.PathFilter(fileFilter);
			directoryFilter_ = new global::ICSharpCode.SharpZipLib.Core.PathFilter(directoryFilter);
		}

		public FileSystemScanner(global::ICSharpCode.SharpZipLib.Core.IScanFilter fileFilter)
		{
			fileFilter_ = fileFilter;
		}

		public FileSystemScanner(global::ICSharpCode.SharpZipLib.Core.IScanFilter fileFilter, global::ICSharpCode.SharpZipLib.Core.IScanFilter directoryFilter)
		{
			fileFilter_ = fileFilter;
			directoryFilter_ = directoryFilter;
		}

		private bool OnDirectoryFailure(string directory, global::System.Exception e)
		{
			global::ICSharpCode.SharpZipLib.Core.DirectoryFailureHandler directoryFailure = DirectoryFailure;
			bool flag = directoryFailure != null;
			if (flag)
			{
				global::ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs e2 = new global::ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs(directory, e);
				directoryFailure(this, e2);
				alive_ = e2.ContinueRunning;
			}
			return flag;
		}

		private bool OnFileFailure(string file, global::System.Exception e)
		{
			global::ICSharpCode.SharpZipLib.Core.FileFailureHandler fileFailure = FileFailure;
			bool flag = fileFailure != null;
			if (flag)
			{
				global::ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs e2 = new global::ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs(file, e);
				FileFailure(this, e2);
				alive_ = e2.ContinueRunning;
			}
			return flag;
		}

		private void OnProcessFile(string file)
		{
			global::ICSharpCode.SharpZipLib.Core.ProcessFileHandler processFile = ProcessFile;
			if (processFile != null)
			{
				global::ICSharpCode.SharpZipLib.Core.ScanEventArgs e = new global::ICSharpCode.SharpZipLib.Core.ScanEventArgs(file);
				processFile(this, e);
				alive_ = e.ContinueRunning;
			}
		}

		private void OnCompleteFile(string file)
		{
			global::ICSharpCode.SharpZipLib.Core.CompletedFileHandler completedFile = CompletedFile;
			if (completedFile != null)
			{
				global::ICSharpCode.SharpZipLib.Core.ScanEventArgs e = new global::ICSharpCode.SharpZipLib.Core.ScanEventArgs(file);
				completedFile(this, e);
				alive_ = e.ContinueRunning;
			}
		}

		private void OnProcessDirectory(string directory, bool hasMatchingFiles)
		{
			global::ICSharpCode.SharpZipLib.Core.ProcessDirectoryHandler processDirectory = ProcessDirectory;
			if (processDirectory != null)
			{
				global::ICSharpCode.SharpZipLib.Core.DirectoryEventArgs e = new global::ICSharpCode.SharpZipLib.Core.DirectoryEventArgs(directory, hasMatchingFiles);
				processDirectory(this, e);
				alive_ = e.ContinueRunning;
			}
		}

		public void Scan(string directory, bool recurse)
		{
			alive_ = true;
			ScanDir(directory, recurse);
		}

		private void ScanDir(string directory, bool recurse)
		{
			try
			{
				string[] files = global::System.IO.Directory.GetFiles(directory);
				bool flag = false;
				for (int i = 0; i < files.Length; i++)
				{
					if (!fileFilter_.IsMatch(files[i]))
					{
						files[i] = null;
					}
					else
					{
						flag = true;
					}
				}
				OnProcessDirectory(directory, flag);
				if (alive_ && flag)
				{
					string[] array = files;
					foreach (string text in array)
					{
						try
						{
							if (text != null)
							{
								OnProcessFile(text);
								if (!alive_)
								{
									break;
								}
							}
						}
						catch (global::System.Exception e)
						{
							if (!OnFileFailure(text, e))
							{
								throw;
							}
						}
					}
				}
			}
			catch (global::System.Exception e2)
			{
				if (!OnDirectoryFailure(directory, e2))
				{
					throw;
				}
			}
			if (!alive_ || !recurse)
			{
				return;
			}
			try
			{
				string[] directories = global::System.IO.Directory.GetDirectories(directory);
				string[] array2 = directories;
				foreach (string text2 in array2)
				{
					if (directoryFilter_ == null || directoryFilter_.IsMatch(text2))
					{
						ScanDir(text2, true);
						if (!alive_)
						{
							break;
						}
					}
				}
			}
			catch (global::System.Exception e3)
			{
				if (!OnDirectoryFailure(directory, e3))
				{
					throw;
				}
			}
		}
	}
}
