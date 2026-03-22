namespace Kampai.Splash
{
	public class DownloadResumeCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DownloadResumeCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Splash.LaunchDownloadSignal launchSignal { get; set; }

		public override void Execute()
		{
			logger.Info("DownloadResumeCommand: Logic bypassed for DLC-independent build. Immediately signaling finished.");
			launchSignal.Dispatch(false);
		}
	}
}
