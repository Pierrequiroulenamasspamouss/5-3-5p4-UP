public class NimbleBridge_MTXCatalogItem : global::System.Runtime.InteropServices.SafeHandle
{
	public enum Type
	{
		UNKNOWN = 0,
		NONCONSUMABLE = 1,
		CONSUMABLE = 2
	}

	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	private NimbleBridge_MTXCatalogItem()
		: base(global::System.IntPtr.Zero, true)
	{
	}

	internal NimbleBridge_MTXCatalogItem(global::System.IntPtr handle)
		: base(global::System.IntPtr.Zero, true)
	{
		SetHandle(handle);
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_MTXCatalogItem_Dispose(NimbleBridge_MTXCatalogItem wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXCatalogItem_getSku(NimbleBridge_MTXCatalogItem wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXCatalogItem_getTitle(NimbleBridge_MTXCatalogItem wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXCatalogItem_getDescription(NimbleBridge_MTXCatalogItem wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern float NimbleBridge_MTXCatalogItem_getPriceDecimal(NimbleBridge_MTXCatalogItem wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXCatalogItem_getPriceWithCurrencyAndFormat(NimbleBridge_MTXCatalogItem wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_MTXCatalogItem_getItemType(NimbleBridge_MTXCatalogItem wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXCatalogItem_getMetaDataUrl(NimbleBridge_MTXCatalogItem wrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_MTXCatalogItem_getAdditionalInfo(NimbleBridge_MTXCatalogItem wrapper);

	protected override bool ReleaseHandle()
	{
		NimbleBridge_MTXCatalogItem_Dispose(this);
		return true;
	}

	public string GetSku()
	{
		return NimbleBridge_MTXCatalogItem_getSku(this);
	}

	public string GetTitle()
	{
		return NimbleBridge_MTXCatalogItem_getTitle(this);
	}

	public string GetDescription()
	{
		return NimbleBridge_MTXCatalogItem_getDescription(this);
	}

	public float GetPriceDecimal()
	{
		return NimbleBridge_MTXCatalogItem_getPriceDecimal(this);
	}

	public string GetPriceWithCurrencyAndFormat()
	{
		return NimbleBridge_MTXCatalogItem_getPriceWithCurrencyAndFormat(this);
	}

	public NimbleBridge_MTXCatalogItem.Type GetItemType()
	{
		return (NimbleBridge_MTXCatalogItem.Type)NimbleBridge_MTXCatalogItem_getItemType(this);
	}

	public string GetMetaDataUrl()
	{
		return NimbleBridge_MTXCatalogItem_getMetaDataUrl(this);
	}

	[global::System.Obsolete("Use GetAdditionalInfoDictionary instead")]
	public string GetAdditionalInfo()
	{
		return NimbleBridge_MTXCatalogItem_getAdditionalInfo(this);
	}

	public global::System.Collections.Generic.Dictionary<string, object> GetAdditionalInfoDictionary()
	{
		global::SimpleJSON.JSONNode jSONNode = global::SimpleJSON.JSON.Parse(NimbleBridge_MTXCatalogItem_getAdditionalInfo(this));
		return MarshalUtility.ConvertJsonToDictionary((global::SimpleJSON.JSONClass)jSONNode);
	}
}
