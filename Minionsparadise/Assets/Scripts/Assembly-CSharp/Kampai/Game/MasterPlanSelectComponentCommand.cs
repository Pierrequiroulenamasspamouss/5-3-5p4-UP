namespace Kampai.Game
{
	public class MasterPlanSelectComponentCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlanDefinition masterPlanDefinition { get; set; }

		[Inject]
		public int componentIndex { get; set; }

		[Inject]
		public bool showPlan { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllMessageDialogsSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.IGhostComponentService ghostService { get; set; }

		[Inject]
		public global::Kampai.Game.EnableOneVillainLairColliderSignal enableOneVillainLairColliderSignal { get; set; }

		public override void Execute()
		{
			closeAllMessageDialogsSignal.Dispatch();
			int num = 0;
			for (int i = 0; i < masterPlanDefinition.ComponentDefinitionIDs.Count; i++)
			{
				int definitionId = masterPlanDefinition.ComponentDefinitionIDs[i];
				int type = masterPlanDefinition.CompBuildingDefinitionIDs[i];
				global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(definitionId);
				if (i == componentIndex)
				{
					if (firstInstanceByDefinitionId.State == global::Kampai.Game.MasterPlanComponentState.NotStarted)
					{
						firstInstanceByDefinitionId.State = global::Kampai.Game.MasterPlanComponentState.InProgress;
					}
					num = ((firstInstanceByDefinitionId.State < global::Kampai.Game.MasterPlanComponentState.TasksCollected) ? firstInstanceByDefinitionId.ID : 711);
					masterPlanService.SelectMasterPlanComponent(firstInstanceByDefinitionId);
					ghostService.DisplayComponentMarkedAsInProgress(firstInstanceByDefinitionId);
					enableOneVillainLairColliderSignal.Dispatch(false, type);
				}
			}
			if (num != 0 || showPlan)
			{
				global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
				CreateQuestPanel((!showPlan) ? num : currentMasterPlan.ID);
			}
		}

		private void CreateQuestPanel(int id)
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CancelBuildingMovementSignal>().Dispatch(false);
			showHUDSignal.Dispatch(true);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_QuestPanel");
			iGUICommand.skrimScreen = "QuestPanelSkrim";
			iGUICommand.darkSkrim = true;
			iGUICommand.singleSkrimClose = true;
			iGUICommand.Args.Add(id);
			guiService.Execute(iGUICommand);
		}
	}
}
