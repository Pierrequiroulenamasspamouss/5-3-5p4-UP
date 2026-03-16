public class NimbleBridge_SynergyEnvironment
{
	public enum NetworkConnectionType
	{
		NETWORK_CONNECTION_UNKNOWN = 0,
		NETWORK_CONNECTION_NONE = 1,
		NETWORK_CONNECTION_WIFI = 2,
		NETWORK_CONNECTION_WIRELESS = 3
	}

	public enum SynergyAppVersionCheckResult
	{
		SYNERGY_APP_VERSION_OK = 0,
		SYNERGY_APP_VERSION_UPDATE_RECOMMENDED = 1,
		SYNERGY_APP_VERSION_UPDATE_REQUIRED = 2
	}

	public const string SERVER_URL_KEY_SYNERGY_DRM = "synergy.drm";

	public const string SERVER_URL_KEY_SYNERGY_DIRECTOR = "synergy.director";

	public const string SERVER_URL_KEY_SYNERGY_MESSAGE_TO_USER = "synergy.m2u";

	public const string SERVER_URL_KEY_SYNERGY_PRODUCT = "synergy.product";

	public const string SERVER_URL_KEY_SYNERGY_TRACKING = "synergy.tracking";

	public const string SERVER_URL_KEY_SYNERGY_USER = "synergy.user";

	public const string SERVER_URL_KEY_SYNERGY_CENTRAL_IP_GEOLOCATION = "synergy.cipgl";

	public const string SERVER_URL_KEY_SYNERGY_S2S = "synergy.s2s";

	public const string SERVER_URL_KEY_ORIGIN_FRIENDS = "friends.url";

	public const string SERVER_URL_KEY_ORIGIN_AVATAR = "avatars.url";

	public const string SERVER_URL_KEY_ORIGIN_CASUAL_APP = "origincasualapp.url";

	public const string SERVER_URL_KEY_ORIGIN_CASUAL_SERVER = "origincasualserver.url";

	public const string SERVER_URL_KEY_AKAMAI = "akamai.url";

	public const string SERVER_URL_KEY_DYNAMIC_MORE_GAMES = "dmg.url";

	public const string SERVER_URL_KEY_MAYHEM = "mayhem.url";

	public const string SYNERGY_ENVIRONMENT_NOTIFICATION_STARTUP_REQUESTS_STARTED = "nimble.environment.notification.startup_requests_started";

	public const string SYNERGY_ENVIRONMENT_NOTIFICATION_STARTUP_REQUESTS_FINISHED = "nimble.environment.notification.startup_requests_finished";

	public const string SYNERGY_ENVIRONMENT_NOTIFICATION_STARTUP_ENVIRONMENT_DATA_CHANGED = "nimble.environment.notification.startup_environment_data_changed";

	public const string SYNERGY_ENVIRONMENT_NOTIFICATION_APP_VERSION_CHECK_FINISHED = "nimble.environment.notification.app_version_check_finished";

	public const string SYNERGY_ENVIRONMENT_NOTIFICATION_RESTORED_FROM_PERSISTENT = "nimble.environment.notification.restored_from_persistent";

	private NimbleBridge_SynergyEnvironment()
	{
	}

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyEnvironment_getEADeviceId();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyEnvironment_getSynergyId();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyEnvironment_getSellId();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyEnvironment_getProductId();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyEnvironment_getEAHardwareId();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyEnvironment_getServerUrlWithKey(string key);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_SynergyEnvironment_getLatestAppVersionCheckResult();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_SynergyEnvironment_isDataAvailable();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_SynergyEnvironment_isUpdateInProgress();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_SynergyEnvironment_checkAndInitiateSynergyEnvironmentUpdate();
#endif

	public static NimbleBridge_SynergyEnvironment GetComponent()
	{
		return new NimbleBridge_SynergyEnvironment();
	}

	public string GetEADeviceId()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyEnvironment_getEADeviceId();
#else
		return string.Empty;
#endif
	}

	public string GetSynergyId()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyEnvironment_getSynergyId();
#else
		return "mock_synergy_id";
#endif
	}

	public string GetSellId()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyEnvironment_getSellId();
#else
		return string.Empty;
#endif
	}

	public string GetProductId()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyEnvironment_getProductId();
#else
		return string.Empty;
#endif
	}

	public string GetEAHardwareId()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyEnvironment_getEAHardwareId();
#else
		return string.Empty;
#endif
	}

	public string GetServerUrlWithKey(string key)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyEnvironment_getServerUrlWithKey(key);
#else
		return string.Empty;
#endif
	}

	public NimbleBridge_SynergyEnvironment.SynergyAppVersionCheckResult GetLatestAppVersionCheckResult()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return (NimbleBridge_SynergyEnvironment.SynergyAppVersionCheckResult)NimbleBridge_SynergyEnvironment_getLatestAppVersionCheckResult();
#else
		return SynergyAppVersionCheckResult.SYNERGY_APP_VERSION_OK;
#endif
	}

	public bool IsDataAvailable()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyEnvironment_isDataAvailable();
#else
		return true;
#endif
	}

	public bool IsUpdateInProgress()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyEnvironment_isUpdateInProgress();
#else
		return false;
#endif
	}

	public NimbleBridge_Error CheckAndInitiateSynergyEnvironmentUpdate()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_SynergyEnvironment_checkAndInitiateSynergyEnvironmentUpdate();
#else
		return null;
#endif
	}
}
