namespace Kampai.Game
{
	public class CameraAutoMoveToInstanceCommand : global::strange.extensions.command.impl.Command
	{
		private const float ZOOM_LEVEL_ACTIONABLE_OBJECT = 0.6f;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CameraAutoMoveToInstanceCommand") as global::Kampai.Util.IKampaiLogger;

		private global::UnityEngine.Vector3 CAMERA_OFFSET_VILLAIN = new global::UnityEngine.Vector3(-2f, 0f, 1.3f);

		private global::UnityEngine.Vector3 CAMERA_OFFSET_TIKIBAR = new global::UnityEngine.Vector3(-2.5f, 0f, 2.5f);

		[Inject]
		public global::Kampai.Game.PanInstructions panInstructions { get; set; }

		[Inject]
		public global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition> boxedScreenPosition { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveSignal autoMoveSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowHiddenBuildingsSignal showHiddenBuildingsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToBuildingSignal buildingMoveSignal { get; set; }

		public override void Execute()
		{
			showHiddenBuildingsSignal.Dispatch();
			global::Kampai.Game.Instance instance = panInstructions.Instance;
			if (instance == null)
			{
				instance = playerService.GetByInstanceId<global::Kampai.Game.Instance>(panInstructions.InstanceId);
			}
			global::Kampai.Game.Building building = instance as global::Kampai.Game.Building;
			if (building != null)
			{
				buildingMoveSignal.Dispatch(building, panInstructions);
				return;
			}
			global::Kampai.Game.View.ActionableObject fromAllObjects = global::Kampai.Game.View.ActionableObjectManagerView.GetFromAllObjects(instance.ID);
			if (fromAllObjects == null)
			{
				logger.Error("CameraAutoMoveToInstanceCommand: Cannot find object {0} {1}", instance, instance.GetType());
				return;
			}
			global::Kampai.Util.Boxed<global::UnityEngine.Vector3> offset = panInstructions.Offset;
			global::Kampai.Util.Boxed<float> zoomDistance = panInstructions.ZoomDistance;
			global::UnityEngine.Vector3 type = fromAllObjects.transform.position;
			global::Kampai.Game.View.CharacterObject characterObject = fromAllObjects as global::Kampai.Game.View.CharacterObject;
			if (characterObject != null)
			{
				if (characterObject is global::Kampai.Game.View.VillainView)
				{
					type = characterObject.GetIndicatorPosition() + CAMERA_OFFSET_VILLAIN;
				}
				else if (characterObject is global::Kampai.Game.View.PhilView)
				{
					type += CAMERA_OFFSET_TIKIBAR;
				}
				else
				{
					type = characterObject.GetIndicatorPosition();
				}
			}
			if (offset != null)
			{
				type += offset.Value;
			}
			global::Kampai.Game.CameraMovementSettings type2 = ((panInstructions.CameraMovementSettings != null) ? panInstructions.CameraMovementSettings : new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.None, null, null));
			global::Kampai.Game.ScreenPosition screenPosition = new global::Kampai.Game.ScreenPosition();
			if (boxedScreenPosition != null)
			{
				screenPosition = screenPosition.Clone(boxedScreenPosition.Value);
			}
			if (zoomDistance != null)
			{
				screenPosition.zoom = zoomDistance.Value;
			}
			autoMoveSignal.Dispatch(type, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), type2, false);
		}
	}
}
