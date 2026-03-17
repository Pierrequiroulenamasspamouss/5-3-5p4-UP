namespace Kampai.Splash
{
	public class DLCModel
	{
		private bool _allow;

		public global::System.Collections.Generic.IList<global::Kampai.Util.BundleInfo> NeededBundles { get; set; }

		public ulong TotalSize { get; set; }

		public global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> PendingRequests { get; set; }

		public global::System.Collections.Generic.List<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> RunningRequests { get; set; }

		public float LastNetworkFailureTime { get; set; }

		public bool AllowDownloadOnMobileNetwork
		{
			get
			{
				return _allow;
			}
			set
			{
				_allow = value;
				global::UnityEngine.PlayerPrefs.SetInt("PermitMobileData", _allow ? 1 : 0);
			}
		}

		public int HighestTierDownloaded { get; set; }

		public bool ShouldLaunchDownloadAgain { get; set; }

		public bool ShouldLoadAudio { get; set; }

		public bool NextDownloadShouldLoadAudio { get; set; }

		public global::System.Collections.Generic.Queue<string> DownloadedAudioBundles { get; set; }

		public bool UdpEnabled { get; set; }

		public long DownloadedTotalSize { get; set; }

		public global::System.DateTime DownloadStartTime { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo> PackagedAssetBundles { get; private set; }

		public DLCModel()
		{
			DownloadedAudioBundles = new global::System.Collections.Generic.Queue<string>(10);
			if (global::UnityEngine.PlayerPrefs.HasKey("PermitMobileData"))
			{
				AllowDownloadOnMobileNetwork = global::UnityEngine.PlayerPrefs.GetInt("PermitMobileData") > 0;
			}
			global::UnityEngine.TextAsset textAsset = global::UnityEngine.Resources.Load<global::UnityEngine.TextAsset>("PackagedAssetBundlesManifest");
			if (null == textAsset)
			{
				PackagedAssetBundles = new global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo>();
				return;
			}
			PackagedAssetBundles = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo>>(textAsset.text);
			global::UnityEngine.Resources.UnloadAsset(textAsset);
		}

		public global::Kampai.Util.BundleInfo GetPackagedAssetBundleInfo(string originalBundleName)
		{
			for (int i = 0; i < PackagedAssetBundles.Count; i++)
			{
				global::Kampai.Util.BundleInfo bundleInfo = PackagedAssetBundles[i];
				if (bundleInfo.originalName == originalBundleName)
				{
					return bundleInfo;
				}
			}
			return null;
		}
	}
}
