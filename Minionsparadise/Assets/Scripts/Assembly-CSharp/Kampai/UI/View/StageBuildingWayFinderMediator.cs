namespace Kampai.UI.View
{
	public class StageBuildingWayFinderMediator : global::Kampai.UI.View.AbstractWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.StageBuildingWayFinderView StageBuildingWayFinderView { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return StageBuildingWayFinderView;
			}
		}
	}
}
