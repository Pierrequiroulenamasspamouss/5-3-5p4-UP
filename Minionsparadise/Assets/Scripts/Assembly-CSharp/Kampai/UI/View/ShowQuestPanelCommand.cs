namespace Kampai.UI.View
{
	public class ShowQuestPanelCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int questInstanceID { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal changeState { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.IQuestController questControllerByInstanceID = questService.GetQuestControllerByInstanceID(questInstanceID);
			global::Kampai.Game.QuestState state = questControllerByInstanceID.State;
			if (state == global::Kampai.Game.QuestState.RunningTasks || state == global::Kampai.Game.QuestState.RunningCompleteScript || state == global::Kampai.Game.QuestState.Harvestable)
			{
				CreateQuestPanel();
			}
			if (state == global::Kampai.Game.QuestState.Notstarted || state == global::Kampai.Game.QuestState.RunningStartScript)
			{
				for (int i = 0; i < questControllerByInstanceID.StepCount; i++)
				{
					global::Kampai.Game.IQuestStepController stepController = questControllerByInstanceID.GetStepController(i);
					if (stepController.StepType == global::Kampai.Game.QuestStepType.BridgeRepair)
					{
						changeState.Dispatch(stepController.StepInstanceTrackedID, global::Kampai.Game.BuildingState.Working);
					}
				}
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.GoToNextQuestStateSignal>().Dispatch(questControllerByInstanceID.Definition.ID);
			}
			questControllerByInstanceID.UpdateTask(global::Kampai.Game.QuestStepType.Delivery);
			questControllerByInstanceID.UpdateTask(global::Kampai.Game.QuestStepType.ThrowParty);
			questControllerByInstanceID.UpdateTask(global::Kampai.Game.QuestStepType.Harvest);
		}

		private void CreateQuestPanel()
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CancelBuildingMovementSignal>().Dispatch(false);
			showHUDSignal.Dispatch(true);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_QuestPanel");
			iGUICommand.skrimScreen = "QuestPanelSkrim";
			iGUICommand.darkSkrim = true;
			iGUICommand.singleSkrimClose = true;
			iGUICommand.Args.Add(questInstanceID);
			guiService.Execute(iGUICommand);
		}
	}
}
