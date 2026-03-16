public class NimbleBridge_HttpResponse : global::System.Runtime.InteropServices.SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	internal NimbleBridge_HttpResponse()
		: base(global::System.IntPtr.Zero, true)
	{
	}

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_HttpResponse_Dispose(NimbleBridge_HttpResponse httpResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_HttpResponse_isCompleted(NimbleBridge_HttpResponse httpResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_HttpResponse_getUrl(NimbleBridge_HttpResponse httpResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_HttpResponse_getStatusCode(NimbleBridge_HttpResponse httpResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_HttpResponse_getHeaders(NimbleBridge_HttpResponse httpResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern long NimbleBridge_HttpResponse_getExpectedContentLength(NimbleBridge_HttpResponse httpResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern long NimbleBridge_HttpResponse_getDownloadedContentLength(NimbleBridge_HttpResponse httpResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern double NimbleBridge_HttpResponse_getLastModified(NimbleBridge_HttpResponse httpResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_HttpResponse_getData(NimbleBridge_HttpResponse httpResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_HttpResponse_getError(NimbleBridge_HttpResponse httpResponseWrapper);
#endif

	protected override bool ReleaseHandle()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_HttpResponse_Dispose(this);
#endif
		return true;
	}

	public bool IsCompleted()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpResponse_isCompleted(this);
#else
		return true;
#endif
	}

	public string GetUrl()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpResponse_getUrl(this);
#else
		return string.Empty;
#endif
	}

	public int GetStatusCode()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpResponse_getStatusCode(this);
#else
		return 200;
#endif
	}

	public global::System.Collections.Generic.Dictionary<string, string> GetHeaders()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr mapPtr = NimbleBridge_HttpResponse_getHeaders(this);
		return MarshalUtility.ConvertPtrToDictionary(mapPtr);
#else
		return new global::System.Collections.Generic.Dictionary<string, string>();
#endif
	}

	public long GetExpectedContentLength()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpResponse_getExpectedContentLength(this);
#else
		return 0L;
#endif
	}

	public long GetDownloadedContentLength()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpResponse_getDownloadedContentLength(this);
#else
		return 0L;
#endif
	}

	public double GetLastModified()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpResponse_getLastModified(this);
#else
		return 0.0;
#endif
	}

	public byte[] GetData()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr dataPtr = NimbleBridge_HttpResponse_getData(this);
		return MarshalUtility.ConvertPtrToData(dataPtr);
#else
		return new byte[0];
#endif
	}

	public NimbleBridge_Error GetError()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpResponse_getError(this);
#else
		return null;
#endif
	}
}
