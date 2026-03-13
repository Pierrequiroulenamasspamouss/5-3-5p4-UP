namespace Kampai.UI.View
{
	public class CabanaChildWayFinderMediator : global::Kampai.UI.View.AbstractChildWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.CabanaChildWayFinderView CabanaChildWayFinderView { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return CabanaChildWayFinderView;
			}
		}
	}
}
