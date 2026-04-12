namespace Kampai.Game
{
	public class GotoCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GotoCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.GotoArgument gotoArgument { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenStoreHighlightItemSignal openStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToBuildingSignal moveToBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.BuildMenuOpenedSignal buildMenuOpened { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void Execute()
		{
			if (gotoArgument.BuildingId > 0)
			{
				global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(gotoArgument.BuildingId);
				moveToBuildingSignal.Dispatch(byInstanceId, new global::Kampai.Game.PanInstructions(byInstanceId));
			}
			else if (gotoArgument.BuildingDefId > 0)
			{
				GotoBuildingDefinition(gotoArgument.BuildingDefId, gotoArgument.ForceStore);
			}
			else if (gotoArgument.ItemId > 0)
			{
				int buildingDefintionIDFromItemDefintionID = definitionService.GetBuildingDefintionIDFromItemDefintionID(gotoArgument.ItemId);
				if (buildingDefintionIDFromItemDefintionID > 0)
				{
					GotoBuildingDefinition(buildingDefintionIDFromItemDefintionID, gotoArgument.ForceStore);
					return;
				}
				logger.Warning("No building for item {0}", gotoArgument.ItemId);
			}
			else
			{
				logger.Warning("Nothing to goto");
			}
		}

		private void GotoBuildingDefinition(int definition, bool forceStore)
		{
			if (forceStore)
			{
				buildMenuOpened.Dispatch();
				openStoreSignal.Dispatch(definition, true);
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.Building> buildingsWithoutState = playerService.GetBuildingsWithoutState(global::Kampai.Game.BuildingState.Inventory);
			if (buildingsWithoutState.Count > 0)
			{
				global::Kampai.Game.Building building = buildingsWithoutState[global::UnityEngine.Random.Range(0, buildingsWithoutState.Count - 1)];
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.PanAndOpenModalSignal>().Dispatch(building.ID, false);
			}
			else
			{
				buildMenuOpened.Dispatch();
				openStoreSignal.Dispatch(definition, true);
			}
		}
	}
}
