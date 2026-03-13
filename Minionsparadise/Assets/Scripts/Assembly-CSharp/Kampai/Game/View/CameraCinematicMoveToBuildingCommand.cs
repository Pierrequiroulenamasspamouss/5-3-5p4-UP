namespace Kampai.Game.View
{
	public class CameraCinematicMoveToBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingID { get; set; }

		[Inject]
		public float moveTime { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.CameraCinematicZoomSignal autoZoomSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraCinematicPanSignal autoPanSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.GameObject mainCameraGO { get; set; }

		[Inject]
		public global::Kampai.Game.View.CameraUtils cameraUtils { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingID);
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(buildingID);
			global::UnityEngine.Vector3 position = buildingObject.transform.position;
			global::Kampai.Game.ScreenPosition screenPosition = byInstanceId.Definition.ScreenPosition;
			if (screenPosition == null)
			{
				screenPosition = new global::Kampai.Game.ScreenPosition();
			}
			float zoom = screenPosition.zoom;
			if (screenPosition.x != -1f || screenPosition.z != -1f)
			{
				global::UnityEngine.Vector3 vector = cameraUtils.GroundPlaneRaycast(screenPosition.x, screenPosition.z);
				global::UnityEngine.Vector3 vector2 = position - vector;
				global::UnityEngine.Vector3 position2 = mainCameraGO.transform.position;
				global::UnityEngine.Vector3 first = new global::UnityEngine.Vector3(position2.x + vector2.x, position2.y, position2.z + vector2.z);
				global::Kampai.Game.CameraMovementSettings cameraMovementSettings = new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.None, null, null);
				autoPanSignal.Dispatch(global::Kampai.Util.Tuple.Create(first, moveTime), cameraMovementSettings, new global::Kampai.Util.Boxed<global::Kampai.Game.Building>(cameraMovementSettings.building), new global::Kampai.Util.Boxed<global::Kampai.Game.Quest>(cameraMovementSettings.quest));
				if (zoom > 0f)
				{
					autoZoomSignal.Dispatch(global::Kampai.Util.Tuple.Create(zoom, moveTime));
				}
			}
		}
	}
}
