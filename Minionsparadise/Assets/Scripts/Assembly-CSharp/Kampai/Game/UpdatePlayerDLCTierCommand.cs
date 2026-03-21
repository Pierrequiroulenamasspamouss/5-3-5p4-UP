namespace Kampai.Game
{
	public class UpdatePlayerDLCTierCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UpdatePlayerDLCTierCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Splash.LaunchDownloadSignal launchDownloadSignal { get; set; }

		public override void Execute()
		{
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.TIER_ID);
			int quantity2 = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.TIER_GATE_ID);
			logger.Info("UpdatePlayerDLCTierCommand: tier={0} gate={1}. Logic bypassed for DLC-independent build.", quantity, quantity2);
			dlcService.SetPlayerDLCTier(quantity);
			
			// Always signal that we are ready to proceed
			launchDownloadSignal.Dispatch(true);
		}
	}
}
