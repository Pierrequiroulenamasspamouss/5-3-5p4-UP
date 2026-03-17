namespace Kampai.UI.View
{
	public class QuestWayFinderMediator : global::Kampai.UI.View.AbstractQuestWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.QuestWayFinderView QuestWayFinderView { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return QuestWayFinderView;
			}
		}
	}
}
