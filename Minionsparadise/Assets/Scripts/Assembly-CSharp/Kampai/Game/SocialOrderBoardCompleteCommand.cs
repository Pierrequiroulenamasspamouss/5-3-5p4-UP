namespace Kampai.Game
{
	public class SocialOrderBoardCompleteCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Game.View.StageBuildingObject sbo;

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.AddStuartToStageSignal addStuartToStageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StuartStartPerformingSignal startPerformingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReleaseMinionFromTikiBarSignal releaseMinionFromTikiBarSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.StuartShowCompleteSignal showCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StageService stageService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimedSocialEventService timedSocialEventService { get; set; }

		[Inject]
		public global::Kampai.Game.CloseConfirmationSignal closeConfirmationSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StuartShowStartSignal stuartShowStartSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.IN, global::Kampai.Game.BuildingZoomType.STAGE, ZoomCompleted));
			global::strange.extensions.signal.impl.Signal signal = new global::strange.extensions.signal.impl.Signal();
			global::Kampai.Util.SignalCallback<global::strange.extensions.signal.impl.Signal> signalCallback = new global::Kampai.Util.SignalCallback<global::strange.extensions.signal.impl.Signal>(signal);
			signal.AddListener(HandleShowFinished);
			global::Kampai.Game.StuartCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StuartCharacter>(70001);
			releaseMinionFromTikiBarSignal.Dispatch(firstInstanceByDefinitionId, true);
			addStuartToStageSignal.Dispatch(global::Kampai.Game.StuartStageAnimationType.IDLEOFFSTAGE);
			startPerformingSignal.Dispatch(signalCallback);
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(370);
			sbo = buildingObject as global::Kampai.Game.View.StageBuildingObject;
			if (sbo != null)
			{
				sbo.PerformanceStarts();
			}
			if (!signalCallback.WillDispatch)
			{
				HandleShowFinished();
			}
			else
			{
				TurnOffUI();
			}
			timedSocialEventService.setRewardCutscene(true);
		}

		private void HandleShowFinished()
		{
			if (zoomCameraModel.ZoomedIn)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.OUT, zoomCameraModel.LastZoomBuildingType));
			}
			closeConfirmationSignal.Dispatch();
			timedSocialEventService.setRewardCutscene(false);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.CheckIfShouldStartPartySignal>().Dispatch();
			if (sbo != null)
			{
				sbo.UpdateStageState(global::Kampai.Game.BuildingState.Idle);
			}
			showCompleteSignal.Dispatch();
		}

		public void ZoomCompleted()
		{
			stuartShowStartSignal.Dispatch();
			stageService.ShowStageBackdrop();
			pickControllerModel.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.StageView;
		}

		private void TurnOffUI()
		{
			showHUDSignal.Dispatch(false);
			showStoreSignal.Dispatch(false);
			hideAllWayFindersSignal.Dispatch();
		}
	}
}
