namespace Kampai.Game
{
	public class CameraMoveToCustomPositionCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int definitionID { get; set; }

		[Inject]
		public global::Kampai.Util.Boxed<global::System.Action> callback { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera mainCamera { get; set; }

		[Inject]
		public global::Kampai.Game.DisableCameraBehaviourSignal disableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableCameraBehaviourSignal enableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalSoundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		public override void Execute()
		{
			bool restoreCameraControl = false;
			if (villainLairModel.cameraFlow != null)
			{
				villainLairModel.cameraFlow.destroy();
				villainLairModel.cameraFlow = null;
			}
			disableCameraSignal.Dispatch(1);
			disableCameraSignal.Dispatch(2);
			global::Kampai.Game.CustomCameraPositionDefinition customCameraPositionDefinition = definitionService.Get<global::Kampai.Game.CustomCameraPositionDefinition>(definitionID);
			restoreCameraControl = customCameraPositionDefinition.enableCameraControl;
			global::UnityEngine.Vector3 endValue = new global::UnityEngine.Vector3(customCameraPositionDefinition.xPos, customCameraPositionDefinition.yPos, customCameraPositionDefinition.zPos);
			global::UnityEngine.Vector3 endValue2 = new global::UnityEngine.Vector3(customCameraPositionDefinition.xRotation, customCameraPositionDefinition.yRotation, customCameraPositionDefinition.zRotation);
			float fOV = customCameraPositionDefinition.FOV;
			PositionTweenProperty position = new PositionTweenProperty(endValue);
			RotationTweenProperty rotation = new RotationTweenProperty(endValue2);
			if (!string.IsNullOrEmpty(customCameraPositionDefinition.panSound))
			{
				playGlobalSoundFXSignal.Dispatch(customCameraPositionDefinition.panSound);
			}
			GoTweenFlow goTweenFlow = CreateFlow(position, rotation, fOV, customCameraPositionDefinition.duration);
			goTweenFlow.setOnCompleteHandler(delegate
			{
				if (callback.Value != null)
				{
					callback.Value();
				}
				if (restoreCameraControl)
				{
					enableCameraSignal.Dispatch(1);
					enableCameraSignal.Dispatch(2);
				}
				pickControllerModel.PanningCameraBlocked = false;
			});
			pickControllerModel.PanningCameraBlocked = true;
			goTweenFlow.play();
			villainLairModel.cameraFlow = goTweenFlow;
			if (customCameraPositionDefinition.nearClip > 0f)
			{
				mainCamera.nearClipPlane = customCameraPositionDefinition.nearClip;
			}
			if (customCameraPositionDefinition.farClip > 0f)
			{
				mainCamera.farClipPlane = customCameraPositionDefinition.farClip;
			}
		}

		private GoTweenFlow CreateFlow(AbstractTweenProperty position, RotationTweenProperty rotation, float fieldOfView, float duration)
		{
			GoTweenConfig config = new GoTweenConfig().addTweenProperty(position).addTweenProperty(rotation).setEaseType(GoEaseType.SineOut);
			GoTweenConfig config2 = new GoTweenConfig().floatProp("fieldOfView", fieldOfView).setEaseType(GoEaseType.SineOut);
			GoTween tween = new GoTween(mainCamera.transform, duration, config);
			GoTween tween2 = new GoTween(mainCamera, duration, config2);
			return new GoTweenFlow().insert(0f, tween).insert(0f, tween2);
		}
	}
}
