namespace Kampai.Game
{
	public class GoToNextQuestStateCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GoToNextQuestStateCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestScriptService questScriptService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.StartTimedQuestSignal startTimedQuestSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowQuestPanelSignal showQuestPanelSignal { get; set; }

		[Inject]
		public int questDefinitionID { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.IQuestController questControllerByDefinitionID = questService.GetQuestControllerByDefinitionID(questDefinitionID);
			if (questControllerByDefinitionID != null)
			{
				GotoNextQuestState(questControllerByDefinitionID);
				return;
			}
			logger.Error("Quest Controller with def Id {0} doesn't exist in quest map", questDefinitionID);
		}

		public void GotoNextQuestState(global::Kampai.Game.IQuestController questController)
		{
			switch (questController.State)
			{
			case global::Kampai.Game.QuestState.Notstarted:
				EventsForQuestStart(questController);
				CheckRunningStartScriptState(questController);
				break;
			case global::Kampai.Game.QuestState.RunningStartScript:
				CheckRunningTasksState(questController);
				break;
			case global::Kampai.Game.QuestState.RunningTasks:
				CheckRunningCompleteScriptState(questController);
				break;
			case global::Kampai.Game.QuestState.RunningCompleteScript:
				CheckHarvestableState(questController);
				break;
			case global::Kampai.Game.QuestState.Harvestable:
				questController.GoToQuestState(global::Kampai.Game.QuestState.Complete);
				break;
			}
		}

		private void CheckRunningStartScriptState(global::Kampai.Game.IQuestController questController)
		{
			if (!questScriptService.HasScript(questController.ID, true))
			{
				CheckRunningTasksState(questController);
			}
			else
			{
				questController.GoToQuestState(global::Kampai.Game.QuestState.RunningStartScript);
			}
		}

		private void CheckRunningTasksState(global::Kampai.Game.IQuestController questController)
		{
			if (questController.StepCount == 0)
			{
				CheckRunningCompleteScriptState(questController);
				return;
			}
			questController.GoToQuestState(global::Kampai.Game.QuestState.RunningTasks);
			if (questController.Definition.SurfaceType != global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated)
			{
				showQuestPanelSignal.Dispatch(questController.ID);
			}
		}

		private void CheckRunningCompleteScriptState(global::Kampai.Game.IQuestController questController)
		{
			if (!questScriptService.HasScript(questController.ID, false))
			{
				CheckHarvestableState(questController);
			}
			else
			{
				questController.GoToQuestState(global::Kampai.Game.QuestState.RunningCompleteScript);
			}
		}

		private void CheckHarvestableState(global::Kampai.Game.IQuestController questController)
		{
			if (questController.Definition.GetReward(definitionService) != null)
			{
				questController.GoToQuestState(global::Kampai.Game.QuestState.Harvestable);
			}
			else
			{
				questController.GoToQuestState(global::Kampai.Game.QuestState.Complete);
			}
		}

		private void EventsForQuestStart(global::Kampai.Game.IQuestController questController)
		{
			string questGiver = string.Empty;
			global::Kampai.Game.QuestDefinition definition = questController.Definition;
			if (definition.SurfaceType == global::Kampai.Game.QuestSurfaceType.Character)
			{
				global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(definition.SurfaceID, false);
				if (prestige != null)
				{
					questGiver = prestige.Definition.LocalizedKey;
				}
			}
			if (definition.QuestVersion != -1)
			{
				telemetryService.Send_Telemetry_EVT_GP_ACHIEVEMENTS_STARTED_EAL(questService.GetEventName(definition.LocalizedKey), global::Kampai.Common.Service.Telemetry.TelemetryAchievementType.Quest, questGiver);
			}
			if (questController.Definition.SurfaceType == global::Kampai.Game.QuestSurfaceType.TimedEvent)
			{
				startTimedQuestSignal.Dispatch(questController.ID);
			}
		}
	}
}
