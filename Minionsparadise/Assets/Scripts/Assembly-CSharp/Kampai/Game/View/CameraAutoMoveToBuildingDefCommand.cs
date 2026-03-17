namespace Kampai.Game.View
{
	public class CameraAutoMoveToBuildingDefCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.BuildingDefinition def { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 position { get; set; }

		[Inject]
		public global::Kampai.Game.PanInstructions panInstructions { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveSignal autoMoveSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.ScreenPosition screenPosition = new global::Kampai.Game.ScreenPosition();
			if (def.ScreenPosition != null)
			{
				screenPosition = screenPosition.Clone(def.ScreenPosition);
			}
			global::Kampai.Util.Boxed<global::UnityEngine.Vector3> offset = panInstructions.Offset;
			global::Kampai.Util.Boxed<float> zoomDistance = panInstructions.ZoomDistance;
			if (zoomDistance != null)
			{
				if (screenPosition == null)
				{
					screenPosition = new global::Kampai.Game.ScreenPosition();
				}
				screenPosition.zoom = zoomDistance.Value;
			}
			global::UnityEngine.Vector3 type = ((offset != null) ? (offset.Value + position) : position);
			autoMoveSignal.Dispatch(type, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.None, null, null), false);
		}
	}
}
