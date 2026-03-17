namespace Kampai.UI.View
{
	public abstract class AbstractChildWayFinderView : global::Kampai.UI.View.AbstractQuestWayFinderView, global::Kampai.UI.View.IChildWayFinderView, global::Kampai.UI.View.IWayFinderView, global::Kampai.UI.View.IWorldToGlassView
	{
		public int ParentWayFinderTrackedId { get; set; }
	}
}
