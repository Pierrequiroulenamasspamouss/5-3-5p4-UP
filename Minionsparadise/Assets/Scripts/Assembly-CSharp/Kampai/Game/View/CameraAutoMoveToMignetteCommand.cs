namespace Kampai.Game.View
{
	public class CameraAutoMoveToMignetteCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CameraAutoMoveToMignetteCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int definitionId { get; set; }

		[Inject]
		public bool bypassModal { get; set; }

		[Inject]
		public global::Kampai.Game.PanInstructions panInstructions { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToBuildingDefSignal moveToBuildingDefSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PanAndOpenModalSignal panAndOpenModalSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowNeedXMinionsSignal showNeedXMinionsSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(definitionId);
			global::Kampai.Game.MignetteBuilding mignetteBuilding = firstInstanceByDefinitionId as global::Kampai.Game.MignetteBuilding;
			int num = 0;
			if (firstInstanceByDefinitionId != null && mignetteBuilding != null)
			{
				if (!global::Kampai.Game.OpenBuildingMenuCommand.HasEnoughFreeMinionsToAssignToBuilding(playerService, mignetteBuilding))
				{
					showNeedXMinionsSignal.Dispatch(mignetteBuilding.GetMinionSlotsOwned());
					return;
				}
				num = mignetteBuilding.Definition.LevelUnlocked;
				if (playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) >= num)
				{
					panAndOpenModalSignal.Dispatch(firstInstanceByDefinitionId.ID, bypassModal);
					return;
				}
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.AspirationalBuildingDefinition> all = definitionService.GetAll<global::Kampai.Game.AspirationalBuildingDefinition>();
			int i = 0;
			for (int count = all.Count; i < count; i++)
			{
				global::Kampai.Game.AspirationalBuildingDefinition aspirationalBuildingDefinition = all[i];
				if (aspirationalBuildingDefinition.BuildingDefinitionID != definitionId)
				{
					continue;
				}
				global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get<global::Kampai.Game.BuildingDefinition>(aspirationalBuildingDefinition.BuildingDefinitionID);
				if (buildingDefinition != null)
				{
					global::Kampai.Game.MignetteBuildingDefinition mignetteBuildingDefinition = definitionService.Get<global::Kampai.Game.MignetteBuildingDefinition>(aspirationalBuildingDefinition.BuildingDefinitionID);
					if (mignetteBuildingDefinition != null)
					{
						string aspirationalMessage = mignetteBuildingDefinition.AspirationalMessage;
						global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
						global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(firstInstanceByDefinitionId.ID);
						global::UnityEngine.Vector3 zoomCenter = buildingObject.ZoomCenter;
						moveToBuildingDefSignal.Dispatch(buildingDefinition, zoomCenter, panInstructions);
						globalSFXSignal.Dispatch("Play_action_locked_01");
						popupMessageSignal.Dispatch(localizationService.GetString(aspirationalMessage, num), global::Kampai.UI.View.PopupMessageType.NORMAL);
						return;
					}
				}
			}
			logger.Error("CameraAutoMoveToMignetteCommand: Failed to find mignette with definition ID {0}", definitionId);
		}
	}
}
