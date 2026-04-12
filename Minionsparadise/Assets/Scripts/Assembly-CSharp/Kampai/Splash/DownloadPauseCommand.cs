namespace Kampai.Splash
{
	public class DownloadPauseCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DownloadPauseCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		public override void Execute()
		{
			logger.Info("DownloadPauseCommand: Logic bypassed for DLC-independent build.");
		}
	}
}
