namespace Kampai.Game
{
	public class PanAndOpenModalCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingID { get; set; }

		[Inject]
		public bool bypassModal { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveSignal autoMoveSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenVillainLairPortalBuildingSignal openPortalSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingID);
			if (byInstanceId == null)
			{
				return;
			}
			global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = byInstanceId as global::Kampai.Game.VillainLairResourcePlot;
			if (villainLairResourcePlot != null)
			{
				global::Kampai.Game.VillainLair parentLair = villainLairResourcePlot.parentLair;
				global::Kampai.Game.View.VillainLairEntranceBuildingObject villainLairEntranceBuildingObject = component.GetBuildingObject(parentLair.portalInstanceID) as global::Kampai.Game.View.VillainLairEntranceBuildingObject;
				if (!(villainLairEntranceBuildingObject == null))
				{
					global::Kampai.Game.VillainLairEntranceBuilding byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(parentLair.portalInstanceID);
					if (byInstanceId2 != null)
					{
						openPortalSignal.Dispatch(byInstanceId2, villainLairEntranceBuildingObject);
					}
				}
				return;
			}
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(buildingID);
			if (!(buildingObject == null))
			{
				global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
				zero = buildingObject.ZoomCenter;
				bool flag = !global::Kampai.Util.GotoBuildingHelpers.BuildingMenuIsAccessible(byInstanceId);
				if (!flag)
				{
					global::Kampai.Game.View.ScaffoldingBuildingObject scaffoldingBuildingObject = buildingObject as global::Kampai.Game.View.ScaffoldingBuildingObject;
					flag = scaffoldingBuildingObject != null;
				}
				ProcessRegularBuilding(zero, flag, byInstanceId);
			}
		}

		private void ProcessRegularBuilding(global::UnityEngine.Vector3 buildingPos, bool menuInaccessible, global::Kampai.Game.Building building)
		{
			global::Kampai.Game.ScreenPosition screenPosition = new global::Kampai.Game.ScreenPosition();
			if (building.Definition.ScreenPosition != null)
			{
				screenPosition = screenPosition.Clone(building.Definition.ScreenPosition);
			}
			global::Kampai.Game.CameraMovementSettings.Settings settings = ((!menuInaccessible) ? global::Kampai.Game.CameraMovementSettings.Settings.ShowMenu : global::Kampai.Game.CameraMovementSettings.Settings.None);
			autoMoveSignal.Dispatch(buildingPos, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), new global::Kampai.Game.CameraMovementSettings(settings, building, null, bypassModal), false);
		}
	}
}
