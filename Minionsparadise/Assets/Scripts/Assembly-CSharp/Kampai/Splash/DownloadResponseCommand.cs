namespace Kampai.Splash
{
	public class DownloadResponseCommand : global::strange.extensions.command.impl.Command
	{
		private const float FAILURE_MORATORIUM_PERIOD_SEC = 5f;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DownloadResponseCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Splash.DownloadDLCPartSignal downloadDLCPartSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.Service.Audio.IFMODService fmodService { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCDownloadFinishedSignal finishedSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.LaunchDownloadSignal launchDownloadSignal { get; set; }

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.IClientHealthService clientHealthService { get; set; }

		[Inject]
		public global::Kampai.Game.ReconcileSalesSignal reconcileSalesSignal { get; set; }

		public override void Execute()
		{
			logger.Debug("DLC: DownloadResponseCommand.Execute() dlcModel.NeededBundles.Count = {0}", dlcModel.NeededBundles.Count);
			global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request = response.Request;
			dlcModel.RunningRequests.Remove(request);
			if (request.IsAborted())
			{
				logger.Debug("DLC: DownloadResponseCommand aborted, url = {0}", request.Uri);
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Util.BundleInfo> neededBundles = dlcModel.NeededBundles;
			if (response.Success)
			{
				string bundleNameFromUrl = global::Kampai.Util.DownloadUtil.GetBundleNameFromUrl(request.Uri);
				sendDownloadTelemetry(bundleNameFromUrl);
				int num = 0;
				foreach (global::Kampai.Util.BundleInfo item in neededBundles)
				{
					if (item.name == bundleNameFromUrl)
					{
						neededBundles.RemoveAt(num);
						if (item.audio)
						{
							dlcModel.DownloadedAudioBundles.Enqueue(bundleNameFromUrl);
						}
						break;
					}
					num++;
				}
				dlcModel.LastNetworkFailureTime = -1f;
				UpdateDownloadTotals(response);
			}
			else
			{
				HandleRequestFailure(request);
			}
			if (neededBundles.Count == 0)
			{
				if (dlcModel.ShouldLoadAudio)
				{
					routineRunner.StartCoroutine(LoadAudioBundles());
				}
				else
				{
					HandleDownloadFinished();
				}
			}
			else
			{
				global::UnityEngine.PlayerPrefs.SetInt("HasUpToDateDlc", 0);
				downloadDLCPartSignal.Dispatch();
			}
		}

		private void sendDownloadTelemetry(string bundleName)
		{
			if (!configurationsService.isKillSwitchOn(global::Kampai.Game.KillSwitch.DLC_TELEMETRY))
			{
				telemetryService.Send_Telemetry_EVT_USER_GAME_DOWNLOAD_FUNNEL(bundleName, response.DownloadTime, response.ContentLength, global::Kampai.Util.NetworkUtil.IsNetworkWiFi());
			}
		}

		private void UpdateDownloadTotals(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (response.ContentLength > 0)
			{
				dlcModel.DownloadedTotalSize += response.ContentLength;
			}
		}

		private void HandleDownloadFinished()
		{
			telemetryService.Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL("60 - Downloaded DLC", playerService.SWRVEGroup, dlcService.GetDownloadQualityLevel());
			global::UnityEngine.PlayerPrefs.SetInt("HasUpToDateDlc", 1);
			if (dlcModel.ShouldLaunchDownloadAgain)
			{
				dlcModel.ShouldLaunchDownloadAgain = false;
				launchDownloadSignal.Dispatch(dlcModel.NextDownloadShouldLoadAudio);
			}
			else
			{
				dlcModel.HighestTierDownloaded = dlcService.GetPlayerDLCTier();
				finishedSignal.Dispatch();
				reconcileSalesSignal.Dispatch(0);
			}
			long num = (long)(global::System.DateTime.Now - dlcModel.DownloadStartTime).TotalMilliseconds;
			logger.Info("DownloadResponse DLC Download Speed Stats : DownloadedTotalTime: {0} , DownloadedTotalSize : {1}  ", num, dlcModel.DownloadedTotalSize);
			if (num > 0)
			{
				string text = ((!dlcModel.UdpEnabled) ? "Download.Http" : "Download.Udp");
				float num2 = (float)dlcModel.DownloadedTotalSize / (float)num;
				clientHealthService.MarkTimerEvent(text, num2);
				logger.Info("DownloadResponse DLC Download Speed Stats : eventname: {0} , downloadSpeed : {1} ", text, num2);
				dlcModel.DownloadedTotalSize = 0L;
				dlcModel.DownloadStartTime = global::System.DateTime.MaxValue;
			}
		}

		private void HandleRequestFailure(global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request)
		{
			if (dlcModel.LastNetworkFailureTime < 0f)
			{
				dlcModel.LastNetworkFailureTime = global::UnityEngine.Time.realtimeSinceStartup;
			}
			float num = global::UnityEngine.Time.realtimeSinceStartup - dlcModel.LastNetworkFailureTime;
			if (num > 5f)
			{
				logger.Debug("DLC: DownloadResponseCommand.HandleRequestFailure(): network switch time is up");
				logger.FatalNoThrow(global::Kampai.Util.FatalCode.DLC_REQ_FAIL, "Unable to download DLC {0}", request.Uri);
			}
			else
			{
				logger.Debug("DLC: DownloadResponseCommand.HandleRequestFailure(): give time to switch networks(possible reason of network failure).");
				dlcModel.PendingRequests.Enqueue(request);
			}
		}

		private global::System.Collections.IEnumerator LoadAudioBundles()
		{
			global::System.Collections.Generic.Queue<string> downloadedAudioBundles = new global::System.Collections.Generic.Queue<string>(dlcModel.DownloadedAudioBundles);
			while (downloadedAudioBundles.Count > 0)
			{
				string bundleName = downloadedAudioBundles.Dequeue();
				fmodService.LoadFromAssetBundleAsync(bundleName);
			}
			fmodService.StartAsyncBankLoadingProcessing();
			while (fmodService.BanksLoadingInProgress())
			{
				yield return null;
			}
			HandleDownloadFinished();
		}
	}
}
