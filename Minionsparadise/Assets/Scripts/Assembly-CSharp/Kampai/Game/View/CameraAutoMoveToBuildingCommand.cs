namespace Kampai.Game.View
{
	public class CameraAutoMoveToBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Building building { get; set; }

		[Inject]
		public global::Kampai.Game.PanInstructions panInstructions { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToBuildingDefSignal autoMoveSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowHiddenBuildingsSignal showHiddenBuildingsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveSignal cameraAutoMoveSignal { get; set; }

		public override void Execute()
		{
			int iD = building.ID;
			bool flag = false;
			showHiddenBuildingsSignal.Dispatch();
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(iD);
			if (buildingObject == null)
			{
				global::Kampai.Game.View.ScaffoldingBuildingObject scaffoldingBuildingObject = component.GetScaffoldingBuildingObject(iD);
				if (scaffoldingBuildingObject == null)
				{
					return;
				}
				flag = true;
				buildingObject = scaffoldingBuildingObject;
			}
			global::UnityEngine.Vector3 vector = ((!(building is global::Kampai.Game.TikiBarBuilding) && !(building is global::Kampai.Game.VillainLairEntranceBuilding)) ? buildingObject.ZoomCenter : buildingObject.transform.position);
			if (flag || building.State == global::Kampai.Game.BuildingState.Construction || building.State == global::Kampai.Game.BuildingState.Complete)
			{
				global::Kampai.Game.ScreenPosition screenPosition = new global::Kampai.Game.ScreenPosition();
				global::Kampai.Util.Boxed<global::UnityEngine.Vector3> offset = panInstructions.Offset;
				global::Kampai.Util.Boxed<float> zoomDistance = panInstructions.ZoomDistance;
				if (zoomDistance != null)
				{
					screenPosition.zoom = zoomDistance.Value;
				}
				global::UnityEngine.Vector3 type = ((offset != null) ? (offset.Value + vector) : vector);
				cameraAutoMoveSignal.Dispatch(type, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.None, null, null), false);
			}
			else
			{
				autoMoveSignal.Dispatch(building.Definition, vector, panInstructions);
			}
		}
	}
}
