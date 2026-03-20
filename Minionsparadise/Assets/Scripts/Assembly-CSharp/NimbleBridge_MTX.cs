public class NimbleBridge_MTX
{
	private delegate void BridgeMTXTransactionCallback(global::System.IntPtr transactionPtr, global::System.IntPtr callbackDataPtr);

	private delegate void BridgeMTXRefreshReceiptCallback(global::System.IntPtr errorPtr, global::System.IntPtr callbackDataPtr);

	public const int ERROR_BILLING_UNAVAILABLE = 10001;

	public const int ERROR_USER_CANCELED = 10002;

	public const int ERROR_ITEM_ALREADY_PURCHASED = 10003;

	public const int ERROR_ITEM_UNAVAILABLE = 10004;

	public const int ERROR_PURCHASE_VERIFICATION_FAILED = 10005;

	public const string NOTIFICATION_REFRESH_CATALOG_FINISHED = "nimble.notification.mtx.refreshcatalogfinished";

	public const string NOTIFICATION_RESTORE_PURCHASED_TRANSACTIONS_FINISHED = "nimble.notification.mtx.restorepurchasedtransactionsfinished";

	public const string NOTIFICATION_TRANSACTIONS_RECOVERED = "nimble.notification.mtx.transactionsrecovered";

	private NimbleBridge_MTX()
	{
	}

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_MTX.BridgeMTXTransactionCallback))]
	private static void OnMTXTransactionCallback(global::System.IntPtr transactionPtr, global::System.IntPtr callbackDataPtr)
	{
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_MTXTransaction transaction = new NimbleBridge_MTXTransaction(transactionPtr);
		MTXTransactionCallback callback = (MTXTransactionCallback)nimbleBridge_CallbackHelper.GetData(callbackDataPtr);
		nimbleBridge_CallbackHelper.RunOnMainThread(delegate
		{
			callback(transaction);
		});
	}

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_MTX.BridgeMTXRefreshReceiptCallback))]
	private static void OnMTXRefreshReceiptCallback(global::System.IntPtr errorPtr, global::System.IntPtr callbackDataPtr)
	{
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_Error error = new NimbleBridge_Error(errorPtr);
		MTXRefreshReceiptCallback callback = (MTXRefreshReceiptCallback)nimbleBridge_CallbackHelper.GetData(callbackDataPtr);
		nimbleBridge_CallbackHelper.RunOnMainThread(delegate
		{
			callback(error);
		});
	}

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_MTX_deleteTransactionArray(global::System.IntPtr array);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_MTX_deleteItemArray(global::System.IntPtr array);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_MTX_purchaseItem(string sku, NimbleBridge_MTX.BridgeMTXTransactionCallback receiptCallback, global::System.IntPtr receiptCallbackData, NimbleBridge_MTX.BridgeMTXTransactionCallback purchaseCallback, global::System.IntPtr purchaseCallbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_MTX_itemGranted(string transactionId, int itemType, NimbleBridge_MTX.BridgeMTXTransactionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_MTX_finalizeTransaction(string transactionId, NimbleBridge_MTX.BridgeMTXTransactionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_MTX_restorePurchasedTransactions();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_MTX_getPurchasedTransactions();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_MTX_getPendingTransactions();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_MTX_getRecoveredTransactions();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_MTX_resumeTransaction(string transactionId, NimbleBridge_MTX.BridgeMTXTransactionCallback receiptCallback, global::System.IntPtr receiptCallbackData, NimbleBridge_MTX.BridgeMTXTransactionCallback purchaseCallback, global::System.IntPtr purchaseCallbackData, NimbleBridge_MTX.BridgeMTXTransactionCallback itemGrantedCallback, global::System.IntPtr itemGrantedCallbackData, NimbleBridge_MTX.BridgeMTXTransactionCallback finalizeCallback, global::System.IntPtr finalizeCallbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_MTX_refreshAvailableCatalogItems();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_MTX_getAvailableCatalogItems();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_MTX_setPlatformParameters(global::System.IntPtr parameters);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_MTXRefreshReceipt(NimbleBridge_MTX.BridgeMTXRefreshReceiptCallback refreshReceiptCallback, global::System.IntPtr refreshReceiptCallbackData);
#endif

	public static NimbleBridge_MTX GetComponent()
	{
		return new NimbleBridge_MTX();
	}

	public NimbleBridge_Error PurchaseItem(string sku, MTXTransactionCallback receiptCallback, MTXTransactionCallback purchaseCallback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr receiptCallbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(receiptCallback);
		global::System.IntPtr purchaseCallbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(purchaseCallback);
		return NimbleBridge_MTX_purchaseItem(sku, OnMTXTransactionCallback, receiptCallbackData, OnMTXTransactionCallback, purchaseCallbackData);
#else
		return null;
#endif
	}

	public NimbleBridge_Error ItemGranted(string transactionId, NimbleBridge_MTXCatalogItem.Type itemType, MTXTransactionCallback callback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
		return NimbleBridge_MTX_itemGranted(transactionId, (int)itemType, OnMTXTransactionCallback, callbackData);
#else
		return null;
#endif
	}

	public NimbleBridge_Error FinalizeTransaction(string transactionId, MTXTransactionCallback callback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
		return NimbleBridge_MTX_finalizeTransaction(transactionId, OnMTXTransactionCallback, callbackData);
#else
		return null;
#endif
	}

	public void RestorePurchasedTransactions()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_MTX_restorePurchasedTransactions();
#endif
	}

	public NimbleBridge_MTXTransaction[] GetPurchasedTransactions()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.Collections.Generic.List<NimbleBridge_MTXTransaction> list = new global::System.Collections.Generic.List<NimbleBridge_MTXTransaction>();
		global::System.IntPtr intPtr = NimbleBridge_MTX_getPurchasedTransactions();
		global::System.IntPtr intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr);
		int num = 1;
		while (intPtr2 != global::System.IntPtr.Zero)
		{
			list.Add(new NimbleBridge_MTXTransaction(intPtr2));
			intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr, num * global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)));
			num++;
		}
		NimbleBridge_MTX_deleteTransactionArray(intPtr);
		return list.ToArray();
#else
		return new NimbleBridge_MTXTransaction[0];
#endif
	}

	public NimbleBridge_MTXTransaction[] GetPendingTransactions()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.Collections.Generic.List<NimbleBridge_MTXTransaction> list = new global::System.Collections.Generic.List<NimbleBridge_MTXTransaction>();
		global::System.IntPtr intPtr = NimbleBridge_MTX_getPendingTransactions();
		global::System.IntPtr intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr);
		int num = 1;
		while (intPtr2 != global::System.IntPtr.Zero)
		{
			list.Add(new NimbleBridge_MTXTransaction(intPtr2));
			intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr, num * global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)));
			num++;
		}
		NimbleBridge_MTX_deleteTransactionArray(intPtr);
		return list.ToArray();
#else
		return new NimbleBridge_MTXTransaction[0];
#endif
	}

	public NimbleBridge_MTXTransaction[] GetRecoveredTransactions()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.Collections.Generic.List<NimbleBridge_MTXTransaction> list = new global::System.Collections.Generic.List<NimbleBridge_MTXTransaction>();
		global::System.IntPtr intPtr = NimbleBridge_MTX_getRecoveredTransactions();
		if (intPtr == global::System.IntPtr.Zero)
		{
			return list.ToArray();
		}
		global::System.IntPtr intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr);
		int num = 1;
		while (intPtr2 != global::System.IntPtr.Zero)
		{
			list.Add(new NimbleBridge_MTXTransaction(intPtr2));
			intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr, num * global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)));
			num++;
		}
		NimbleBridge_MTX_deleteTransactionArray(intPtr);
		return list.ToArray();
#else
		return new NimbleBridge_MTXTransaction[0];
#endif
	}

	public NimbleBridge_Error ResumeTransaction(string transactionId, MTXTransactionCallback receiptCallback, MTXTransactionCallback purchaseCallback, MTXTransactionCallback itemGrantedCallback, MTXTransactionCallback finalizeCallback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		global::System.IntPtr receiptCallbackData = nimbleBridge_CallbackHelper.MakeCallbackData(receiptCallback);
		global::System.IntPtr purchaseCallbackData = nimbleBridge_CallbackHelper.MakeCallbackData(purchaseCallback);
		global::System.IntPtr itemGrantedCallbackData = nimbleBridge_CallbackHelper.MakeCallbackData(itemGrantedCallback);
		global::System.IntPtr finalizeCallbackData = nimbleBridge_CallbackHelper.MakeCallbackData(finalizeCallback);
		return NimbleBridge_MTX_resumeTransaction(transactionId, OnMTXTransactionCallback, receiptCallbackData, OnMTXTransactionCallback, purchaseCallbackData, OnMTXTransactionCallback, itemGrantedCallbackData, OnMTXTransactionCallback, finalizeCallbackData);
#else
		return null;
#endif
	}

	public void RefreshAvailableCatalogItems()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_MTX_refreshAvailableCatalogItems();
#endif
	}

	public NimbleBridge_MTXCatalogItem[] GetAvailableCatalogItems()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.Collections.Generic.List<NimbleBridge_MTXCatalogItem> list = new global::System.Collections.Generic.List<NimbleBridge_MTXCatalogItem>();
		global::System.IntPtr intPtr = NimbleBridge_MTX_getAvailableCatalogItems();
		if (intPtr != global::System.IntPtr.Zero)
		{
			global::System.IntPtr intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr);
			int num = 1;
			while (intPtr2 != global::System.IntPtr.Zero)
			{
				list.Add(new NimbleBridge_MTXCatalogItem(intPtr2));
				intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr, num * global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)));
				num++;
			}
			NimbleBridge_MTX_deleteItemArray(intPtr);
		}
		return list.ToArray();
#else
		return new NimbleBridge_MTXCatalogItem[0];
#endif
	}

	public void SetPlatformParameters(global::System.Collections.Generic.Dictionary<string, string> parameters)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr intPtr = global::System.IntPtr.Zero;
		try
		{
			intPtr = MarshalUtility.ConvertDictionaryToPtr(parameters);
			NimbleBridge_MTX_setPlatformParameters(intPtr);
		}
		finally
		{
			if (intPtr != global::System.IntPtr.Zero)
			{
				MarshalUtility.DisposeMapPtr(intPtr);
			}
		}
#endif
	}

	public void RefreshReceipt(MTXRefreshReceiptCallback refreshReceiptCallback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		global::System.IntPtr refreshReceiptCallbackData = nimbleBridge_CallbackHelper.MakeCallbackData(refreshReceiptCallback);
		NimbleBridge_MTXRefreshReceipt(OnMTXRefreshReceiptCallback, refreshReceiptCallbackData);
#endif
	}
}
