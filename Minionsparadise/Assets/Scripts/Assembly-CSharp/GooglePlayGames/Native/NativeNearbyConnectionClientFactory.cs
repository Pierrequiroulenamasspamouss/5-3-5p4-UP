namespace GooglePlayGames.Native
{
	public class NativeNearbyConnectionClientFactory
	{
		private static volatile global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManager sManager;

		private static global::System.Action<global::GooglePlayGames.BasicApi.Nearby.INearbyConnectionClient> sCreationCallback;

		internal static global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManager GetManager()
		{
			return sManager;
		}

		public static void Create(global::System.Action<global::GooglePlayGames.BasicApi.Nearby.INearbyConnectionClient> callback)
		{
			if (sManager == null)
			{
				sCreationCallback = callback;
				InitializeFactory();
			}
			else
			{
				callback(new global::GooglePlayGames.Native.NativeNearbyConnectionsClient(GetManager()));
			}
		}

		internal static void InitializeFactory()
		{
			global::GooglePlayGames.OurUtils.PlayGamesHelperObject.CreateObject();
			global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManager.ReadServiceId();
			global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManagerBuilder nearbyConnectionsManagerBuilder = new global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManagerBuilder();
			nearbyConnectionsManagerBuilder.SetOnInitializationFinished(OnManagerInitialized);
			global::GooglePlayGames.Native.PInvoke.PlatformConfiguration configuration = new global::GooglePlayGames.Android.AndroidClient().CreatePlatformConfiguration();
			global::UnityEngine.Debug.Log("Building manager Now");
			sManager = nearbyConnectionsManagerBuilder.Build(configuration);
		}

		internal static void OnManagerInitialized(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus status)
		{
			global::UnityEngine.Debug.Log(string.Concat("Nearby Init Complete: ", status, " sManager = ", sManager));
			if (status == global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus.VALID)
			{
				if (sCreationCallback != null)
				{
					sCreationCallback(new global::GooglePlayGames.Native.NativeNearbyConnectionsClient(GetManager()));
					sCreationCallback = null;
				}
			}
			else
			{
				global::UnityEngine.Debug.LogError("ERROR: NearbyConnectionManager not initialized: " + status);
			}
		}
	}
}
