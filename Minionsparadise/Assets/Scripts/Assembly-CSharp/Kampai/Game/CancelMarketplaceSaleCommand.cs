namespace Kampai.Game
{
	public class CancelMarketplaceSaleCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Game.MarketplaceSaleItem item;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateStorageItemsSignal updateStorageItemsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSaleSlotSignal updateSaleSlotSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MarketplaceUpdateSoldItemsSignal updateSoldItemsSignal { get; set; }

		[Inject]
		public int instanceId { get; set; }

		public override void Execute()
		{
			item = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(instanceId);
			if (item != null)
			{
				int saleCancellationCost = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>().SaleCancellationCost;
				playerService.ProcessSaleCancel(saleCancellationCost, TransactionCallback);
			}
		}

		private void TransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				setPremiumCurrencySignal.Dispatch();
				global::Kampai.Game.MarketplaceSaleSlot slotByItem = marketplaceService.GetSlotByItem(item);
				playerService.Remove(item);
				timeEventService.RemoveEvent(instanceId);
				updateStorageItemsSignal.Dispatch();
				updateSaleSlotSignal.Dispatch(slotByItem.ID);
				updateSoldItemsSignal.Dispatch(true);
			}
		}
	}
}
