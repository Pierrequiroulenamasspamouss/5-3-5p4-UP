namespace Kampai.Game.Trigger
{
	public class RewardTriggerCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RewardTriggerCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.Trigger.TriggerInstance triggerInstance { get; set; }

		[Inject]
		public global::Kampai.Game.Trigger.TriggerRewardDefinition triggerReward { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseTSMModalSignal closeModalSignal { get; set; }

		[Inject]
		public global::Kampai.Game.HideTSMCharacterSignal HideTSMCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CheckTriggersSignal triggersSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.RewardTriggerSignal rewardTriggerSignal { get; set; }

		public override void Execute()
		{
			if (triggerInstance == null || triggerReward == null)
			{
				logger.Error("Null trigger");
				return;
			}
			int iD = triggerReward.ID;
			global::System.Collections.Generic.IList<int> recievedRewardIds = triggerInstance.RecievedRewardIds;
			if (triggerReward.IsUniquePerInstance && recievedRewardIds.Contains(iD))
			{
				logger.Warning("Duplicate reward {0}", iD);
				return;
			}
			global::Kampai.Game.Trigger.TriggerDefinition definition = triggerInstance.Definition;
			recievedRewardIds.Add(iD);
			triggerReward.RewardPlayer(gameContext);
			telemetryService.Send_Telemetry_EVT_TSM_TRIGGER_ACTION(definition, triggerReward);
			global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerRewardDefinition> rewards = definition.rewards;
			bool flag = false;
			global::Kampai.Game.Trigger.UpsellTriggerRewardDefinition upsellTriggerRewardDefinition = null;
			for (int i = 0; i < rewards.Count; i++)
			{
				global::Kampai.Game.Trigger.TriggerRewardDefinition triggerRewardDefinition = rewards[i];
				if (triggerRewardDefinition.type == global::Kampai.Game.Trigger.TriggerRewardType.Identifier.Upsell)
				{
					upsellTriggerRewardDefinition = triggerRewardDefinition as global::Kampai.Game.Trigger.UpsellTriggerRewardDefinition;
				}
				else if (!recievedRewardIds.Contains(triggerRewardDefinition.ID))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				logger.Debug("Trigger {0} on cooldown.", triggerInstance.Definition.ID);
				triggerInstance.StartGameTime = playerDurationService.TotalGamePlaySeconds;
				closeModalSignal.Dispatch();
				OnRewardCompleted();
				if (upsellTriggerRewardDefinition != null)
				{
					rewardTriggerSignal.Dispatch(triggerInstance, upsellTriggerRewardDefinition);
				}
			}
		}

		private void OnRewardCompleted()
		{
			global::Kampai.Game.TSMCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.TSMCharacter>(70008);
			if (firstInstanceByDefinitionId == null)
			{
				logger.Error("Failed to Cancel Trigger {0} because there isn't a traveling sales minion found", triggerInstance.Definition.ID);
				return;
			}
			if (triggerInstance.Definition.TreasureIntro)
			{
				HideTSMCharacterSignal.Dispatch(global::Kampai.Game.View.TSMCharacterHideState.Chest);
			}
			else
			{
				HideTSMCharacterSignal.Dispatch(global::Kampai.Game.View.TSMCharacterHideState.Celebrate);
			}
			firstInstanceByDefinitionId.Created = false;
			removeWayFinderSignal.Dispatch(301);
			firstInstanceByDefinitionId.PreviousTaskUTCTime = timeService.CurrentTime();
			timeEventService.AddEvent(firstInstanceByDefinitionId.ID, firstInstanceByDefinitionId.PreviousTaskUTCTime, firstInstanceByDefinitionId.Definition.CooldownInSeconds, triggersSignal);
		}
	}
}
