namespace Kampai.Game
{
	public class PurchaseMarketplaceSlotCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PurchaseMarketplaceSlotCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateLockedPremiumSlotSignal createPremiumSlotSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSaleSlotSignal updateSaleSlot { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshSlotsSignal refreshSlotsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displaySignal { get; set; }

		[Inject]
		public int slotId { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			int count = playerService.GetByDefinitionId<global::Kampai.Game.MarketplaceSaleSlot>(1000008096).Count;
			if (count == 0 || count > marketplaceDefinition.MaxPremiumSlots)
			{
				logger.Error("Invalid number of premium slots");
				return;
			}
			global::Kampai.Game.MarketplaceSaleSlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleSlot>(slotId);
			if (byInstanceId.Definition.type != global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType.PREMIUM_UNLOCKABLE)
			{
				logger.Error("Slot is not premium unlockable");
			}
			else
			{
				playerService.ProcessSlotPurchase(byInstanceId.premiumCost, true, count - 1, PurchaseSlotCallback, 1000008096);
			}
		}

		private void PurchaseSlotCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				setPremiumCurrencySignal.Dispatch();
				global::Kampai.Game.MarketplaceSaleSlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleSlot>(slotId);
				byInstanceId.state = global::Kampai.Game.MarketplaceSaleSlot.State.UNLOCKED;
				updateSaleSlot.Dispatch(byInstanceId.ID);
				globalSFXSignal.Dispatch("Play_button_premium_01");
				displaySignal.Dispatch(19000012, false, new global::strange.extensions.signal.impl.Signal<bool>());
				global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
				int count = playerService.GetByDefinitionId<global::Kampai.Game.MarketplaceSaleSlot>(1000008096).Count;
				if (count < marketplaceDefinition.MaxPremiumSlots)
				{
					createPremiumSlotSignal.Dispatch();
					refreshSlotsSignal.Dispatch(true);
				}
			}
		}
	}
}
