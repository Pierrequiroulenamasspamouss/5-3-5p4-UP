namespace Kampai.Game
{
	public class CameraMoveToCustomLairPlotCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CameraMoveToCustomLairPlotCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::UnityEngine.Vector3 cameraPosition { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera mainCamera { get; set; }

		[Inject]
		public global::Kampai.Game.DisableCameraBehaviourSignal disableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		public override void Execute()
		{
			if (villainLairModel.cameraFlow != null)
			{
				int id = villainLairModel.cameraFlow.id;
				villainLairModel.cameraFlow.destroy();
				villainLairModel.cameraFlow = null;
				logger.Error("lairbug CameraMoveToCustomLairPlotCommand flow (id={0}) destroyed!", id);
			}
			disableCameraSignal.Dispatch(1);
			disableCameraSignal.Dispatch(2);
			PositionTweenProperty position = new PositionTweenProperty(cameraPosition);
			RotationTweenProperty rotation = new RotationTweenProperty(global::Kampai.Util.GameConstants.LairResourcePlotCustomUIOffsets.rotation);
			GoTweenFlow goTweenFlow = CreateFlow(position, rotation, 30f, 1f);
			pickControllerModel.PanningCameraBlocked = true;
			goTweenFlow.play();
			villainLairModel.cameraFlow = goTweenFlow;
			mainCamera.nearClipPlane = 0.3f;
			sfxSignal.Dispatch("Play_low_woosh_01");
		}

		private GoTweenFlow CreateFlow(AbstractTweenProperty position, RotationTweenProperty rotation, float fieldOfView, float duration)
		{
			GoTweenConfig config = new GoTweenConfig().addTweenProperty(position).addTweenProperty(rotation).setEaseType(GoEaseType.SineOut)
				.onComplete(delegate
				{
					pickControllerModel.PanningCameraBlocked = false;
				});
			GoTweenConfig config2 = new GoTweenConfig().floatProp("fieldOfView", fieldOfView).setEaseType(GoEaseType.SineOut);
			GoTween tween = new GoTween(mainCamera.transform, duration, config);
			GoTween tween2 = new GoTween(mainCamera, duration, config2);
			return new GoTweenFlow().insert(0f, tween).insert(0f, tween2);
		}
	}
}
