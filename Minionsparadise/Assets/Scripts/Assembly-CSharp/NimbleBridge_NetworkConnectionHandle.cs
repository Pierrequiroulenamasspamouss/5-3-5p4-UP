public class NimbleBridge_NetworkConnectionHandle : global::System.Runtime.InteropServices.SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	private NimbleBridge_NetworkConnectionHandle()
		: base(global::System.IntPtr.Zero, true)
	{
	}

	internal NimbleBridge_NetworkConnectionHandle(global::System.IntPtr handle)
		: base(global::System.IntPtr.Zero, true)
	{
		SetHandle(handle);
	}

	[global::AOT.MonoPInvokeCallback(typeof(BridgeNetworkConnectionCallback))]
	internal static void OnNetworkConnectionCallback(global::System.IntPtr handlePtr, global::System.IntPtr callbackDataPtr)
	{
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_NetworkConnectionHandle handle = new NimbleBridge_NetworkConnectionHandle(handlePtr);
		NetworkConnectionCallback callback = (NetworkConnectionCallback)nimbleBridge_CallbackHelper.GetData(callbackDataPtr);
		nimbleBridge_CallbackHelper.RunOnMainThread(delegate
		{
			callback(handle);
		});
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_NetworkConnectionHandleWrapper_Dispose(NimbleBridge_NetworkConnectionHandle handleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_NetworkConnectionHandle_wait(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_NetworkConnectionHandle_cancel(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_HttpRequest NimbleBridge_NetworkConnectionHandle_getRequest(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_HttpResponse NimbleBridge_NetworkConnectionHandle_getResponse(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_NetworkConnectionHandle_getHeaderCallback(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_NetworkConnectionHandle_setHeaderCallback(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper, BridgeNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_NetworkConnectionHandle_getProgressCallback(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_NetworkConnectionHandle_setProgressCallback(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper, BridgeNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_NetworkConnectionHandle_getCompletionCallback(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_NetworkConnectionHandle_setCompletionCallback(NimbleBridge_NetworkConnectionHandle networkConnectionHandleWrapper, BridgeNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	protected override bool ReleaseHandle()
	{
		NimbleBridge_NetworkConnectionHandleWrapper_Dispose(this);
		return true;
	}

	public void Wait()
	{
		NimbleBridge_NetworkConnectionHandle_wait(this);
	}

	public void Cancel()
	{
		NimbleBridge_NetworkConnectionHandle_cancel(this);
	}

	public NimbleBridge_HttpRequest GetRequest()
	{
		return NimbleBridge_NetworkConnectionHandle_getRequest(this);
	}

	public NimbleBridge_HttpResponse GetResponse()
	{
		return NimbleBridge_NetworkConnectionHandle_getResponse(this);
	}

	public NetworkConnectionCallback GetHeaderCallback()
	{
		global::System.IntPtr dataPtr = NimbleBridge_NetworkConnectionHandle_getHeaderCallback(this);
		return (NetworkConnectionCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
	}

	public void SetHeaderCallback(NetworkConnectionCallback networkConnectionCallback)
	{
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(networkConnectionCallback);
		NimbleBridge_NetworkConnectionHandle_setHeaderCallback(this, OnNetworkConnectionCallback, callbackData);
	}

	public NetworkConnectionCallback GetProgressCallback()
	{
		global::System.IntPtr dataPtr = NimbleBridge_NetworkConnectionHandle_getProgressCallback(this);
		return (NetworkConnectionCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
	}

	public void SetProgressCallback(NetworkConnectionCallback networkConnectionCallback)
	{
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(networkConnectionCallback);
		NimbleBridge_NetworkConnectionHandle_setProgressCallback(this, OnNetworkConnectionCallback, callbackData);
	}

	public NetworkConnectionCallback GetCompletionCallback()
	{
		global::System.IntPtr dataPtr = NimbleBridge_NetworkConnectionHandle_getCompletionCallback(this);
		return (NetworkConnectionCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
	}

	public void SetCompletionCallback(NetworkConnectionCallback networkConnectionCallback)
	{
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(networkConnectionCallback);
		NimbleBridge_NetworkConnectionHandle_setCompletionCallback(this, OnNetworkConnectionCallback, callbackData);
	}
}
