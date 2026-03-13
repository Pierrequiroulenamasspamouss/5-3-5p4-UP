namespace Kampai.Splash
{
	public class DownloadDLCPartCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DownloadDLCPartCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		public override void Execute()
		{
			if (dlcModel.PendingRequests == null || dlcModel.RunningRequests == null)
			{
				logger.Error("Attempting to run DLCPartCommand with no model!");
				return;
			}
			global::Kampai.Main.LoadState.Set(global::Kampai.Main.LoadStateType.DLC);
			while (dlcModel.PendingRequests.Count > 0 && dlcModel.RunningRequests.Count < 5)
			{
				global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request = dlcModel.PendingRequests.Dequeue();
				dlcModel.RunningRequests.Add(request);
				downloadService.Perform(request);
			}
		}
	}
}
