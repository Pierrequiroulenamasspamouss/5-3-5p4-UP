namespace Kampai.Game
{
	public class CameraZoomBeachCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		public override void Execute()
		{
			if (!zoomCameraModel.ZoomedIn && !zoomCameraModel.ZoomInProgress)
			{
				global::UnityEngine.Vector3 type = new global::UnityEngine.Vector3(131.1315f, 15.4357f, 162.0232f);
				global::Kampai.Game.CameraMovementSettings type2 = new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.Default, null, null);
				global::Kampai.Game.ScreenPosition screenPosition = new global::Kampai.Game.ScreenPosition();
				screenPosition.zoom = 0.8567233f;
				base.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveSignal>().Dispatch(type, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), type2, true);
			}
		}
	}
}
