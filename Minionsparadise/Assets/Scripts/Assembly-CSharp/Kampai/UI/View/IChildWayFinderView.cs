namespace Kampai.UI.View
{
	public interface IChildWayFinderView : global::Kampai.UI.View.IWayFinderView, global::Kampai.UI.View.IWorldToGlassView
	{
		int ParentWayFinderTrackedId { get; set; }
	}
}
