namespace Kampai.Game
{
	public class CleanupMasterPlanCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlan plan { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveBuildingSignal removeBuildingSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.MasterPlanComponentBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponentBuilding>(plan.Definition.BuildingDefID);
			if (firstInstanceByDefinitionId == null)
			{
				return;
			}
			component.RemoveBuilding(firstInstanceByDefinitionId.ID);
			removeBuildingSignal.Dispatch(firstInstanceByDefinitionId.Location, definitionService.GetBuildingFootprint(firstInstanceByDefinitionId.Definition.FootprintID));
			playerService.Remove(firstInstanceByDefinitionId);
			for (int i = 0; i < plan.Definition.ComponentDefinitionIDs.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(plan.Definition.ComponentDefinitionIDs[i]);
				if (firstInstanceByDefinitionId2 != null)
				{
					playerService.Remove(firstInstanceByDefinitionId2);
				}
			}
		}
	}
}
