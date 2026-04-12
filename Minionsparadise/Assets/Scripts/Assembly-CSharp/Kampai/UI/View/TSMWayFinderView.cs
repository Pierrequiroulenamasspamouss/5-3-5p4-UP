namespace Kampai.UI.View
{
	public class TSMWayFinderView : global::Kampai.UI.View.AbstractQuestWayFinderView
	{
		private bool TSMReachedDestination;

		protected override string UIName
		{
			get
			{
				return "TSMWayFinder";
			}
		}

		protected override string WayFinderDefaultIcon
		{
			get
			{
				return wayFinderDefinition.TSMDefaultIcon;
			}
		}

		protected override bool CanUpdateQuestIcon()
		{
			return false;
		}

		public void SetDestinationReached()
		{
			TSMReachedDestination = true;
		}

		protected override bool OnCanUpdate()
		{
			if (!TSMReachedDestination)
			{
				return false;
			}
			return base.OnCanUpdate();
		}
	}
}
