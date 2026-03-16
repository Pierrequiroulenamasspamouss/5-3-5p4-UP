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

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
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
#endif

	protected override bool ReleaseHandle()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_NetworkConnectionHandleWrapper_Dispose(this);
#endif
		return true;
	}

	public void Wait()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_NetworkConnectionHandle_wait(this);
#endif
	}

	public void Cancel()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_NetworkConnectionHandle_cancel(this);
#endif
	}

	public NimbleBridge_HttpRequest GetRequest()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_NetworkConnectionHandle_getRequest(this);
#else
		return null;
#endif
	}

	public NimbleBridge_HttpResponse GetResponse()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_NetworkConnectionHandle_getResponse(this);
#else
		return null;
#endif
	}

	public NetworkConnectionCallback GetHeaderCallback()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr dataPtr = NimbleBridge_NetworkConnectionHandle_getHeaderCallback(this);
		return (NetworkConnectionCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
#else
		return null;
#endif
	}

	public void SetHeaderCallback(NetworkConnectionCallback networkConnectionCallback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(networkConnectionCallback);
		NimbleBridge_NetworkConnectionHandle_setHeaderCallback(this, OnNetworkConnectionCallback, callbackData);
#endif
	}

	public NetworkConnectionCallback GetProgressCallback()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr dataPtr = NimbleBridge_NetworkConnectionHandle_getProgressCallback(this);
		return (NetworkConnectionCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
#else
		return null;
#endif
	}

	public void SetProgressCallback(NetworkConnectionCallback networkConnectionCallback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(networkConnectionCallback);
		NimbleBridge_NetworkConnectionHandle_setProgressCallback(this, OnNetworkConnectionCallback, callbackData);
#endif
	}

	public NetworkConnectionCallback GetCompletionCallback()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr dataPtr = NimbleBridge_NetworkConnectionHandle_getCompletionCallback(this);
		return (NetworkConnectionCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
#else
		return null;
#endif
	}

	public void SetCompletionCallback(NetworkConnectionCallback networkConnectionCallback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(networkConnectionCallback);
		NimbleBridge_NetworkConnectionHandle_setCompletionCallback(this, OnNetworkConnectionCallback, callbackData);
#endif
	}
}
