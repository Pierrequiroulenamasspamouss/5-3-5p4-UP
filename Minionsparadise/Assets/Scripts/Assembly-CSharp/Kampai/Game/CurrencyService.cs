namespace Kampai.Game
{
	public abstract class CurrencyService : global::Kampai.Game.ICurrencyService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CurrencyService") as global::Kampai.Util.IKampaiLogger;

		protected global::System.Collections.Generic.Stack<global::Kampai.Game.PendingCurrencyTransaction> pendingTransactions = new global::System.Collections.Generic.Stack<global::Kampai.Game.PendingCurrencyTransaction>();

		[Inject]
		public global::Kampai.Game.FinishPremiumPurchaseSignal finishPremiumPurchaseSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CancelPremiumPurchaseSignal cancelPremiumPurchaseSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public abstract string GetPriceWithCurrencyAndFormat(string SKU);

		public abstract void RequestPurchase(global::Kampai.Game.KampaiPendingTransaction item);

		public abstract void ReceiptValidationCallback(global::Kampai.Game.Mtx.ReceiptValidationResult result);

		public abstract void CollectRedemption(global::Kampai.Game.Mtx.ReceiptValidationResult pendingRedemption);

		public abstract void PauseTransactionsHandling();

		public abstract void ResumeTransactionsHandling();

		public abstract void RestorePurchases();

		public void PurchaseCanceledCallback(string SKU, uint errorCode)
		{
			logger.Debug("[NCS] PurchaseCanceledCallback(): sku {0}, errorCode = {1}", SKU, errorCode);
			cancelPremiumPurchaseSignal.Dispatch(SKU, errorCode);
			CurrencyDialogClosed(false);
		}

		public void PurchaseSucceededAndValidatedCallback(string SKU)
		{
			finishPremiumPurchaseSignal.Dispatch(SKU);
		}

		public void PurchaseDeferredCallback(string SKU)
		{
			logger.Debug("[NCS] PurchaseDeferredCallback(): sku {0}", SKU);
			CurrencyDialogClosed(false);
		}

		public void CurrencyDialogClosed(bool success)
		{
			if (pendingTransactions.Count > 0)
			{
				global::Kampai.Game.PendingCurrencyTransaction pendingCurrencyTransaction = pendingTransactions.Pop();
				pendingCurrencyTransaction.ParentSuccess = success;
				pendingCurrencyTransaction.GetCallback()(pendingCurrencyTransaction);
			}
		}

		public void CurrencyDialogOpened(global::Kampai.Game.PendingCurrencyTransaction pendingTransaction)
		{
			pendingTransactions.Push(pendingTransaction);
		}

		public virtual bool IsRefreshingCatalog
		{
			get
			{
				return false;
			}
		}

		public virtual void RefreshCatalog()
		{
		}

		public virtual bool TransactionProcessingEnabled()
		{
			return playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) >= 1;
		}
	}
}
