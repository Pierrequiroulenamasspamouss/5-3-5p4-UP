namespace Kampai.Game
{
	public class StartTSMQuestTaskCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Quest quest { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.PanAndOpenModalSignal panAndOpenSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenStoreHighlightItemSignal openStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.GoToNextQuestStateSignal goToNextQuestStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveQuestWorldIconSignal removeQuestWorldIconSignal { get; set; }

		public override void Execute()
		{
			int iD = quest.GetActiveDefinition().ID;
			global::Kampai.Game.IQuestController questControllerByDefinitionID = questService.GetQuestControllerByDefinitionID(iD);
			if (questControllerByDefinitionID.State == global::Kampai.Game.QuestState.Notstarted)
			{
				goToNextQuestStateSignal.Dispatch(iD);
			}
			if (questControllerByDefinitionID.StepCount > 0)
			{
				global::Kampai.Game.IQuestStepController stepController = questControllerByDefinitionID.GetStepController(0);
				if (stepController.StepState == global::Kampai.Game.QuestStepState.Notstarted)
				{
					stepController.GoToNextState();
				}
				if (questControllerByDefinitionID.StepCount == 1)
				{
					removeQuestWorldIconSignal.Dispatch(quest);
				}
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> list = null;
			int num = 0;
			switch (quest.GetActiveDefinition().QuestSteps[0].Type)
			{
			default:
				return;
			case global::Kampai.Game.QuestStepType.OrderBoard:
				list = playerService.GetInstancesByDefinition<global::Kampai.Game.BlackMarketBoardDefinition>();
				num = 3022;
				break;
			case global::Kampai.Game.QuestStepType.MinionTask:
				num = quest.GetActiveDefinition().QuestSteps[0].ItemDefinitionID;
				list = playerService.GetInstancesByDefinitionID(num);
				break;
			case global::Kampai.Game.QuestStepType.BridgeRepair:
				return;
			}
			if (list.Count > 0)
			{
				global::Kampai.Game.Building building = FindPlacedBuilding(list);
				if (building != null)
				{
					panAndOpenSignal.Dispatch(list[0].ID, false);
				}
				else
				{
					uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.CloseAllOtherMenuSignal>().Dispatch(null);
				}
			}
			else if (num != 0)
			{
				openStoreSignal.Dispatch(num, true);
			}
		}

		private global::Kampai.Game.Building FindPlacedBuilding(global::System.Collections.Generic.IList<global::Kampai.Game.Instance> instances)
		{
			foreach (global::Kampai.Game.Instance instance in instances)
			{
				global::Kampai.Game.Building building = instance as global::Kampai.Game.Building;
				if (building != null && building.State != global::Kampai.Game.BuildingState.Inventory)
				{
					return building;
				}
			}
			return null;
		}
	}
}
