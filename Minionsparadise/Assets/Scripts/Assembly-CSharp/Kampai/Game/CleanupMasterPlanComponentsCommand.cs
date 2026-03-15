namespace Kampai.Game
{
	public class CleanupMasterPlanComponentsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int componentBuildingId { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveBuildingSignal removeBuildingSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MasterPlanComponentBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MasterPlanComponentBuilding>(componentBuildingId);
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			component.RemoveBuilding(componentBuildingId);
			removeBuildingSignal.Dispatch(byInstanceId.Location, definitionService.GetBuildingFootprint(byInstanceId.Definition.FootprintID));
			playerService.Remove(byInstanceId);
		}
	}
}
