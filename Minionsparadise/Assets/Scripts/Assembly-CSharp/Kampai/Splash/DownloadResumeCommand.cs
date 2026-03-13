namespace Kampai.Splash
{
	public class DownloadResumeCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DownloadResumeCommand") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.IEnumerator restartDownloadCoroutine;

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Splash.LaunchDownloadSignal launchSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.AppPauseSignal pauseSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		public override void Execute()
		{
			if (global::Kampai.Main.LoadState.Get() != global::Kampai.Main.LoadStateType.STARTED)
			{
				global::Kampai.Util.Native.LogInfo(string.Format("AppResume, DownloadResumeCommand, realtimeSinceStartup: {0}", timeService.RealtimeSinceStartup()));
			}
			if (global::Kampai.Main.LoadState.Get() != global::Kampai.Main.LoadStateType.DLC)
			{
				logger.Info("DownloadResumeCommand.Execute(): do not restart DLC downloading, it was not started yet.");
				return;
			}
			if (dlcModel.NeededBundles.Count == 0)
			{
				logger.Info("DownloadResumeCommand.Execute(): no more DLC needed.");
				return;
			}
			restartDownloadCoroutine = RestartDownload();
			routineRunner.StartCoroutine(restartDownloadCoroutine);
		}

		private global::System.Collections.IEnumerator RestartDownload()
		{
			pauseSignal.AddOnce(OnPause);
			yield return null;
			logger.Info("DownloadResumeCommand.RestartDownload(): restart DLC downloading.");
			global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> pendingRequests = dlcModel.PendingRequests;
			if (pendingRequests != null && pendingRequests.Count != 0)
			{
				pendingRequests.Clear();
			}
			global::System.Collections.Generic.List<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> runningRequests = dlcModel.RunningRequests;
			if (runningRequests != null && runningRequests.Count != 0)
			{
				int retries = 10;
				while (runningRequests.Count != 0 && retries-- != 0)
				{
					logger.Info("DownloadResumeCommand.RestartDownload: wait for {0} request(s) [{1} retry(ies) left].", runningRequests.Count, retries);
					yield return new global::UnityEngine.WaitForSeconds(0.1f);
				}
				runningRequests.Clear();
			}
			pauseSignal.RemoveListener(OnPause);
			restartDownloadCoroutine = null;
			logger.Info("DownloadResumeCommand.RestartDownload: launch DLC downloading.");
			launchSignal.Dispatch(dlcModel.ShouldLoadAudio);
		}

		private void OnPause()
		{
			if (restartDownloadCoroutine != null)
			{
				routineRunner.StopCoroutine(restartDownloadCoroutine);
				restartDownloadCoroutine = null;
			}
		}
	}
}
