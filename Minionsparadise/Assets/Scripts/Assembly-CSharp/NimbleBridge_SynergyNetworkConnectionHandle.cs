public class NimbleBridge_SynergyNetworkConnectionHandle : global::System.Runtime.InteropServices.SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	private NimbleBridge_SynergyNetworkConnectionHandle()
		: base(global::System.IntPtr.Zero, true)
	{
	}

	internal NimbleBridge_SynergyNetworkConnectionHandle(global::System.IntPtr handle)
		: base(global::System.IntPtr.Zero, true)
	{
		SetHandle(handle);
	}

	[global::AOT.MonoPInvokeCallback(typeof(BridgeSynergyNetworkConnectionCallback))]
	internal static void OnSynergyNetworkConnectionCallback(global::System.IntPtr handlePtr, global::System.IntPtr callbackDataPtr)
	{
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_SynergyNetworkConnectionHandle handle = new NimbleBridge_SynergyNetworkConnectionHandle(handlePtr);
		SynergyNetworkConnectionCallback callback = (SynergyNetworkConnectionCallback)nimbleBridge_CallbackHelper.GetData(callbackDataPtr);
		nimbleBridge_CallbackHelper.RunOnMainThread(delegate
		{
			callback(handle);
		});
	}

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyNetworkConnectionHandleWrapper_Dispose(NimbleBridge_SynergyNetworkConnectionHandle handleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyNetworkConnectionHandle_wait(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyNetworkConnectionHandle_cancel(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_SynergyRequest NimbleBridge_SynergyNetworkConnectionHandle_getRequest(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_SynergyResponse NimbleBridge_SynergyNetworkConnectionHandle_getResponse(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_SynergyNetworkConnectionHandle_getHeaderCallback(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyNetworkConnectionHandle_setHeaderCallback(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper, BridgeSynergyNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_SynergyNetworkConnectionHandle_getProgressCallback(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyNetworkConnectionHandle_setProgressCallback(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper, BridgeSynergyNetworkConnectionCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_SynergyNetworkConnectionHandle_getCompletionCallback(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyNetworkConnectionHandle_setCompletionCallback(NimbleBridge_SynergyNetworkConnectionHandle synergyNetworkConnectionHandleWrapper, BridgeSynergyNetworkConnectionCallback callback, global::System.IntPtr callbackData);
#endif

	protected override bool ReleaseHandle()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyNetworkConnectionHandleWrapper_Dispose(this);
#endif
		return true;
	}

	public void Wait()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyNetworkConnectionHandle_wait(this);
#endif
	}

	public void Cancel()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyNetworkConnectionHandle_cancel(this);
#endif
	}

	public NimbleBridge_SynergyRequest GetRequest()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyNetworkConnectionHandle_getRequest(this);
#else
		return null;
#endif
	}

	public NimbleBridge_SynergyResponse GetResponse()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyNetworkConnectionHandle_getResponse(this);
#else
		return null;
#endif
	}

	public SynergyNetworkConnectionCallback GetHeaderCallback()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr dataPtr = NimbleBridge_SynergyNetworkConnectionHandle_getHeaderCallback(this);
		return (SynergyNetworkConnectionCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
#else
		return null;
#endif
	}

	public void SetHeaderCallback(SynergyNetworkConnectionCallback synergyNetworkConnectionCallback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(synergyNetworkConnectionCallback);
		NimbleBridge_SynergyNetworkConnectionHandle_setHeaderCallback(this, OnSynergyNetworkConnectionCallback, callbackData);
#endif
	}

	public SynergyNetworkConnectionCallback GetProgressCallback()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr dataPtr = NimbleBridge_SynergyNetworkConnectionHandle_getProgressCallback(this);
		return (SynergyNetworkConnectionCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
#else
		return null;
#endif
	}

	public void SetProgressCallback(SynergyNetworkConnectionCallback synergyNetworkConnectionCallback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(synergyNetworkConnectionCallback);
		NimbleBridge_SynergyNetworkConnectionHandle_setProgressCallback(this, OnSynergyNetworkConnectionCallback, callbackData);
#endif
	}

	public SynergyNetworkConnectionCallback GetCompletionCallback()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr dataPtr = NimbleBridge_SynergyNetworkConnectionHandle_getCompletionCallback(this);
		return (SynergyNetworkConnectionCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
#else
		return null;
#endif
	}

	public void SetCompletionCallback(SynergyNetworkConnectionCallback synergyNetworkConnectionCallback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(synergyNetworkConnectionCallback);
		NimbleBridge_SynergyNetworkConnectionHandle_setCompletionCallback(this, OnSynergyNetworkConnectionCallback, callbackData);
#endif
	}
}
