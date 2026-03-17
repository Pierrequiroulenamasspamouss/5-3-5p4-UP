namespace GooglePlayGames.BasicApi.Nearby
{
	public struct ConnectionRequest
	{
		private readonly global::GooglePlayGames.BasicApi.Nearby.EndpointDetails mRemoteEndpoint;

		private readonly byte[] mPayload;

		public global::GooglePlayGames.BasicApi.Nearby.EndpointDetails RemoteEndpoint
		{
			get
			{
				return mRemoteEndpoint;
			}
		}

		public byte[] Payload
		{
			get
			{
				return mPayload;
			}
		}

		public ConnectionRequest(string remoteEndpointId, string remoteDeviceId, string remoteEndpointName, string serviceId, byte[] payload)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Constructing ConnectionRequest");
			mRemoteEndpoint = new global::GooglePlayGames.BasicApi.Nearby.EndpointDetails(remoteEndpointId, remoteDeviceId, remoteEndpointName, serviceId);
			mPayload = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(payload);
		}
	}
}
