public class NimbleBridge_MTXTransaction : global::System.Runtime.InteropServices.SafeHandle
{
	public enum Type
	{
		PURCHASE = 0,
		RESTORE = 1
	}

	public enum State
	{
		UNDEFINED = 0,
		USER_INITIATED = 1,
		WAITING_FOR_PREPURCHASE_INFO = 2,
		WAITING_FOR_PLATFORM_RESPONSE = 3,
		WAITING_FOR_VERIFICATION = 4,
		WAITING_FOR_GAME_TO_CONFIRM_ITEM_GRANT = 5,
		WAITING_FOR_PLATFORM_CONSUMPTION = 6,
		COMPLETE = 7
	}

	public const string NIMBLE_MTX_IOS6_FORMAT_RECEIPT_KEY = "iOS6Receipt";

	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	private NimbleBridge_MTXTransaction()
		: base(global::System.IntPtr.Zero, true)
	{
	}

	internal NimbleBridge_MTXTransaction(global::System.IntPtr handle)
		: base(global::System.IntPtr.Zero, true)
	{
		SetHandle(handle);
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_MTXTransaction_Dispose(NimbleBridge_MTXTransaction wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXTransaction_getTransactionId(NimbleBridge_MTXTransaction wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXTransaction_getItemSku(NimbleBridge_MTXTransaction wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_MTXTransaction_getState(NimbleBridge_MTXTransaction wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_MTXTransaction_getType(NimbleBridge_MTXTransaction wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern float NimbleBridge_MTXTransaction_getPriceDecimal(NimbleBridge_MTXTransaction wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern double NimbleBridge_MTXTransaction_getTimestamp(NimbleBridge_MTXTransaction wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXTransaction_getReceipt(NimbleBridge_MTXTransaction wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXTransaction_getAdditionalInfo(NimbleBridge_MTXTransaction wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_MTXTransaction_getError(NimbleBridge_MTXTransaction wrapper);

	protected override bool ReleaseHandle()
	{
		NimbleBridge_MTXTransaction_Dispose(this);
		return true;
	}

	public string GetTransactionId()
	{
		return NimbleBridge_MTXTransaction_getTransactionId(this);
	}

	public string GetItemSku()
	{
		return NimbleBridge_MTXTransaction_getItemSku(this);
	}

	public NimbleBridge_MTXTransaction.State GetTransactionState()
	{
		return (NimbleBridge_MTXTransaction.State)NimbleBridge_MTXTransaction_getState(this);
	}

	public NimbleBridge_MTXTransaction.Type GetTransactionType()
	{
		return (NimbleBridge_MTXTransaction.Type)NimbleBridge_MTXTransaction_getType(this);
	}

	public float GetPriceDecimal()
	{
		return NimbleBridge_MTXTransaction_getPriceDecimal(this);
	}

	public double GetTimestamp()
	{
		return NimbleBridge_MTXTransaction_getTimestamp(this);
	}

	public string GetReceipt()
	{
		return NimbleBridge_MTXTransaction_getReceipt(this);
	}

	[global::System.Obsolete("Use GetAdditionalInfoDictionary instead")]
	public string GetAdditionalInfo()
	{
		return NimbleBridge_MTXTransaction_getAdditionalInfo(this);
	}

	public global::System.Collections.Generic.Dictionary<string, object> GetAdditionalInfoDictionary()
	{
		global::SimpleJSON.JSONNode jSONNode = global::SimpleJSON.JSON.Parse(NimbleBridge_MTXTransaction_getAdditionalInfo(this));
		return MarshalUtility.ConvertJsonToDictionary((global::SimpleJSON.JSONClass)jSONNode);
	}

	public NimbleBridge_Error GetError()
	{
		return NimbleBridge_MTXTransaction_getError(this);
	}
}
