namespace Kampai.UI.View
{
	public class OrderBoardWayFinderMediator : global::Kampai.UI.View.AbstractWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.OrderBoardWayFinderView OrderBoardWayFinderView { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return OrderBoardWayFinderView;
			}
		}
	}
}
