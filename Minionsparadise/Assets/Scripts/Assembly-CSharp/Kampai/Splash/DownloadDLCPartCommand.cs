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
			logger.Info("DownloadDLCPartCommand: DLC downloading bypassed.");
		}
	}
}
