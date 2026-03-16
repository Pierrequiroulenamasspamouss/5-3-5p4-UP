public class NimbleBridge_Base
{
	public enum NimbleBridge_Base_NimbleConfiguration
	{
		CONFIGURATION_UNKNOWN = 0,
		CONFIGURATION_INTEGRATION = 1,
		CONFIGURATION_STAGE = 2,
		CONFIGURATION_LIVE = 3,
		CONFIGURATION_CUSTOMIZED = 4
	}

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Base_setupNimble();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Base_teardownNimble();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern global::System.IntPtr NimbleBridge_Base_getComponentList();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_Base_getConfiguration();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Base_restartWithConfiguration(int configuration);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_Base_configurationToName(int configuration);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_Base_configurationFromName(string config);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_Base_getSdkVersion();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_Base_getReleaseVersion();
#endif

	public static void SetupNimble()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_Base_setupNimble();
#endif
	}

	public static void TeardownNimble()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_Base_teardownNimble();
#endif
	}

	public static string[] GetComponentList()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return MarshalUtility.ConvertPtrToArray(NimbleBridge_Base_getComponentList());
#else
		return new string[0];
#endif
	}

	public static NimbleBridge_Base.NimbleBridge_Base_NimbleConfiguration GetConfiguration()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return (NimbleBridge_Base.NimbleBridge_Base_NimbleConfiguration)NimbleBridge_Base_getConfiguration();
#else
		return NimbleBridge_Base_NimbleConfiguration.CONFIGURATION_LIVE;
#endif
	}

	public static void RestartWithConfiguration(NimbleBridge_Base.NimbleBridge_Base_NimbleConfiguration configuration)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		NimbleBridge_Base_restartWithConfiguration((int)configuration);
#endif
	}

	public static string ConfigurationToName(NimbleBridge_Base.NimbleBridge_Base_NimbleConfiguration configuration)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_Base_configurationToName((int)configuration);
#else
		return configuration.ToString();
#endif
	}

	public static NimbleBridge_Base.NimbleBridge_Base_NimbleConfiguration ConfigurationFromName(string config)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return (NimbleBridge_Base.NimbleBridge_Base_NimbleConfiguration)NimbleBridge_Base_configurationFromName(config);
#else
		return NimbleBridge_Base_NimbleConfiguration.CONFIGURATION_LIVE;
#endif
	}

	public static string GetSdkVersion()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_Base_getSdkVersion();
#else
		return "stub_sdk_version";
#endif
	}

	public static string GetReleaseVersion()
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_Base_getReleaseVersion();
#else
		return "stub_release_version";
#endif
	}
}
