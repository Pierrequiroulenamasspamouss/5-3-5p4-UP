public class NimbleBridge_ApplicationEnvironment
{
	public const string NOTIFICATION_AGE_COMPLIANCE_REFRESHED = "nimble.notification.age_compliance_refreshed";

	private NimbleBridge_ApplicationEnvironment()
	{
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getApplicationBundleId();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_ApplicationEnvironment_setApplicationBundleId(string bundleId);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getApplicationName();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getApplicationVersion();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getApplicationLanguageCode();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_ApplicationEnvironment_setApplicationLanguageCode(string languageCode);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getDocumentPath();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getCachePath();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getTempPath();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getCarrier();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getDeviceString();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getMACAddress();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_ApplicationEnvironment_getIPAddress();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_ApplicationEnvironment_isAppCracked();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_ApplicationEnvironment_isDeviceJailbroken();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_ApplicationEnvironment_getAgeCompliance();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_ApplicationEnvironment_refreshAgeCompliance();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_ApplicationEnvironment_getIadAttribution();

	public static NimbleBridge_ApplicationEnvironment GetComponent()
	{
		return new NimbleBridge_ApplicationEnvironment();
	}

	public string GetApplicationBundleId()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getApplicationBundleId();
#else
		return "com.ea.minions";
#endif
	}

	public void SetApplicationBundleId(string bundleId)
	{
#if !UNITY_EDITOR
		NimbleBridge_ApplicationEnvironment_setApplicationBundleId(bundleId);
#endif
	}

	public string GetApplicationName()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getApplicationName();
#else
		return "Minions";
#endif
	}

	public string GetApplicationVersion()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getApplicationVersion();
#else
		return "1.0.0";
#endif
	}

	public string GetApplicationLanguageCode()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getApplicationLanguageCode();
#else
		return "en";
#endif
	}

	public void SetApplicationLanguageCode(string languageCode)
	{
#if !UNITY_EDITOR
		NimbleBridge_ApplicationEnvironment_setApplicationLanguageCode(languageCode);
#endif
	}

	public string GetDocumentPath()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getDocumentPath();
#else
		return global::UnityEngine.Application.persistentDataPath;
#endif
	}

	public string GetCachePath()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getCachePath();
#else
		return global::UnityEngine.Application.temporaryCachePath;
#endif
	}

	public string GetTempPath()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getTempPath();
#else
		return global::System.IO.Path.GetTempPath();
#endif
	}

	public string GetCarrier()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getCarrier();
#else
		return "Wifi";
#endif
	}

	public string GetDeviceString()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getDeviceString();
#else
		return global::UnityEngine.SystemInfo.deviceModel;
#endif
	}

	public string GetMACAddress()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getMACAddress();
#else
		return "00:00:00:00:00:00";
#endif
	}

	public string GetIPAddress()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getIPAddress();
#else
		return "127.0.0.1";
#endif
	}

	public bool IsAppCracked()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_isAppCracked();
#else
		return false;
#endif
	}

	public bool IsDeviceJailbroken()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_isDeviceJailbroken();
#else
		return false;
#endif
	}

	public int GetAgeCompliance()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getAgeCompliance();
#else
		return 0;
#endif
	}

	public void RefreshAgeCompliance()
	{
#if !UNITY_EDITOR
		NimbleBridge_ApplicationEnvironment_refreshAgeCompliance();
#endif
	}

	public bool GetIadAttribution()
	{
#if !UNITY_EDITOR
		return NimbleBridge_ApplicationEnvironment_getIadAttribution();
#else
		return false;
#endif
	}
}
