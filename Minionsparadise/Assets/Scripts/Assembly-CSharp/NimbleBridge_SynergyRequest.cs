public class NimbleBridge_SynergyRequest : global::System.Runtime.InteropServices.SafeHandle
{
	private delegate void BridgeSynergyRequestPreparingCallback(global::System.IntPtr requestPtr, global::System.IntPtr callbackDataPtr);

	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	private NimbleBridge_SynergyRequest()
		: base(global::System.IntPtr.Zero, true)
	{
	}

	internal NimbleBridge_SynergyRequest(global::System.IntPtr handle)
		: base(global::System.IntPtr.Zero, true)
	{
		SetHandle(handle);
	}

	public NimbleBridge_SynergyRequest(string api, NimbleBridge_HttpRequest.Method method, SynergyRequestPreparingCallback callback)
		: this()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
		global::System.IntPtr intPtr = NimbleBridge_SynergyRequest_SynergyRequest(api, (int)method, OnSynergyRequestPreparingCallback, callbackData);
		SetHandle(intPtr);
#endif
	}

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_SynergyRequest.BridgeSynergyRequestPreparingCallback))]
	private static void OnSynergyRequestPreparingCallback(global::System.IntPtr requestPtr, global::System.IntPtr callbackDataPtr)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_SynergyRequest request = new NimbleBridge_SynergyRequest(requestPtr);
		SynergyRequestPreparingCallback callback = (SynergyRequestPreparingCallback)nimbleBridge_CallbackHelper.GetData(callbackDataPtr);
		nimbleBridge_CallbackHelper.RunOnMainThread(delegate
		{
			callback(request);
		});
#endif
	}

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyRequest_Dispose(NimbleBridge_SynergyRequest synergyRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_SynergyRequest_SynergyRequest(string api, int method, NimbleBridge_SynergyRequest.BridgeSynergyRequestPreparingCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_HttpRequest NimbleBridge_SynergyRequest_getHttpRequest(NimbleBridge_SynergyRequest synergyRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyRequest_getBaseUrl(NimbleBridge_SynergyRequest synergyRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyRequest_getApi(NimbleBridge_SynergyRequest synergyRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_SynergyRequest_getUrlParameters(NimbleBridge_SynergyRequest synergyRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyRequest_getJsonData(NimbleBridge_SynergyRequest synergyRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyRequest_setHttpRequest(NimbleBridge_SynergyRequest synergyRequestWrapper, NimbleBridge_HttpRequest requestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyRequest_setBaseUrl(NimbleBridge_SynergyRequest synergyRequestWrapper, string url);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyRequest_setApi(NimbleBridge_SynergyRequest synergyRequestWrapper, string api);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyRequest_setUrlParameters(NimbleBridge_SynergyRequest synergyRequestWrapper, global::System.IntPtr parameters);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyRequest_setJsonData(NimbleBridge_SynergyRequest synergyRequestWrapper, string jsonData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_SynergyRequest_getMethod(NimbleBridge_SynergyRequest synergyRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyRequest_setMethod(NimbleBridge_SynergyRequest synergyRequestWrapper, int method);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_SynergyRequest_getPrepareRequestCallback(NimbleBridge_SynergyRequest synergyRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyRequest_setPrepareRequestCallback(NimbleBridge_SynergyRequest synergyRequestWrapper, NimbleBridge_SynergyRequest.BridgeSynergyRequestPreparingCallback callback, global::System.IntPtr callbackData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyRequest_send(NimbleBridge_SynergyRequest synergyRequestWrapper);
#endif

	protected override bool ReleaseHandle()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyRequest_Dispose(this);
#endif
		return true;
	}

	public NimbleBridge_HttpRequest GetHttpRequest()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyRequest_getHttpRequest(this);
#else
		return null;
#endif
	}

	public string GetBaseUrl()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyRequest_getBaseUrl(this);
#else
		return string.Empty;
#endif
	}

	public string GetApi()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyRequest_getApi(this);
#else
		return string.Empty;
#endif
	}

	public global::System.Collections.Generic.Dictionary<string, string> GetUrlParameters()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr mapPtr = NimbleBridge_SynergyRequest_getUrlParameters(this);
		return MarshalUtility.ConvertPtrToDictionary(mapPtr);
#else
		return new global::System.Collections.Generic.Dictionary<string, string>();
#endif
	}

	public string GetJsonData()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyRequest_getJsonData(this);
#else
		return string.Empty;
#endif
	}

	public void SetHttpRequest(NimbleBridge_HttpRequest request)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyRequest_setHttpRequest(this, request);
#endif
	}

	public void SetBaseUrl(string url)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyRequest_setBaseUrl(this, url);
#endif
	}

	public void SetApi(string api)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyRequest_setApi(this, api);
#endif
	}

	public void SetUrlParameters(global::System.Collections.Generic.Dictionary<string, string> parameters)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr intPtr = global::System.IntPtr.Zero;
		try
		{
			intPtr = MarshalUtility.ConvertDictionaryToPtr(parameters);
			NimbleBridge_SynergyRequest_setUrlParameters(this, intPtr);
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

	public void SetJsonData(string jsonData)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyRequest_setJsonData(this, jsonData);
#endif
	}

	public NimbleBridge_HttpRequest.Method GetMethod()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return (NimbleBridge_HttpRequest.Method)NimbleBridge_SynergyRequest_getMethod(this);
#else
		return NimbleBridge_HttpRequest.Method.HTTP_GET;
#endif
	}

	public void SetMethod(NimbleBridge_HttpRequest.Method method)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyRequest_setMethod(this, (int)method);
#endif
	}

	public SynergyRequestPreparingCallback GetPrepareRequestCallback()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr dataPtr = NimbleBridge_SynergyRequest_getPrepareRequestCallback(this);
		return (SynergyRequestPreparingCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
#else
		return null;
#endif
	}

	public void SetPrepareRequestCallback(SynergyRequestPreparingCallback callback)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
		NimbleBridge_SynergyRequest_setPrepareRequestCallback(this, OnSynergyRequestPreparingCallback, callbackData);
#endif
	}

	public void Send()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyRequest_send(this);
#endif
	}
}
