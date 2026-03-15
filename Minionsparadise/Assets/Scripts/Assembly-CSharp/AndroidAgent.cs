public class AndroidAgent : SupersonicIAgent
{
	private const string REWARD_AMOUNT = "reward_amount";

	private const string REWARD_NAME = "reward_name";

	private const string PLACEMENT_NAME = "placement_name";

	private static global::UnityEngine.AndroidJavaObject _androidBridge;

	private static readonly string AndroidBridge = "com.supersonic.unity.androidbridge.AndroidBridge";

	public AndroidAgent()
	{
		global::UnityEngine.Debug.Log("AndroidAgent ctr");
	}

	public void start()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		global::UnityEngine.Debug.Log("Android started");
		using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass(AndroidBridge))
		{
			_androidBridge = androidJavaClass.CallStatic<global::UnityEngine.AndroidJavaObject>("getInstance", new object[0]);
		}
		_androidBridge.Call("setPluginData", "Unity", Supersonic.pluginVersion(), Supersonic.unityVersion());
		global::UnityEngine.Debug.Log("Android started - ended");
#endif
	}

	public void reportAppStarted()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("reportAppStarted");
#endif
	}

	public void onResume()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("onResume");
#endif
	}

	public void onPause()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("onPause");
#endif
	}

	public void setAge(int age)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("setAge", age);
#endif
	}

	public void setGender(string gender)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("setGender", gender);
#endif
	}

	public void setMediationSegment(string segment)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("setMediationSegment", segment);
#endif
	}

	public void initRewardedVideo(string appKey, string userId)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("initRewardedVideo", appKey, userId);
#endif
	}

	public void showRewardedVideo()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("showRewardedVideo");
#endif
	}

	public void showRewardedVideo(string placementName)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("showRewardedVideo", placementName);
#endif
	}

	public bool isRewardedVideoAvailable()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return _androidBridge.Call<bool>("isRewardedVideoAvailable", new object[0]);
#else
		return false;
#endif
	}

	public bool isRewardedVideoPlacementCapped(string placementName)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return _androidBridge.Call<bool>("isRewardedVideoPlacementCapped", new object[1] { placementName });
#else
		return false;
#endif
	}

	public string getAdvertiserId()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return _androidBridge.Call<string>("getAdvertiserId", new object[0]);
#else
		return string.Empty;
#endif
	}

	public void shouldTrackNetworkState(bool track)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("shouldTrackNetworkState", track);
#endif
	}

	public void validateIntegration()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("validateIntegration");
#endif
	}

	public SupersonicPlacement getPlacementInfo(string placementName)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		string text = _androidBridge.Call<string>("getPlacementInfo", new object[1] { placementName });
		SupersonicPlacement result = null;
		if (text != null)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = global::SupersonicJSON.Json.Deserialize(text) as global::System.Collections.Generic.Dictionary<string, object>;
			string pName = dictionary["placement_name"].ToString();
			string rName = dictionary["reward_name"].ToString();
			int rAmount = global::System.Convert.ToInt32(dictionary["reward_amount"].ToString());
			result = new SupersonicPlacement(pName, rName, rAmount);
		}
		return result;
#else
		return null;
#endif
	}

	public void initInterstitial(string appKey, string userId)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("initInterstitial", appKey, userId);
#endif
	}

	public void loadInterstitial()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("loadInterstitial");
#endif
	}

	public void showInterstitial()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("showInterstitial");
#endif
	}

	public void showInterstitial(string placementName)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("showInterstitial", placementName);
#endif
	}

	public bool isInterstitialReady()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return _androidBridge.Call<bool>("isInterstitialReady", new object[0]);
#else
		return false;
#endif
	}

	public bool isInterstitialPlacementCapped(string placementName)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return _androidBridge.Call<bool>("isInterstitialPlacementCapped", new object[1] { placementName });
#else
		return false;
#endif
	}

	public void initOfferwall(string appKey, string userId)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("initOfferwall", appKey, userId);
#endif
	}

	public void showOfferwall()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("showOfferwall");
#endif
	}

	public void getOfferwallCredits()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		_androidBridge.Call("getOfferwallCredits");
#endif
	}

	public bool isOfferwallAvailable()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return _androidBridge.Call<bool>("isOfferwallAvailable", new object[0]);
#else
		return false;
#endif
	}
}
