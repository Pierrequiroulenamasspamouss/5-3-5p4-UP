namespace Kampai.Game
{
	public class GameResumeCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GameResumeCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.SavePlayerSignal savePlayerSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CancelAllNotificationSignal signal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreCraftingBuildingsSignal restoreCraftingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TeleportMinionToBuildingSignal teleportSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshQueueSlotSignal updateQueueSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal changeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistence { get; set; }

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.IClientHealthService clientHealthService { get; set; }

		[Inject]
		public global::Kampai.Game.MuteVolumeSignal muteVolumeSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppResumeCompletedSignal appResumeCompletedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ReconcileDLCSignal reconcileDLCSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkModel networkModel { get; set; }

		[Inject]
		public global::Kampai.Splash.IBackgroundDownloadDlcService backgroundDownloadDlcService { get; set; }

		[Inject]
		public global::Kampai.Game.LoadServerSalesSignal loadServerSalesSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ReloadGameSignal reloadGameSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Common.CheckAvailableStorageSignal checkAvailableStorageSignal { get; set; }

		private void updateBuildings()
		{
			foreach (global::Kampai.Game.Building item in playerService.GetInstancesByType<global::Kampai.Game.Building>())
			{
				global::Kampai.Game.TaskableBuilding taskableBuilding = item as global::Kampai.Game.TaskableBuilding;
				if (taskableBuilding != null)
				{
					if (taskableBuilding.GetNumCompleteMinions() > 0)
					{
						changeStateSignal.Dispatch(item.ID, global::Kampai.Game.BuildingState.Harvestable);
					}
					continue;
				}
				global::Kampai.Game.CraftingBuilding craftingBuilding = item as global::Kampai.Game.CraftingBuilding;
				if (craftingBuilding != null)
				{
					restoreCraftingSignal.Dispatch(craftingBuilding);
				}
			}
		}

		public override void Execute()
		{
			logger.Info("GameResumeCommand");
			global::Kampai.Util.Native.LogInfo(string.Format("AppResume, GameResumeCommand, realtimeSinceStartup: {0}", timeService.RealtimeSinceStartup()));
			playerDurationService.OnAppResume();
			currencyService.RefreshCatalog();
			global::Kampai.Main.LoadState.Set(global::Kampai.Main.LoadStateType.STARTED);
			if (ShouldSaveGame())
			{
				savePlayerSignal.Dispatch(new global::Kampai.Util.Tuple<global::Kampai.Game.SaveLocation, string, bool>(global::Kampai.Game.SaveLocation.REMOTE, string.Empty, false));
			}
			muteVolumeSignal.Dispatch();
			signal.Dispatch();
			foreach (global::Kampai.Game.Minion item in playerService.GetInstancesByType<global::Kampai.Game.Minion>())
			{
				if (item.State == global::Kampai.Game.MinionState.Tasking)
				{
					teleportSignal.Dispatch(item.ID);
				}
			}
			updateBuildings();
			updateQueueSignal.Dispatch(false);
			clientHealthService.MarkMeterEvent("AppFlow.Resume");
			loadServerSalesSignal.Dispatch();
			currencyService.ResumeTransactionsHandling();
			appResumeCompletedSignal.Dispatch();
			ReconcileDlc();
		}

		private bool ShouldSaveGame()
		{
			if (localPersistence.GetData("SocialInProgress").Equals("True"))
			{
				return false;
			}
			if (localPersistence.GetData("MtxPurchaseInProgress").Equals("True"))
			{
				return false;
			}
			if (localPersistence.GetData("ExternalLinkOpened").Equals("True"))
			{
				localPersistence.PutData("ExternalLinkOpened", "False");
				return false;
			}
			if (localPersistence.GetData("AdVideoInProgress").Equals("True"))
			{
				logger.Debug("Ads: GameResumeCommand.ShouldSaveGame(): reset AdVideoInProgress flag");
				localPersistence.PutData("AdVideoInProgress", "False");
				return false;
			}
			return true;
		}

		private void ReconcileDlc()
		{
			reconcileDLCSignal.Dispatch(false);
		}
	}
}
