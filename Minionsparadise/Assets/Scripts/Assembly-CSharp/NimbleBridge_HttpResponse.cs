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

	protected override bool ReleaseHandle()
	{
		NimbleBridge_HttpResponse_Dispose(this);
		return true;
	}

	public bool IsCompleted()
	{
		return NimbleBridge_HttpResponse_isCompleted(this);
	}

	public string GetUrl()
	{
		return NimbleBridge_HttpResponse_getUrl(this);
	}

	public int GetStatusCode()
	{
		return NimbleBridge_HttpResponse_getStatusCode(this);
	}

	public global::System.Collections.Generic.Dictionary<string, string> GetHeaders()
	{
		global::System.IntPtr mapPtr = NimbleBridge_HttpResponse_getHeaders(this);
		return MarshalUtility.ConvertPtrToDictionary(mapPtr);
	}

	public long GetExpectedContentLength()
	{
		return NimbleBridge_HttpResponse_getExpectedContentLength(this);
	}

	public long GetDownloadedContentLength()
	{
		return NimbleBridge_HttpResponse_getDownloadedContentLength(this);
	}

	public double GetLastModified()
	{
		return NimbleBridge_HttpResponse_getLastModified(this);
	}

	public byte[] GetData()
	{
		global::System.IntPtr dataPtr = NimbleBridge_HttpResponse_getData(this);
		return MarshalUtility.ConvertPtrToData(dataPtr);
	}

	public NimbleBridge_Error GetError()
	{
		return NimbleBridge_HttpResponse_getError(this);
	}
}
