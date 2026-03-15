namespace Kampai.Splash
{
	public class DownloadPauseCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DownloadPauseCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkModel networkModel { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		public override void Execute()
		{
			if (global::Kampai.Main.LoadState.Get() != global::Kampai.Main.LoadStateType.STARTED)
			{
				global::Kampai.Util.Native.LogInfo(string.Format("AppPause, DownloadPauseCommand, realtimeSinceStartup: {0}", timeService.RealtimeSinceStartup()));
			}
			if (global::Kampai.Main.LoadState.Get() != global::Kampai.Main.LoadStateType.DLC)
			{
				logger.Info("DownloadPauseCommand.Execute(): do not abort DLC downloading, it was not started yet.");
				return;
			}
			if (dlcModel.NeededBundles.Count == 0)
			{
				logger.Info("DownloadPauseCommand.Execute(): no DLC to abort.");
				return;
			}
			logger.Info("DownloadPauseCommand.Execute(): abort DLC downloading.");
			global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> pendingRequests = dlcModel.PendingRequests;
			if (pendingRequests != null && pendingRequests.Count != 0)
			{
				pendingRequests.Clear();
			}
			global::System.Collections.Generic.List<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> runningRequests = dlcModel.RunningRequests;
			if (runningRequests == null || runningRequests.Count == 0)
			{
				return;
			}
			foreach (global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest item in runningRequests)
			{
				item.Abort();
			}
			if (networkModel.isConnectionLost)
			{
				downloadService.ProcessQueue();
			}
		}
	}
}
