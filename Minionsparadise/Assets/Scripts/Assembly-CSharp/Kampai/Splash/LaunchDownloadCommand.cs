namespace Kampai.Splash
{
	public class LaunchDownloadCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LaunchDownloadCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCDownloadFinishedSignal downloadFinishedSignal { get; set; }

		public override void Execute()
		{
			logger.Info("LaunchDownloadCommand: DLC downloading bypassed.");
			dlcModel.TotalSize = 0;
			downloadFinishedSignal.Dispatch();
		}
	}
}
