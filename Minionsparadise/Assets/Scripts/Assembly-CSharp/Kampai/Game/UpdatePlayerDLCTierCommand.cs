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

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Main.ReloadGameSignal reloadSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SavePlayerSignal saveSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		public override void Execute()
		{
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.TIER_ID);
			int quantity2 = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.TIER_GATE_ID);
			logger.Info("UpdatePlayerDLCTierCommand: tier={0} gate={1}", quantity, quantity2);
			dlcService.SetPlayerDLCTier(quantity);
			if (dlcModel.HighestTierDownloaded < quantity2)
			{
				saveSignal.Dispatch(new global::Kampai.Util.Tuple<global::Kampai.Game.SaveLocation, string, bool>(global::Kampai.Game.SaveLocation.REMOTE, string.Empty, true));
				routineRunner.StartCoroutine(WaitAFrame());
			}
			else
			{
				launchDownloadSignal.Dispatch(true);
			}
		}

		private global::System.Collections.IEnumerator WaitAFrame()
		{
			yield return null;
			logger.Debug("UpdatePlayerDLCTierCommand: Kicking player out of game.");
			reloadSignal.Dispatch();
		}
	}
}
