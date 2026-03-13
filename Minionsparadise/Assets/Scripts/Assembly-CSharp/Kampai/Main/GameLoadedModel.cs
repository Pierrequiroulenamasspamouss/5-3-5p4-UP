namespace Kampai.Main
{
	public class GameLoadedModel
	{
		public bool gameLoaded { get; set; }

		public long coldStartTime { get; set; }

		public GameLoadedModel()
		{
			gameLoaded = false;
			coldStartTime = -1L;
		}
	}
}
