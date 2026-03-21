namespace Kampai.Common
{
	public class ManifestService : global::Kampai.Common.IManifestService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ManifestService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Dictionary<string, string> assetManifest = new global::System.Collections.Generic.Dictionary<string, string>();

		private global::System.Collections.Generic.Dictionary<string, string> bundleManifest = new global::System.Collections.Generic.Dictionary<string, string>();

		private global::System.Collections.Generic.Dictionary<string, global::Kampai.Util.BundleInfo> bundleInfoMap = new global::System.Collections.Generic.Dictionary<string, global::Kampai.Util.BundleInfo>();

		private global::System.Collections.Generic.Dictionary<string, global::Kampai.Util.BundleInfo> originalNameToBundleInfoMap = new global::System.Collections.Generic.Dictionary<string, global::Kampai.Util.BundleInfo>();

		private global::System.Collections.Generic.List<string> sharedBundles = new global::System.Collections.Generic.List<string>();

		private global::System.Collections.Generic.List<string> shaderBundles = new global::System.Collections.Generic.List<string>();

		private global::System.Collections.Generic.List<string> audioBundles = new global::System.Collections.Generic.List<string>();

		private global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo> bundles = new global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo>();

		private global::System.Collections.Generic.HashSet<string> bundleNames = new global::System.Collections.Generic.HashSet<string>();

		private global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo> unstreamablePackagedBundles = new global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo>();

		private global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.List<string>> bundleAssetsMap = new global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.List<string>>();

		private string dlcURL;

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		public void GenerateMasterManifest()
		{
			logger.Info("ManifestService.GenerateMasterManifest starting");
			Clear();
#if !UNITY_WEBPLAYER
			if (!global::System.IO.File.Exists(global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH))
			{
				logger.Warning("ManifestService.GenerateMasterManifest: manifest file NOT found at {0}", global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH);
				return;
			}
			logger.Info("ManifestService.GenerateMasterManifest: manifest file found at {0}", global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH);
			if (new global::System.IO.FileInfo(global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH).Length == 0L)
			{
				throw new global::Kampai.Util.FatalException(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, 1, "Read empty manifest");
			}
#else
			if (false)
			{
			}
#endif
			global::Kampai.Util.ManifestObject manifestObject = null;
			try
			{
#if !UNITY_WEBPLAYER
				logger.Info("ManifestService.GenerateMasterManifest: about to deserialize manifest");
				manifestObject = global::Kampai.Util.FastJSONDeserializer.DeserializeFromFile<global::Kampai.Util.ManifestObject>(global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH);
				logger.Info("ManifestService.GenerateMasterManifest: deserialization complete");
#endif
			}
			catch (global::Newtonsoft.Json.JsonReaderException ex)
			{
				throw new global::Kampai.Util.FatalException(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, 2, ex.ToString());
			}
			catch (global::Newtonsoft.Json.JsonSerializationException ex2)
			{
				throw new global::Kampai.Util.FatalException(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, 2, ex2.ToString());
			}
			if (manifestObject == null)
			{
				throw new global::Kampai.Util.FatalException(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, 2, "Load null manifest");
			}
			assetManifest = manifestObject.assets;
			if (assetManifest == null)
			{
				throw new global::Kampai.Util.FatalException(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, 3, "Null assets in manifest");
			}
			if (manifestObject.bundles == null)
			{
				throw new global::Kampai.Util.FatalException(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, 4, "Null bundles in manifest");
			}
			foreach (global::Kampai.Util.BundleInfo bundle in manifestObject.bundles)
			{
				global::Kampai.Util.BundleInfo bundleInfo = AddBundle(bundle);
				bundleNames.Add(bundleInfo.name);
				if (bundle.shaders)
				{
					shaderBundles.Add(bundleInfo.name);
				}
				else if (bundle.audio)
				{
					audioBundles.Add(bundleInfo.name);
				}
				else if (bundle.shared)
				{
					sharedBundles.Add(bundleInfo.name);
				}
			}
			dlcURL = manifestObject.baseURL;
			if (dlcURL == null)
			{
				throw new global::Kampai.Util.FatalException(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, 5, "Null dlcURL in manifest");
			}
			logger.Info("ManifestService.GenerateMasterManifest: building bundled assets lookup");
			buildBundledAssetsLookup();
			logger.Info("ManifestService.GenerateMasterManifest: finished");
		}

		private global::Kampai.Util.BundleInfo AddBundle(global::Kampai.Util.BundleInfo bundle)
		{
			global::Kampai.Util.BundleInfo packagedAssetBundleInfo = dlcModel.GetPackagedAssetBundleInfo(bundle.originalName);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			bool flag = true;
#else
			bool flag = packagedAssetBundleInfo != null && packagedAssetBundleInfo.sum == bundle.sum && packagedAssetBundleInfo.size == bundle.size;
#endif
			bool flag2 = flag && (packagedAssetBundleInfo == null || packagedAssetBundleInfo.isStreamable);
			string value = ((!flag2) ? global::Kampai.Util.GameConstants.DLC_PATH : ((!bundle.audio) ? global::Kampai.Util.GameConstants.PRE_INSTALLED_DLC_PATH : global::Kampai.Util.GameConstants.PRE_INSTALLED_FMOD_PATH));
			global::Kampai.Util.BundleInfo bundleInfo = ((!flag || packagedAssetBundleInfo == null) ? bundle : packagedAssetBundleInfo);
			bundleManifest.Add(bundleInfo.name, value);
			bundleInfoMap.Add(bundleInfo.name, bundleInfo);
			if (flag && packagedAssetBundleInfo != null && !packagedAssetBundleInfo.isStreamable)
			{
				unstreamablePackagedBundles.Add(packagedAssetBundleInfo);
			}
			bundles.Add(bundleInfo);
			logger.Info("ManifestService added bundle: '{0}' (packaged: {1}, streamable: {2})", bundle.originalName, flag, flag2);
			return bundleInfo;
		}

		public string GetAssetLocation(string asset)
		{
			if (!assetManifest.ContainsKey(asset))
			{
				return string.Empty;
			}
			return assetManifest[asset];
		}

		public string GetBundleLocation(string bundle)
		{
			if (!bundleManifest.ContainsKey(bundle))
			{
				logger.Error("Unable to find bundle: {0}", bundle);
				return string.Empty;
			}
			return bundleManifest[bundle];
		}

		public string GetBundleOriginalName(string bundle)
		{
			if (!bundleInfoMap.ContainsKey(bundle))
			{
				logger.Error("Unable to find bundle: {0}", bundle);
				return string.Empty;
			}
			return bundleInfoMap[bundle].originalName;
		}

		public int GetBundleTier(string bundle)
		{
			if (!bundleInfoMap.ContainsKey(bundle))
			{
				logger.Error("Unable to find bundle: {0}", bundle);
				return int.MaxValue;
			}
			return bundleInfoMap[bundle].tier;
		}

		public global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo> GetBundles()
		{
			return bundles;
		}

		public string GetDLCURL()
		{
			return dlcURL;
		}

		public global::System.Collections.Generic.IList<string> GetSharedBundles()
		{
			return sharedBundles;
		}

		public global::System.Collections.Generic.IList<string> GetShaderBundles()
		{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			return new global::System.Collections.Generic.List<string>();
#else
			return shaderBundles;
#endif
		}

		public global::System.Collections.Generic.IList<string> GetAudioBundles()
		{
			return audioBundles;
		}

		public bool ContainsBundle(string name)
		{
			return bundleNames.Contains(name);
		}

		public global::System.Collections.Generic.IList<string> GetAssetsInBundle(string bundle)
		{
			if (!bundleAssetsMap.ContainsKey(bundle))
			{
				return new global::System.Collections.Generic.List<string>();
			}
			return bundleAssetsMap[bundle];
		}

		private void buildBundledAssetsLookup()
		{
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in assetManifest)
			{
				string key = item.Key;
				string value = item.Value;
				if (!bundleAssetsMap.ContainsKey(value))
				{
					bundleAssetsMap.Add(value, new global::System.Collections.Generic.List<string>());
				}
				bundleAssetsMap[value].Add(key);
			}
		}

		private void Clear()
		{
			bundleManifest.Clear();
			bundleInfoMap.Clear();
			sharedBundles.Clear();
			shaderBundles.Clear();
			audioBundles.Clear();
			bundleNames.Clear();
			originalNameToBundleInfoMap.Clear();
		}

		public string GetBundleByOriginalName(string name)
		{
			global::Kampai.Util.BundleInfo value;
			if (originalNameToBundleInfoMap.TryGetValue(name, out value))
			{
				return value.name;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Kampai.Util.BundleInfo> item in bundleInfoMap)
			{
				if (item.Value.originalName.StartsWith(name, global::System.StringComparison.Ordinal))
				{
					originalNameToBundleInfoMap.Add(name, item.Value);
					return item.Key;
				}
			}
			return string.Empty;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Util.BundleInfo> GetUnstreamablePackagedBundlesList()
		{
			return unstreamablePackagedBundles;
		}

		public bool IsStreamingBundle(string bundle)
		{
			global::Kampai.Util.BundleInfo value;
			if (bundleInfoMap.TryGetValue(bundle, out value))
			{
				return value.isStreamable;
			}
			return false;
		}

		public bool IsBundleTierTooHigh(string bundle)
		{
			global::Kampai.Util.BundleInfo value;
			return bundleInfoMap.TryGetValue(bundle, out value) && value.tier > dlcService.GetPlayerDLCTier();
		}

		public bool IsAssetTierTooHigh(string asset)
		{
			return IsBundleTierTooHigh(GetAssetLocation(asset));
		}
	}
}
