namespace Kampai.Game
{
	public interface ICurrencyService
	{
		string GetPriceWithCurrencyAndFormat(string SKU);

		void RequestPurchase(global::Kampai.Game.KampaiPendingTransaction item);

		void PurchaseCanceledCallback(string SKU, uint errorCode);

		void PurchaseDeferredCallback(string SKU);

		void PurchaseSucceededAndValidatedCallback(string SKU);

		void ReceiptValidationCallback(global::Kampai.Game.Mtx.ReceiptValidationResult result);

		void CurrencyDialogClosed(bool success);

		void CurrencyDialogOpened(global::Kampai.Game.PendingCurrencyTransaction pendingTransaction);

		void PauseTransactionsHandling();

		void ResumeTransactionsHandling();

		void RestorePurchases();

		void CollectRedemption(global::Kampai.Game.Mtx.ReceiptValidationResult pendingRedemption);

		void RefreshCatalog();

		bool IsRefreshingCatalog { get; }

		bool TransactionProcessingEnabled();
	}
}
