namespace Kampai.Tools.AnimationToolKit
{
	public class AnimationToolKitDLCService : global::Kampai.Game.IDLCService
	{
		public int GetPlayerDLCTier()
		{
			throw new global::System.NotImplementedException();
		}

		public void SetPlayerDLCTier(int tier)
		{
			throw new global::System.NotImplementedException();
		}

		public void SetDownloadQualityLevel(global::Kampai.Util.TargetPerformance target)
		{
			throw new global::System.NotImplementedException();
		}

		public string GetDownloadQualityLevel()
		{
			return global::Kampai.Util.TargetPerformance.HIGH.ToString().ToLower();
		}

		public void SetDisplayQualityLevel(string qualityDef)
		{
			throw new global::System.NotImplementedException();
		}

		public string GetDisplayQualityLevel()
		{
			throw new global::System.NotImplementedException();
		}
	}
}
