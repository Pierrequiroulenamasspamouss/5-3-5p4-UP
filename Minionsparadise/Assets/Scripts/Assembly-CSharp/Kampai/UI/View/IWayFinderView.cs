namespace Kampai.UI.View
{
	public interface IWayFinderView : global::Kampai.UI.View.IWorldToGlassView
	{
		bool ClickedOnce { get; set; }

		bool Snappable { get; set; }

		bool AvoidsHUD { get; set; }

		global::Kampai.Game.Prestige Prestige { get; }

		bool IsTargetObjectVisible();

		void SimulateClick();
	}
}
