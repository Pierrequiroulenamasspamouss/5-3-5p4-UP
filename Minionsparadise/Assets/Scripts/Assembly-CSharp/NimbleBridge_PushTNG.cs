public class NimbleBridge_PushTNG
{
	public enum DisabledReason
	{
		OPT_OUT = 0,
		GAME_SERVER = 1,
		REGISTER_FAILURE = 2
	}

	public delegate void OnRegistrationSuccessDelegate(int statusCode, string registrationToken);

	public delegate void OnConnectionErrorDelegate(int statusCode, string errorMessage);

	public delegate void OnTrackingSuccessDelegate(int statusCode, string trackingData);

	public delegate void OnGetInAppSuccessDelegate(int statusCode, string inAppNotificationData);

	private delegate void BridgeOnRegistrationSuccessDelegate(int statusCode, string registrationToken, global::System.IntPtr callbackData);

	private delegate void BridgeOnConnectionErrorDelegate(int statusCode, string errorMessage, global::System.IntPtr callbackData);

	private delegate void BridgeOnTrackingSuccessDelegate(int statusCode, string trackingData, global::System.IntPtr callbackData);

	private delegate void BridgeOnGetInAppSuccessDelegate(int statusCode, string inAppNotificationData, global::System.IntPtr callbackData);

	private NimbleBridge_PushTNG()
	{
	}

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_PushTNG_start(string userAlias, double dateOfBirth, bool sandbox, NimbleBridge_PushTNG.BridgeOnRegistrationSuccessDelegate registrationSuccessDelegate, global::System.IntPtr registrationSuccessData, NimbleBridge_PushTNG.BridgeOnConnectionErrorDelegate connectionErrorDelegate, global::System.IntPtr connectionErrorData, NimbleBridge_PushTNG.BridgeOnTrackingSuccessDelegate trackingSuccessDelegate, global::System.IntPtr trackingSuccessData, NimbleBridge_PushTNG.BridgeOnGetInAppSuccessDelegate getInAppSuccessDelegate, global::System.IntPtr getInAppSuccessData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_PushTNG_startDisabled(string userAlias, double dateOfBirth, int disabledReason, bool sandbox, NimbleBridge_PushTNG.BridgeOnRegistrationSuccessDelegate registrationSuccessDelegate, global::System.IntPtr registrationSuccessData, NimbleBridge_PushTNG.BridgeOnConnectionErrorDelegate connectionErrorDelegate, global::System.IntPtr connectionErrorData, NimbleBridge_PushTNG.BridgeOnTrackingSuccessDelegate trackingSuccessDelegate, global::System.IntPtr trackingSuccessData, NimbleBridge_PushTNG.BridgeOnGetInAppSuccessDelegate getInAppSuccessDelegate, global::System.IntPtr getInAppSuccessData);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_PushTNG_getRegistrationStatus();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_PushTNG_getDisableStatus();
#endif

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_PushTNG.BridgeOnRegistrationSuccessDelegate))]
	private static void OnRegistrationSuccess(int statusCode, string registrationToken, global::System.IntPtr callbackData)
	{
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_PushTNG.OnRegistrationSuccessDelegate callback = (NimbleBridge_PushTNG.OnRegistrationSuccessDelegate)nimbleBridge_CallbackHelper.GetData(callbackData);
		if (callback != null)
		{
			nimbleBridge_CallbackHelper.RunOnMainThread(delegate
			{
				callback(statusCode, registrationToken);
			});
		}
	}

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_PushTNG.BridgeOnConnectionErrorDelegate))]
	private static void OnConnectionError(int statusCode, string errorMessage, global::System.IntPtr callbackData)
	{
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_PushTNG.OnConnectionErrorDelegate callback = (NimbleBridge_PushTNG.OnConnectionErrorDelegate)nimbleBridge_CallbackHelper.GetData(callbackData);
		if (callback != null)
		{
			nimbleBridge_CallbackHelper.RunOnMainThread(delegate
			{
				callback(statusCode, errorMessage);
			});
		}
	}

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_PushTNG.BridgeOnTrackingSuccessDelegate))]
	private static void OnTrackingSuccess(int statusCode, string trackingData, global::System.IntPtr callbackData)
	{
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_PushTNG.OnTrackingSuccessDelegate callback = (NimbleBridge_PushTNG.OnTrackingSuccessDelegate)nimbleBridge_CallbackHelper.GetData(callbackData);
		if (callback != null)
		{
			nimbleBridge_CallbackHelper.RunOnMainThread(delegate
			{
				callback(statusCode, trackingData);
			});
		}
	}

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_PushTNG.BridgeOnGetInAppSuccessDelegate))]
	private static void OnGetInAppSuccess(int statusCode, string inAppNotificationData, global::System.IntPtr callbackData)
	{
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		NimbleBridge_PushTNG.OnGetInAppSuccessDelegate callback = (NimbleBridge_PushTNG.OnGetInAppSuccessDelegate)nimbleBridge_CallbackHelper.GetData(callbackData);
		if (callback != null)
		{
			nimbleBridge_CallbackHelper.RunOnMainThread(delegate
			{
				callback(statusCode, inAppNotificationData);
			});
		}
	}

	public static NimbleBridge_PushTNG GetComponent()
	{
		return new NimbleBridge_PushTNG();
	}

	public void Start(string userAlias, double dateOfBirth, bool sandbox)
	{
		Start(userAlias, dateOfBirth, sandbox, null, null, null, null);
	}

	public void Start(string userAlias, double dateOfBirth, bool sandbox, NimbleBridge_PushTNG.OnRegistrationSuccessDelegate registrationSuccessDelegate, NimbleBridge_PushTNG.OnConnectionErrorDelegate connectionErrorDelegate, NimbleBridge_PushTNG.OnTrackingSuccessDelegate trackingSuccessDelegate, NimbleBridge_PushTNG.OnGetInAppSuccessDelegate getInAppSuccessDelegate)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		global::System.IntPtr registrationSuccessData = nimbleBridge_CallbackHelper.MakeCallbackData(registrationSuccessDelegate);
		global::System.IntPtr connectionErrorData = nimbleBridge_CallbackHelper.MakeCallbackData(connectionErrorDelegate);
		global::System.IntPtr trackingSuccessData = nimbleBridge_CallbackHelper.MakeCallbackData(trackingSuccessDelegate);
		global::System.IntPtr getInAppSuccessData = nimbleBridge_CallbackHelper.MakeCallbackData(getInAppSuccessDelegate);
		NimbleBridge_PushTNG_start(userAlias, dateOfBirth, sandbox, OnRegistrationSuccess, registrationSuccessData, OnConnectionError, connectionErrorData, OnTrackingSuccess, trackingSuccessData, OnGetInAppSuccess, getInAppSuccessData);
#endif
	}

	public void StartDisabled(string userAlias, double dateOfBirth, NimbleBridge_PushTNG.DisabledReason disabledReason, bool sandbox, NimbleBridge_PushTNG.OnRegistrationSuccessDelegate registrationSuccessDelegate, NimbleBridge_PushTNG.OnConnectionErrorDelegate connectionErrorDelegate, NimbleBridge_PushTNG.OnTrackingSuccessDelegate trackingSuccessDelegate, NimbleBridge_PushTNG.OnGetInAppSuccessDelegate getInAppSuccessDelegate)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_CallbackHelper nimbleBridge_CallbackHelper = NimbleBridge_CallbackHelper.Get();
		global::System.IntPtr registrationSuccessData = nimbleBridge_CallbackHelper.MakeCallbackData(registrationSuccessDelegate);
		global::System.IntPtr connectionErrorData = nimbleBridge_CallbackHelper.MakeCallbackData(connectionErrorDelegate);
		global::System.IntPtr trackingSuccessData = nimbleBridge_CallbackHelper.MakeCallbackData(trackingSuccessDelegate);
		global::System.IntPtr getInAppSuccessData = nimbleBridge_CallbackHelper.MakeCallbackData(getInAppSuccessDelegate);
		NimbleBridge_PushTNG_startDisabled(userAlias, dateOfBirth, (int)disabledReason, sandbox, OnRegistrationSuccess, registrationSuccessData, OnConnectionError, connectionErrorData, OnTrackingSuccess, trackingSuccessData, OnGetInAppSuccess, getInAppSuccessData);
#endif
	}

	public bool getRegistrationStatus()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_PushTNG_getRegistrationStatus();
#else
		return false;
#endif
	}

	public string getDisableStatus()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_PushTNG_getDisableStatus();
#else
		return string.Empty;
#endif
	}
}
