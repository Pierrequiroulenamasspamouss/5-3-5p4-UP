namespace Kampai.Game
{
	internal sealed class CreateMasterPlanCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlanDefinition masterPlanDefinition { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.PurchaseNewBuildingSignal purchaseNewBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.EnableAllVillainLairCollidersSignal enableAllVillainLairCollidersSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayfinderSignal { get; set; }

		public override void Execute()
		{
			moveAudioListenerSignal.Dispatch(true, null);
			global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(masterPlanDefinition.BuildingDefID);
			global::Kampai.Game.VillainLairDefinition villainLairDefinition = definitionService.Get<global::Kampai.Game.VillainLairDefinition>(3137);
			global::Kampai.Game.Building building = masterPlanComponentBuildingDefinition.BuildBuilding();
			building.Location = villainLairDefinition.Platforms[villainLairDefinition.Platforms.Count - 1].placementLocation;
			playerService.Add(building);
			purchaseNewBuildingSignal.Dispatch(building);
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::System.Collections.Generic.List<int> compBuildingDefinitionIDs = masterPlanDefinition.CompBuildingDefinitionIDs;
			for (int i = 0; i < compBuildingDefinitionIDs.Count; i++)
			{
				int definitionId = compBuildingDefinitionIDs[i];
				global::Kampai.Game.MasterPlanComponentBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponentBuilding>(definitionId);
				if (firstInstanceByDefinitionId != null)
				{
					string buildingRemovalAnimController = villainLairDefinition.Platforms[i].buildingRemovalAnimController;
					global::Kampai.Game.View.MasterPlanComponentBuildingObject masterPlanComponentBuildingObject = component.GetBuildingObject(firstInstanceByDefinitionId.ID) as global::Kampai.Game.View.MasterPlanComponentBuildingObject;
					masterPlanComponentBuildingObject.TriggerMasterPlanCompleteAnimation(buildingRemovalAnimController);
				}
			}
			HandleCleanup();
		}

		private void HandleCleanup()
		{
			global::Kampai.Game.Villain firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Villain>(masterPlanDefinition.VillainCharacterDefID);
			removeWayfinderSignal.Dispatch(firstInstanceByDefinitionId.ID);
			enableAllVillainLairCollidersSignal.Dispatch(true);
			globalSFXSignal.Dispatch("Play_componentsFlyIntoMP_01");
		}
	}
}
