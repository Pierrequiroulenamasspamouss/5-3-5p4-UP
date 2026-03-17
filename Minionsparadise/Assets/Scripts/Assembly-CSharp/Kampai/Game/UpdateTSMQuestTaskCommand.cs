namespace Kampai.Game
{
	public class UpdateTSMQuestTaskCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UpdateTSMQuestTaskCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.Quest Quest { get; set; }

		[Inject]
		public bool Completed { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.Game.HideTSMCharacterSignal HideTSMCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveQuestWorldIconSignal RemoveQuestWorldIconSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CheckTriggersSignal TriggersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService QuestService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService TimeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService TimeService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService TelemetryService { get; set; }

		public override void Execute()
		{
			if (Quest == null)
			{
				logger.Error("Quest does not exist on traveling sales minion.");
				return;
			}
			global::Kampai.Game.TSMCharacter firstInstanceByDefinitionId = PlayerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.TSMCharacter>(70008);
			if (firstInstanceByDefinitionId == null)
			{
				logger.Error("Failed to Cancel QuestId {0} because there isn't a traveling sales minion found", Quest.ID);
				return;
			}
			HideTSM(firstInstanceByDefinitionId);
			if (Quest.dynamicDefinition == null)
			{
				logger.Error("Quest dynamic definition does not exist on traveling sales minion.");
				return;
			}
			int xPOutputForTransaction = global::Kampai.Game.Transaction.TransactionUtil.GetXPOutputForTransaction(Quest.GetActiveDefinition().GetReward(definitionService));
			string achievementName = new global::System.Text.StringBuilder().Append("ProceduralQuest").Append(Quest.dynamicDefinition.ID).ToString();
			TelemetryService.Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL_ProceduralQuest(achievementName, (!Completed) ? global::Kampai.Common.Service.Telemetry.ProceduralQuestEndState.Dismissed : global::Kampai.Common.Service.Telemetry.ProceduralQuestEndState.Completed, xPOutputForTransaction);
		}

		private void HideTSM(global::Kampai.Game.TSMCharacter tsmCharacter)
		{
			HideTSMCharacterSignal.Dispatch(Completed ? global::Kampai.Game.View.TSMCharacterHideState.Celebrate : global::Kampai.Game.View.TSMCharacterHideState.Hide);
			tsmCharacter.Created = false;
			RemoveQuestWorldIconSignal.Dispatch(Quest);
			tsmCharacter.PreviousTaskUTCTime = TimeService.CurrentTime();
			QuestService.RemoveQuest(Quest.GetActiveDefinition().ID);
			TimeEventService.AddEvent(tsmCharacter.ID, tsmCharacter.PreviousTaskUTCTime, tsmCharacter.Definition.CooldownInSeconds, TriggersSignal);
		}
	}
}
