public class SupersonicEvents : global::UnityEngine.MonoBehaviour
{
	private const string ERROR_CODE = "error_code";

	private const string ERROR_DESCRIPTION = "error_description";

	private static event global::System.Action _onRewardedVideoInitSuccessEvent;

	public static event global::System.Action onRewardedVideoInitSuccessEvent
	{
		add
		{
			if (SupersonicEvents._onRewardedVideoInitSuccessEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoInitSuccessEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoInitSuccessEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onRewardedVideoInitSuccessEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoInitSuccessEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoInitSuccessEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onRewardedVideoInitSuccessEvent, value);
			}
		}
	}

	private static event global::System.Action<SupersonicError> _onRewardedVideoInitFailEvent;

	public static event global::System.Action<SupersonicError> onRewardedVideoInitFailEvent
	{
		add
		{
			if (SupersonicEvents._onRewardedVideoInitFailEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoInitFailEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoInitFailEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Combine(SupersonicEvents._onRewardedVideoInitFailEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoInitFailEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoInitFailEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Remove(SupersonicEvents._onRewardedVideoInitFailEvent, value);
			}
		}
	}

	private static event global::System.Action _onRewardedVideoAdOpenedEvent;

	public static event global::System.Action onRewardedVideoAdOpenedEvent
	{
		add
		{
			if (SupersonicEvents._onRewardedVideoAdOpenedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoAdOpenedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoAdOpenedEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onRewardedVideoAdOpenedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoAdOpenedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoAdOpenedEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onRewardedVideoAdOpenedEvent, value);
			}
		}
	}

	private static event global::System.Action _onRewardedVideoAdClosedEvent;

	public static event global::System.Action onRewardedVideoAdClosedEvent
	{
		add
		{
			if (SupersonicEvents._onRewardedVideoAdClosedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoAdClosedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoAdClosedEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onRewardedVideoAdClosedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoAdClosedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoAdClosedEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onRewardedVideoAdClosedEvent, value);
			}
		}
	}

	private static event global::System.Action _onVideoStartEvent;

	public static event global::System.Action onVideoStartEvent
	{
		add
		{
			if (SupersonicEvents._onVideoStartEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onVideoStartEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onVideoStartEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onVideoStartEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onVideoStartEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onVideoStartEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onVideoStartEvent, value);
			}
		}
	}

	private static event global::System.Action _onVideoEndEvent;

	public static event global::System.Action onVideoEndEvent
	{
		add
		{
			if (SupersonicEvents._onVideoEndEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onVideoEndEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onVideoEndEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onVideoEndEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onVideoEndEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onVideoEndEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onVideoEndEvent, value);
			}
		}
	}

	private static event global::System.Action<SupersonicPlacement> _onRewardedVideoAdRewardedEvent;

	public static event global::System.Action<SupersonicPlacement> onRewardedVideoAdRewardedEvent
	{
		add
		{
			if (SupersonicEvents._onRewardedVideoAdRewardedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoAdRewardedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoAdRewardedEvent = (global::System.Action<SupersonicPlacement>)global::System.Delegate.Combine(SupersonicEvents._onRewardedVideoAdRewardedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onRewardedVideoAdRewardedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onRewardedVideoAdRewardedEvent = (global::System.Action<SupersonicPlacement>)global::System.Delegate.Remove(SupersonicEvents._onRewardedVideoAdRewardedEvent, value);
			}
		}
	}

	private static event global::System.Action<bool> _onVideoAvailabilityChangedEvent;

	public static event global::System.Action<bool> onVideoAvailabilityChangedEvent
	{
		add
		{
			if (SupersonicEvents._onVideoAvailabilityChangedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onVideoAvailabilityChangedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onVideoAvailabilityChangedEvent = (global::System.Action<bool>)global::System.Delegate.Combine(SupersonicEvents._onVideoAvailabilityChangedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onVideoAvailabilityChangedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onVideoAvailabilityChangedEvent = (global::System.Action<bool>)global::System.Delegate.Remove(SupersonicEvents._onVideoAvailabilityChangedEvent, value);
			}
		}
	}

	private static event global::System.Action _onInterstitialInitSuccessEvent;

	public static event global::System.Action onInterstitialInitSuccessEvent
	{
		add
		{
			if (SupersonicEvents._onInterstitialInitSuccessEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialInitSuccessEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialInitSuccessEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onInterstitialInitSuccessEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialInitSuccessEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialInitSuccessEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onInterstitialInitSuccessEvent, value);
			}
		}
	}

	private static event global::System.Action<SupersonicError> _onInterstitialInitFailedEvent;

	public static event global::System.Action<SupersonicError> onInterstitialInitFailedEvent
	{
		add
		{
			if (SupersonicEvents._onInterstitialInitFailedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialInitFailedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialInitFailedEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Combine(SupersonicEvents._onInterstitialInitFailedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialInitFailedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialInitFailedEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Remove(SupersonicEvents._onInterstitialInitFailedEvent, value);
			}
		}
	}

	private static event global::System.Action _onInterstitialReadyEvent;

	public static event global::System.Action onInterstitialReadyEvent
	{
		add
		{
			if (SupersonicEvents._onInterstitialReadyEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialReadyEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialReadyEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onInterstitialReadyEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialReadyEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialReadyEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onInterstitialReadyEvent, value);
			}
		}
	}

	private static event global::System.Action<SupersonicError> _onInterstitialLoadFailedEvent;

	public static event global::System.Action<SupersonicError> onInterstitialLoadFailedEvent
	{
		add
		{
			if (SupersonicEvents._onInterstitialLoadFailedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialLoadFailedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialLoadFailedEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Combine(SupersonicEvents._onInterstitialLoadFailedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialLoadFailedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialLoadFailedEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Remove(SupersonicEvents._onInterstitialLoadFailedEvent, value);
			}
		}
	}

	private static event global::System.Action _onInterstitialOpenEvent;

	public static event global::System.Action onInterstitialOpenEvent
	{
		add
		{
			if (SupersonicEvents._onInterstitialOpenEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialOpenEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialOpenEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onInterstitialOpenEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialOpenEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialOpenEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onInterstitialOpenEvent, value);
			}
		}
	}

	private static event global::System.Action _onInterstitialCloseEvent;

	public static event global::System.Action onInterstitialCloseEvent
	{
		add
		{
			if (SupersonicEvents._onInterstitialCloseEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialCloseEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialCloseEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onInterstitialCloseEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialCloseEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialCloseEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onInterstitialCloseEvent, value);
			}
		}
	}

	private static event global::System.Action _onInterstitialShowSuccessEvent;

	public static event global::System.Action onInterstitialShowSuccessEvent
	{
		add
		{
			if (SupersonicEvents._onInterstitialShowSuccessEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialShowSuccessEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialShowSuccessEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onInterstitialShowSuccessEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialShowSuccessEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialShowSuccessEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onInterstitialShowSuccessEvent, value);
			}
		}
	}

	private static event global::System.Action<SupersonicError> _onInterstitialShowFailedEvent;

	public static event global::System.Action<SupersonicError> onInterstitialShowFailedEvent
	{
		add
		{
			if (SupersonicEvents._onInterstitialShowFailedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialShowFailedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialShowFailedEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Combine(SupersonicEvents._onInterstitialShowFailedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialShowFailedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialShowFailedEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Remove(SupersonicEvents._onInterstitialShowFailedEvent, value);
			}
		}
	}

	private static event global::System.Action _onInterstitialClickEvent;

	public static event global::System.Action onInterstitialClickEvent
	{
		add
		{
			if (SupersonicEvents._onInterstitialClickEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialClickEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialClickEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onInterstitialClickEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onInterstitialClickEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onInterstitialClickEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onInterstitialClickEvent, value);
			}
		}
	}

	private static event global::System.Action _onOfferwallInitSuccessEvent;

	public static event global::System.Action onOfferwallInitSuccessEvent
	{
		add
		{
			if (SupersonicEvents._onOfferwallInitSuccessEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallInitSuccessEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallInitSuccessEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onOfferwallInitSuccessEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallInitSuccessEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallInitSuccessEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onOfferwallInitSuccessEvent, value);
			}
		}
	}

	private static event global::System.Action<SupersonicError> _onOfferwallInitFailEvent;

	public static event global::System.Action<SupersonicError> onOfferwallInitFailEvent
	{
		add
		{
			if (SupersonicEvents._onOfferwallInitFailEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallInitFailEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallInitFailEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Combine(SupersonicEvents._onOfferwallInitFailEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallInitFailEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallInitFailEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Remove(SupersonicEvents._onOfferwallInitFailEvent, value);
			}
		}
	}

	private static event global::System.Action _onOfferwallOpenedEvent;

	public static event global::System.Action onOfferwallOpenedEvent
	{
		add
		{
			if (SupersonicEvents._onOfferwallOpenedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallOpenedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallOpenedEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onOfferwallOpenedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallOpenedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallOpenedEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onOfferwallOpenedEvent, value);
			}
		}
	}

	private static event global::System.Action<SupersonicError> _onOfferwallShowFailEvent;

	public static event global::System.Action<SupersonicError> onOfferwallShowFailEvent
	{
		add
		{
			if (SupersonicEvents._onOfferwallShowFailEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallShowFailEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallShowFailEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Combine(SupersonicEvents._onOfferwallShowFailEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallShowFailEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallShowFailEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Remove(SupersonicEvents._onOfferwallShowFailEvent, value);
			}
		}
	}

	private static event global::System.Action _onOfferwallClosedEvent;

	public static event global::System.Action onOfferwallClosedEvent
	{
		add
		{
			if (SupersonicEvents._onOfferwallClosedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallClosedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallClosedEvent = (global::System.Action)global::System.Delegate.Combine(SupersonicEvents._onOfferwallClosedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallClosedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallClosedEvent = (global::System.Action)global::System.Delegate.Remove(SupersonicEvents._onOfferwallClosedEvent, value);
			}
		}
	}

	private static event global::System.Action<SupersonicError> _onGetOfferwallCreditsFailEvent;

	public static event global::System.Action<SupersonicError> onGetOfferwallCreditsFailEvent
	{
		add
		{
			if (SupersonicEvents._onGetOfferwallCreditsFailEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onGetOfferwallCreditsFailEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onGetOfferwallCreditsFailEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Combine(SupersonicEvents._onGetOfferwallCreditsFailEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onGetOfferwallCreditsFailEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onGetOfferwallCreditsFailEvent = (global::System.Action<SupersonicError>)global::System.Delegate.Remove(SupersonicEvents._onGetOfferwallCreditsFailEvent, value);
			}
		}
	}

	private static event global::System.Action<global::System.Collections.Generic.Dictionary<string, object>> _onOfferwallAdCreditedEvent;

	public static event global::System.Action<global::System.Collections.Generic.Dictionary<string, object>> onOfferwallAdCreditedEvent
	{
		add
		{
			if (SupersonicEvents._onOfferwallAdCreditedEvent == null || !global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallAdCreditedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallAdCreditedEvent = (global::System.Action<global::System.Collections.Generic.Dictionary<string, object>>)global::System.Delegate.Combine(SupersonicEvents._onOfferwallAdCreditedEvent, value);
			}
		}
		remove
		{
			if (global::System.Linq.Enumerable.Contains(SupersonicEvents._onOfferwallAdCreditedEvent.GetInvocationList(), value))
			{
				SupersonicEvents._onOfferwallAdCreditedEvent = (global::System.Action<global::System.Collections.Generic.Dictionary<string, object>>)global::System.Delegate.Remove(SupersonicEvents._onOfferwallAdCreditedEvent, value);
			}
		}
	}

	private void Awake()
	{
		base.gameObject.name = "SupersonicEvents";
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void onRewardedVideoInitSuccess(string empty)
	{
		if (SupersonicEvents._onRewardedVideoInitSuccessEvent != null)
		{
			SupersonicEvents._onRewardedVideoInitSuccessEvent();
		}
	}

	public void onRewardedVideoInitFail(string description)
	{
		if (SupersonicEvents._onRewardedVideoInitFailEvent != null)
		{
			global::UnityEngine.Debug.Log("entered onRewardedVideoInitFail 1");
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onRewardedVideoInitFailEvent(errorFromErrorString);
		}
	}

	public void onRewardedVideoAdOpened(string empty)
	{
		if (SupersonicEvents._onRewardedVideoAdOpenedEvent != null)
		{
			SupersonicEvents._onRewardedVideoAdOpenedEvent();
		}
	}

	public void onRewardedVideoAdClosed(string empty)
	{
		if (SupersonicEvents._onRewardedVideoAdClosedEvent != null)
		{
			SupersonicEvents._onRewardedVideoAdClosedEvent();
		}
	}

	public void onVideoStart(string empty)
	{
		if (SupersonicEvents._onVideoStartEvent != null)
		{
			SupersonicEvents._onVideoStartEvent();
		}
	}

	public void onVideoEnd(string empty)
	{
		if (SupersonicEvents._onVideoEndEvent != null)
		{
			SupersonicEvents._onVideoEndEvent();
		}
	}

	public void onRewardedVideoAdRewarded(string description)
	{
		if (SupersonicEvents._onRewardedVideoAdRewardedEvent != null)
		{
			SupersonicPlacement placementFromString = getPlacementFromString(description);
			SupersonicEvents._onRewardedVideoAdRewardedEvent(placementFromString);
		}
	}

	public void onVideoAvailabilityChanged(string stringAvailable)
	{
		bool obj = stringAvailable == "true";
		if (SupersonicEvents._onVideoAvailabilityChangedEvent != null)
		{
			SupersonicEvents._onVideoAvailabilityChangedEvent(obj);
		}
	}

	public void onInterstitialInitSuccess(string empty)
	{
		if (SupersonicEvents._onInterstitialInitSuccessEvent != null)
		{
			SupersonicEvents._onInterstitialInitSuccessEvent();
		}
	}

	public void onInterstitialInitFailed(string description)
	{
		if (SupersonicEvents._onInterstitialInitFailedEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onInterstitialInitFailedEvent(errorFromErrorString);
		}
	}

	public void onInterstitialReady()
	{
		if (SupersonicEvents._onInterstitialReadyEvent != null)
		{
			SupersonicEvents._onInterstitialReadyEvent();
		}
	}

	public void onInterstitialLoadFailed(string description)
	{
		if (SupersonicEvents._onInterstitialLoadFailedEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onInterstitialLoadFailedEvent(errorFromErrorString);
		}
	}

	public void onInterstitialOpen(string empty)
	{
		if (SupersonicEvents._onInterstitialOpenEvent != null)
		{
			SupersonicEvents._onInterstitialOpenEvent();
		}
	}

	public void onInterstitialClose(string empty)
	{
		if (SupersonicEvents._onInterstitialCloseEvent != null)
		{
			SupersonicEvents._onInterstitialCloseEvent();
		}
	}

	public void onInterstitialShowSuccess(string empty)
	{
		if (SupersonicEvents._onInterstitialShowSuccessEvent != null)
		{
			SupersonicEvents._onInterstitialShowSuccessEvent();
		}
	}

	public void onInterstitialShowFailed(string description)
	{
		if (SupersonicEvents._onInterstitialShowFailedEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onInterstitialShowFailedEvent(errorFromErrorString);
		}
	}

	public void onInterstitialClick(string empty)
	{
		if (SupersonicEvents._onInterstitialClickEvent != null)
		{
			SupersonicEvents._onInterstitialClickEvent();
		}
	}

	public void onOfferwallInitSuccess(string empty)
	{
		if (SupersonicEvents._onOfferwallInitSuccessEvent != null)
		{
			SupersonicEvents._onOfferwallInitSuccessEvent();
		}
	}

	public void onOfferwallInitFail(string description)
	{
		if (SupersonicEvents._onOfferwallInitFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onOfferwallInitFailEvent(errorFromErrorString);
		}
	}

	public void onOfferwallOpened(string empty)
	{
		if (SupersonicEvents._onOfferwallOpenedEvent != null)
		{
			SupersonicEvents._onOfferwallOpenedEvent();
		}
	}

	public void onOfferwallShowFail(string description)
	{
		if (SupersonicEvents._onOfferwallShowFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onOfferwallShowFailEvent(errorFromErrorString);
		}
	}

	public void onOfferwallClosed(string empty)
	{
		if (SupersonicEvents._onOfferwallClosedEvent != null)
		{
			SupersonicEvents._onOfferwallClosedEvent();
		}
	}

	public void onGetOfferwallCreditsFail(string description)
	{
		if (SupersonicEvents._onGetOfferwallCreditsFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onGetOfferwallCreditsFailEvent(errorFromErrorString);
		}
	}

	public void onOfferwallAdCredited(string json)
	{
		if (SupersonicEvents._onOfferwallAdCreditedEvent != null)
		{
			SupersonicEvents._onOfferwallAdCreditedEvent(global::SupersonicJSON.Json.Deserialize(json) as global::System.Collections.Generic.Dictionary<string, object>);
		}
	}

	public SupersonicError getErrorFromErrorString(string description)
	{
		if (!string.IsNullOrEmpty(description))
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = global::SupersonicJSON.Json.Deserialize(description) as global::System.Collections.Generic.Dictionary<string, object>;
			if (dictionary != null)
			{
				int errCode = global::System.Convert.ToInt32(dictionary["error_code"].ToString());
				string errDescription = dictionary["error_description"].ToString();
				return new SupersonicError(errCode, errDescription);
			}
			return new SupersonicError(-1, string.Empty);
		}
		return new SupersonicError(-1, string.Empty);
	}

	public SupersonicPlacement getPlacementFromString(string jsonPlacement)
	{
		global::System.Collections.Generic.Dictionary<string, object> dictionary = global::SupersonicJSON.Json.Deserialize(jsonPlacement) as global::System.Collections.Generic.Dictionary<string, object>;
		int rAmount = global::System.Convert.ToInt32(dictionary["placement_reward_amount"].ToString());
		string rName = dictionary["placement_reward_name"].ToString();
		string pName = dictionary["placement_name"].ToString();
		return new SupersonicPlacement(pName, rName, rAmount);
	}
}
