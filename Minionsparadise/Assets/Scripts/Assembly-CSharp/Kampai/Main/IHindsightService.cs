namespace Kampai.Main
{
	public interface IHindsightService
	{
		void Initialize();

		void UpdateCache();

		global::Kampai.Main.HindsightCampaign GetCachedContent(global::Kampai.Main.HindsightCampaign.Scope scope);
	}
}
