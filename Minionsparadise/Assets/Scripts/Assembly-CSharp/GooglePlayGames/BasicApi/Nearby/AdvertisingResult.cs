namespace GooglePlayGames.BasicApi.Nearby
{
	public struct AdvertisingResult
	{
		private readonly global::GooglePlayGames.BasicApi.ResponseStatus mStatus;

		private readonly string mLocalEndpointName;

		public bool Succeeded
		{
			get
			{
				return mStatus == global::GooglePlayGames.BasicApi.ResponseStatus.Success;
			}
		}

		public global::GooglePlayGames.BasicApi.ResponseStatus Status
		{
			get
			{
				return mStatus;
			}
		}

		public string LocalEndpointName
		{
			get
			{
				return mLocalEndpointName;
			}
		}

		public AdvertisingResult(global::GooglePlayGames.BasicApi.ResponseStatus status, string localEndpointName)
		{
			mStatus = status;
			mLocalEndpointName = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(localEndpointName);
		}
	}
}
