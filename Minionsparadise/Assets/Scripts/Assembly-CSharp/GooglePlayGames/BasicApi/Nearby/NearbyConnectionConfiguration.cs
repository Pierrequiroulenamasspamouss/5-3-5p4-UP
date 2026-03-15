namespace GooglePlayGames.BasicApi.Nearby
{
	public struct NearbyConnectionConfiguration
	{
		public const int MaxUnreliableMessagePayloadLength = 1168;

		public const int MaxReliableMessagePayloadLength = 4096;

		private readonly global::System.Action<global::GooglePlayGames.BasicApi.Nearby.InitializationStatus> mInitializationCallback;

		private readonly long mLocalClientId;

		public long LocalClientId
		{
			get
			{
				return mLocalClientId;
			}
		}

		public global::System.Action<global::GooglePlayGames.BasicApi.Nearby.InitializationStatus> InitializationCallback
		{
			get
			{
				return mInitializationCallback;
			}
		}

		public NearbyConnectionConfiguration(global::System.Action<global::GooglePlayGames.BasicApi.Nearby.InitializationStatus> callback, long localClientId)
		{
			mInitializationCallback = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			mLocalClientId = localClientId;
		}
	}
}
