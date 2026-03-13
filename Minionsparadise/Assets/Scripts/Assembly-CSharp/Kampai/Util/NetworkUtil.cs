namespace Kampai.Util
{
	public static class NetworkUtil
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		private static readonly global::UnityEngine.AndroidJavaClass miscUtils = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.Misc");
#endif

		public static bool IsConnected()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return miscUtils.CallStatic<bool>("isConnected", new object[0]);
#else
			return global::UnityEngine.Application.internetReachability != global::UnityEngine.NetworkReachability.NotReachable;
#endif
		}

		public static bool IsNetworkWiFi()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return miscUtils.CallStatic<bool>("isNetworkWiFi", new object[0]);
#else
			return global::UnityEngine.Application.internetReachability == global::UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork;
#endif
		}

		public static global::UnityEngine.NetworkReachability GetNetworkReachability()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return IsNetworkWiFi() ? global::UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork : (IsConnected() ? global::UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork : global::UnityEngine.NetworkReachability.NotReachable);
#else
			return global::UnityEngine.Application.internetReachability;
#endif
		}
	}
}
