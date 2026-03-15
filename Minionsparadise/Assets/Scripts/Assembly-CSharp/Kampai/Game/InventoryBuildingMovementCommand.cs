namespace Kampai.Game
{
	public class InventoryBuildingMovementCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveBuildingSignal removeBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DebugUpdateGridSignal gridSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DeselectBuildingSignal deselectBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SendBuildingToInventorySignal sendBuildingToInventorySignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void Execute()
		{
			if (pickControllerModel.SelectedBuilding.HasValue && pickControllerModel.SelectedBuilding != -1)
			{
				int value = pickControllerModel.SelectedBuilding.Value;
				buildingChangeStateSignal.Dispatch(value, global::Kampai.Game.BuildingState.Inventory);
				global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(value);
				if (byInstanceId != null)
				{
					removeBuildingSignal.Dispatch(byInstanceId.Location, definitionService.GetBuildingFootprint(byInstanceId.Definition.FootprintID));
					gridSignal.Dispatch();
				}
				LocalPersistBuildingInventoryStorageAction();
				sendBuildingToInventorySignal.Dispatch(value);
				deselectBuildingSignal.Dispatch(value);
			}
		}

		private void LocalPersistBuildingInventoryStorageAction()
		{
			if (!localPersistService.HasKey("didyouknow_PutBuildingInInventory"))
			{
				localPersistService.PutDataInt("didyouknow_PutBuildingInInventory", 1);
			}
		}
	}
}
