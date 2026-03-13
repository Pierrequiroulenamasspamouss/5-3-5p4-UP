public class NimbleBridge_Tracking
{
	private NimbleBridge_Tracking()
	{
	}

#if !UNITY_WEBPLAYER && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Tracking_logEvent(string type, global::System.IntPtr map);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_Tracking_isEnabled();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Tracking_setEnabled(bool enable);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Tracking_addCustomSessionData(string keyString, string valueString);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Tracking_removeCustomSessionData(string keyString);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Tracking_setTrackingAttribute(string keyString, string valueString);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_Tracking_getSessionId();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_Tracking_isNimbleStandardEvent(string type);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_Tracking_isEventTypeEqual(string _event, string otherEvent);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_Tracking_isEventTypeMemberOfSet(string _event, string[] eventTypeArray);
#endif

	public static NimbleBridge_Tracking GetComponent()
	{
		return new NimbleBridge_Tracking();
	}

	public void LogEvent(string type, global::System.Collections.Generic.Dictionary<string, string> parameters)
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR && UNITY_ANDROID
		global::System.IntPtr intPtr = global::System.IntPtr.Zero;
		try
		{
			intPtr = MarshalUtility.ConvertDictionaryToPtr(parameters);
			NimbleBridge_Tracking_logEvent(type, intPtr);
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

	public bool IsEnabled()
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR
		return NimbleBridge_Tracking_isEnabled();
#else
		return false;
#endif
	}

	public void SetEnabled(bool enable)
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR
		NimbleBridge_Tracking_setEnabled(enable);
#endif
	}

	public void AddCustomSessionValue(string key, string value)
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR
		NimbleBridge_Tracking_addCustomSessionData(key, value);
#endif
	}

	public void RemoveCustomSessionValue(string key)
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR
		NimbleBridge_Tracking_removeCustomSessionData(key);
#endif
	}

	public void ClearCustomSessionData()
	{
	}

	public void SetTrackingAttribute(string key, string value)
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR
		NimbleBridge_Tracking_setTrackingAttribute(key, value);
#endif
	}

	public string GetSessionId()
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR
		return NimbleBridge_Tracking_getSessionId();
#else
		return string.Empty;
#endif
	}

	public static bool IsNimbleStandardEvent(string type)
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR
		return NimbleBridge_Tracking_isNimbleStandardEvent(type);
#else
		return false;
#endif
	}

	public static bool IsEventTypeEqual(string _event, string otherEvent)
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR
		return NimbleBridge_Tracking_isEventTypeEqual(_event, otherEvent);
#else
		return _event == otherEvent;
#endif
	}

	public static bool IsEventTypeMemberOfSet(string _event, global::System.Collections.Generic.List<string> eventTypeSet)
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR && UNITY_ANDROID
		string[] array = new string[eventTypeSet.Count + 1];
		eventTypeSet.CopyTo(array);
		array[eventTypeSet.Count] = null;
		return NimbleBridge_Tracking_isEventTypeMemberOfSet(_event, array);
#else
		return eventTypeSet.Contains(_event);
#endif
	}
}
