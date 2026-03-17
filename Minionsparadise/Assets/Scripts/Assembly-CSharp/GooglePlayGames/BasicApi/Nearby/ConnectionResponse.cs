namespace GooglePlayGames.BasicApi.Nearby
{
	public struct ConnectionResponse
	{
		public enum Status
		{
			Accepted = 0,
			Rejected = 1,
			ErrorInternal = 2,
			ErrorNetworkNotConnected = 3,
			ErrorEndpointNotConnected = 4,
			ErrorAlreadyConnected = 5
		}

		private static readonly byte[] EmptyPayload = new byte[0];

		private readonly long mLocalClientId;

		private readonly string mRemoteEndpointId;

		private readonly global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Status mResponseStatus;

		private readonly byte[] mPayload;

		public long LocalClientId
		{
			get
			{
				return mLocalClientId;
			}
		}

		public string RemoteEndpointId
		{
			get
			{
				return mRemoteEndpointId;
			}
		}

		public global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Status ResponseStatus
		{
			get
			{
				return mResponseStatus;
			}
		}

		public byte[] Payload
		{
			get
			{
				return mPayload;
			}
		}

		private ConnectionResponse(long localClientId, string remoteEndpointId, global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Status code, byte[] payload)
		{
			mLocalClientId = localClientId;
			mRemoteEndpointId = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(remoteEndpointId);
			mResponseStatus = code;
			mPayload = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(payload);
		}

		public static global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse Rejected(long localClientId, string remoteEndpointId)
		{
			return new global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse(localClientId, remoteEndpointId, global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Status.Rejected, EmptyPayload);
		}

		public static global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse NetworkNotConnected(long localClientId, string remoteEndpointId)
		{
			return new global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse(localClientId, remoteEndpointId, global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Status.ErrorNetworkNotConnected, EmptyPayload);
		}

		public static global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse InternalError(long localClientId, string remoteEndpointId)
		{
			return new global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse(localClientId, remoteEndpointId, global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Status.ErrorInternal, EmptyPayload);
		}

		public static global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse EndpointNotConnected(long localClientId, string remoteEndpointId)
		{
			return new global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse(localClientId, remoteEndpointId, global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Status.ErrorEndpointNotConnected, EmptyPayload);
		}

		public static global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse Accepted(long localClientId, string remoteEndpointId, byte[] payload)
		{
			return new global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse(localClientId, remoteEndpointId, global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Status.Accepted, payload);
		}

		public static global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse AlreadyConnected(long localClientId, string remoteEndpointId)
		{
			return new global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse(localClientId, remoteEndpointId, global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Status.ErrorAlreadyConnected, EmptyPayload);
		}
	}
}
