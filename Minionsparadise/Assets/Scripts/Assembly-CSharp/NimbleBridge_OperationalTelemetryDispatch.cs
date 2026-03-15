public class NimbleBridge_OperationalTelemetryDispatch
{
	public const string EVENTTYPE_TRACKING_SYNERGY_PAYLOADS = "com.ea.nimble.trackingimpl.synergy";

	public const string EVENTTYPE_NETWORK_METRICS = "com.ea.nimble.network";

	public const string NOTIFICATION_OT_EVENT_THRESHOLD_WARNING = "nimble.notification.ot.eventthresholdwarning";

	private NimbleBridge_OperationalTelemetryDispatch()
	{
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_OperationalTelemetryDispatch_deleteEventsArray(global::System.IntPtr array);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_OperationalTelemetryDispatch_getEvents(string type);

	public static NimbleBridge_OperationalTelemetryDispatch GetComponent()
	{
		return new NimbleBridge_OperationalTelemetryDispatch();
	}

	public NimbleBridge_OperationalTelemetryEvent[] GetEvents(string type)
	{
#if !UNITY_EDITOR
		global::System.Collections.Generic.List<NimbleBridge_OperationalTelemetryEvent> list = new global::System.Collections.Generic.List<NimbleBridge_OperationalTelemetryEvent>();
		global::System.IntPtr intPtr = NimbleBridge_OperationalTelemetryDispatch_getEvents(type);
		global::System.IntPtr intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr);
		int num = 1;
		while (intPtr2 != global::System.IntPtr.Zero)
		{
			list.Add(new NimbleBridge_OperationalTelemetryEvent(intPtr2));
			intPtr2 = global::System.Runtime.InteropServices.Marshal.ReadIntPtr(intPtr, num * global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)));
			num++;
		}
		NimbleBridge_OperationalTelemetryDispatch_deleteEventsArray(intPtr);
		return list.ToArray();
#else
		return new NimbleBridge_OperationalTelemetryEvent[0];
#endif
	}

	public void SetMaxEventCount(string type, int count)
	{
#if !UNITY_EDITOR
		NimbleBridge_OperationalTelemetryDispatch_setMaxEventCount(type, count);
#endif
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_OperationalTelemetryDispatch_setMaxEventCount(string type, int count);

	public int GetMaxEventCount(string type)
	{
#if !UNITY_EDITOR
		return NimbleBridge_OperationalTelemetryDispatch_getMaxEventCount(type);
#else
		return 0;
#endif
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_OperationalTelemetryDispatch_getMaxEventCount(string type);
}
