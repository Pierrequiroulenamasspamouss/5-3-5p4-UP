namespace Kampai.UI.View
{
	public class SpecialEventWayFinderMediator : global::Kampai.UI.View.AbstractQuestWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.SpecialEventWayFinderView SpecialEventWayFinderView { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return SpecialEventWayFinderView;
			}
		}
	}
}
