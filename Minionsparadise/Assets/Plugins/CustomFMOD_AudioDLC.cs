#if UNITY_EDITOR
public static class CustomFMOD_AudioDLC
{
	private sealed class AudioBundleFiles
	{
		public string BankFile { get; set; }

		public string MapFile { get; set; }

		public bool Shared { get; set; }
	}

	private static readonly string BANK_FILE_LOCATION = "StreamingAssets/";

	private static readonly string SHARED_CONTENT_LOCATION = "Content/Shared/";

	private static readonly string DLC_CONTENT_LOCATION = "Content/DLC/";

	private static readonly string ASSET_BUNDLE_BYTES_EXTENSION = ".bytes";

	private static readonly string SHARED_ASSET_BUNDLE_DIRECTORY_NAME = "Shared_Audio_{0}";

	private static readonly string DLC_AUDIO_DIRECTORY_NAME = "Audio";

	public static void MoveBanksToDLCContentDirectory()
	{
		string text = global::UnityEngine.Application.dataPath + global::System.IO.Path.DirectorySeparatorChar + BANK_FILE_LOCATION;
		if (!global::System.IO.Directory.Exists(text))
		{
			global::UnityEngine.Debug.LogError("Bank file directory not found.");
			return;
		}
		global::System.Collections.Generic.HashSet<string> existingDLCBundleNames = GetExistingDLCBundleNames();
		global::System.Collections.Generic.Dictionary<string, CustomFMOD_AudioDLC.AudioBundleFiles> audioBundleFiles = GetAudioBundleFiles(text, existingDLCBundleNames);
		CopyAudioBundleFilesToContentDirectory(text, audioBundleFiles);
		RemoveUnusedSharedAudioDirectories(audioBundleFiles);
	}

	private static global::System.Collections.Generic.HashSet<string> GetExistingDLCBundleNames()
	{
		string path = global::UnityEngine.Application.dataPath + global::System.IO.Path.DirectorySeparatorChar + DLC_CONTENT_LOCATION;
		return global::System.Linq.Enumerable.Select(global::System.IO.Directory.GetDirectories(path), (string path2) => global::System.IO.Path.GetFileName(path2)).ToHashSet();
	}

	private static global::System.Collections.Generic.Dictionary<string, CustomFMOD_AudioDLC.AudioBundleFiles> GetAudioBundleFiles(string bankFileDirectory, global::System.Collections.Generic.HashSet<string> dlcBundleNames)
	{
		global::System.Collections.Generic.Dictionary<string, CustomFMOD_AudioDLC.AudioBundleFiles> dictionary = new global::System.Collections.Generic.Dictionary<string, CustomFMOD_AudioDLC.AudioBundleFiles>();
		string[] files = global::System.IO.Directory.GetFiles(bankFileDirectory);
		foreach (string path in files)
		{
			string empty = string.Empty;
			switch (global::System.IO.Path.GetExtension(path))
			{
			case ".bank":
				empty = global::System.IO.Path.GetFileNameWithoutExtension(path);
				if (!dictionary.ContainsKey(empty))
				{
					dictionary.Add(empty, new CustomFMOD_AudioDLC.AudioBundleFiles());
				}
				dictionary[empty].BankFile = global::System.IO.Path.GetFileName(path);
				dictionary[empty].Shared = !dlcBundleNames.Contains(empty);
				break;
			case ".json":
				empty = global::System.IO.Path.GetFileNameWithoutExtension(path);
				empty = empty.Replace("_map", string.Empty);
				if (!dictionary.ContainsKey(empty))
				{
					dictionary.Add(empty, new CustomFMOD_AudioDLC.AudioBundleFiles());
				}
				dictionary[empty].MapFile = global::System.IO.Path.GetFileName(path);
				dictionary[empty].Shared = !dlcBundleNames.Contains(empty);
				break;
			}
		}
		return dictionary;
	}

	private static void CopyAudioBundleFilesToContentDirectory(string bankFileDirectory, global::System.Collections.Generic.Dictionary<string, CustomFMOD_AudioDLC.AudioBundleFiles> audioBundleFiles)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		foreach (global::System.Collections.Generic.KeyValuePair<string, CustomFMOD_AudioDLC.AudioBundleFiles> audioBundleFile in audioBundleFiles)
		{
			empty = global::UnityEngine.Application.dataPath + global::System.IO.Path.DirectorySeparatorChar;
			if (audioBundleFile.Value.Shared)
			{
				empty += SHARED_CONTENT_LOCATION;
				empty2 = string.Format(SHARED_ASSET_BUNDLE_DIRECTORY_NAME, audioBundleFile.Key);
			}
			else
			{
				empty += DLC_CONTENT_LOCATION;
				empty2 = audioBundleFile.Key + global::System.IO.Path.DirectorySeparatorChar + DLC_AUDIO_DIRECTORY_NAME;
			}
			string text = empty + empty2 + global::System.IO.Path.DirectorySeparatorChar;
			if (!global::System.IO.Directory.Exists(text))
			{
				global::System.IO.Directory.CreateDirectory(text);
			}
			string originPath = bankFileDirectory + audioBundleFile.Value.BankFile;
			string destinationPath = text + audioBundleFile.Value.BankFile + ASSET_BUNDLE_BYTES_EXTENSION;
			MoveFile(originPath, destinationPath);
			if (!string.IsNullOrEmpty(audioBundleFile.Value.MapFile))
			{
				originPath = bankFileDirectory + audioBundleFile.Value.MapFile;
				destinationPath = text + audioBundleFile.Value.MapFile;
				MoveFile(originPath, destinationPath);
			}
		}
	}

	private static void RemoveUnusedSharedAudioDirectories(global::System.Collections.Generic.Dictionary<string, CustomFMOD_AudioDLC.AudioBundleFiles> audioBundleFiles)
	{
		string path = global::UnityEngine.Application.dataPath + global::System.IO.Path.DirectorySeparatorChar + SHARED_CONTENT_LOCATION;
		string[] directories = global::System.IO.Directory.GetDirectories(path, string.Format(SHARED_ASSET_BUNDLE_DIRECTORY_NAME, "*"));
		foreach (string path2 in directories)
		{
			string key = new global::System.IO.DirectoryInfo(path2).Name.Replace("Shared_Audio_", string.Empty);
			if (!audioBundleFiles.ContainsKey(key))
			{
				try
				{
					// Implement directory deletion by deleting files first (workaround for stripped System.IO)
					foreach (string file in global::System.IO.Directory.GetFiles(path2))
					{
						global::System.IO.File.Delete(file);
					}
					// If Directory.Delete doesn't exist, we skip deleting the empty folder itself,
					// or rely on a platform-specific method if available later.
				}
				catch (global::System.Exception e)
				{
					global::UnityEngine.Debug.LogWarning("Failed to clear directory: " + path2 + " " + e.Message);
				}
			}
		}
	}

	private static void MoveFile(string originPath, string destinationPath)
	{
		// Implement missing FileInfo.CopyTo and Delete by using System.IO.File equivalents
		global::System.IO.File.Copy(originPath, destinationPath, true);
		global::System.IO.File.Delete(originPath);
	}
}
#endif
