namespace Kampai.Common
{
	public class ShowSocialPartyRewardCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ShowSocialPartyRewardCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int eventIndex { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimedSocialEventService timedSocialEventService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal spawnDooberSignal { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkConnectionLostSignal networkConnectionLostSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SocialOrderBoardCompleteSignal orderBoardCompleteSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
			if (firstInstanceByDefinitionId != null)
			{
				global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
				global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(firstInstanceByDefinitionId.ID);
				global::Kampai.Game.View.StageBuildingObject stageBuildingObject = buildingObject as global::Kampai.Game.View.StageBuildingObject;
				if (stageBuildingObject != null)
				{
					stageBuildingObject.UpdateStageState(global::Kampai.Game.BuildingState.SocialComplete);
					logger.Info("Social Event has been Completed! Collecting rewards.");
				}
				global::Kampai.Game.TimedSocialEventDefinition socialEvent = timedSocialEventService.GetSocialEvent(eventIndex);
				global::Kampai.Game.Transaction.TransactionDefinition reward = socialEvent.GetReward(definitionService);
				if (reward != null)
				{
					playerService.RunEntireTransaction(reward, global::Kampai.Game.TransactionTarget.NO_VISUAL, TransactionCallback);
				}
				else
				{
					logger.Info("Reward is null, nothing to do.");
				}
			}
		}

		public void TransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (!pct.Success)
			{
				logger.Error("CollectTransactionCallback PendingCurrencyTransaction was a failure.");
			}
			global::Kampai.Game.SocialTeamResponse socialEventStateCached = timedSocialEventService.GetSocialEventStateCached(eventIndex);
			if (socialEventStateCached == null)
			{
				logger.Error("SocialPartyRewardMediator: Failed to get cached event state for social event definition id {0}.", eventIndex);
				return;
			}
			if (socialEventStateCached.Team == null)
			{
				logger.Error("SocialPartyRewardMediator: Team is null.");
				return;
			}
			global::Kampai.Game.TimedSocialEventDefinition socialEvent = timedSocialEventService.GetSocialEvent(eventIndex);
			global::Kampai.Game.Transaction.TransactionDefinition reward = socialEvent.GetReward(definitionService);
			global::Kampai.Game.StageBuilding firstInstanceByDefintion = playerService.GetFirstInstanceByDefintion<global::Kampai.Game.StageBuilding, global::Kampai.Game.StageBuildingDefinition>();
			for (int i = 0; i < reward.Outputs.Count; i++)
			{
				global::Kampai.UI.View.DestinationType destinationType = DooberUtil.GetDestinationType(reward.ID, definitionService);
				spawnDooberSignal.Dispatch(uiCamera.WorldToScreenPoint(firstInstanceByDefintion.Location.ToVector3()), destinationType, reward.Outputs[i].ID, false);
			}
			global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse>();
			signal.AddListener(ClaimRewardResponse);
			timedSocialEventService.ClaimReward(eventIndex, timedSocialEventService.GetSocialEventStateCached(eventIndex).Team.ID, signal);
		}

		public void ClaimRewardResponse(global::Kampai.Game.SocialTeamResponse response, global::Kampai.Game.ErrorResponse error)
		{
			if (error != null)
			{
				networkConnectionLostSignal.Dispatch();
				return;
			}
			if (response != null && response.Team != null && response.Team.Members != null)
			{
				telemetryService.Send_Telemetry_EVT_SOCIAL_EVENT_COMPLETION(response.Team.Members.Count);
			}
			global::Kampai.Game.TimedSocialEventDefinition socialEvent = timedSocialEventService.GetSocialEvent(eventIndex);
			int iD = socialEvent.ID;
			if (timedSocialEventService.GetCurrentSocialEvent() == null || timedSocialEventService.GetCurrentSocialEvent().ID == iD)
			{
				global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
				if (firstInstanceByDefinitionId != null)
				{
					removeWayFinderSignal.Dispatch(firstInstanceByDefinitionId.ID);
				}
			}
			orderBoardCompleteSignal.Dispatch();
		}
	}
}
