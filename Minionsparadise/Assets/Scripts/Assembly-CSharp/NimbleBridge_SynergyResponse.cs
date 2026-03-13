public class NimbleBridge_SynergyResponse : global::System.Runtime.InteropServices.SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	internal NimbleBridge_SynergyResponse()
		: base(global::System.IntPtr.Zero, true)
	{
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_SynergyResponse_Dispose(NimbleBridge_SynergyResponse synergyResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_HttpResponse NimbleBridge_SynergyResponse_getHttpResponse(NimbleBridge_SynergyResponse synergyResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_SynergyResponse_isCompleted(NimbleBridge_SynergyResponse synergyResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_SynergyResponse_getError(NimbleBridge_SynergyResponse synergyResponseWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyResponse_getJsonData(NimbleBridge_SynergyResponse synergyResponseWrapper);

	protected override bool ReleaseHandle()
	{
		NimbleBridge_SynergyResponse_Dispose(this);
		return true;
	}

	public NimbleBridge_HttpResponse GetHttpResponse()
	{
		return NimbleBridge_SynergyResponse_getHttpResponse(this);
	}

	public bool IsCompleted()
	{
		return NimbleBridge_SynergyResponse_isCompleted(this);
	}

	public NimbleBridge_Error GetError()
	{
		return NimbleBridge_SynergyResponse_getError(this);
	}

	public global::System.Collections.Generic.Dictionary<string, object> GetJsonData()
	{
		string aJSON = NimbleBridge_SynergyResponse_getJsonData(this);
		global::SimpleJSON.JSONNode jSONNode = global::SimpleJSON.JSON.Parse(aJSON);
		return MarshalUtility.ConvertJsonToDictionary((global::SimpleJSON.JSONClass)jSONNode);
	}
}
