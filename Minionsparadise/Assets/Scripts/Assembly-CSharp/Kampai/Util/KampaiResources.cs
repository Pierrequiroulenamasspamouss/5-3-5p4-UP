namespace Kampai.Util
{
	public static class KampaiResources
	{
		private sealed class AssetsCache
		{
			private readonly global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<global::System.Type, global::UnityEngine.Object>> cache = new global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<global::System.Type, global::UnityEngine.Object>>(4096);

			public global::UnityEngine.Object Get(string name, global::System.Type type)
			{
				global::System.Collections.Generic.Dictionary<global::System.Type, global::UnityEngine.Object> value;
				if (name == null || !cache.TryGetValue(name, out value))
				{
					return null;
				}
				global::UnityEngine.Object value2;
				return (!value.TryGetValue(type, out value2)) ? null : value2;
			}

			public void Clear()
			{
				foreach (global::System.Collections.Generic.KeyValuePair<string, global::System.Collections.Generic.Dictionary<global::System.Type, global::UnityEngine.Object>> item in cache)
				{
					foreach (global::System.Collections.Generic.KeyValuePair<global::System.Type, global::UnityEngine.Object> item2 in item.Value)
					{
						if (!(item2.Value is global::UnityEngine.GameObject))
						{
							global::UnityEngine.Resources.UnloadAsset(item2.Value);
						}
					}
				}
				cache.Clear();
				global::System.GC.Collect();
				global::System.GC.WaitForPendingFinalizers();
			}

			public void Add(string name, global::UnityEngine.Object obj, global::System.Type type)
			{
				global::System.Collections.Generic.Dictionary<global::System.Type, global::UnityEngine.Object> value;
				if (!cache.TryGetValue(name, out value))
				{
					value = new global::System.Collections.Generic.Dictionary<global::System.Type, global::UnityEngine.Object>(1);
					cache.Add(name, value);
				}
				value.Add(type, obj);
			}
		}

		private static global::Kampai.Common.IManifestService manifestService;

		private static global::Kampai.Main.IAssetBundlesService assetBundlesService;

		private static global::Kampai.Main.ILocalContentService localContentService;

		private static global::Kampai.Util.IKampaiLogger logger;

		private static readonly global::Kampai.Util.KampaiResources.AssetsCache cachedObjects = new global::Kampai.Util.KampaiResources.AssetsCache();

#if UNITY_EDITOR
		private static global::System.Collections.Generic.Dictionary<string, string> editorAssetPathMap;

		private static void InitializeEditorAssetMap()
		{
			if (editorAssetPathMap != null) return;
			editorAssetPathMap = new global::System.Collections.Generic.Dictionary<string, string>(global::System.StringComparer.OrdinalIgnoreCase);
			string contentPath = global::System.IO.Path.Combine(global::UnityEngine.Application.dataPath, "content");
			if (!global::System.IO.Directory.Exists(contentPath)) return;

			string[] files = global::System.IO.Directory.GetFiles(contentPath, "*.*", global::System.IO.SearchOption.AllDirectories);
			foreach (string file in files)
			{
				if (file.EndsWith(".meta")) continue;
				string fileName = global::System.IO.Path.GetFileNameWithoutExtension(file);
				// Standard names or GUID names from manifest
				if (!editorAssetPathMap.ContainsKey(fileName))
				{
					string relativePath = "Assets" + file.Substring(global::UnityEngine.Application.dataPath.Length).Replace('\\', '/');
					editorAssetPathMap.Add(fileName, relativePath);
				}
			}
		}
#endif

		public static void SetManifestService(global::Kampai.Common.IManifestService service)
		{
			manifestService = service;
		}

		public static void SetAssetBundlesService(global::Kampai.Main.IAssetBundlesService service)
		{
			assetBundlesService = service;
		}

		public static void SetLocalContentService(global::Kampai.Main.ILocalContentService service)
		{
			localContentService = service;
		}

		public static void SetLogger()
		{
			logger = global::Elevation.Logging.LogManager.GetClassLogger("KampaiResources") as global::Kampai.Util.IKampaiLogger;
		}

		public static void ClearCache()
		{
			cachedObjects.Clear();
		}

		public static bool FileExists(string path)
		{
			return manifestService.GetAssetLocation(path).Length > 0 || localContentService.IsLocalAsset(path);
		}

		public static T Load<T>(string path) where T : class
		{
			object obj = Load(path, typeof(T));
			return obj as T;
		}

		public static global::UnityEngine.Object Load(string path)
		{
			return Load(path, typeof(global::UnityEngine.Object));
		}

		public static global::UnityEngine.AsyncOperation LoadAsync(string path, global::Kampai.Util.IRoutineRunner routineRunner, global::System.Action<global::UnityEngine.Object> onComplete = null)
		{
			return LoadAsync(path, typeof(global::UnityEngine.Object), routineRunner, onComplete);
		}

		public static bool FileDownloaded(string path, global::Kampai.Splash.DLCModel dlcModel)
		{
			string assetLocation = manifestService.GetAssetLocation(path);
			if (assetLocation.Length == 0)
			{
				return localContentService.IsLocalAsset(path);
			}
			int bundleTier = manifestService.GetBundleTier(assetLocation);
			return dlcModel.HighestTierDownloaded >= bundleTier;
		}

		public static global::System.Collections.IEnumerator LoadAsyncWait(global::UnityEngine.AsyncOperation request, global::System.Action<global::UnityEngine.Object> onComplete, string name, global::System.Type type)
		{
			if (request == null)
			{
				yield break;
			}
			yield return request;
			global::UnityEngine.Object obj = null;
			global::UnityEngine.ResourceRequest resourceRequest = request as global::UnityEngine.ResourceRequest;
			if (resourceRequest != null)
			{
				obj = resourceRequest.asset;
			}
			else
			{
				global::UnityEngine.AssetBundleRequest assetRequest = request as global::UnityEngine.AssetBundleRequest;
				if (assetRequest != null)
				{
					obj = assetRequest.asset;
				}
			}
			if (obj != null)
			{
				cachedObjects.Add(name, obj, type);
				if (onComplete != null)
				{
					onComplete(obj);
				}
			}
		}

		public static global::UnityEngine.AsyncOperation LoadAsync(string path, global::System.Type type, global::Kampai.Util.IRoutineRunner routineRunner, global::System.Action<global::UnityEngine.Object> onComplete = null)
		{
			global::UnityEngine.Object obj = cachedObjects.Get(path, type);
			if (obj != null)
			{
				if (onComplete != null)
				{
					onComplete(obj);
				}
				return null;
			}
			global::UnityEngine.AsyncOperation asyncOperation = null;

#if UNITY_EDITOR
			InitializeEditorAssetMap();
			string fileName = global::System.IO.Path.GetFileNameWithoutExtension(path);
			if (editorAssetPathMap.ContainsKey(fileName))
			{
				string assetPath = editorAssetPathMap[fileName];
				global::UnityEngine.Object editorObj = global::UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, type);
				if (editorObj != null)
				{
					cachedObjects.Add(path, editorObj, type);
					if (onComplete != null) onComplete(editorObj);
					return null;
				}
			}
#endif

			string assetLocation = manifestService.GetAssetLocation(path);
			if (assetLocation.Length == 0)
			{
				global::UnityEngine.Object obj2 = Load(path, type);
				if (onComplete != null)
				{
					onComplete(obj2);
				}
				return null;
			}
			global::UnityEngine.AssetBundle assetBundle;
			if (assetBundlesService.IsSharedBundle(assetLocation))
			{
				assetBundle = assetBundlesService.GetSharedBundle(assetLocation);
				if (assetBundle == null)
				{
					logger.Debug(string.Format("assetBundlesService.GetSharedBundle({0}) returned a null bundle", assetLocation));
				}
			}
			else
			{
				assetBundle = assetBundlesService.GetDLCBundle(assetLocation);
				if (assetBundle == null)
				{
					logger.Debug(string.Format("assetBundlesService.GetDLCBundle({0}) returned a null bundle", assetLocation));
				}
			}
			if (assetBundle != null)
			{
				asyncOperation = assetBundle.LoadAssetAsync(path, type);
			}
			if (asyncOperation != null)
			{
				routineRunner.StartCoroutine(LoadAsyncWait(asyncOperation, onComplete, path, type));
			}
			else
			{
				logger.Error("KampaiResources.Load is returning NULL for object " + path);
			}
			return asyncOperation;
		}

		public static bool IsAssetTierGated(string asset)
		{
			return manifestService.IsAssetTierTooHigh(asset);
		}

		public static global::UnityEngine.Object Load(string path, global::System.Type type)
		{
			global::UnityEngine.Object obj = cachedObjects.Get(path, type);
			if (null != obj)
			{
				return obj;
			}
			global::Kampai.Util.TimeProfiler.StartAssetLoadSection(path);
			global::UnityEngine.Object obj2 = null;

#if UNITY_EDITOR
			InitializeEditorAssetMap();
			string fileName = global::System.IO.Path.GetFileNameWithoutExtension(path);
			if (editorAssetPathMap.ContainsKey(fileName))
			{
				string assetPath = editorAssetPathMap[fileName];
				obj2 = global::UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, type);
				if (obj2 != null)
				{
					cachedObjects.Add(path, obj2, type);
					global::Kampai.Util.TimeProfiler.EndAssetLoadSection();
					return obj2;
				}
			}
#endif

			string assetLocation = manifestService.GetAssetLocation(path);
			bool flag = assetLocation.Length > 0 && manifestService.IsBundleTierTooHigh(assetLocation);
			if (assetLocation.Length == 0)
			{
				if (localContentService.IsLocalAsset(path))
				{
					string assetPath = localContentService.GetAssetPath(path);
					obj2 = global::UnityEngine.Resources.Load(assetPath, type);
					if (obj2 == null)
					{
						logger.Error(string.Format("Resources.Load( {0}, {1}) returned a null value", assetPath, type.ToString()));
					}
				}
				else
				{
					logger.Error(string.Format("Unable to find bundle for '{0}'. This should only be an issue if you see it on device.", path));
				}
			}
			else if (!flag)
			{
				global::UnityEngine.AssetBundle assetBundle;
				if (assetBundlesService.IsSharedBundle(assetLocation))
				{
					assetBundle = assetBundlesService.GetSharedBundle(assetLocation);
					if (assetBundle == null)
					{
						logger.Error(string.Format("assetBundlesService.GetSharedBundle({0}) returned a null bundle", assetLocation));
					}
				}
				else
				{
					assetBundle = assetBundlesService.GetDLCBundle(assetLocation);
					if (assetBundle == null)
					{
						logger.Error(string.Format("assetBundlesService.GetDLCBundle({0}) returned a null bundle", assetLocation));
					}
				}
				if (null != assetBundle)
				{
					obj2 = assetBundle.LoadAsset(path, type);
				}
			}
			if (null != obj2)
			{
				cachedObjects.Add(path, obj2, type);
			}
			else if (!flag)
			{
				logger.Error("KampaiResources.Load is returning NULL for object '{0}'", path);
			}
			else
			{
				logger.Info("Asset '{0}' is not available for the current tier", path);
			}
			global::Kampai.Util.TimeProfiler.EndAssetLoadSection();
			return obj2;
		}
	}
}
