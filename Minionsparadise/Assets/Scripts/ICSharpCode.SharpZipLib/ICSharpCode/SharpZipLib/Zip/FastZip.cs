namespace ICSharpCode.SharpZipLib.Zip
{
	public class FastZip
	{
		public enum Overwrite
		{
			Prompt = 0,
			Never = 1,
			Always = 2
		}

		public delegate bool ConfirmOverwriteDelegate(string fileName);

		private bool continueRunning_;

		private byte[] buffer_;

		private global::ICSharpCode.SharpZipLib.Zip.ZipOutputStream outputStream_;

		private global::ICSharpCode.SharpZipLib.Zip.ZipFile zipFile_;

		private string sourceDirectory_;

		private global::ICSharpCode.SharpZipLib.Core.NameFilter fileFilter_;

		private global::ICSharpCode.SharpZipLib.Core.NameFilter directoryFilter_;

		private global::ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite overwrite_;

		private global::ICSharpCode.SharpZipLib.Zip.FastZip.ConfirmOverwriteDelegate confirmDelegate_;

		private bool restoreDateTimeOnExtract_;

		private bool restoreAttributesOnExtract_;

		private bool createEmptyDirectories_;

		private global::ICSharpCode.SharpZipLib.Zip.FastZipEvents events_;

		private global::ICSharpCode.SharpZipLib.Zip.IEntryFactory entryFactory_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory();

		private global::ICSharpCode.SharpZipLib.Core.INameTransform extractNameTransform_;

		private global::ICSharpCode.SharpZipLib.Zip.UseZip64 useZip64_ = global::ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;

		private string password_;

		public bool CreateEmptyDirectories
		{
			get
			{
				return createEmptyDirectories_;
			}
			set
			{
				createEmptyDirectories_ = value;
			}
		}

		public string Password
		{
			get
			{
				return password_;
			}
			set
			{
				password_ = value;
			}
		}

		public global::ICSharpCode.SharpZipLib.Core.INameTransform NameTransform
		{
			get
			{
				return entryFactory_.NameTransform;
			}
			set
			{
				entryFactory_.NameTransform = value;
			}
		}

		public global::ICSharpCode.SharpZipLib.Zip.IEntryFactory EntryFactory
		{
			get
			{
				return entryFactory_;
			}
			set
			{
				if (value == null)
				{
					entryFactory_ = new global::ICSharpCode.SharpZipLib.Zip.ZipEntryFactory();
				}
				else
				{
					entryFactory_ = value;
				}
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

		public bool RestoreDateTimeOnExtract
		{
			get
			{
				return restoreDateTimeOnExtract_;
			}
			set
			{
				restoreDateTimeOnExtract_ = value;
			}
		}

		public bool RestoreAttributesOnExtract
		{
			get
			{
				return restoreAttributesOnExtract_;
			}
			set
			{
				restoreAttributesOnExtract_ = value;
			}
		}

		public FastZip()
		{
		}

		public FastZip(global::ICSharpCode.SharpZipLib.Zip.FastZipEvents events)
		{
			events_ = events;
		}

		public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
		{
#if !UNITY_WEBPLAYER
			CreateZip(global::System.IO.File.Create(zipFileName), sourceDirectory, recurse, fileFilter, directoryFilter);
#endif
		}

		public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter)
		{
#if !UNITY_WEBPLAYER
			CreateZip(global::System.IO.File.Create(zipFileName), sourceDirectory, recurse, fileFilter, null);
#endif
		}

		public void CreateZip(global::System.IO.Stream outputStream, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
		{
			NameTransform = new global::ICSharpCode.SharpZipLib.Zip.ZipNameTransform(sourceDirectory);
			sourceDirectory_ = sourceDirectory;
			using (outputStream_ = new global::ICSharpCode.SharpZipLib.Zip.ZipOutputStream(outputStream))
			{
				if (password_ != null)
				{
					outputStream_.Password = password_;
				}
				outputStream_.UseZip64 = UseZip64;
				global::ICSharpCode.SharpZipLib.Core.FileSystemScanner fileSystemScanner = new global::ICSharpCode.SharpZipLib.Core.FileSystemScanner(fileFilter, directoryFilter);
				fileSystemScanner.ProcessFile = (global::ICSharpCode.SharpZipLib.Core.ProcessFileHandler)global::System.Delegate.Combine(fileSystemScanner.ProcessFile, new global::ICSharpCode.SharpZipLib.Core.ProcessFileHandler(ProcessFile));
				if (CreateEmptyDirectories)
				{
					fileSystemScanner.ProcessDirectory = (global::ICSharpCode.SharpZipLib.Core.ProcessDirectoryHandler)global::System.Delegate.Combine(fileSystemScanner.ProcessDirectory, new global::ICSharpCode.SharpZipLib.Core.ProcessDirectoryHandler(ProcessDirectory));
				}
				if (events_ != null)
				{
					if (events_.FileFailure != null)
					{
						fileSystemScanner.FileFailure = (global::ICSharpCode.SharpZipLib.Core.FileFailureHandler)global::System.Delegate.Combine(fileSystemScanner.FileFailure, events_.FileFailure);
					}
					if (events_.DirectoryFailure != null)
					{
						fileSystemScanner.DirectoryFailure = (global::ICSharpCode.SharpZipLib.Core.DirectoryFailureHandler)global::System.Delegate.Combine(fileSystemScanner.DirectoryFailure, events_.DirectoryFailure);
					}
				}
				fileSystemScanner.Scan(sourceDirectory, recurse);
			}
		}

		public void ExtractZip(string zipFileName, string targetDirectory, string fileFilter)
		{
			ExtractZip(zipFileName, targetDirectory, global::ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite.Always, null, fileFilter, null, restoreDateTimeOnExtract_);
		}

		public void ExtractZip(string zipFileName, string targetDirectory, global::ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite overwrite, global::ICSharpCode.SharpZipLib.Zip.FastZip.ConfirmOverwriteDelegate confirmDelegate, string fileFilter, string directoryFilter, bool restoreDateTime)
		{
#if !UNITY_WEBPLAYER
			global::System.IO.Stream inputStream = global::System.IO.File.Open(zipFileName, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.Read);
			ExtractZip(inputStream, targetDirectory, overwrite, confirmDelegate, fileFilter, directoryFilter, restoreDateTime, true);
#endif
		}

		public void ExtractZip(global::System.IO.Stream inputStream, string targetDirectory, global::ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite overwrite, global::ICSharpCode.SharpZipLib.Zip.FastZip.ConfirmOverwriteDelegate confirmDelegate, string fileFilter, string directoryFilter, bool restoreDateTime, bool isStreamOwner)
		{
			if (overwrite == global::ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite.Prompt && confirmDelegate == null)
			{
				throw new global::System.ArgumentNullException("confirmDelegate");
			}
			continueRunning_ = true;
			overwrite_ = overwrite;
			confirmDelegate_ = confirmDelegate;
			extractNameTransform_ = new global::ICSharpCode.SharpZipLib.Zip.WindowsNameTransform(targetDirectory);
			fileFilter_ = new global::ICSharpCode.SharpZipLib.Core.NameFilter(fileFilter);
			directoryFilter_ = new global::ICSharpCode.SharpZipLib.Core.NameFilter(directoryFilter);
			restoreDateTimeOnExtract_ = restoreDateTime;
			using (zipFile_ = new global::ICSharpCode.SharpZipLib.Zip.ZipFile(inputStream))
			{
				if (password_ != null)
				{
					zipFile_.Password = password_;
				}
				zipFile_.IsStreamOwner = isStreamOwner;
				global::System.Collections.IEnumerator enumerator = zipFile_.GetEnumerator();
				while (continueRunning_ && enumerator.MoveNext())
				{
					global::ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = (global::ICSharpCode.SharpZipLib.Zip.ZipEntry)enumerator.Current;
					if (zipEntry.IsFile)
					{
						if (directoryFilter_.IsMatch(global::System.IO.Path.GetDirectoryName(zipEntry.Name)) && fileFilter_.IsMatch(zipEntry.Name))
						{
							ExtractEntry(zipEntry);
						}
					}
					else if (zipEntry.IsDirectory && directoryFilter_.IsMatch(zipEntry.Name) && CreateEmptyDirectories)
					{
						ExtractEntry(zipEntry);
					}
				}
			}
		}

		private void ProcessDirectory(object sender, global::ICSharpCode.SharpZipLib.Core.DirectoryEventArgs e)
		{
			if (!e.HasMatchingFiles && CreateEmptyDirectories)
			{
				if (events_ != null)
				{
					events_.OnProcessDirectory(e.Name, e.HasMatchingFiles);
				}
				if (e.ContinueRunning && e.Name != sourceDirectory_)
				{
					global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry = entryFactory_.MakeDirectoryEntry(e.Name);
					outputStream_.PutNextEntry(entry);
				}
			}
		}

		private void ProcessFile(object sender, global::ICSharpCode.SharpZipLib.Core.ScanEventArgs e)
		{
			if (events_ != null && events_.ProcessFile != null)
			{
				events_.ProcessFile(sender, e);
			}
			if (!e.ContinueRunning)
			{
				return;
			}
			try
			{
#if !UNITY_WEBPLAYER
				using (global::System.IO.FileStream stream = global::System.IO.File.Open(e.Name, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.Read))
				{
					global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry = entryFactory_.MakeFileEntry(e.Name);
					outputStream_.PutNextEntry(entry);
					AddFileContents(e.Name, stream);
				}
#endif
			}
			catch (global::System.Exception e2)
			{
				if (events_ != null)
				{
					continueRunning_ = events_.OnFileFailure(e.Name, e2);
					return;
				}
				continueRunning_ = false;
				throw;
			}
		}

		private void AddFileContents(string name, global::System.IO.Stream stream)
		{
			if (stream == null)
			{
				throw new global::System.ArgumentNullException("stream");
			}
			if (buffer_ == null)
			{
				buffer_ = new byte[4096];
			}
			if (events_ != null && events_.Progress != null)
			{
				global::ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(stream, outputStream_, buffer_, events_.Progress, events_.ProgressInterval, this, name);
			}
			else
			{
				global::ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(stream, outputStream_, buffer_);
			}
			if (events_ != null)
			{
				continueRunning_ = events_.OnCompletedFile(name);
			}
		}

		private void ExtractFileEntry(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry, string targetName)
		{
			bool flag = true;
#if !UNITY_WEBPLAYER
			if (overwrite_ != global::ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite.Always && global::System.IO.File.Exists(targetName))
			{
				flag = overwrite_ == global::ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite.Prompt && confirmDelegate_ != null && confirmDelegate_(targetName);
			}
#endif
			if (!flag)
			{
				return;
			}
			if (events_ != null)
			{
				continueRunning_ = events_.OnProcessFile(entry.Name);
			}
			if (!continueRunning_)
			{
				return;
			}
			try
			{
#if !UNITY_WEBPLAYER
				using (global::System.IO.FileStream destination = global::System.IO.File.Create(targetName))
				{
					if (buffer_ == null)
					{
						buffer_ = new byte[4096];
					}
					if (events_ != null && events_.Progress != null)
					{
						global::ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(zipFile_.GetInputStream(entry), destination, buffer_, events_.Progress, events_.ProgressInterval, this, entry.Name, entry.Size);
					}
					else
					{
						global::ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(zipFile_.GetInputStream(entry), destination, buffer_);
					}
					if (events_ != null)
					{
						continueRunning_ = events_.OnCompletedFile(entry.Name);
					}
				}
#endif
				if (restoreDateTimeOnExtract_)
				{
#if !UNITY_WEBPLAYER
					global::System.IO.File.SetLastWriteTime(targetName, entry.DateTime);
#endif
				}
				if (RestoreAttributesOnExtract && entry.IsDOSEntry && entry.ExternalFileAttributes != -1)
				{
					global::System.IO.FileAttributes externalFileAttributes = (global::System.IO.FileAttributes)entry.ExternalFileAttributes;
					externalFileAttributes &= global::System.IO.FileAttributes.Archive | global::System.IO.FileAttributes.Hidden | global::System.IO.FileAttributes.Normal | global::System.IO.FileAttributes.ReadOnly;
#if !UNITY_WEBPLAYER
					global::System.IO.File.SetAttributes(targetName, externalFileAttributes);
#endif
				}
			}
			catch (global::System.Exception e)
			{
				if (events_ != null)
				{
					continueRunning_ = events_.OnFileFailure(targetName, e);
					return;
				}
				continueRunning_ = false;
				throw;
			}
		}

		private void ExtractEntry(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			bool flag = entry.IsCompressionMethodSupported();
			string text = entry.Name;
			if (flag)
			{
				if (entry.IsFile)
				{
					text = extractNameTransform_.TransformFile(text);
				}
				else if (entry.IsDirectory)
				{
					text = extractNameTransform_.TransformDirectory(text);
				}
				flag = text != null && text.Length != 0;
			}
			string path = null;
			if (flag)
			{
				path = ((!entry.IsDirectory) ? global::System.IO.Path.GetDirectoryName(global::System.IO.Path.GetFullPath(text)) : text);
			}
#if !UNITY_WEBPLAYER
			if (flag && !global::System.IO.Directory.Exists(path) && (!entry.IsDirectory || CreateEmptyDirectories))
			{
				try
				{
					global::System.IO.Directory.CreateDirectory(path);
				}
				catch (global::System.Exception e)
				{
					flag = false;
					if (events_ == null)
					{
						continueRunning_ = false;
						throw;
					}
					if (entry.IsDirectory)
					{
						continueRunning_ = events_.OnDirectoryFailure(text, e);
					}
					else
					{
						continueRunning_ = events_.OnFileFailure(text, e);
					}
				}
			}
#endif
			if (flag && entry.IsFile)
			{
				ExtractFileEntry(entry, text);
			}
		}

		private static int MakeExternalAttributes(global::System.IO.FileInfo info)
		{
#if !UNITY_WEBPLAYER
			return (int)info.Attributes;
#else
			return 0;
#endif
		}

		private static bool NameIsValid(string name)
		{
			if (name != null && name.Length > 0)
			{
				return name.IndexOfAny(global::System.IO.Path.GetInvalidPathChars()) < 0;
			}
			return false;
		}
	}
}
