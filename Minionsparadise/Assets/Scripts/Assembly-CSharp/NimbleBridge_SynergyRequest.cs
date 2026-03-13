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
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
		global::System.IntPtr intPtr = NimbleBridge_SynergyRequest_SynergyRequest(api, (int)method, OnSynergyRequestPreparingCallback, callbackData);
		SetHandle(intPtr);
	}

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_SynergyRequest.BridgeSynergyRequestPreparingCallback))]
	private static void OnSynergyRequestPreparingCallback(global::System.IntPtr requestPtr, global::System.IntPtr callbackDataPtr)
	{
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_SynergyRequest request = new NimbleBridge_SynergyRequest(requestPtr);
		SynergyRequestPreparingCallback callback = (SynergyRequestPreparingCallback)nimbleBridge_CallbackHelper.GetData(callbackDataPtr);
		nimbleBridge_CallbackHelper.RunOnMainThread(delegate
		{
			callback(request);
		});
	}

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

	protected override bool ReleaseHandle()
	{
		NimbleBridge_SynergyRequest_Dispose(this);
		return true;
	}

	public NimbleBridge_HttpRequest GetHttpRequest()
	{
		return NimbleBridge_SynergyRequest_getHttpRequest(this);
	}

	public string GetBaseUrl()
	{
		return NimbleBridge_SynergyRequest_getBaseUrl(this);
	}

	public string GetApi()
	{
		return NimbleBridge_SynergyRequest_getApi(this);
	}

	public global::System.Collections.Generic.Dictionary<string, string> GetUrlParameters()
	{
		global::System.IntPtr mapPtr = NimbleBridge_SynergyRequest_getUrlParameters(this);
		return MarshalUtility.ConvertPtrToDictionary(mapPtr);
	}

	public string GetJsonData()
	{
		return NimbleBridge_SynergyRequest_getJsonData(this);
	}

	public void SetHttpRequest(NimbleBridge_HttpRequest request)
	{
		NimbleBridge_SynergyRequest_setHttpRequest(this, request);
	}

	public void SetBaseUrl(string url)
	{
		NimbleBridge_SynergyRequest_setBaseUrl(this, url);
	}

	public void SetApi(string api)
	{
		NimbleBridge_SynergyRequest_setApi(this, api);
	}

	public void SetUrlParameters(global::System.Collections.Generic.Dictionary<string, string> parameters)
	{
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
	}

	public void SetJsonData(string jsonData)
	{
		NimbleBridge_SynergyRequest_setJsonData(this, jsonData);
	}

	public NimbleBridge_HttpRequest.Method GetMethod()
	{
		return (NimbleBridge_HttpRequest.Method)NimbleBridge_SynergyRequest_getMethod(this);
	}

	public void SetMethod(NimbleBridge_HttpRequest.Method method)
	{
		NimbleBridge_SynergyRequest_setMethod(this, (int)method);
	}

	public SynergyRequestPreparingCallback GetPrepareRequestCallback()
	{
		global::System.IntPtr dataPtr = NimbleBridge_SynergyRequest_getPrepareRequestCallback(this);
		return (SynergyRequestPreparingCallback)NimbleBridge_CallbackHelper.Get().GetData(dataPtr);
	}

	public void SetPrepareRequestCallback(SynergyRequestPreparingCallback callback)
	{
		global::System.IntPtr callbackData = NimbleBridge_CallbackHelper.Get().MakeCallbackData(callback);
		NimbleBridge_SynergyRequest_setPrepareRequestCallback(this, OnSynergyRequestPreparingCallback, callbackData);
	}

	public void Send()
	{
		NimbleBridge_SynergyRequest_send(this);
	}
}
