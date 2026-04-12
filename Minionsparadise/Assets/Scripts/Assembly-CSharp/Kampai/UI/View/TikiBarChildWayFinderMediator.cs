namespace Kampai.UI.View
{
	public class TikiBarChildWayFinderMediator : global::Kampai.UI.View.AbstractChildWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.TikiBarChildWayFinderView TikiBarChildWayFinderView { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return TikiBarChildWayFinderView;
			}
		}
	}
}
