namespace Kampai.Game
{
	public class DisplayRedemptionConfirmationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService defService { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenUpSellModalSignal openUpsellModalSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Mtx.ReceiptValidationResult receiptValidationResult = playerService.topPendingRedemption();
			if (receiptValidationResult == null)
			{
				return;
			}
			foreach (global::Kampai.Game.SalePackDefinition item in defService.GetAll<global::Kampai.Game.SalePackDefinition>())
			{
				if (global::Kampai.Util.ItemUtil.CompareSKU(item.SKU, receiptValidationResult.sku))
				{
					openUpsellModalSignal.Dispatch(item, "REDEMPTION", true);
				}
			}
		}
	}
}
