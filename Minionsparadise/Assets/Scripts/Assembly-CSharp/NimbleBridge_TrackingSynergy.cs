public class NimbleBridge_TrackingSynergy
{
	public const string EVENT_SYNERGY_CUSTOM = "SYNERGYTRACKING::CUSTOM";

	public const string NOTIFICATION_TRACKING_SYNERGY_POSTING_TO_SERVER = "nimble.notification.trackingimpl.synergy.postingToServer";

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_TrackingSynergy_isSessionStartEventType(int eventType);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_TrackingSynergy_isSessionEndEventType(int eventType);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_TrackingSynergy_getStringNameForSynergyTrackingEventType(int type);

	public static bool IsSessionStartEventType(SynergyTrackingEventType eventType)
	{
		return NimbleBridge_TrackingSynergy_isSessionStartEventType((int)eventType);
	}

	public static bool IsSessionEndEventType(SynergyTrackingEventType eventType)
	{
		return NimbleBridge_TrackingSynergy_isSessionEndEventType((int)eventType);
	}

	public static string GetStringNameForSynergyTrackingEventType(SynergyTrackingEventType type)
	{
		return NimbleBridge_TrackingSynergy_getStringNameForSynergyTrackingEventType((int)type);
	}
}
