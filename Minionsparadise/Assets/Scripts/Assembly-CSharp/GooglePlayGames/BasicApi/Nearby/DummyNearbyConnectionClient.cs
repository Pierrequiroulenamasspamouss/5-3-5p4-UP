namespace GooglePlayGames.BasicApi.Nearby
{
	public class DummyNearbyConnectionClient : global::GooglePlayGames.BasicApi.Nearby.INearbyConnectionClient
	{
		public int MaxUnreliableMessagePayloadLength()
		{
			return 1168;
		}

		public int MaxReliableMessagePayloadLength()
		{
			return 4096;
		}

		public void SendReliable(global::System.Collections.Generic.List<string> recipientEndpointIds, byte[] payload)
		{
			global::UnityEngine.Debug.LogError("SendReliable called from dummy implementation");
		}

		public void SendUnreliable(global::System.Collections.Generic.List<string> recipientEndpointIds, byte[] payload)
		{
			global::UnityEngine.Debug.LogError("SendUnreliable called from dummy implementation");
		}

		public void StartAdvertising(string name, global::System.Collections.Generic.List<string> appIdentifiers, global::System.TimeSpan? advertisingDuration, global::System.Action<global::GooglePlayGames.BasicApi.Nearby.AdvertisingResult> resultCallback, global::System.Action<global::GooglePlayGames.BasicApi.Nearby.ConnectionRequest> connectionRequestCallback)
		{
			global::GooglePlayGames.BasicApi.Nearby.AdvertisingResult obj = new global::GooglePlayGames.BasicApi.Nearby.AdvertisingResult(global::GooglePlayGames.BasicApi.ResponseStatus.LicenseCheckFailed, string.Empty);
			resultCallback(obj);
		}

		public void StopAdvertising()
		{
			global::UnityEngine.Debug.LogError("StopAvertising in dummy implementation called");
		}

		public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, global::System.Action<global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse> responseCallback, global::GooglePlayGames.BasicApi.Nearby.IMessageListener listener)
		{
			global::UnityEngine.Debug.LogError("SendConnectionRequest called from dummy implementation");
			if (responseCallback != null)
			{
				global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse obj = global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Rejected(0L, string.Empty);
				responseCallback(obj);
			}
		}

		public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, global::GooglePlayGames.BasicApi.Nearby.IMessageListener listener)
		{
			global::UnityEngine.Debug.LogError("AcceptConnectionRequest in dummy implementation called");
		}

		public void StartDiscovery(string serviceId, global::System.TimeSpan? advertisingTimeout, global::GooglePlayGames.BasicApi.Nearby.IDiscoveryListener listener)
		{
			global::UnityEngine.Debug.LogError("StartDiscovery in dummy implementation called");
		}

		public void StopDiscovery(string serviceId)
		{
			global::UnityEngine.Debug.LogError("StopDiscovery in dummy implementation called");
		}

		public void RejectConnectionRequest(string requestingEndpointId)
		{
			global::UnityEngine.Debug.LogError("RejectConnectionRequest in dummy implementation called");
		}

		public void DisconnectFromEndpoint(string remoteEndpointId)
		{
			global::UnityEngine.Debug.LogError("DisconnectFromEndpoint in dummy implementation called");
		}

		public void StopAllConnections()
		{
			global::UnityEngine.Debug.LogError("StopAllConnections in dummy implementation called");
		}

		public string LocalEndpointId()
		{
			return string.Empty;
		}

		public string LocalDeviceId()
		{
			return "DummyDevice";
		}

		public string GetAppBundleId()
		{
			return "dummy.bundle.id";
		}

		public string GetServiceId()
		{
			return "dummy.service.id";
		}
	}
}
