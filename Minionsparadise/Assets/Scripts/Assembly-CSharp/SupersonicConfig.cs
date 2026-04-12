public class SupersonicConfig
{
	private const string unsupportedPlatformStr = "Unsupported Platform";

	private static SupersonicConfig mInstance;

	private static global::UnityEngine.AndroidJavaObject _androidBridge;

	private static readonly string AndroidBridge = "com.supersonic.unity.androidbridge.AndroidBridge";

	public static SupersonicConfig Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new SupersonicConfig();
			}
			return mInstance;
		}
	}

	public SupersonicConfig()
	{
		createBridge();
	}

	private static void createBridge()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return;
		/*
		using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass(AndroidBridge))
		{
			_androidBridge = androidJavaClass.CallStatic<global::UnityEngine.AndroidJavaObject>("getInstance", new object[0]);
		}
		*/
#endif
	}

	public void setMaxVideoLength(int length)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if (_androidBridge != null) _androidBridge.Call("setSupersonicMaxVideoLength", length);
#endif
	}

	public void setLanguage(string language)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if (_androidBridge != null) _androidBridge.Call("setSupersonicLanguage", language);
#endif
	}

	public void setClientSideCallbacks(bool status)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if (_androidBridge != null) _androidBridge.Call("setSupersonicClientSideCallbacks", status);
#endif
	}

	public void setPrivateKey(string key)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if (_androidBridge != null) _androidBridge.Call("setSupersonicPrivateKey", key);
#endif
	}

	public void setItemName(string name)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if (_androidBridge != null) _androidBridge.Call("setSupersonicItemName", name);
#endif
	}

	public void setItemCount(int count)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if (_androidBridge != null) _androidBridge.Call("setSupersonicItemCount", count);
#endif
	}

	public void setRewardedVideoCustomParams(global::System.Collections.Generic.Dictionary<string, string> rvCustomParams)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		string text = global::SupersonicJSON.Json.Serialize(rvCustomParams);
		if (_androidBridge != null) _androidBridge.Call("setSupersonicRewardedVideoCustomParams", text);
#endif
	}

	public void setOfferwallCustomParams(global::System.Collections.Generic.Dictionary<string, string> owCustomParams)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		string text = global::SupersonicJSON.Json.Serialize(owCustomParams);
		if (_androidBridge != null) _androidBridge.Call("setSupersonicOfferwallCustomParams", text);
#endif
	}
}
