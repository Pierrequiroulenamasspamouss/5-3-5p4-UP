namespace Kampai.Common.Service.Audio
{
	public class FMODService : global::Kampai.Common.Service.Audio.IFMODService
	{
		private struct PendingBank
		{
			public global::FMOD.Studio.Bank Bank;

			public string Name;

			public override string ToString()
			{
				return Name ?? "uninitialized";
			}
		}

		private sealed class InitializeSystem_003Ec__Iterator1F : global::System.IDisposable, global::System.Collections.Generic.IEnumerator<object>, global::System.Collections.IEnumerator
		{
			internal int _0024PC;

			internal object _0024current;

			internal global::Kampai.Common.Service.Audio.FMODService _003C_003Ef__this;

			object global::System.Collections.Generic.IEnumerator<object>.Current
			{
				[global::System.Diagnostics.DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			object global::System.Collections.IEnumerator.Current
			{
				[global::System.Diagnostics.DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			public bool MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 0u:
					_003C_003Ef__this.logger.EventStart("FMODService.InitializeSystem");
					global::Kampai.Util.TimeProfiler.StartSection("fmod");
					global::Kampai.Util.TimeProfiler.StartSection("maps");
					_003C_003Ef__this.LoadEventMapsFromRawBundle();
					global::Kampai.Util.TimeProfiler.EndSection("maps");
					global::Kampai.Util.TimeProfiler.StartSection("bundles async load start");
					_003C_003Ef__this.LoadAllFromAssetBundles();
					global::Kampai.Util.TimeProfiler.EndSection("bundles async load start");
					global::Kampai.Util.TimeProfiler.StartSection("streaming");
					_0024current = _003C_003Ef__this.LoadStreamingAudioBanks();
					_0024PC = 1;
					return true;
				case 1u:
					global::Kampai.Util.TimeProfiler.EndSection("streaming");
					global::Kampai.Util.TimeProfiler.EndSection("fmod");
					_003C_003Ef__this.logger.EventStop("FMODService.InitializeSystem");
					_0024PC = -1;
					break;
				}
				return false;
			}

			[global::System.Diagnostics.DebuggerHidden]
			public void Dispose()
			{
				_0024PC = -1;
			}

			[global::System.Diagnostics.DebuggerHidden]
			public void Reset()
			{
				throw new global::System.NotSupportedException();
			}
		}

		private const string TAG = "FMODService";

		private const string SHARED_AUDIO_CONTENT_LOCATION = "Content/Shared/";

		private const string LOCAL_AUDIO_CONTENT_LOCATION = "Content/Resources/Local/";

		private const string DLC_AUDIO_CONTENT_LOCATION = "Content/DLC/";

		private const string RESOURCES_AUDIO_CONTENT_LOCATION = "Content/Resources/";

		public const string RAW_MAP_BUNDLE_NAME = "Raw_FMOD_GlobalMap";

		private readonly global::Kampai.Common.IManifestService _manifestService;

		private readonly global::System.Collections.Generic.Dictionary<string, string> _nameIdMap = new global::System.Collections.Generic.Dictionary<string, string>();

		private global::System.Collections.Generic.Queue<global::Kampai.Common.Service.Audio.FMODService.PendingBank> pendingBanks = new global::System.Collections.Generic.Queue<global::Kampai.Common.Service.Audio.FMODService.PendingBank>();

		private global::System.Diagnostics.Stopwatch allBanksAsyncSW;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("FMODService") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.ILocalContentService localContentService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		public FMODService(global::Kampai.Common.IManifestService manifestService)
		{
			_manifestService = manifestService;
		}

		[global::System.Diagnostics.DebuggerHidden]
		global::System.Collections.IEnumerator global::Kampai.Common.Service.Audio.IFMODService.InitializeSystem()
		{
			global::Kampai.Common.Service.Audio.FMODService.InitializeSystem_003Ec__Iterator1F initializeSystem_003Ec__Iterator1F = new global::Kampai.Common.Service.Audio.FMODService.InitializeSystem_003Ec__Iterator1F();
			initializeSystem_003Ec__Iterator1F._003C_003Ef__this = this;
			return initializeSystem_003Ec__Iterator1F;
		}

		string global::Kampai.Common.Service.Audio.IFMODService.GetGuid(string eventName)
		{
			if (_nameIdMap.ContainsKey(eventName))
			{
				return _nameIdMap[eventName];
			}
			logger.Error("eventName '{0}' was not found in the dictionary.", eventName);
			return null;
		}

		private void LoadAllFromAssetBundles()
		{
			logger.Debug("Starting Load of Audio Assets from Bundles");
			allBanksAsyncSW = global::System.Diagnostics.Stopwatch.StartNew();
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			logger.Debug("LoadAllFromAssetBundles: Skipping DLC audio bundle loading. Banks will be loaded from StreamingAssets/FMOD.");
			StartAsyncBankLoadingProcessing();
#else
			foreach (string audioBundle in _manifestService.GetAudioBundles())
			{
				LoadFromAssetBundleAsync(audioBundle);
			}
			StartAsyncBankLoadingProcessing();
#endif
		}

		private string GetEventMapFilePath()
		{
#if UNITY_EDITOR
			string path = global::System.IO.Path.Combine(global::UnityEngine.Application.streamingAssetsPath, "FMOD/Android/Raw_FMOD_GlobalMap.json");
			if (global::System.IO.File.Exists(path))
			{
				return path;
			}
#elif UNITY_STANDALONE_WIN
			string path = global::System.IO.Path.Combine(global::UnityEngine.Application.streamingAssetsPath, "FMOD/Windows/Raw_FMOD_GlobalMap.json");
			if (global::System.IO.File.Exists(path))
			{
				return path;
			}
#endif
			string bundleByOriginalName = _manifestService.GetBundleByOriginalName("Raw_FMOD_GlobalMap");
			return GetRawAssetPathByOriginalName(bundleByOriginalName);
		}

		private void LoadEventMapsFromRawBundle(bool logErrors = false)
		{
			global::Kampai.Common.Service.Audio.FmodGlobalEventMap fmodGlobalEventMap = null;
			try
			{
				string eventMapFilePath = GetEventMapFilePath();
				logger.Info("Loading FmodGlobalEventMap from: {0}", eventMapFilePath);
#if !UNITY_WEBPLAYER
				if (global::System.IO.File.Exists(eventMapFilePath))
				{
					using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(eventMapFilePath))
					{
						global::Newtonsoft.Json.JsonTextReader reader = new global::Newtonsoft.Json.JsonTextReader(streamReader);
						fmodGlobalEventMap = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Common.Service.Audio.FmodGlobalEventMap>(reader);
					}
				}
				else
				{
					logger.Error("FmodGlobalEventMap file not found: {0}", eventMapFilePath);
				}
#endif
			}
			catch (global::Newtonsoft.Json.JsonSerializationException ex)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.FMOD_EVENT_MAP_ERROR, "Json Parse Err: {0}", ex);
			}
			catch (global::Newtonsoft.Json.JsonReaderException ex2)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.FMOD_EVENT_MAP_ERROR, "Json Parse Err: {0}", ex2);
			}
			catch (global::System.Exception ex3)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.FMOD_EVENT_MAP_ERROR, "FmodGlobalEventMap load error: {0}", ex3);
			}
			if (fmodGlobalEventMap == null || fmodGlobalEventMap.maps == null)
			{
				logger.Error("FmodGlobalEventMap is null or has no maps. Skipping map processing.");
				return;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::System.Collections.Generic.Dictionary<string, string>> map in fmodGlobalEventMap.maps)
			{
				foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in map.Value)
				{
					if (_nameIdMap.ContainsKey(item.Key))
					{
						if (logErrors && _nameIdMap[item.Key] != item.Value)
						{
							logger.Error("LoadEventMapsFromRawBundle: key: {0}\tvalue: {1}", item.Key, item.Value);
						}
					}
					else
					{
						_nameIdMap.Add(item.Key, item.Value);
					}
				}
			}
		}

		public bool LoadFromAssetBundleAsync(string bundleName)
		{
			bool flag = _manifestService.IsStreamingBundle(bundleName);
			string bundleOriginalName = _manifestService.GetBundleOriginalName(bundleName);
			if (bundleOriginalName.StartsWith("Raw_FMOD_GlobalMap"))
			{
				return false;
			}
			logger.Verbose("Async loading audio data from bundle {0} [{1}]", bundleName, bundleOriginalName);
			string rawAssetPathByOriginalName = GetRawAssetPathByOriginalName(bundleName);
			if (!string.IsNullOrEmpty(rawAssetPathByOriginalName))
			{
				if (!global::System.IO.File.Exists(rawAssetPathByOriginalName) && !flag)
				{
					return false;
				}
				global::FMOD.Studio.Bank bank = LoadLocalBankAsync(rawAssetPathByOriginalName);
				pendingBanks.Enqueue(new global::Kampai.Common.Service.Audio.FMODService.PendingBank
				{
					Bank = bank,
					Name = rawAssetPathByOriginalName
				});
				return true;
			}
			return false;
		}

		public void StartAsyncBankLoadingProcessing()
		{
			routineRunner.StartCoroutine(ProcessAsyncBankLoading());
		}

		private global::System.Collections.IEnumerator ProcessAsyncBankLoading()
		{
			int maxIterations = 300; // ~5 minutes at 60fps before giving up
			while (maxIterations-- > 0)
			{
				int queueCount = pendingBanks.Count;
				while (queueCount-- != 0)
				{
					global::Kampai.Common.Service.Audio.FMODService.PendingBank pb = pendingBanks.Dequeue();
					if (pb.Bank == null)
					{
						logger.Error("{0}: bank was removed during async bank loading, bank {1}.", "FMODService", pb);
						continue;
					}
					global::FMOD.Studio.LOADING_STATE bankState;
					global::FMOD.RESULT r = pb.Bank.getLoadingState(out bankState);
					if (r == global::FMOD.RESULT.OK)
					{
						switch (bankState)
						{
						case global::FMOD.Studio.LOADING_STATE.LOADING:
							pendingBanks.Enqueue(pb);
							break;
						case global::FMOD.Studio.LOADING_STATE.UNLOADING:
						case global::FMOD.Studio.LOADING_STATE.UNLOADED:
							logger.Error("{0}: unexpected bank state {1} on async bank loading, bank {2}", "FMODService", bankState, pb);
							break;
						case global::FMOD.Studio.LOADING_STATE.ERROR:
							logger.Error("{0}: error bank state {1}, bank {2}", "FMODService", bankState, pb);
							pb.Bank.unload();
							break;
						}
					}
					else
					{
						logger.Error("{0}: getLoadingState error {1}, bank {2}", "FMODService", r, pb);
						pb.Bank.unload();
					}
				}
				if (pendingBanks.Count == 0)
				{
					break;
				}
				yield return null;
			}
			if (pendingBanks.Count > 0)
			{
				logger.Error("{0}: Timed out waiting for {1} audio bank(s) to load. Clearing to unblock game start.", "FMODService", pendingBanks.Count);
				while (pendingBanks.Count > 0)
				{
					var pb = pendingBanks.Dequeue();
					if (pb.Bank != null) pb.Bank.unload();
				}
			}
			logger.Debug("{0}: All banks are loaded asynchronously in : {1}", "FMODService", allBanksAsyncSW.Elapsed);
		}

		public bool BanksLoadingInProgress()
		{
			return pendingBanks.Count != 0;
		}

		private void LoadEventsMap(string json, bool logErrors = false)
		{
			global::System.Collections.Generic.Dictionary<string, string> dictionary = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::System.Collections.Generic.Dictionary<string, string>>(json);
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in dictionary)
			{
				if (_nameIdMap.ContainsKey(item.Key))
				{
					if (logErrors && _nameIdMap[item.Key] != item.Value)
					{
						logger.Error("key: {0}\tvalue: {1}", item.Key, item.Value);
					}
				}
				else
				{
					_nameIdMap.Add(item.Key, item.Value);
				}
			}
		}

		private global::System.Collections.IEnumerator LoadBanksFromFileSystem()
		{
			global::System.Collections.Generic.List<string> bankFiles = GetFiles(".bytes");
			if (bankFiles == null)
			{
				yield break;
			}
			foreach (string bankFile in bankFiles)
			{
				if (FMOD_StudioSystem.instance.IsPaused())
				{
					yield return null;
				}
				global::FMOD.Studio.Bank bank = null;
				global::FMOD.RESULT result = FMOD_StudioSystem.instance.System.loadBankFile(bankFile, global::FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out bank);
				if (result == global::FMOD.RESULT.ERR_VERSION)
				{
					global::FMOD.Studio.UnityUtil.LogError("These banks were built with an incompatible version of FMOD Studio.");
				}
				global::FMOD.Studio.UnityUtil.Log("bank load: " + ((!(bank != null)) ? "failed!!" : "succeeded"));
				yield return null;
			}
		}

		private global::FMOD.Studio.Bank LoadLocalBankAsync(string bankFile)
		{
			global::FMOD.Studio.Bank bank = null;
			global::FMOD.RESULT rESULT = FMOD_StudioSystem.instance.System.loadBankFile(bankFile, global::FMOD.Studio.LOAD_BANK_FLAGS.NONBLOCKING, out bank);
			if (rESULT != global::FMOD.RESULT.OK)
			{
				logger.Error("LoadLocalBankAsync: for async loading OK always expected but got: {0}. Bank is null: {1}", rESULT, bank == null);
			}
			else
			{
				logger.Verbose("LoadLocalBankAsync: Bank {0} scheduled for loading", bankFile);
			}
			return bank;
		}

		private string GetStreamingBankPath(string bank)
		{
#if UNITY_EDITOR
			string path = global::System.IO.Path.Combine(global::UnityEngine.Application.streamingAssetsPath, "FMOD/Android/" + bank + ".bank");
			if (global::System.IO.File.Exists(path))
			{
				return path;
			}
#endif
			return global::Kampai.Util.GameConstants.PRE_INSTALLED_FMOD_PATH + bank + ".bytes";
		}

		private global::System.Collections.IEnumerator LoadStreamingAudioBanks()
		{
			logger.Debug("Start Loading Streaming Audio Banks");
#if UNITY_EDITOR
			string fmodPath = global::System.IO.Path.Combine(global::UnityEngine.Application.streamingAssetsPath, "FMOD/Android");
#elif UNITY_STANDALONE_WIN
			string fmodPath = global::System.IO.Path.Combine(global::UnityEngine.Application.streamingAssetsPath, "FMOD/Windows");
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			if (global::System.IO.Directory.Exists(fmodPath))
			{
				string[] files = global::System.IO.Directory.GetFiles(fmodPath, "*.bank");
				foreach (string file in files)
				{
					if (FMOD_StudioSystem.instance.IsPaused())
					{
						yield return null;
					}
					LoadLocalBankAsync(file);
				}
			}
			else
			{
				logger.Error("LoadStreamingAudioBanks: FMOD directory not found at: {0}", fmodPath);
			}
#else
			global::System.Collections.Generic.List<string> streamingBanks = localContentService.GetStreamingAudioBanks();
			foreach (string bankName in streamingBanks)
			{
				if (string.IsNullOrEmpty(_manifestService.GetAssetLocation(bankName)))
				{
					if (FMOD_StudioSystem.instance.IsPaused())
					{
						yield return null;
					}
					string path = GetStreamingBankPath(bankName);
					LoadLocalBankAsync(path);
				}
			}
#endif
			yield break;
		}

		private string GetRawAssetPathByOriginalName(string bundleName)
		{
			string bundleLocation = _manifestService.GetBundleLocation(bundleName);
			string path = global::System.IO.Path.Combine(bundleLocation, bundleName + ".unity3d");
			if (global::System.IO.File.Exists(path))
			{
				return path;
			}
			return global::System.IO.Path.Combine(bundleLocation, bundleName);
		}

		private global::System.Collections.IEnumerator LoadMapsFromFileSystem()
		{
			global::System.Collections.Generic.List<string> mapFiles = GetFiles("_map.json");
			if (mapFiles == null)
			{
				yield break;
			}
			foreach (string file in mapFiles)
			{
#if !UNITY_WEBPLAYER
#if !UNITY_WEBPLAYER
				using (global::System.IO.FileStream stream = global::System.IO.File.OpenRead(file))
#else
				using (global::System.IO.Stream stream = null)
#endif
				{
					using (global::System.IO.StreamReader reader = new global::System.IO.StreamReader(stream))
					{
						string json = reader.ReadToEnd();
						LoadEventsMap(json, true);
					}
				}
#endif
				yield return null;
			}
		}

		private void GetFilesAtPath(global::System.Collections.Generic.List<string> files, string path, string pattern, string fileEnding, bool recursive)
		{
#if !UNITY_WEBPLAYER
			if (!global::System.IO.Directory.Exists(path))
			{
				global::FMOD.Studio.UnityUtil.LogError(path + " not found, no banks loaded.");
			}
			string[] directories = global::System.IO.Directory.GetDirectories(path, pattern, recursive ? global::System.IO.SearchOption.AllDirectories : global::System.IO.SearchOption.TopDirectoryOnly);
			string[] array = directories;
			foreach (string path2 in array)
			{
				global::System.Collections.Generic.IEnumerable<string> collection = global::System.Linq.Enumerable.Where(global::System.IO.Directory.GetFiles(path2, "*.*", global::System.IO.SearchOption.AllDirectories), (string file) => file.EndsWith(fileEnding));
				files.AddRange(collection);
			}
#endif
		}

		private global::System.Collections.Generic.List<string> GetFiles(string fileEnding)
		{
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			string path = global::UnityEngine.Application.dataPath + global::System.IO.Path.DirectorySeparatorChar + "Content/Shared/";
			GetFilesAtPath(list, path, "Shared_Audio_*", fileEnding, false);
			string path2 = global::UnityEngine.Application.dataPath + global::System.IO.Path.DirectorySeparatorChar + "Content/DLC/";
			GetFilesAtPath(list, path2, "Audio", fileEnding, true);
			string path3 = global::UnityEngine.Application.dataPath + global::System.IO.Path.DirectorySeparatorChar + "Content/Resources/";
			GetFilesAtPath(list, path3, "Audio", fileEnding, true);
			return list;
		}
	}
}
