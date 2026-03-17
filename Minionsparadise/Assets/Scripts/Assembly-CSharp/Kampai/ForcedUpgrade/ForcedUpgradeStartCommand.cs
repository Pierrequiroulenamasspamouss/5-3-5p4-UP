namespace Kampai.ForcedUpgrade
{
	public class ForcedUpgradeStartCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ForcedUpgradeStartCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.InitLocalizationServiceSignal initLocalizationServiceSignal { get; set; }

		public override void Execute()
		{
			logger.Info("ForcedUpgrade scene starting...");
			initLocalizationServiceSignal.Dispatch();
		}
	}
}
