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
#if UNITY_ANDROID && !UNITY_EDITOR
		_platformAgent = new AndroidAgent();
#else
		_platformAgent = null; // Or a MockAgent if needed
#endif
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
		if (_platformAgent != null)
		{
			_platformAgent.start();
		}
	}

	public void onResume()
	{
		if (_platformAgent != null)
		{
			_platformAgent.onResume();
		}
	}

	public void onPause()
	{
		if (_platformAgent != null)
		{
			_platformAgent.onPause();
		}
	}

	public void setAge(int age)
	{
		if (_platformAgent != null)
		{
			_platformAgent.setAge(age);
		}
	}

	public void setGender(string gender)
	{
		if (_platformAgent != null)
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
	}

	public void setMediationSegment(string segment)
	{
		if (_platformAgent != null)
		{
			_platformAgent.setMediationSegment(segment);
		}
	}

	public void reportAppStarted()
	{
		if (_platformAgent != null)
		{
			_platformAgent.reportAppStarted();
		}
	}

	public void initRewardedVideo(string appKey, string userId)
	{
		if (_platformAgent != null)
		{
			_platformAgent.initRewardedVideo(appKey, userId);
		}
	}

	public string getAdvertiserId()
	{
		if (_platformAgent != null)
		{
			return _platformAgent.getAdvertiserId();
		}
		return string.Empty;
	}

	public void validateIntegration()
	{
		if (_platformAgent != null)
		{
			_platformAgent.validateIntegration();
		}
	}

	public void shouldTrackNetworkState(bool track)
	{
		if (_platformAgent != null)
		{
			_platformAgent.shouldTrackNetworkState(track);
		}
	}

	public void showRewardedVideo()
	{
		if (_platformAgent != null)
		{
			_platformAgent.showRewardedVideo();
		}
	}

	public void showRewardedVideo(string placementName)
	{
		if (_platformAgent != null)
		{
			_platformAgent.showRewardedVideo(placementName);
		}
	}

	public SupersonicPlacement getPlacementInfo(string placementName)
	{
		if (_platformAgent != null)
		{
			return _platformAgent.getPlacementInfo(placementName);
		}
		return null;
	}

	public bool isRewardedVideoAvailable()
	{
		return _platformAgent != null && _platformAgent.isRewardedVideoAvailable();
	}

	public bool isRewardedVideoPlacementCapped(string placementName)
	{
		return _platformAgent != null && _platformAgent.isRewardedVideoPlacementCapped(placementName);
	}

	public void initInterstitial(string appKey, string userId)
	{
		if (_platformAgent != null)
		{
			_platformAgent.initInterstitial(appKey, userId);
		}
	}

	public void loadInterstitial()
	{
		if (_platformAgent != null)
		{
			_platformAgent.loadInterstitial();
		}
	}

	public void showInterstitial()
	{
		if (_platformAgent != null)
		{
			_platformAgent.showInterstitial();
		}
	}

	public void showInterstitial(string placementName)
	{
		if (_platformAgent != null)
		{
			_platformAgent.showInterstitial(placementName);
		}
	}

	public bool isInterstitialReady()
	{
		return _platformAgent != null && _platformAgent.isInterstitialReady();
	}

	public bool isInterstitialPlacementCapped(string placementName)
	{
		return _platformAgent != null && _platformAgent.isInterstitialPlacementCapped(placementName);
	}

	public void initOfferwall(string appKey, string userId)
	{
		if (_platformAgent != null)
		{
			_platformAgent.initOfferwall(appKey, userId);
		}
	}

	public void showOfferwall()
	{
		if (_platformAgent != null)
		{
			_platformAgent.showOfferwall();
		}
	}

	public void getOfferwallCredits()
	{
		if (_platformAgent != null)
		{
			_platformAgent.getOfferwallCredits();
		}
	}

	public bool isOfferwallAvailable()
	{
		return _platformAgent != null && _platformAgent.isOfferwallAvailable();
	}
}
