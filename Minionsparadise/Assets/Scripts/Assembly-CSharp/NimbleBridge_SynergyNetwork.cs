public class NimbleBridge_SynergyNetwork
{
	public enum NimbleBridge_SynergyNetwork_Status
	{
		STATUS_UNKNOWN = 0,
		STATUS_NONE = 1,
		STATUS_DEAD = 2,
		STATUS_OK = 3
	}

	private NimbleBridge_SynergyNetwork()
	{
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_SynergyNetworkConnectionHandle NimbleBridge_SynergyNetwork_sendGetRequest(string baseUrl, string api, global::System.IntPtr urlParameters, BridgeSynergyNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_SynergyNetworkConnectionHandle NimbleBridge_SynergyNetwork_sendPostRequest(string baseUrl, string api, global::System.IntPtr urlParameters, string jsonPostBody, BridgeSynergyNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_SynergyNetwork_sendPostRequest_withHeaders(string baseUrl, string api, global::System.IntPtr additionalHeaders, global::System.IntPtr urlParameters, string jsonPostBody, BridgeSynergyNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyNetwork_sendRequest(NimbleBridge_SynergyRequest request, BridgeSynergyNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	public static NimbleBridge_SynergyNetwork GetComponent()
	{
		return new NimbleBridge_SynergyNetwork();
	}

	public NimbleBridge_SynergyNetworkConnectionHandle SendGetRequest(string baseUrl, string api, global::System.Collections.Generic.Dictionary<string, string> urlParameters, SynergyNetworkConnectionCallback callback)
	{
		global::System.IntPtr intPtr = global::System.IntPtr.Zero;
		try
		{
			intPtr = MarshalUtility.ConvertDictionaryToPtr(urlParameters);
			global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
			return NimbleBridge_SynergyNetwork_sendGetRequest(baseUrl, api, intPtr, NimbleBridge_SynergyNetworkConnectionHandle.OnSynergyNetworkConnectionCallback, callbackData);
		}
		finally
		{
			if (intPtr != global::System.IntPtr.Zero)
			{
				MarshalUtility.DisposeMapPtr(intPtr);
			}
		}
	}

	public NimbleBridge_SynergyNetworkConnectionHandle SendPostRequest(string baseUrl, string api, global::System.Collections.Generic.Dictionary<string, string> urlParameters, string jsonPostBody, SynergyNetworkConnectionCallback callback)
	{
		global::System.IntPtr intPtr = global::System.IntPtr.Zero;
		try
		{
			intPtr = MarshalUtility.ConvertDictionaryToPtr(urlParameters);
			global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
			return NimbleBridge_SynergyNetwork_sendPostRequest(baseUrl, api, intPtr, jsonPostBody, NimbleBridge_SynergyNetworkConnectionHandle.OnSynergyNetworkConnectionCallback, callbackData);
		}
		finally
		{
			if (intPtr != global::System.IntPtr.Zero)
			{
				MarshalUtility.DisposeMapPtr(intPtr);
			}
		}
	}

	public void SendRequest(NimbleBridge_SynergyRequest request, SynergyNetworkConnectionCallback callback)
	{
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
		NimbleBridge_SynergyNetwork_sendRequest(request, NimbleBridge_SynergyNetworkConnectionHandle.OnSynergyNetworkConnectionCallback, callbackData);
	}
}
