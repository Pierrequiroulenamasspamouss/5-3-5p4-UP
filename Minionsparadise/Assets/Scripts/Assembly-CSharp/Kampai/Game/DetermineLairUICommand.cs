namespace Kampai.Game
{
	public class DetermineLairUICommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanCooldownAlertSignal displayAlertUISignal { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanSelectComponentSignal selectComponentSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayMasterPlanSelectComponentSimpleSignal displaySelectSimpleSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanIntroDialogSignal displayMasterPlanIntroDialogSignal { get; set; }

		[Inject]
		public global::Kampai.Game.QuestHarvestableSignal questHarvestSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		public override void Execute()
		{
			questService.UpdateMasterPlanQuestLine();
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			global::Kampai.Game.MasterPlanDefinition definition = currentMasterPlan.Definition;
			global::Kampai.Game.MasterPlanComponentBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponentBuilding>(definition.BuildingDefID);
			if (currentMasterPlan.cooldownUTCStartTime > 0)
			{
				displayAlertUISignal.Dispatch(currentMasterPlan);
			}
			else if (!currentMasterPlan.introHasBeenDisplayed)
			{
				displayMasterPlanIntroDialogSignal.Dispatch();
			}
			else
			{
				bool allComplete;
				int selectedIndex;
				if (SelectOrHarvest(definition, out allComplete, out selectedIndex))
				{
					return;
				}
				if (allComplete && (firstInstanceByDefinitionId == null || firstInstanceByDefinitionId.State == global::Kampai.Game.BuildingState.Idle))
				{
					if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.State == global::Kampai.Game.BuildingState.Idle)
					{
						global::Kampai.Game.Quest questByInstanceId = masterPlanQuestService.GetQuestByInstanceId(currentMasterPlan.ID);
						questHarvestSignal.Dispatch(questByInstanceId);
					}
					else
					{
						selectComponentSignal.Dispatch(definition, -1, true);
					}
				}
				else if (!allComplete || firstInstanceByDefinitionId == null)
				{
					if (selectedIndex != -1)
					{
						selectComponentSignal.Dispatch(definition, selectedIndex, false);
					}
					else
					{
						displaySelectSimpleSignal.Dispatch();
					}
				}
			}
		}

		private bool SelectOrHarvest(global::Kampai.Game.MasterPlanDefinition planDef, out bool allComplete, out int selectedIndex)
		{
			allComplete = true;
			selectedIndex = -1;
			for (int i = 0; i < planDef.ComponentDefinitionIDs.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(planDef.ComponentDefinitionIDs[i]);
				if (firstInstanceByDefinitionId != null)
				{
					if (firstInstanceByDefinitionId.State == global::Kampai.Game.MasterPlanComponentState.InProgress || firstInstanceByDefinitionId.State == global::Kampai.Game.MasterPlanComponentState.Scaffolding || firstInstanceByDefinitionId.State == global::Kampai.Game.MasterPlanComponentState.TasksCollected)
					{
						selectedIndex = i;
					}
					else
					{
						if (firstInstanceByDefinitionId.State == global::Kampai.Game.MasterPlanComponentState.TasksComplete)
						{
							global::Kampai.Game.Quest questByInstanceId = masterPlanQuestService.GetQuestByInstanceId(firstInstanceByDefinitionId.ID);
							questHarvestSignal.Dispatch(questByInstanceId);
							return true;
						}
						if (firstInstanceByDefinitionId.State == global::Kampai.Game.MasterPlanComponentState.Built)
						{
							global::Kampai.Game.Quest questByInstanceId2 = masterPlanQuestService.GetQuestByInstanceId(711);
							questHarvestSignal.Dispatch(questByInstanceId2);
							return true;
						}
					}
					if (firstInstanceByDefinitionId.State != global::Kampai.Game.MasterPlanComponentState.Complete)
					{
						allComplete = false;
					}
					continue;
				}
				displaySelectSimpleSignal.Dispatch();
				return true;
			}
			return false;
		}
	}
}
