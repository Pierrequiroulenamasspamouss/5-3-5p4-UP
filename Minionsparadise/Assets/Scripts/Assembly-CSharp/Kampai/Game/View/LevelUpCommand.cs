namespace Kampai.Game.View
{
	public class LevelUpCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.AwardLevelSignal awardLevelSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService characterService { get; set; }

		[Inject]
		public global::Kampai.Game.GetNewQuestSignal getNewQuestSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateForSaleSignsSignal updateForSaleSignsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateMarketplaceRepairStateSignal updateMarketplaceSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockCharacterModel model { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayNotificationReminderSignal notificationSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.ILoadInService loadInService { get; set; }

		[Inject]
		public global::Kampai.Game.ReconcileSalesSignal reconcileSalesSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		public override void Execute()
		{
			playerService.AlterQuantity(global::Kampai.Game.StaticItem.LEVEL_ID, 1);
			loadInService.SaveTipsForNextLaunch((int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID));
			if (model.characterUnlocks.Count == 0)
			{
				closeAllMenuSignal.Dispatch(null);
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.DisplayLevelUpRewardSignal>().Dispatch(true);
			}
			global::Kampai.Game.Transaction.TransactionDefinition rewardTransaction = RewardUtil.GetRewardTransaction(definitionService, playerService);
			awardLevelSignal.Dispatch(rewardTransaction);
			characterService.UpdateEligiblePrestigeList();
			getNewQuestSignal.Dispatch();
			telemetryService.Send_Telemetry_EVT_GP_LEVEL_PROMOTION();
			playerDurationService.MarkLevelUpUTC();
			updateMarketplaceSignal.Dispatch();
			updateForSaleSignsSignal.Dispatch();
			if (playerService.GetHighestFtueCompleted() >= 999999)
			{
				reconcileSalesSignal.Dispatch(0);
			}
			CheckNotifications();
		}

		private void CheckNotifications()
		{
			if (global::Kampai.Util.Native.AreNotificationsEnabled())
			{
				return;
			}
			global::Kampai.Game.NotificationSystemDefinition notificationSystemDefinition = definitionService.Get<global::Kampai.Game.NotificationSystemDefinition>(66666);
			foreach (global::Kampai.Game.NotificationReminder notificationReminder in notificationSystemDefinition.notificationReminders)
			{
				if (notificationReminder.level == playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID))
				{
					notificationSignal.Dispatch(localizationService.GetString(notificationReminder.messageLocalizedKey), true);
				}
			}
		}
	}
}
