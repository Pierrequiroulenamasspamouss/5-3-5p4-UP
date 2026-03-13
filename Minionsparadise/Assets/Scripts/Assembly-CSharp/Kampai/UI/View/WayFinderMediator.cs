namespace Kampai.UI.View
{
	public class WayFinderMediator : global::Kampai.UI.View.AbstractWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.WayFinderView WayFinderView { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return WayFinderView;
			}
		}
	}
}
