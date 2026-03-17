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

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
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
#endif

	protected override bool ReleaseHandle()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_SynergyResponse_Dispose(this);
#endif
		return true;
	}

	public NimbleBridge_HttpResponse GetHttpResponse()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyResponse_getHttpResponse(this);
#else
		return null;
#endif
	}

	public bool IsCompleted()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyResponse_isCompleted(this);
#else
		return true;
#endif
	}

	public NimbleBridge_Error GetError()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyResponse_getError(this);
#else
		return null;
#endif
	}

	public global::System.Collections.Generic.Dictionary<string, object> GetJsonData()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		string aJSON = NimbleBridge_SynergyResponse_getJsonData(this);
		global::SimpleJSON.JSONNode jSONNode = global::SimpleJSON.JSON.Parse(aJSON);
		return MarshalUtility.ConvertJsonToDictionary((global::SimpleJSON.JSONClass)jSONNode);
#else
		return new global::System.Collections.Generic.Dictionary<string, object>();
#endif
	}
}
