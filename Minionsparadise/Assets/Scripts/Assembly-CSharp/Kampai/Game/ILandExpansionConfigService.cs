namespace Kampai.Game
{
	public interface ILandExpansionConfigService
	{
		global::System.Collections.Generic.IList<int> GetExpansionIds();

		global::Kampai.Game.LandExpansionConfig GetExpansionConfig(int expansion);
	}
}
