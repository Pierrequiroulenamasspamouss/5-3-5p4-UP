namespace GooglePlayGames.BasicApi.Nearby
{
	public interface INearbyConnectionClient
	{
		int MaxUnreliableMessagePayloadLength();

		int MaxReliableMessagePayloadLength();

		void SendReliable(global::System.Collections.Generic.List<string> recipientEndpointIds, byte[] payload);

		void SendUnreliable(global::System.Collections.Generic.List<string> recipientEndpointIds, byte[] payload);

		void StartAdvertising(string name, global::System.Collections.Generic.List<string> appIdentifiers, global::System.TimeSpan? advertisingDuration, global::System.Action<global::GooglePlayGames.BasicApi.Nearby.AdvertisingResult> resultCallback, global::System.Action<global::GooglePlayGames.BasicApi.Nearby.ConnectionRequest> connectionRequestCallback);

		void StopAdvertising();

		void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, global::System.Action<global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse> responseCallback, global::GooglePlayGames.BasicApi.Nearby.IMessageListener listener);

		void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, global::GooglePlayGames.BasicApi.Nearby.IMessageListener listener);

		void StartDiscovery(string serviceId, global::System.TimeSpan? advertisingTimeout, global::GooglePlayGames.BasicApi.Nearby.IDiscoveryListener listener);

		void StopDiscovery(string serviceId);

		void RejectConnectionRequest(string requestingEndpointId);

		void DisconnectFromEndpoint(string remoteEndpointId);

		void StopAllConnections();

		string LocalEndpointId();

		string LocalDeviceId();

		string GetAppBundleId();

		string GetServiceId();
	}
}
