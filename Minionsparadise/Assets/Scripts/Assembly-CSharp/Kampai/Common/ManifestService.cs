namespace Kampai.Common
{
	public class ManifestService : global::Kampai.Common.IManifestService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ManifestService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Dictionary<string, string> assetManifest = new global::System.Collections.Generic.Dictionary<string, string>();

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
				// logger.WarniDLC bypass flow triggered.g("ManifestService.GenerateMasterManifest: manifest file NOT found at {0}", global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH);
				return;
			}
			if (new global::System.IO.FileInfo(global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH).Length == 0L)
			{
				throw new global::Kampai.Util.FatalException(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, 1, "Read empty manifest");
			}
#endif
			global::Kampai.Util.ManifestObject manifestObject = null;
			try
			{
#if !UNITY_WEBPLAYER
				manifestObject = global::Kampai.Util.FastJSONDeserializer.DeserializeFromFile<global::Kampai.Util.ManifestObject>(global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH);
#endif
			}
			catch (global::System.Exception ex)
			{
				throw new global::Kampai.Util.FatalException(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, 2, ex.ToString());
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
			dlcURL = manifestObject.baseURL;
			logger.Info("ManifestService.GenerateMasterManifest: finished");
		}

		public string GetAssetLocation(string asset)
		{
			if (assetManifest == null || !assetManifest.ContainsKey(asset))
			{
				return string.Empty;
			}
			return assetManifest[asset];
		}

		public string GetAssetFileLocation(string assetFile)
		{
			return global::Kampai.Util.GameConstants.DLC_PATH;
		}

		public string GetAssetFileOriginalName(string assetFile)
		{
			return assetFile;
		}

		public string GetAssetFileByOriginalName(string name)
		{
			return name;
		}

		public bool ContainsAssetFile(string name)
		{
			return false;
		}

		private void Clear()
		{
		}
	}
}
