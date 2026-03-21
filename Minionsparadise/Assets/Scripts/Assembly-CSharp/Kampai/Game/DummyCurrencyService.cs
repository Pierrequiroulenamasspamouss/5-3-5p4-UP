namespace Kampai.Game
{
	public class DummyCurrencyService : global::Kampai.Game.CurrencyService
	{
		public override string GetPriceWithCurrencyAndFormat(string SKU)
		{
			return string.Empty;
		}

		public override void RequestPurchase(global::Kampai.Game.KampaiPendingTransaction item)
		{
			logger.Debug("[DCS] RequestPurchase(): SKU {0} - Dummy implementation, doing nothing.", item.ExternalIdentifier);
		}

		public override void ReceiptValidationCallback(global::Kampai.Game.Mtx.ReceiptValidationResult result)
		{
		}

		public override void CollectRedemption(global::Kampai.Game.Mtx.ReceiptValidationResult pendingRedemption)
		{
		}

		public override void PauseTransactionsHandling()
		{
		}

		public override void ResumeTransactionsHandling()
		{
		}

		public override void RestorePurchases()
		{
			logger.Debug("[DCS] RestorePurchases(): Dummy implementation, doing nothing.");
		}
	}
}
