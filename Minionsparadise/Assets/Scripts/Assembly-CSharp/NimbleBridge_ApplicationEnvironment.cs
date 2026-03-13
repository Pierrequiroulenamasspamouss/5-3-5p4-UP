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
		return NimbleBridge_ApplicationEnvironment_getApplicationBundleId();
	}

	public void SetApplicationBundleId(string bundleId)
	{
		NimbleBridge_ApplicationEnvironment_setApplicationBundleId(bundleId);
	}

	public string GetApplicationName()
	{
		return NimbleBridge_ApplicationEnvironment_getApplicationName();
	}

	public string GetApplicationVersion()
	{
		return NimbleBridge_ApplicationEnvironment_getApplicationVersion();
	}

	public string GetApplicationLanguageCode()
	{
		return NimbleBridge_ApplicationEnvironment_getApplicationLanguageCode();
	}

	public void SetApplicationLanguageCode(string languageCode)
	{
		NimbleBridge_ApplicationEnvironment_setApplicationLanguageCode(languageCode);
	}

	public string GetDocumentPath()
	{
		return NimbleBridge_ApplicationEnvironment_getDocumentPath();
	}

	public string GetCachePath()
	{
		return NimbleBridge_ApplicationEnvironment_getCachePath();
	}

	public string GetTempPath()
	{
		return NimbleBridge_ApplicationEnvironment_getTempPath();
	}

	public string GetCarrier()
	{
		return NimbleBridge_ApplicationEnvironment_getCarrier();
	}

	public string GetDeviceString()
	{
		return NimbleBridge_ApplicationEnvironment_getDeviceString();
	}

	public string GetMACAddress()
	{
		return NimbleBridge_ApplicationEnvironment_getMACAddress();
	}

	public string GetIPAddress()
	{
		return NimbleBridge_ApplicationEnvironment_getIPAddress();
	}

	public bool IsAppCracked()
	{
		return NimbleBridge_ApplicationEnvironment_isAppCracked();
	}

	public bool IsDeviceJailbroken()
	{
		return NimbleBridge_ApplicationEnvironment_isDeviceJailbroken();
	}

	public int GetAgeCompliance()
	{
		return NimbleBridge_ApplicationEnvironment_getAgeCompliance();
	}

	public void RefreshAgeCompliance()
	{
		NimbleBridge_ApplicationEnvironment_refreshAgeCompliance();
	}

	public bool GetIadAttribution()
	{
		return NimbleBridge_ApplicationEnvironment_getIadAttribution();
	}
}
