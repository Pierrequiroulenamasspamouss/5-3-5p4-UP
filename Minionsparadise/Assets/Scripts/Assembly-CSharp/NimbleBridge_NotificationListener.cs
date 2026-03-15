// Forced re-import to resolve potential compiler staleness
public class NimbleBridge_NotificationListener : global::System.Runtime.InteropServices.SafeHandle
{
	private delegate void BridgeNotificationCallback(string name, string userData, global::System.IntPtr callbackDataPtr);

	private NotificationCallback m_callback;

	private global::System.IntPtr m_dataPtr;

	internal global::System.IntPtr DataPtr
	{
		get
		{
			return m_dataPtr;
		}
	}

	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	private NimbleBridge_NotificationListener()
		: base(global::System.IntPtr.Zero, true)
	{
	}

	private NimbleBridge_NotificationListener(global::System.IntPtr handle)
		: this()
	{
		SetHandle(handle);
	}

	public NimbleBridge_NotificationListener(NotificationCallback callback)
		: this()
	{
		m_callback = callback;
	}

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_NotificationListener.BridgeNotificationCallback))]
	internal static void OnNotificationCallback(string name, string userData, global::System.IntPtr callbackDataPtr)
	{
		global::SimpleJSON.JSONNode jSONNode = global::SimpleJSON.JSON.Parse(userData);
		global::System.Collections.Generic.Dictionary<string, object> userDataDict = MarshalUtility.ConvertJsonToDictionary((global::SimpleJSON.JSONClass)jSONNode);
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_NotificationListener listener = (NimbleBridge_NotificationListener)nimbleBridge_CallbackHelper.GetData(callbackDataPtr);
		nimbleBridge_CallbackHelper.RunOnMainThread(delegate
		{
			listener.m_callback(name, userDataDict, listener);
		});
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_NotificationListener_Dispose(NimbleBridge_NotificationListener listenerWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_NotificationListener_NotificationListener(NimbleBridge_NotificationListener.BridgeNotificationCallback callback, global::System.IntPtr callbackData);

	protected override bool ReleaseHandle()
	{
		NimbleBridge_NotificationListener_Dispose(this);
		return true;
	}

	internal void EnsureInitialized()
	{
		if (m_dataPtr == global::System.IntPtr.Zero)
		{
			m_dataPtr = NimbleBridge_CallbackHelper.Get().MakeCallbackData(this);
			SetHandle(NimbleBridge_NotificationListener_NotificationListener(OnNotificationCallback, m_dataPtr));
		}
	}

	internal void Unregister()
	{
		if (m_dataPtr != global::System.IntPtr.Zero)
		{
			NimbleBridge_NotificationListener_Dispose(this);
			m_dataPtr = global::System.IntPtr.Zero;
			SetHandle(global::System.IntPtr.Zero);
		}
	}
}
