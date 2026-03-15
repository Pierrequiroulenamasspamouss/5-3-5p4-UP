namespace Kampai.Game
{
	public interface IDLCService
	{
		int GetPlayerDLCTier();

		void SetPlayerDLCTier(int tier);

		void SetDownloadQualityLevel(global::Kampai.Util.TargetPerformance target);

		string GetDownloadQualityLevel();

		void SetDisplayQualityLevel(string qualityDef);

		string GetDisplayQualityLevel();
	}
}
