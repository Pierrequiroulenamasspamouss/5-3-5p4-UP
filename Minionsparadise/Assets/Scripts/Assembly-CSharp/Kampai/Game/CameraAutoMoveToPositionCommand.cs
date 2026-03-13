namespace Kampai.Game
{
	public class CameraAutoMoveToPositionCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::UnityEngine.Vector3 position { get; set; }

		[Inject]
		public float zoom { get; set; }

		[Inject]
		public bool useOffset { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveSignal autoMoveSignal { get; set; }

		public override void Execute()
		{
			global::UnityEngine.Vector3 type = position;
			if (useOffset)
			{
				type += global::Kampai.Util.GameConstants.CAMERA_OFFSET_ACTIONABLE_OBJECT;
			}
			global::Kampai.Game.ScreenPosition screenPosition = new global::Kampai.Game.ScreenPosition();
			screenPosition.x = -1f;
			screenPosition.z = -1f;
			screenPosition.zoom = zoom;
			autoMoveSignal.Dispatch(type, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.None, null, null), true);
		}
	}
}
