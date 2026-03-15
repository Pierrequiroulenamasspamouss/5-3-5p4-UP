public class SwrveComponent : global::UnityEngine.MonoBehaviour
{
	public SwrveSDK SDK;

	public int GameId;

	public string ApiKey = "your_api_key_here";

	public global::Swrve.SwrveConfig Config;

	public bool FlushEventsOnApplicationQuit = true;

	public bool InitialiseOnStart = true;

	protected static SwrveComponent instance;

	public static SwrveComponent Instance
	{
		get
		{
			if (!instance)
			{
				SwrveComponent[] array = global::UnityEngine.Resources.FindObjectsOfTypeAll(typeof(SwrveComponent)) as SwrveComponent[];
				if (array != null && array.Length > 0)
				{
					instance = array[0];
				}
				else
				{
					SwrveLog.LogError("There needs to be one active SwrveComponent script on a GameObject in your scene.");
				}
			}
			return instance;
		}
	}

	public SwrveComponent()
	{
		Config = new global::Swrve.SwrveConfig();
		SDK = new SwrveSDK();
	}

	public void Init(int gameId, string apiKey)
	{
		SDK.Init(this, gameId, apiKey, Config);
	}

	public void Start()
	{
		base.useGUILayout = false;
		if (InitialiseOnStart)
		{
			Init(GameId, ApiKey);
		}
	}

	public void OnGUI()
	{
		SDK.OnGUI();
	}

	public void Update()
	{
		if (SDK != null && SDK.Initialised)
		{
			SDK.Update();
		}
	}

	public virtual void OnDeviceRegistered(string registrationId)
	{
		SwrveLog.LogError("Obtained registration id: " + registrationId);
		if (SDK != null && SDK.Initialised)
		{
			SDK.RegistrationIdReceived(registrationId);
		}
	}

	public virtual void OnNotificationReceived(string notificationJson)
	{
		if (SDK != null && SDK.Initialised)
		{
			SDK.NotificationReceived(notificationJson);
		}
	}

	public virtual void OnOpenedFromPushNotification(string notificationJson)
	{
		if (SDK != null && SDK.Initialised)
		{
			SDK.OpenedFromPushNotification(notificationJson);
		}
	}

	public void OnDestroy()
	{
		if (SDK.Initialised)
		{
			SDK.OnSwrveDestroy();
		}
		StopAllCoroutines();
	}

	public void OnApplicationQuit()
	{
		if (SDK.Initialised && FlushEventsOnApplicationQuit)
		{
			SDK.OnSwrveDestroy();
		}
	}

	public void OnApplicationPause(bool pauseStatus)
	{
		if (SDK != null && SDK.Initialised && Config != null && Config.AutomaticSessionManagement)
		{
			if (pauseStatus)
			{
				SDK.OnSwrvePause();
			}
			else
			{
				SDK.OnSwrveResume();
			}
		}
	}
}
