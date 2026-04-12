namespace Kampai.UI.View
{
	public class TSMTriggerWayFinderView : global::Kampai.UI.View.AbstractWayFinderView
	{
		private global::Kampai.Game.ITriggerService triggerService;

		private bool isPartyPaused;

		private bool TSMReachedDestination;

		protected override string UIName
		{
			get
			{
				return "TSMTriggerWayFinder";
			}
		}

		protected override string WayFinderDefaultIcon
		{
			get
			{
				if (triggerService == null)
				{
					triggerService = gameContext.injectionBinder.GetInstance<global::Kampai.Game.ITriggerService>();
				}
				if (triggerService != null && triggerService.ActiveTrigger != null)
				{
					string wayFinderIcon = triggerService.ActiveTrigger.Definition.WayFinderIcon;
					if (!string.IsNullOrEmpty(wayFinderIcon))
					{
						return wayFinderIcon;
					}
				}
				return wayFinderDefinition.TSMDefaultIcon;
			}
		}

		protected override bool OnCanUpdate()
		{
			return !zoomCameraModel.ZoomedIn && TSMReachedDestination;
		}

		public void SetDestinationReached()
		{
			TSMReachedDestination = true;
		}

		public void SetPartyPauseState(bool isPartyPaused)
		{
			this.isPartyPaused = isPartyPaused;
		}

		public override void SetForceHide(bool forceHide)
		{
			isForceHideEnabled = !isPartyPaused && forceHide;
		}
	}
}
