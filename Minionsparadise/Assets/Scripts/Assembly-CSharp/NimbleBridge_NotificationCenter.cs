public class NimbleBridge_NotificationCenter
{
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_NotificationCenter_registerListener(string notification, NimbleBridge_NotificationListener listenerWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_NotificationCenter_unregisterListener(NimbleBridge_NotificationListener listenerWrapper);

	public static void RegisterListener(string notification, NimbleBridge_NotificationListener listener)
	{
		listener.EnsureInitialized();
		NimbleBridge_NotificationCenter_registerListener(notification, listener);
	}

	public static void UnregisterListener(NimbleBridge_NotificationListener listener)
	{
		NimbleBridge_NotificationCenter_unregisterListener(listener);
		listener.Unregister();
	}
}
