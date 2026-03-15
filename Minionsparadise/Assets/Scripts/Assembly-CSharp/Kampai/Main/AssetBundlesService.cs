namespace Kampai.Main
{
	public class AssetBundlesService : global::Kampai.Main.IAssetBundlesService
	{
		public const string BUNDLE_DEPENDENCY_MANIFEST_NAME = "Raw_Bundle_Dependency_Manifest";

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("AssetBundlesService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AssetBundle> loadedSharedBundles = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AssetBundle>();

		private global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AssetBundle> loadedDLCBundles = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AssetBundle>();

		private global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.List<string>> dependencyManifest;

		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		private void loadDependenciesManifest()
		{
			string bundleByOriginalName = manifestService.GetBundleByOriginalName("Raw_Bundle_Dependency_Manifest");
			if (string.IsNullOrEmpty(bundleByOriginalName))
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DLC_DEPENDENCY_MANIFEST_ERROR, 1);
			}
			string bundlePath = GetBundlePath(bundleByOriginalName);
			try
			{
#if !UNITY_WEBPLAYER
				using (global::System.IO.FileStream stream = global::System.IO.File.OpenRead(bundlePath))
#else
				using (global::System.IO.Stream stream = null)
#endif
				{
					using (global::System.IO.StreamReader reader = new global::System.IO.StreamReader(stream))
					{
						using (global::Newtonsoft.Json.JsonTextReader reader2 = new global::Newtonsoft.Json.JsonTextReader(reader))
						{
							global::Newtonsoft.Json.JsonSerializer jsonSerializer = global::Newtonsoft.Json.JsonSerializer.Create(null);
							dependencyManifest = jsonSerializer.Deserialize<global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.List<string>>>(reader2);
						}
					}
				}
			}
			catch (global::System.Exception ex)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DLC_DEPENDENCY_MANIFEST_ERROR, 2, "Failed to load '{0}': {1}'", bundleByOriginalName, ex.ToString());
			}
			if (dependencyManifest == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DLC_DEPENDENCY_MANIFEST_ERROR, 3);
			}
		}

		private string GetBundlePath(string bundleName)
		{
			string bundleLocation = manifestService.GetBundleLocation(bundleName);
			if (bundleLocation.Length == 0)
			{
				logger.Error("Unable to find bundle: {0}", bundleName);
			}
			return global::System.IO.Path.Combine(bundleLocation, bundleName + ".unity3d");
		}

		public bool IsSharedBundle(string bundleName)
		{
			return loadedSharedBundles.ContainsKey(bundleName);
		}

		public void LoadSharedBundle(string bundleName)
		{
			LoadAssetBundleWithDependencies(bundleName, true);
		}

		public global::UnityEngine.AssetBundle GetSharedBundle(string bundleName)
		{
			return loadedSharedBundles[bundleName];
		}

		private global::UnityEngine.AssetBundle LoadAssetBundleWithDependencies(string bundleName, bool isShared)
		{
			if (dependencyManifest == null)
			{
				loadDependenciesManifest();
			}
			global::UnityEngine.AssetBundle loadedBundle = GetLoadedBundle(bundleName);
			if (loadedBundle != null)
			{
				return loadedBundle;
			}
			string bundleOriginalName = manifestService.GetBundleOriginalName(bundleName);
			global::System.Collections.Generic.List<string> value;
			if (dependencyManifest.TryGetValue(bundleOriginalName, out value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					string bundleByOriginalName = manifestService.GetBundleByOriginalName(value[i]);
					global::UnityEngine.AssetBundle loadedBundle2 = GetLoadedBundle(bundleByOriginalName);
					if (loadedBundle2 == null)
					{
						LoadAssetBundle(bundleByOriginalName, true);
					}
				}
			}
			return LoadAssetBundle(bundleName, isShared);
		}

		private global::UnityEngine.AssetBundle GetLoadedBundle(string bundleName)
		{
			global::UnityEngine.AssetBundle value;
			if (loadedSharedBundles.TryGetValue(bundleName, out value))
			{
				return value;
			}
			if (loadedDLCBundles.TryGetValue(bundleName, out value))
			{
				return value;
			}
			return null;
		}

		private global::UnityEngine.AssetBundle LoadAssetBundle(string bundleName, bool isDependency)
		{
			logger.Info("Loading bundle: '{0}'", manifestService.GetBundleOriginalName(bundleName));
			string bundlePath = GetBundlePath(bundleName);
#if !UNITY_WEBPLAYER
			if (!global::System.IO.File.Exists(bundlePath))
#else
			if (false)
#endif
			{
				logger.Error("Content bundle '{0}' was not found. ('{1}')", manifestService.GetBundleOriginalName(bundleName), bundlePath);
				return null;
			}
			global::UnityEngine.AssetBundle assetBundle = global::UnityEngine.AssetBundle.LoadFromFile(bundlePath);
			if (null == assetBundle)
			{
				logger.Error("Failed to load bundle '{0}'.", manifestService.GetBundleOriginalName(bundleName));
				return null;
			}
			if (isDependency)
			{
				loadedSharedBundles.Add(bundleName, assetBundle);
			}
			else
			{
				loadedDLCBundles.Add(bundleName, assetBundle);
			}
			return assetBundle;
		}

		public global::UnityEngine.AssetBundle GetDLCBundle(string bundleName)
		{
			return LoadAssetBundleWithDependencies(bundleName, false);
		}

		public void UnloadSharedBundles()
		{
			global::Kampai.Util.TimeProfiler.StartSection("unloading shared bundles");
			foreach (global::UnityEngine.AssetBundle value in loadedSharedBundles.Values)
			{
				value.Unload(false);
			}
			loadedSharedBundles.Clear();
			global::Kampai.Util.TimeProfiler.EndSection("unloading shared bundles");
		}

		public void UnloadDLCBundles()
		{
			global::Kampai.Util.TimeProfiler.StartSection("unload dlc bundles");
			foreach (global::UnityEngine.AssetBundle value in loadedDLCBundles.Values)
			{
				value.Unload(false);
			}
			loadedDLCBundles.Clear();
			global::Kampai.Util.TimeProfiler.EndSection("unload dlc bundles");
		}
	}
}
