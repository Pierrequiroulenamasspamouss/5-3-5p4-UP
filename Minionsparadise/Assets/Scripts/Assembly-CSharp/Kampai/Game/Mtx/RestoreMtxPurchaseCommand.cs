namespace Kampai.Game.Mtx
{
	public class RestoreMtxPurchaseCommand : global::strange.extensions.command.impl.Command, global::System.IDisposable
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RestoreMtxPurchaseCommand") as global::Kampai.Util.IKampaiLogger;

		private NimbleBridge_NotificationListener mtxRestorePurchaseListener;

		private bool _isDisposed;

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		public override void Execute()
		{
			DisposedCheck();
			mtxRestorePurchaseListener = new NimbleBridge_NotificationListener(OnMTXRestorePurchases);
			NimbleBridge_NotificationCenter.RegisterListener("nimble.notification.mtx.restorepurchasedtransactionsfinished", mtxRestorePurchaseListener);
			currencyService.RestorePurchases();
		}

		private void OnMTXRestorePurchases(string name, global::System.Collections.Generic.Dictionary<string, object> userData, NimbleBridge_NotificationListener listener)
		{
			if (!name.Equals("nimble.notification.mtx.restorepurchasedtransactionsfinished"))
			{
				return;
			}
			NimbleBridge_NotificationCenter.UnregisterListener(mtxRestorePurchaseListener);
			if (!userData.ContainsKey("result"))
			{
				popupMessageSignal.Dispatch(localService.GetString("RestorePurchasesFail"), global::Kampai.UI.View.PopupMessageType.NORMAL);
				return;
			}
			if (!"1".Equals(userData["result"]))
			{
				popupMessageSignal.Dispatch(localService.GetString("RestorePurchasesFail"), global::Kampai.UI.View.PopupMessageType.NORMAL);
				return;
			}
			global::System.Collections.Generic.IList<string> list = new global::System.Collections.Generic.List<string>(playerService.GetMTXPurchaseTracking());
			NimbleBridge_MTXTransaction[] purchasedTransactions = NimbleBridge_MTX.GetComponent().GetPurchasedTransactions();
			NimbleBridge_MTXTransaction[] array = purchasedTransactions;
			foreach (NimbleBridge_MTXTransaction nimbleBridge_MTXTransaction in array)
			{
				using (NimbleBridge_Error nimbleBridge_Error = nimbleBridge_MTXTransaction.GetError())
				{
					if (nimbleBridge_Error != null && !nimbleBridge_Error.IsNull())
					{
						logger.Warning("Skipping an invalid transaction: {0}", nimbleBridge_MTXTransaction.GetItemSku());
						continue;
					}
				}
				if (nimbleBridge_MTXTransaction.GetTransactionType() != NimbleBridge_MTXTransaction.Type.RESTORE)
				{
					logger.Warning("Skipping a transaction that is not type RESTORE: {0}", nimbleBridge_MTXTransaction.GetItemSku());
					continue;
				}
				string itemSku = nimbleBridge_MTXTransaction.GetItemSku();
				if (list.Contains(itemSku))
				{
					list.Remove(itemSku);
					continue;
				}
				global::System.Collections.Generic.List<global::Kampai.Game.PackDefinition> all = definitionService.GetAll<global::Kampai.Game.PackDefinition>();
				foreach (global::Kampai.Game.PackDefinition item in all)
				{
					if (item.PlatformStoreSku == null)
					{
						continue;
					}
					foreach (global::Kampai.Game.PlatformStoreSkuDefinition item2 in item.PlatformStoreSku)
					{
						if (global::Kampai.Util.ItemUtil.CompareSKU(itemSku, item2.appleAppstore))
						{
							logger.Info("Restoring purchase: {0}", nimbleBridge_MTXTransaction.GetItemSku());
							global::Kampai.Game.KampaiPendingTransaction kampaiPendingTransaction = new global::Kampai.Game.KampaiPendingTransaction();
							kampaiPendingTransaction.ExternalIdentifier = itemSku;
							kampaiPendingTransaction.StoreItemDefinitionId = item.ID;
							kampaiPendingTransaction.TransactionInstance = item.TransactionDefinition;
							kampaiPendingTransaction.UTCTimeCreated = timeService.CurrentTime();
							playerService.QueuePendingTransaction(kampaiPendingTransaction);
							uiContext.injectionBinder.GetInstance<global::Kampai.Game.FinishPremiumPurchaseSignal>().Dispatch(itemSku);
							break;
						}
					}
				}
			}
			popupMessageSignal.Dispatch(localService.GetString("RestorePurchasesSuccess"), global::Kampai.UI.View.PopupMessageType.NORMAL);
		}

		public void Dispose()
		{
			Dispose(true);
			global::System.GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool fromDispose)
		{
			if (fromDispose)
			{
				DisposedCheck();
				mtxRestorePurchaseListener.Dispose();
			}
			_isDisposed = true;
		}

		private void DisposedCheck()
		{
			if (_isDisposed)
			{
				throw new global::System.ObjectDisposedException(ToString());
			}
		}

		~RestoreMtxPurchaseCommand()
		{
			Dispose(false);
		}
	}
}
