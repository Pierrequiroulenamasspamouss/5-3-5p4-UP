namespace Kampai.Game
{
	public class CollectMarketplaceSaleCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CollectMarketplaceSaleCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSaleSlotSignal updateSaleSlot { get; set; }

		[Inject]
		public global::Kampai.Game.MarketplaceUpdateSoldItemsSignal updateSoldItemsSignal { get; set; }

		[Inject]
		public int slotId { get; set; }

		[Inject]
		public global::Kampai.Game.TransactionArg arg { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MarketplaceSaleSlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleSlot>(slotId);
			if (byInstanceId == null)
			{
				return;
			}
			global::Kampai.Game.MarketplaceSaleItem byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(byInstanceId.itemId);
			if (byInstanceId2 == null)
			{
				return;
			}
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(byInstanceId2.Definition.TransactionID);
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition2 = transactionDefinition.CopyTransaction();
			foreach (global::Kampai.Util.QuantityItem output in transactionDefinition2.Outputs)
			{
				output.Quantity *= (uint)byInstanceId2.SalePrice;
			}
			playerService.RunEntireTransaction(transactionDefinition2, global::Kampai.Game.TransactionTarget.MARKETPLACE, TransactionCallback, arg);
		}

		private void TransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				global::Kampai.Game.MarketplaceSaleSlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleSlot>(slotId);
				if (byInstanceId != null)
				{
					global::Kampai.Game.MarketplaceSaleItem byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(byInstanceId.itemId);
					playerService.Remove(byInstanceId2);
					byInstanceId.itemId = 0;
					updateSaleSlot.Dispatch(byInstanceId.ID);
					updateSoldItemsSignal.Dispatch(true);
				}
			}
			else
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Failed to collect sold item reward");
			}
		}
	}
}
