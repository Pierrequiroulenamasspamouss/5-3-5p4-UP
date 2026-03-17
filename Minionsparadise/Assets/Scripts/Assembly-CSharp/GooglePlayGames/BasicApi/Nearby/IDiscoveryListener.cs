namespace GooglePlayGames.BasicApi.Nearby
{
	public interface IDiscoveryListener
	{
		void OnEndpointFound(global::GooglePlayGames.BasicApi.Nearby.EndpointDetails discoveredEndpoint);

		void OnEndpointLost(string lostEndpointId);
	}
}
