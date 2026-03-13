namespace Kampai.Game
{
	public class CameraAutoMoveCommand : global::strange.extensions.command.impl.Command
	{
		private float zoomPercentage;

		private global::Kampai.Game.ScreenPosition screenPosition;

		[Inject]
		public global::UnityEngine.Vector3 position { get; set; }

		[Inject]
		public global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition> boxedScreenPosition { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMovementSettings modalInfo { get; set; }

		[Inject]
		public bool absolutePosition { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoZoomSignal autoZoomSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoPanSignal autoPanSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraModel model { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSoundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickModel { get; set; }

		[Inject]
		public global::Kampai.Game.ShowHiddenBuildingsSignal showHiddenBuildingsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CancelAutozoomSignal cancelAutozoomSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingZoomSignal buildingZoomSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.GameObject mainCameraGO { get; set; }

		[Inject]
		public global::Kampai.Game.View.CameraUtils cameraUtils { get; set; }

		private void SetupZoomCameraModel()
		{
			zoomCameraModel.PreviousCameraPosition = new global::UnityEngine.Vector3(position.x, global::UnityEngine.Mathf.Lerp(30f, 13f, zoomPercentage), position.z);
			zoomCameraModel.PreviousCameraRotation = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Lerp(55f, 25f, zoomPercentage), zoomCameraModel.PreviousCameraRotation.y, zoomCameraModel.PreviousCameraRotation.z);
			zoomCameraModel.PreviousCameraFieldOfView = global::UnityEngine.Mathf.Lerp(40f, 9f, zoomPercentage);
			modalInfo.cameraSpeed = 1f;
		}

		public override void Execute()
		{
			CalculatePosition();
			showHiddenBuildingsSignal.Dispatch();
			if ((model.CurrentBehaviours & 8) == 8)
			{
				return;
			}
			pickModel.PanningCameraBlocked = true;
			if (zoomCameraModel.ZoomedIn || zoomCameraModel.ZoomInProgress)
			{
				global::Kampai.Game.BuildingZoomSettings type = new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.OUT, zoomCameraModel.LastZoomBuildingType);
				if (zoomCameraModel.LastZoomBuildingType == global::Kampai.Game.BuildingZoomType.ORDERBOARD)
				{
					SetupZoomCameraModel();
				}
				buildingZoomSignal.Dispatch(type);
			}
			cancelAutozoomSignal.Dispatch();
			autoPanSignal.Dispatch(position, modalInfo, new global::Kampai.Util.Boxed<global::Kampai.Game.Building>(modalInfo.building), new global::Kampai.Util.Boxed<global::Kampai.Game.Quest>(modalInfo.quest));
			if (zoomPercentage > 0f)
			{
				autoZoomSignal.Dispatch(zoomPercentage);
			}
			if (pickModel.PanningCameraBlocked || pickModel.ZoomingCameraBlocked)
			{
				playSoundFXSignal.Dispatch("Play_low_woosh_01");
			}
			if (modalInfo.settings != global::Kampai.Game.CameraMovementSettings.Settings.KeepUIOpen)
			{
				GetUISignal<global::Kampai.UI.View.CloseAllOtherMenuSignal>().Dispatch(null);
			}
		}

		private T GetUISignal<T>()
		{
			return uiContext.injectionBinder.GetInstance<T>();
		}

		private void CalculatePosition()
		{
			if (boxedScreenPosition != null)
			{
				screenPosition = boxedScreenPosition.Value;
			}
			if (screenPosition == null)
			{
				screenPosition = new global::Kampai.Game.ScreenPosition();
			}
			zoomPercentage = screenPosition.zoom;
			if (!absolutePosition)
			{
				global::UnityEngine.Vector3 vector = cameraUtils.GroundPlaneRaycast(screenPosition.x, screenPosition.z);
				global::UnityEngine.Vector3 pointInSpace = position;
				global::UnityEngine.Vector3 vector2 = cameraUtils.GroundPlaneRaycastFromPoint(pointInSpace);
				global::UnityEngine.Vector3 vector3 = vector2 - vector;
				global::UnityEngine.Vector3 vector4 = mainCameraGO.transform.position;
				position = new global::UnityEngine.Vector3(vector4.x + vector3.x, vector4.y, vector4.z + vector3.z);
			}
		}
	}
}
