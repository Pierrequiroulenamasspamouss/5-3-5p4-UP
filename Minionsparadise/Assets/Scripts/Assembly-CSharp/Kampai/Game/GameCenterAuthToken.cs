namespace Kampai.Game
{
	public class GameCenterAuthToken
	{
		public string playerId { get; set; }

		public string bundleId { get; set; }

		public string publicKeyUrl { get; set; }

		public string signature { get; set; }

		public string salt { get; set; }

		public string timestamp { get; set; }
	}
}
