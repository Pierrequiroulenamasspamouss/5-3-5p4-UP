namespace Kampai.Game.View
{
	public class RepairBridgeCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.BridgeBuilding building { get; set; }

		[Inject]
		public global::Kampai.Game.CreateInventoryBuildingSignal createInventroyBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal changeState { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.PurchaseLandExpansionSignal landExpansionSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveBuildingSignal removeBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveSignal autoMoveSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DebugUpdateGridSignal gridSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AddFootprintSignal addFootprintSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		public override void Execute()
		{
			routineRunner.StartCoroutine(WaitAFrame());
		}

		private global::System.Collections.IEnumerator WaitAFrame()
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			int buildingId = building.ID;
			global::Kampai.Game.BridgeDefinition bridgeDef = definitionService.GetBridgeDefinition(building.BridgeId);
			global::Kampai.Game.View.BuildingManagerView buildingManagerView = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildObj = buildingManagerView.GetBuildingObject(buildingId);
			buildingManagerView.RemoveBuilding(buildingId);
			global::Kampai.Game.Location location = building.Location;
			removeBuildingSignal.Dispatch(location, definitionService.GetBuildingFootprint(building.Definition.FootprintID));
			playerService.Remove(building);
			global::Kampai.Game.BridgeBuildingDefinition bridgeBuildingDef = definitionService.Get(bridgeDef.RepairedBuildingID) as global::Kampai.Game.BridgeBuildingDefinition;
			global::Kampai.Game.BridgeBuilding bridge = bridgeBuildingDef.Build() as global::Kampai.Game.BridgeBuilding;
			bridge.Location = location;
			bridge.BridgeId = 0;
			playerService.Add(bridge);
			createInventroyBuildingSignal.Dispatch(bridge, location);
			changeState.Dispatch(bridge.ID, global::Kampai.Game.BuildingState.Idle);
			gridSignal.Dispatch();
			buildObj = buildingManagerView.GetBuildingObject(bridge.ID);
			buildObj.SetVFXState("RepairBuilding");
			globalAudioSignal.Dispatch("Play_building_repair_01");
			yield return new global::UnityEngine.WaitForSeconds(2f);
			global::Kampai.Game.Building buildingBridge = playerService.GetByInstanceId<global::Kampai.Game.Building>(bridge.ID);
			if (buildingBridge.IsFootprintable)
			{
				addFootprintSignal.Dispatch(buildingBridge, location);
			}
			if (villainLairModel.currentActiveLair == null && !villainLairModel.goingToLair)
			{
				global::UnityEngine.Vector3 position = new global::UnityEngine.Vector3(bridgeDef.cameraPan.x, bridgeDef.cameraPan.y, bridgeDef.cameraPan.z);
				global::Kampai.Game.ScreenPosition sp = new global::Kampai.Game.ScreenPosition();
				autoMoveSignal.Dispatch(position, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(sp), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.None, null, null), true);
			}
			landExpansionSignal.Dispatch(bridgeDef.LandExpansionID, true);
			buildObj.SetVFXState("None");
		}
	}
}
