namespace Kampai.Game
{
	public class KillSwitchChangedCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("KillSwitchChangedCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.GOOGLEPLAY)]
		public global::Kampai.Game.ISocialService gpService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Common.NimbleTelemetrySender nimbleTelemetryService { get; set; }

		public override void Execute()
		{
			facebookService.updateKillSwitchFlag();
			gpService.updateKillSwitchFlag();
			if (configService.isKillSwitchOn(global::Kampai.Game.KillSwitch.SYNERGY))
			{
				logger.Info("=======================================================================================================================================================================================");
				logger.Info("=======================================================================================================================================================================================");
				logger.Info("#                                                                                                                                                                                     #");
				logger.Info("#                  SYNERGY KillSwitch ON                                                                                                                                              #");
				logger.Info("#                                                                                                                                                                                     #");
				logger.Info("=======================================================================================================================================================================================");
				telemetryService.SharingUsage(nimbleTelemetryService, false);
			}
			else
			{
				logger.Info("=======================================================================================================================================================================================");
				logger.Info("=======================================================================================================================================================================================");
				logger.Info("#                                                                                                                                                                                     #");
				logger.Info("#                  SYNERGY KillSwitch OFF                                                                                                                                             #");
				logger.Info("#                                                                                                                                                                                     #");
				logger.Info("=======================================================================================================================================================================================");
				telemetryService.SharingUsage(nimbleTelemetryService, true);
			}
		}
	}
}
