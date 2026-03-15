namespace Kampai.UI.View
{
	public class PartyCameraControlPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PartyCameraControlPanelMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.CameraControlSettings cameraSettings;

		[Inject]
		public global::Kampai.UI.View.PartyCameraControlPanelView view { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			view.arrowTop.ClickedSignal.AddListener(TopButtonClicked);
			view.arrowLeft.ClickedSignal.AddListener(LeftButtonClicked);
			view.arrowRight.ClickedSignal.AddListener(RightButtonClicked);
			view.arrowBottom.ClickedSignal.AddListener(BottomButtonClicked);
			cameraSettings = definitionService.Get<global::Kampai.Game.MinionPartyDefinition>().cameraControlSettings;
		}

		public override void OnRemove()
		{
			base.OnRemove();
			view.arrowTop.ClickedSignal.RemoveListener(TopButtonClicked);
			view.arrowLeft.ClickedSignal.RemoveListener(LeftButtonClicked);
			view.arrowRight.ClickedSignal.RemoveListener(RightButtonClicked);
			view.arrowBottom.ClickedSignal.RemoveListener(BottomButtonClicked);
		}

		private void TopButtonClicked()
		{
			MoveCamera(cameraSettings.customCameraPosTiki);
		}

		private void LeftButtonClicked()
		{
			MoveCamera(cameraSettings.customCameraPosStage);
		}

		private void RightButtonClicked()
		{
			MoveCamera(cameraSettings.customCameraPosTownHall);
		}

		private void BottomButtonClicked()
		{
			MoveCamera(cameraSettings.customCameraPosPartyDefault);
		}

		private void MoveCamera(int customCameraPositionID)
		{
			if (customCameraPositionID != 0)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraMoveToCustomPositionSignal>().Dispatch(customCameraPositionID, new global::Kampai.Util.Boxed<global::System.Action>(null));
				return;
			}
			logger.Warning("Invalid camera postion ID {0}", customCameraPositionID);
		}
	}
}
