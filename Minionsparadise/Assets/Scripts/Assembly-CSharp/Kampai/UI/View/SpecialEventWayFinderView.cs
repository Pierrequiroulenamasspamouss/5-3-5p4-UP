namespace Kampai.UI.View
{
	public class SpecialEventWayFinderView : global::Kampai.UI.View.AbstractQuestWayFinderView
	{
		protected override string UIName
		{
			get
			{
				return "SpecialEventWayFinder";
			}
		}

		protected override string WayFinderDefaultIcon
		{
			get
			{
				return wayFinderDefinition.SpecialEventNewQuestIcon;
			}
		}

		protected override string WayFinderQuestCompleteIcon
		{
			get
			{
				return wayFinderDefinition.SpecialEventQuestCompleteIcon;
			}
		}

		protected override string WayFinderTaskCompleteIcon
		{
			get
			{
				return wayFinderDefinition.TaskCompleteIcon;
			}
		}
	}
}
