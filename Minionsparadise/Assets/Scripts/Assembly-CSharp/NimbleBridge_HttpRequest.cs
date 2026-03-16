public class NimbleBridge_HttpRequest : global::System.Runtime.InteropServices.SafeHandle
{
	public enum Method
	{
		HTTP_GET = 0,
		HTTP_HEAD = 1,
		HTTP_POST = 2,
		HTTP_PUT = 3
	}

	public enum DownloadOverwritePolicy
	{
		OVERWRITE = 0,
		RESUME_DOWNLOAD = 1,
		DATE_CHECK = 2,
		LENGTH_CHECK = 4,
		SMART = 15
	}

	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	internal NimbleBridge_HttpRequest()
		: base(global::System.IntPtr.Zero, true)
	{
	}

#if UNITY_IOS || UNITY_ANDROID
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_HttpRequest_Dispose(NimbleBridge_HttpRequest httpRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_HttpRequest NimbleBridge_HttpRequest_requestWithUrl(string url);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_HttpRequest_getUrl(NimbleBridge_HttpRequest httpRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_HttpRequest_getMethod(NimbleBridge_HttpRequest httpRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_HttpRequest_setMethod(NimbleBridge_HttpRequest httpRequestWrapper, int method);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_HttpRequest_getData(NimbleBridge_HttpRequest httpRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_HttpRequest_setData(NimbleBridge_HttpRequest httpRequestWrapper, global::System.IntPtr dataWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_HttpRequest_getHeaders(NimbleBridge_HttpRequest httpRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_HttpRequest_setHeaders(NimbleBridge_HttpRequest httpRequestWrapper, global::System.IntPtr map);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern double NimbleBridge_HttpRequest_getTimeout(NimbleBridge_HttpRequest httpRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_HttpRequest_setTimeout(NimbleBridge_HttpRequest httpRequestWrapper, double timeout);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_HttpRequest_getTargetFilePath(NimbleBridge_HttpRequest httpRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_HttpRequest_setTargetFilePath(NimbleBridge_HttpRequest httpRequestWrapper, string targetFilePath);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_HttpRequest_getOverwritePolicy(NimbleBridge_HttpRequest httpRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_HttpRequest_setOverwritePolicy(NimbleBridge_HttpRequest httpRequestWrapper, int policy);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_HttpRequest_getRunInBackground(NimbleBridge_HttpRequest httpRequestWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_HttpRequest_setRunInBackground(NimbleBridge_HttpRequest httpRequestWrapper, bool runInBackground);
#endif

	protected override bool ReleaseHandle()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_HttpRequest_Dispose(this);
#endif
		return true;
	}

	public static NimbleBridge_HttpRequest RequestWithUrl(string url)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpRequest_requestWithUrl(url);
#else
		return null;
#endif
	}

	public string GetUrl()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpRequest_getUrl(this);
#else
		return string.Empty;
#endif
	}

	public NimbleBridge_HttpRequest.Method GetMethod()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return (NimbleBridge_HttpRequest.Method)NimbleBridge_HttpRequest_getMethod(this);
#else
		return Method.HTTP_GET;
#endif
	}

	public void SetMethod(NimbleBridge_HttpRequest.Method method)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_HttpRequest_setMethod(this, (int)method);
#endif
	}

	public byte[] GetData()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr dataPtr = NimbleBridge_HttpRequest_getData(this);
		return MarshalUtility.ConvertPtrToData(dataPtr);
#else
		return new byte[0];
#endif
	}

	public void SetData(byte[] data)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr intPtr = global::System.IntPtr.Zero;
		try
		{
			intPtr = MarshalUtility.ConvertDataToPtr(data);
			NimbleBridge_HttpRequest_setData(this, intPtr);
		}
		finally
		{
			if (intPtr != global::System.IntPtr.Zero)
			{
				MarshalUtility.DisposeDataPtr(intPtr);
			}
		}
#endif
	}

	public global::System.Collections.Generic.Dictionary<string, string> GetHeaders()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr mapPtr = NimbleBridge_HttpRequest_getHeaders(this);
		return MarshalUtility.ConvertPtrToDictionary(mapPtr);
#else
		return new global::System.Collections.Generic.Dictionary<string, string>();
#endif
	}

	public void SetHeaders(global::System.Collections.Generic.Dictionary<string, string> dictionary)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		global::System.IntPtr intPtr = global::System.IntPtr.Zero;
		try
		{
			intPtr = MarshalUtility.ConvertDictionaryToPtr(dictionary);
			NimbleBridge_HttpRequest_setHeaders(this, intPtr);
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

	public double GetTimeout()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpRequest_getTimeout(this);
#else
		return 30.0;
#endif
	}

	public void SetTimeout(double timeout)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_HttpRequest_setTimeout(this, timeout);
#endif
	}

	public string GetTargetFilePath()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpRequest_getTargetFilePath(this);
#else
		return string.Empty;
#endif
	}

	public void SetTargetFilePath(string targetFilePath)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_HttpRequest_setTargetFilePath(this, targetFilePath);
#endif
	}

	public int GetOverwritePolicy()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpRequest_getOverwritePolicy(this);
#else
		return 0;
#endif
	}

	public void SetOverwritePolicy(int policy)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_HttpRequest_setOverwritePolicy(this, policy);
#endif
	}

	public bool GetRunInBackground()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_HttpRequest_getRunInBackground(this);
#else
		return false;
#endif
	}

	public void SetRunInBackground(bool runInBackground)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_HttpRequest_setRunInBackground(this, runInBackground);
#endif
	}
}
