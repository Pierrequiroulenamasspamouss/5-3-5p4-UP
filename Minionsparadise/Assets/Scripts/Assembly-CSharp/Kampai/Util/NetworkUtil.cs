namespace Kampai.Util
{
	public static class NetworkUtil
	{
		private static global::UnityEngine.NetworkReachability cachedReachability = global::UnityEngine.NetworkReachability.NotReachable;
		private static int mainThreadId = -1;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod(global::UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void CaptureMainThread()
		{
			mainThreadId = global::System.Threading.Thread.CurrentThread.ManagedThreadId;
		}

		public static bool IsConnected()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return global::UnityEngine.Application.internetReachability != global::UnityEngine.NetworkReachability.NotReachable;
#else
			// Use cached value if not on main thread or in editor
#if UNITY_EDITOR
			return true;
#else
			return GetNetworkReachability() != global::UnityEngine.NetworkReachability.NotReachable;
#endif
#endif
		}

		public static bool IsNetworkWiFi()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return global::UnityEngine.Application.internetReachability == global::UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork;
#else
#if UNITY_EDITOR
			return true;
#else
			return GetNetworkReachability() == global::UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork;
#endif
#endif
		}

		public static global::UnityEngine.NetworkReachability GetNetworkReachability()
		{
			// Special handling for UNITY_EDITOR always returning connected
#if UNITY_EDITOR
			return global::UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork;
#elif UNITY_ANDROID && !UNITY_EDITOR
			return IsNetworkWiFi() ? global::UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork : (IsConnected() ? global::UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork : global::UnityEngine.NetworkReachability.NotReachable);
#else
			// Only update reachability if we are on the main thread
			// Otherwise return the last known cached value
			if (mainThreadId == -1 || global::System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadId)
			{
				cachedReachability = global::UnityEngine.Application.internetReachability;
			}
			return cachedReachability;
#endif
		}
	}
}
