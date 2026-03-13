namespace Kampai.UI.View
{
	public interface IParentWayFinderView : global::Kampai.UI.View.IWayFinderView, global::Kampai.UI.View.IWorldToGlassView
	{
		global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.IChildWayFinderView> ChildrenWayFinders { get; }

		void AddChildWayFinder(global::Kampai.UI.View.IChildWayFinderView childWayFinderView);

		void RemoveChildWayFinder(int childTrackedId);

		void UpdateWayFinderIcon();
	}
}
