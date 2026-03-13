public class NimbleBridge_SynergyIdManager
{
	public const string NOTIFICATION_SYNERGY_ID_CHANGED = "nimble.synergyidmanager.notification.synergy_id_changed";

	public const string NOTIFICATION_ANONYMOUS_SYNERGY_ID_CHANGED = "nimble.synergyidmanager.notification.anonymous_synergy_id_changed";

	private NimbleBridge_SynergyIdManager()
	{
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyIdManager_getSynergyId();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_SynergyIdManager_getAnonymousSynergyId();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_SynergyIdManager_login(string userSynergyId, string authenticatorIdentifier);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Error NimbleBridge_SynergyIdManager_logout(string authenticatorIdentifier);

	public static NimbleBridge_SynergyIdManager GetComponent()
	{
		return new NimbleBridge_SynergyIdManager();
	}

	public string GetSynergyId()
	{
#if !UNITY_EDITOR
		return NimbleBridge_SynergyIdManager_getSynergyId();
#else
		return "mock_synergy_id";
#endif
	}

	public string GetAnonymousSynergyId()
	{
#if !UNITY_EDITOR
		return NimbleBridge_SynergyIdManager_getAnonymousSynergyId();
#else
		return "mock_anon_id";
#endif
	}

	public NimbleBridge_Error Login(string userSynergyId, string authenticatorIdentifier)
	{
#if !UNITY_EDITOR
		return NimbleBridge_SynergyIdManager_login(userSynergyId, authenticatorIdentifier);
#else
		return null;
#endif
	}

	public NimbleBridge_Error Logout(string authenticatorIdentifier)
	{
#if !UNITY_EDITOR
		return NimbleBridge_SynergyIdManager_logout(authenticatorIdentifier);
#else
		return null;
#endif
	}
}
