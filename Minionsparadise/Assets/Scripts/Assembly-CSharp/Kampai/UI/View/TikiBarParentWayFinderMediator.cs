namespace Kampai.UI.View
{
	public class TikiBarParentWayFinderMediator : global::Kampai.UI.View.AbstractParentWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.TikiBarParentWayFinderView TikiBarParentWayFinderView { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return TikiBarParentWayFinderView;
			}
		}
	}
}
