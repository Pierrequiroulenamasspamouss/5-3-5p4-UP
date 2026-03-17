namespace ICSharpCode.SharpZipLib.Zip
{
	public class FastZipEvents
	{
		public global::ICSharpCode.SharpZipLib.Core.ProcessDirectoryHandler ProcessDirectory;

		public global::ICSharpCode.SharpZipLib.Core.ProcessFileHandler ProcessFile;

		public global::ICSharpCode.SharpZipLib.Core.ProgressHandler Progress;

		public global::ICSharpCode.SharpZipLib.Core.CompletedFileHandler CompletedFile;

		public global::ICSharpCode.SharpZipLib.Core.DirectoryFailureHandler DirectoryFailure;

		public global::ICSharpCode.SharpZipLib.Core.FileFailureHandler FileFailure;

		private global::System.TimeSpan progressInterval_ = global::System.TimeSpan.FromSeconds(3.0);

		public global::System.TimeSpan ProgressInterval
		{
			get
			{
				return progressInterval_;
			}
			set
			{
				progressInterval_ = value;
			}
		}

		public bool OnDirectoryFailure(string directory, global::System.Exception e)
		{
			bool result = false;
			global::ICSharpCode.SharpZipLib.Core.DirectoryFailureHandler directoryFailure = DirectoryFailure;
			if (directoryFailure != null)
			{
				global::ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs e2 = new global::ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs(directory, e);
				directoryFailure(this, e2);
				result = e2.ContinueRunning;
			}
			return result;
		}

		public bool OnFileFailure(string file, global::System.Exception e)
		{
			global::ICSharpCode.SharpZipLib.Core.FileFailureHandler fileFailure = FileFailure;
			bool flag = fileFailure != null;
			if (flag)
			{
				global::ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs e2 = new global::ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs(file, e);
				fileFailure(this, e2);
				flag = e2.ContinueRunning;
			}
			return flag;
		}

		public bool OnProcessFile(string file)
		{
			bool result = true;
			global::ICSharpCode.SharpZipLib.Core.ProcessFileHandler processFile = ProcessFile;
			if (processFile != null)
			{
				global::ICSharpCode.SharpZipLib.Core.ScanEventArgs e = new global::ICSharpCode.SharpZipLib.Core.ScanEventArgs(file);
				processFile(this, e);
				result = e.ContinueRunning;
			}
			return result;
		}

		public bool OnCompletedFile(string file)
		{
			bool result = true;
			global::ICSharpCode.SharpZipLib.Core.CompletedFileHandler completedFile = CompletedFile;
			if (completedFile != null)
			{
				global::ICSharpCode.SharpZipLib.Core.ScanEventArgs e = new global::ICSharpCode.SharpZipLib.Core.ScanEventArgs(file);
				completedFile(this, e);
				result = e.ContinueRunning;
			}
			return result;
		}

		public bool OnProcessDirectory(string directory, bool hasMatchingFiles)
		{
			bool result = true;
			global::ICSharpCode.SharpZipLib.Core.ProcessDirectoryHandler processDirectory = ProcessDirectory;
			if (processDirectory != null)
			{
				global::ICSharpCode.SharpZipLib.Core.DirectoryEventArgs e = new global::ICSharpCode.SharpZipLib.Core.DirectoryEventArgs(directory, hasMatchingFiles);
				processDirectory(this, e);
				result = e.ContinueRunning;
			}
			return result;
		}
	}
}
