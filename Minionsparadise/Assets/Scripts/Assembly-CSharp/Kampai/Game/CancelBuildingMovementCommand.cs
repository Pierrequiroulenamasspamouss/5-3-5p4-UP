namespace Kampai.Game
{
	public class CancelBuildingMovementCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public bool InvalidLocation { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.Game.CancelPurchaseSignal cancelPurchaseSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetBuildingPositionSignal setBuildingPositionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DeselectBuildingSignal deselectBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateConnectablesSignal updateConnectablesSignal { get; set; }

		public override void Execute()
		{
			if (!pickControllerModel.SelectedBuilding.HasValue)
			{
				return;
			}
			if (pickControllerModel.SelectedBuilding == -1)
			{
				cancelPurchaseSignal.Dispatch(InvalidLocation);
			}
			else
			{
				global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(pickControllerModel.SelectedBuilding.Value);
				global::Kampai.Game.Location location = byInstanceId.Location;
				setBuildingPositionSignal.Dispatch(byInstanceId.ID, new global::UnityEngine.Vector3(location.x, 0f, location.y));
				deselectBuildingSignal.Dispatch(byInstanceId.ID);
				global::Kampai.Game.ConnectableBuilding connectableBuilding = byInstanceId as global::Kampai.Game.ConnectableBuilding;
				if (connectableBuilding != null)
				{
					updateConnectablesSignal.Dispatch(location, connectableBuilding.Definition.connectableType);
				}
			}
			pickControllerModel.SelectedBuilding = null;
		}
	}
}
