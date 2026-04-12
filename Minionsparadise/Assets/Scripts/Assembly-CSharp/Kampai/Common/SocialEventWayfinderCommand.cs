namespace Kampai.Common
{
	public class SocialEventWayfinderCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal createWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimedSocialEventService timedSocialEventService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
			global::Kampai.Game.BuildingState state = firstInstanceByDefinitionId.State;
			if (state == global::Kampai.Game.BuildingState.Inactive || state == global::Kampai.Game.BuildingState.Construction || state == global::Kampai.Game.BuildingState.Disabled || state == global::Kampai.Game.BuildingState.Broken || state == global::Kampai.Game.BuildingState.Complete || state == global::Kampai.Game.BuildingState.Inaccessible)
			{
				return;
			}
			global::System.Collections.Generic.IList<int> pastEventsWithUnclaimedReward = timedSocialEventService.GetPastEventsWithUnclaimedReward();
			if (pastEventsWithUnclaimedReward.Count > 0)
			{
				createWayFinderSignal.Dispatch(new global::Kampai.UI.View.WayFinderSettings(firstInstanceByDefinitionId.ID));
			}
			else if (timedSocialEventService.GetCurrentSocialEvent() != null)
			{
				global::Kampai.Game.SocialTeamResponse socialEventStateCached = timedSocialEventService.GetSocialEventStateCached(timedSocialEventService.GetCurrentSocialEvent().ID);
				if (socialEventStateCached == null || socialEventStateCached.UserEvent == null || !socialEventStateCached.UserEvent.RewardClaimed || socialEventStateCached.Team == null || socialEventStateCached.Team.OrderProgress == null || socialEventStateCached.Team.OrderProgress.Count != timedSocialEventService.GetCurrentSocialEvent().Orders.Count)
				{
					createWayFinderSignal.Dispatch(new global::Kampai.UI.View.WayFinderSettings(firstInstanceByDefinitionId.ID));
				}
			}
		}
	}
}
