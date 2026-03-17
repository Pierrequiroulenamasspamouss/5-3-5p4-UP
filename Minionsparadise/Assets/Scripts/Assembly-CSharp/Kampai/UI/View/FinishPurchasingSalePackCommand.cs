namespace Kampai.UI.View
{
	public class FinishPurchasingSalePackCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("FinishPurchasingSalePackCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.UpdatePurchasedSalesSignal updatePurchasedSalesSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReconcileSalesSignal reconcileSalesSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshMTXStoreSignal refreshMTXStoreSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveSalePackSignal removeSalePackSignal { get; set; }

		[Inject]
		public int saleDefinitionId { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.PackDefinition packDefinition = definitionService.Get<global::Kampai.Game.PackDefinition>(saleDefinitionId);
			if (packDefinition == null)
			{
				logger.Error("Unable to find the sale definition with id: {0}", saleDefinitionId);
				return;
			}
			playerService.AddUpsellToPurchased(saleDefinitionId);
			global::Kampai.Game.SalePackDefinition salePackDefinition = packDefinition as global::Kampai.Game.SalePackDefinition;
			if (salePackDefinition != null)
			{
				global::Kampai.Game.Sale firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Sale>(saleDefinitionId);
				if (firstInstanceByDefinitionId == null)
				{
					logger.Error("Sale instance not found for definition id {0}", saleDefinitionId);
					return;
				}
				removeSalePackSignal.Dispatch(firstInstanceByDefinitionId.ID);
				UpdateServerSales(salePackDefinition);
				playerService.Remove(firstInstanceByDefinitionId);
			}
			refreshMTXStoreSignal.Dispatch();
			reconcileSalesSignal.Dispatch(0);
		}

		private void UpdateServerSales(global::Kampai.Game.SalePackDefinition salePackDefinition)
		{
			if (!string.IsNullOrEmpty(salePackDefinition.ServerSaleId))
			{
				updatePurchasedSalesSignal.Dispatch(salePackDefinition.ServerSaleId);
			}
		}
	}
}
