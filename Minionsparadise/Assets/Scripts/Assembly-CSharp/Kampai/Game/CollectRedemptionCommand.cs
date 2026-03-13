namespace Kampai.Game
{
	public class CollectRedemptionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CollectRedemptionCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Mtx.ReceiptValidationResult receiptValidationResult = playerService.popPendingRedemption();
			if (receiptValidationResult != null)
			{
				logger.Info("Redeeming pending Redemption, sku = " + receiptValidationResult.sku);
				currencyService.CollectRedemption(receiptValidationResult);
			}
			else
			{
				logger.Error("Attempting to Collect Redemption but there is not pending redemption");
			}
		}
	}
}
