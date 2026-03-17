namespace Kampai.Splash
{
	public class LaunchDownloadCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LaunchDownloadCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public bool shouldLoadAudio { get; set; }

		[Inject]
		public global::Kampai.Common.ReconcileDLCSignal reconcileSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationService { get; set; }

		[Inject]
		public global::Kampai.Splash.DownloadResponseSignal downloadResponseSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DownloadInitializeSignal initSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Splash.DownloadProgressSignal progressSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DownloadDLCPartSignal downloadDLCPartSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCDownloadFinishedSignal downloadFinishedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkModel networkModel { get; set; }

		[Inject]
		public global::Kampai.Splash.ShowNoWiFiPanelSignal showNoWiFiPanelSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ResumeNetworkOperationSignal resumeNetworkOperationSignal { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[Inject]
		public global::Kampai.Splash.IBackgroundDownloadDlcService backgroundDownloadDlcService { get; set; }

		[Inject]
		public global::Kampai.Common.CheckAvailableStorageSignal checkAvailableStorageSignal { get; set; }

		public override void Execute()
		{
			if ((dlcModel.PendingRequests != null && dlcModel.PendingRequests.Count > 0) || (dlcModel.RunningRequests != null && dlcModel.RunningRequests.Count > 0))
			{
				dlcModel.ShouldLaunchDownloadAgain = true;
				dlcModel.NextDownloadShouldLoadAudio = shouldLoadAudio;
				return;
			}
			dlcModel.ShouldLoadAudio = shouldLoadAudio;
			reconcileSignal.Dispatch(false);
			dlcModel.PendingRequests = CreateNetworkRequests();
			dlcModel.RunningRequests = new global::System.Collections.Generic.List<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest>();
			dlcModel.LastNetworkFailureTime = -1f;
			checkAvailableStorageSignal.Dispatch(global::Kampai.Util.GameConstants.PERSISTENT_DATA_PATH, dlcModel.TotalSize, StartDownloadProxy);
		}

		private void StartDownloadProxy()
		{
			routineRunner.StartCoroutine(StartDownload());
		}

		private global::System.Collections.IEnumerator StartDownload()
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			initSignal.Dispatch(dlcModel.TotalSize);
			if (dlcModel.NeededBundles != null && dlcModel.NeededBundles.Count == 0)
			{
				downloadFinishedSignal.Dispatch();
				yield break;
			}
			if (networkModel.reachability == global::UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork && !dlcModel.AllowDownloadOnMobileNetwork)
			{
				resumeNetworkOperationSignal.AddOnce(StartDownloadProxy);
				showNoWiFiPanelSignal.Dispatch(true);
				yield break;
			}
			dlcModel.DownloadStartTime = global::System.DateTime.Now;
			dlcModel.DownloadedTotalSize = 0L;
			if (global::Kampai.Main.LoadState.Get() == global::Kampai.Main.LoadStateType.STARTED)
			{
				backgroundDownloadDlcService.Start();
				yield break;
			}
#if !UNITY_WEBPLAYER
			string dlcPath = global::Kampai.Util.GameConstants.DLC_PATH;
			if (!global::System.IO.Directory.Exists(dlcPath))
			{
				global::System.IO.Directory.CreateDirectory(dlcPath);
			}
#endif
			downloadDLCPartSignal.Dispatch();
		}

		private global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> CreateNetworkRequests()
		{
			string userId = global::UnityEngine.Random.Range(0, 100).ToString();
			bool udpEnabled = global::Kampai.Util.FeatureAccessUtil.isAccessible(global::Kampai.Game.AccessControlledFeature.AKAMAI_UDP, configurationService.GetConfigurations(), userId, logger);
			dlcModel.UdpEnabled = udpEnabled;
			global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> queue = new global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest>(dlcModel.NeededBundles.Count);
			foreach (global::Kampai.Util.BundleInfo neededBundle in dlcModel.NeededBundles)
			{
				global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest item = CreateRequest(neededBundle, udpEnabled);
				queue.Enqueue(item);
			}
			return queue;
		}

		private global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest CreateRequest(global::Kampai.Util.BundleInfo bundleInfo, bool udpEnabled)
		{
			string name = bundleInfo.name;
			string uri = global::Kampai.Util.DownloadUtil.CreateBundleURL(manifestService.GetDLCURL(), name);
			string filePath = global::Kampai.Util.DownloadUtil.CreateBundlePath(global::Kampai.Util.GameConstants.DLC_PATH, name);
			return requestFactory.Resource(uri).WithOutputFile(filePath).WithMd5(bundleInfo.sum)
				.WithGZip(true)
				.WithUdp(udpEnabled)
				.WithRetry(true, 1)
				.WithResume(true)
				.WithResponseSignal(downloadResponseSignal)
				.WithProgressSignal(progressSignal);
		}
	}
}
