namespace Kampai.Game
{
	public class QuestTaskStateChangeCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.UpdateQuestWorldIconsSignal updateWorldIconSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateProceduralQuestPanelSignal updateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displayPlayerTrainingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Quest quest { get; set; }

		[Inject]
		public int stepIndex { get; set; }

		[Inject]
		public global::Kampai.Game.QuestStepState previousState { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.QuestStepState state = quest.Steps[stepIndex].state;
			int iD = quest.GetActiveDefinition().ID;
			global::Kampai.Game.IQuestController questControllerByDefinitionID = questService.GetQuestControllerByDefinitionID(iD);
			global::Kampai.Game.QuestDefinition definition = questControllerByDefinitionID.Definition;
			if (previousState == global::Kampai.Game.QuestStepState.Notstarted && definition.QuestVersion != -1)
			{
				telemetryService.Send_Telemetry_EVT_GP_ACHIEVEMENTS_STARTED_EAL(questService.GetEventName(quest.GetActiveDefinition().LocalizedKey), global::Kampai.Common.Service.Telemetry.TelemetryAchievementType.QuestStep, string.Empty);
			}
			if (previousState == global::Kampai.Game.QuestStepState.Ready && state == global::Kampai.Game.QuestStepState.Inprogress)
			{
				updateWorldIconSignal.Dispatch(quest);
			}
			switch (state)
			{
			case global::Kampai.Game.QuestStepState.Ready:
				OnStepReady(definition);
				break;
			case global::Kampai.Game.QuestStepState.WaitComplete:
				questService.UnlockMinionParty(iD);
				break;
			case global::Kampai.Game.QuestStepState.Complete:
				OnStepComplete(questControllerByDefinitionID, iD);
				break;
			case global::Kampai.Game.QuestStepState.RunningCompleteScript:
				break;
			}
		}

		private void OnStepReady(global::Kampai.Game.QuestDefinition questDefinition)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestStepDefinition> questSteps = questDefinition.QuestSteps;
			if (questDefinition.SurfaceType == global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated && questSteps != null && questSteps.Count > 0 && questSteps[0].Type != global::Kampai.Game.QuestStepType.OrderBoard)
			{
				updateSignal.Dispatch(quest.ID);
			}
			updateWorldIconSignal.Dispatch(quest);
		}

		private void OnStepComplete(global::Kampai.Game.IQuestController questController, int definitionID)
		{
			global::Kampai.Game.QuestStepDefinition questStepDefinition = quest.GetActiveDefinition().QuestSteps[stepIndex];
			displayPlayerTrainingSignal.Dispatch(questStepDefinition.QuestStepCompletePlayerTrainingCategoryItemId, false, new global::strange.extensions.signal.impl.Signal<bool>());
			questController.CheckAndUpdateQuestCompleteState();
			if (questController.Definition.QuestVersion != -1)
			{
				telemetryService.Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL(questService.GetEventName(quest.GetActiveDefinition().LocalizedKey), global::Kampai.Common.Service.Telemetry.TelemetryAchievementType.QuestStep, 0, string.Empty);
			}
			questService.UnlockMinionParty(definitionID);
		}
	}
}
