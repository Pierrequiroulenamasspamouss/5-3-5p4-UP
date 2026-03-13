namespace Kampai.Game
{
	public class SellToAICommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSaleSlotSignal updateSaleSlot { get; set; }

		[Inject]
		public global::Kampai.Game.MarketplaceItemSoldSignal marketplaceItemSoldSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateStorageItemsSignal updateStorageItemsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.MarketplaceUpdateSoldItemsSignal updateSoldItemsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.InterpolateSaleTimeSignal interpolateSaleTimeSignal { get; set; }

		[Inject]
		public global::Kampai.Util.Tuple<int, int, int, int> saleParameters { get; set; }

		[Inject]
		public global::Kampai.Game.ReportMarketplaceTransactionSignal reportMarketplaceTransactionSignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		public override void Execute()
		{
			int item = saleParameters.Item1;
			int startTime = timeService.CurrentTime();
			global::Kampai.Game.MarketplaceItemDefinition marketplaceItemDefinition = definitionService.Get<global::Kampai.Game.MarketplaceItemDefinition>(item);
			global::Kampai.Game.MarketplaceSaleItem marketplaceSaleItem = CreateMarketplaceItem(startTime, marketplaceItemDefinition);
			if (localPersistService.GetData("ZeroSellTime") == "true")
			{
				marketplaceSaleItem.LengthOfSale = 0;
			}
			playerService.Add(marketplaceSaleItem);
			global::Kampai.Game.MarketplaceSaleSlot nextAvailableSlot = marketplaceService.GetNextAvailableSlot();
			nextAvailableSlot.itemId = marketplaceSaleItem.ID;
			updateSaleSlot.Dispatch(nextAvailableSlot.ID);
			updateSoldItemsSignal.Dispatch(true);
			timeEventService.AddEvent(marketplaceSaleItem.ID, startTime, marketplaceSaleItem.LengthOfSale, marketplaceItemSoldSignal);
			RemoveItemsFromInventory(marketplaceSaleItem);
			updateStorageItemsSignal.Dispatch();
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(marketplaceItemDefinition.ItemID);
			telemetryService.Send_Telemtry_EVT_MARKETPLACE_ITEM_LISTED(itemDefinition.Description, saleParameters.Item3, saleParameters.Item2, itemDefinition.TaxonomyHighLevel, itemDefinition.TaxonomySpecific, itemDefinition.TaxonomyType, itemDefinition.TaxonomyOther);
			reportMarketplaceTransactionSignal.Dispatch(marketplaceSaleItem);
		}

		private global::Kampai.Game.MarketplaceSaleItem CreateMarketplaceItem(int startTime, global::Kampai.Game.MarketplaceItemDefinition marketplaceItemDefinition)
		{
			global::Kampai.Game.MarketplaceSaleItem marketplaceSaleItem = new global::Kampai.Game.MarketplaceSaleItem(marketplaceItemDefinition);
			marketplaceSaleItem.SaleStartTime = startTime;
			marketplaceSaleItem.SalePrice = saleParameters.Item2;
			marketplaceSaleItem.QuantitySold = saleParameters.Item3;
			interpolateSaleTimeSignal.Dispatch(marketplaceSaleItem);
			return marketplaceSaleItem;
		}

		private void RemoveItemsFromInventory(global::Kampai.Game.MarketplaceSaleItem marketplaceItem)
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
			transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			transactionDefinition.Inputs.Add(new global::Kampai.Util.QuantityItem(marketplaceItem.Definition.ItemID, (uint)marketplaceItem.QuantitySold));
			playerService.RunEntireTransaction(transactionDefinition, global::Kampai.Game.TransactionTarget.NO_VISUAL, null, new global::Kampai.Game.TransactionArg
			{
				InstanceId = 314,
				Source = "Marketplace"
			});
		}
	}
}
