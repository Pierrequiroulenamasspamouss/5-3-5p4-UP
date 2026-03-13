public class Supersonic : SupersonicIAgent
{
	private const string UNITY_PLUGIN_VERSION = "6.4.5";

	public const string GENDER_MALE = "male";

	public const string GENDER_FEMALE = "female";

	public const string GENDER_UNKNOWN = "unknown";

	private SupersonicIAgent _platformAgent;

	private static Supersonic mInstance;

	public static Supersonic Agent
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new Supersonic();
			}
			return mInstance;
		}
	}

	private Supersonic()
	{
		_platformAgent = new AndroidAgent();
	}

	public static string pluginVersion()
	{
		return "6.4.5";
	}

	public static string unityVersion()
	{
		return global::UnityEngine.Application.unityVersion;
	}

	public void start()
	{
		_platformAgent.start();
	}

	public void onResume()
	{
		_platformAgent.onResume();
	}

	public void onPause()
	{
		_platformAgent.onPause();
	}

	public void setAge(int age)
	{
		_platformAgent.setAge(age);
	}

	public void setGender(string gender)
	{
		if (gender.Equals("male"))
		{
			_platformAgent.setGender("male");
		}
		else if (gender.Equals("female"))
		{
			_platformAgent.setGender("female");
		}
		else if (gender.Equals("unknown"))
		{
			_platformAgent.setGender("unknown");
		}
	}

	public void setMediationSegment(string segment)
	{
		_platformAgent.setMediationSegment(segment);
	}

	public void reportAppStarted()
	{
		_platformAgent.reportAppStarted();
	}

	public void initRewardedVideo(string appKey, string userId)
	{
		_platformAgent.initRewardedVideo(appKey, userId);
	}

	public string getAdvertiserId()
	{
		return _platformAgent.getAdvertiserId();
	}

	public void validateIntegration()
	{
		_platformAgent.validateIntegration();
	}

	public void shouldTrackNetworkState(bool track)
	{
		_platformAgent.shouldTrackNetworkState(track);
	}

	public void showRewardedVideo()
	{
		_platformAgent.showRewardedVideo();
	}

	public void showRewardedVideo(string placementName)
	{
		_platformAgent.showRewardedVideo(placementName);
	}

	public SupersonicPlacement getPlacementInfo(string placementName)
	{
		return _platformAgent.getPlacementInfo(placementName);
	}

	public bool isRewardedVideoAvailable()
	{
		return _platformAgent.isRewardedVideoAvailable();
	}

	public bool isRewardedVideoPlacementCapped(string placementName)
	{
		return _platformAgent.isRewardedVideoPlacementCapped(placementName);
	}

	public void initInterstitial(string appKey, string userId)
	{
		_platformAgent.initInterstitial(appKey, userId);
	}

	public void loadInterstitial()
	{
		_platformAgent.loadInterstitial();
	}

	public void showInterstitial()
	{
		_platformAgent.showInterstitial();
	}

	public void showInterstitial(string placementName)
	{
		_platformAgent.showInterstitial(placementName);
	}

	public bool isInterstitialReady()
	{
		return _platformAgent.isInterstitialReady();
	}

	public bool isInterstitialPlacementCapped(string placementName)
	{
		return _platformAgent.isInterstitialPlacementCapped(placementName);
	}

	public void initOfferwall(string appKey, string userId)
	{
		_platformAgent.initOfferwall(appKey, userId);
	}

	public void showOfferwall()
	{
		_platformAgent.showOfferwall();
	}

	public void getOfferwallCredits()
	{
		_platformAgent.getOfferwallCredits();
	}

	public bool isOfferwallAvailable()
	{
		return _platformAgent.isOfferwallAvailable();
	}
}
