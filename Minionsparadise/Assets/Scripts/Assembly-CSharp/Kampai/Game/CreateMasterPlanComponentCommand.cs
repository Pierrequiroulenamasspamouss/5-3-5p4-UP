namespace Kampai.Game
{
	public class CreateMasterPlanComponentCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlanDefinition masterPlanDefinition { get; set; }

		[Inject]
		public int componentIndex { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.PurchaseNewBuildingSignal purchaseNewBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableOneVillainLairColliderSignal enableOneVillainLairColliderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayfinderSignal { get; set; }

		public override void Execute()
		{
			int id = masterPlanDefinition.ComponentDefinitionIDs[componentIndex];
			global::Kampai.Game.MasterPlanComponentDefinition masterPlanComponentDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentDefinition>(id);
			int num = masterPlanDefinition.CompBuildingDefinitionIDs[componentIndex];
			global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(masterPlanComponentDefinition.ID);
			firstInstanceByDefinitionId.State = global::Kampai.Game.MasterPlanComponentState.Scaffolding;
			global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(num);
			global::Kampai.Game.Building building = masterPlanComponentBuildingDefinition.BuildBuilding();
			building.Location = firstInstanceByDefinitionId.buildingLocation;
			playerService.Add(building);
			purchaseNewBuildingSignal.Dispatch(building);
			enableOneVillainLairColliderSignal.Dispatch(false, num);
			global::Kampai.Game.Villain firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Villain>(masterPlanDefinition.VillainCharacterDefID);
			removeWayfinderSignal.Dispatch(firstInstanceByDefinitionId2.ID);
		}
	}
}
