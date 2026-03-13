namespace Kampai.Splash
{
	public class LogoPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.Splash.LogoPanelView view { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkModel networkModel { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkTypeChangedSignal networkTypeChangedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ResumeNetworkOperationSignal resumeNetworkOperationSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Splash.ShowNoWiFiPanelSignal showNoWiFiPanelSignal { get; set; }

		public override void OnRegister()
		{
			view.SetupRefs();
			global::UnityEngine.Screen.sleepTimeout = -1;
			networkTypeChangedSignal.AddListener(OnNetworkTypeChanged);
			resumeNetworkOperationSignal.AddListener(OnNetworkResume);
			showNoWiFiPanelSignal.AddListener(view.ShowNoWiFi);
		}

		public override void OnRemove()
		{
			networkTypeChangedSignal.RemoveListener(OnNetworkTypeChanged);
			resumeNetworkOperationSignal.RemoveListener(OnNetworkResume);
			showNoWiFiPanelSignal.RemoveListener(view.ShowNoWiFi);
			global::UnityEngine.Screen.sleepTimeout = -2;
		}

		private void OnNetworkTypeChanged(global::UnityEngine.NetworkReachability type)
		{
			switch (type)
			{
			case global::UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork:
				dlcModel.AllowDownloadOnMobileNetwork = false;
				break;
			case global::UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork:
				if (!networkModel.isConnectionLost && !dlcModel.AllowDownloadOnMobileNetwork)
				{
					PauseDownload();
				}
				break;
			}
		}

		private void OnNetworkResume()
		{
			if (networkModel.reachability == global::UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork && !dlcModel.AllowDownloadOnMobileNetwork)
			{
				PauseDownload();
			}
		}

		private void PauseDownload()
		{
			downloadService.Restart();
			showNoWiFiPanelSignal.Dispatch(true);
		}
	}
}
