namespace Kampai.Game
{
	public class ConstructionCompleteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingID { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanComponentCompleteSignal componentCompleteSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingID);
			if (byInstanceId != null)
			{
				if (byInstanceId.Definition.ConstructionTime > 0 || byInstanceId is global::Kampai.Game.MasterPlanComponentBuilding)
				{
					buildingChangeStateSignal.Dispatch(buildingID, global::Kampai.Game.BuildingState.Complete);
				}
				else
				{
					buildingChangeStateSignal.Dispatch(buildingID, global::Kampai.Game.BuildingState.Idle);
				}
			}
			componentCompleteSignal.Dispatch(buildingID);
			questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Construction);
		}
	}
}
