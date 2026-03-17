namespace GooglePlayGames
{
	public static class NearbyConnectionClientFactory
	{
		public static void Create(global::System.Action<global::GooglePlayGames.BasicApi.Nearby.INearbyConnectionClient> callback)
		{
			if (global::UnityEngine.Application.isEditor)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Creating INearbyConnection in editor, using DummyClient.");
				callback(new global::GooglePlayGames.BasicApi.Nearby.DummyNearbyConnectionClient());
			}
			global::GooglePlayGames.OurUtils.Logger.d("Creating real INearbyConnectionClient");
			global::GooglePlayGames.Native.NativeNearbyConnectionClientFactory.Create(callback);
		}

		private static global::GooglePlayGames.BasicApi.Nearby.InitializationStatus ToStatus(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus status)
		{
			switch (status)
			{
			case global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus.VALID:
				return global::GooglePlayGames.BasicApi.Nearby.InitializationStatus.Success;
			case global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus.ERROR_INTERNAL:
				return global::GooglePlayGames.BasicApi.Nearby.InitializationStatus.InternalError;
			case global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus.ERROR_VERSION_UPDATE_REQUIRED:
				return global::GooglePlayGames.BasicApi.Nearby.InitializationStatus.VersionUpdateRequired;
			default:
				global::GooglePlayGames.OurUtils.Logger.w("Unknown initialization status: " + status);
				return global::GooglePlayGames.BasicApi.Nearby.InitializationStatus.InternalError;
			}
		}
	}
}
