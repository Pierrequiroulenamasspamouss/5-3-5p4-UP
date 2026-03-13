public class NimbleBridge_Network
{
	public enum NimbleBridge_Network_Status
	{
		STATUS_UNKNOWN = 0,
		STATUS_NONE = 1,
		STATUS_DEAD = 2,
		STATUS_OK = 3
	}

	private NimbleBridge_Network()
	{
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_NetworkConnectionHandle NimbleBridge_Network_sendGetRequest(string url, BridgeNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_NetworkConnectionHandle NimbleBridge_Network_sendPostRequest(string url, global::System.IntPtr data, BridgeNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_NetworkConnectionHandle NimbleBridge_Network_sendRequest(NimbleBridge_HttpRequest request, BridgeNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Network_forceRedetectNetworkStatus();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_Network_getNetworkStatus();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_Network_isNetworkWifi();

	public static NimbleBridge_Network GetComponent()
	{
		return new NimbleBridge_Network();
	}

	public NimbleBridge_NetworkConnectionHandle SendGetRequest(string url, NetworkConnectionCallback callback)
	{
#if !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
		return NimbleBridge_Network_sendGetRequest(url, NimbleBridge_NetworkConnectionHandle.OnNetworkConnectionCallback, callbackData);
#else
		return null;
#endif
	}

	public NimbleBridge_NetworkConnectionHandle SendPostRequest(string url, byte[] data, NetworkConnectionCallback callback)
	{
#if !UNITY_EDITOR
		global::System.IntPtr intPtr = global::System.IntPtr.Zero;
		try
		{
			intPtr = MarshalUtility.ConvertDataToPtr(data);
			global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
			return NimbleBridge_Network_sendPostRequest(url, intPtr, NimbleBridge_NetworkConnectionHandle.OnNetworkConnectionCallback, callbackData);
		}
		finally
		{
			if (intPtr != global::System.IntPtr.Zero)
			{
				MarshalUtility.DisposeDataPtr(intPtr);
			}
		}
#else
		return null;
#endif
	}

	public NimbleBridge_NetworkConnectionHandle SendRequest(NimbleBridge_HttpRequest request, NetworkConnectionCallback callback)
	{
#if !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
		return NimbleBridge_Network_sendRequest(request, NimbleBridge_NetworkConnectionHandle.OnNetworkConnectionCallback, callbackData);
#else
		return null;
#endif
	}

	public void ForceRedetectNetworkStatus()
	{
#if !UNITY_EDITOR
		NimbleBridge_Network_forceRedetectNetworkStatus();
#endif
	}

	public NimbleBridge_Network.NimbleBridge_Network_Status GetNetworkStatus()
	{
#if !UNITY_EDITOR
		return (NimbleBridge_Network.NimbleBridge_Network_Status)NimbleBridge_Network_getNetworkStatus();
#else
		return NimbleBridge_Network_Status.STATUS_OK;
#endif
	}

	public bool IsNetworkWifi()
	{
#if !UNITY_EDITOR
		return NimbleBridge_Network_isNetworkWifi();
#else
		return true;
#endif
	}
}
