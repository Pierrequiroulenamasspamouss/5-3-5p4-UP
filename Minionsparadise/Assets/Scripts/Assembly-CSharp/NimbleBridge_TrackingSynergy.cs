public class NimbleBridge_TrackingSynergy
{
	public const string EVENT_SYNERGY_CUSTOM = "SYNERGYTRACKING::CUSTOM";

	public const string NOTIFICATION_TRACKING_SYNERGY_POSTING_TO_SERVER = "nimble.notification.trackingimpl.synergy.postingToServer";

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_TrackingSynergy_isSessionStartEventType(int eventType);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_TrackingSynergy_isSessionEndEventType(int eventType);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_TrackingSynergy_getStringNameForSynergyTrackingEventType(int type);
#endif

	public static bool IsSessionStartEventType(SynergyTrackingEventType eventType)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_TrackingSynergy_isSessionStartEventType((int)eventType);
#else
		return false;
#endif
	}

	public static bool IsSessionEndEventType(SynergyTrackingEventType eventType)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_TrackingSynergy_isSessionEndEventType((int)eventType);
#else
		return false;
#endif
	}

	public static string GetStringNameForSynergyTrackingEventType(SynergyTrackingEventType type)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_TrackingSynergy_getStringNameForSynergyTrackingEventType((int)type);
#else
		return type.ToString();
#endif
	}
}
