namespace Kampai.Splash
{
	public interface ILoadInService
	{
		global::Kampai.Splash.TipToShow GetNextTip();

		void SaveTipsForNextLaunch(int level);
	}
}
