namespace Kampai.Common
{
	public class SocialEventAvailableCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialEventAvailableCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.ITimedSocialEventService timedSocialEventService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
			if (firstInstanceByDefinitionId == null)
			{
				return;
			}
			global::Kampai.Game.Prestige firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(40003);
			if (firstInstanceByDefinitionId2 == null)
			{
				return;
			}
			global::Kampai.Game.BuildingState state = firstInstanceByDefinitionId.State;
			if (state == global::Kampai.Game.BuildingState.Inactive || state == global::Kampai.Game.BuildingState.Construction || state == global::Kampai.Game.BuildingState.Disabled || state == global::Kampai.Game.BuildingState.Broken || state == global::Kampai.Game.BuildingState.Complete || state == global::Kampai.Game.BuildingState.Inaccessible || (firstInstanceByDefinitionId2.state != global::Kampai.Game.PrestigeState.Taskable && firstInstanceByDefinitionId2.state != global::Kampai.Game.PrestigeState.TaskableWhileQuesting))
			{
				return;
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			if (quantity < firstInstanceByDefinitionId.Definition.SocialEventMinimumLevel)
			{
				return;
			}
			global::strange.extensions.injector.api.ICrossContextInjectionBinder crossContextInjectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.TimedSocialEventDefinition currentSocialEvent = timedSocialEventService.GetCurrentSocialEvent();
			if (currentSocialEvent != null)
			{
				crossContextInjectionBinder.GetInstance<global::Kampai.Game.SocialEventWayfinderSignal>().Dispatch();
			}
			else
			{
				global::System.Collections.Generic.IList<int> pastEventsWithUnclaimedReward = timedSocialEventService.GetPastEventsWithUnclaimedReward();
				if (pastEventsWithUnclaimedReward.Count > 0)
				{
					crossContextInjectionBinder.GetInstance<global::Kampai.Game.SocialEventWayfinderSignal>().Dispatch();
				}
			}
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(firstInstanceByDefinitionId.ID);
			global::Kampai.Game.View.StageBuildingObject stageBuildingObject = buildingObject as global::Kampai.Game.View.StageBuildingObject;
			if (stageBuildingObject != null && currentSocialEvent != null)
			{
				global::Kampai.Game.SocialTeamResponse socialEventStateCached = timedSocialEventService.GetSocialEventStateCached(timedSocialEventService.GetCurrentSocialEvent().ID);
				if (socialEventStateCached != null && socialEventStateCached.UserEvent != null && socialEventStateCached.UserEvent.RewardClaimed)
				{
					stageBuildingObject.UpdateStageState(global::Kampai.Game.BuildingState.Idle);
					return;
				}
				logger.Warning("Social Event is Available");
				stageBuildingObject.UpdateStageState(global::Kampai.Game.BuildingState.SocialAvailable);
				crossContextInjectionBinder.GetInstance<global::Kampai.Game.AddStuartToStageSignal>().Dispatch(global::Kampai.Game.StuartStageAnimationType.IDLEONSTAGE);
			}
		}
	}
}
