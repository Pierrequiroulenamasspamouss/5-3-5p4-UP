namespace Kampai.Game
{
	public class BuildingZoomCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("BuildingZoomCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.BuildingZoomType buildingZoomType;

		[Inject]
		public global::Kampai.Game.BuildingZoomSettings zoomBuildingSettings { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ToggleHitboxSignal toggleHitboxSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSoundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisableCameraBehaviourSignal disableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableCameraBehaviourSignal enableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraModel model { get; set; }

		[Inject]
		public global::Kampai.Game.CameraResetPanVelocitySignal cameraResetPanVelocitySignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera mainCamera { get; set; }

		[Inject]
		public global::Kampai.Game.StageService stageService { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUETikiOpened ftueSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoPanCompleteSignal cameraAutoPanCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToBuildingSignal cameraAutoMoveToBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Main.FadeBackgroundAudioSignal fadeBackgroundAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.ZoomType zoomType = zoomBuildingSettings.ZoomType;
			buildingZoomType = zoomBuildingSettings.ZoomBuildingType;
			logger.Debug("We are going to zoom {0} {1} building", zoomType, buildingZoomType);
			switch (zoomType)
			{
			case global::Kampai.Game.ZoomType.IN:
				Zoom(true);
				break;
			case global::Kampai.Game.ZoomType.OUT:
				Zoom(false);
				break;
			default:
				logger.Fatal(global::Kampai.Util.FatalCode.EX_INVALID_ENUM);
				break;
			}
		}

		private void Zoom(bool zoomIn)
		{
			if (zoomCameraModel.ZoomInProgress)
			{
				CallCallback();
				return;
			}
			if (zoomCameraModel.ZoomedIn && zoomIn)
			{
				CallCallback();
				return;
			}
			if (!zoomCameraModel.ZoomedIn && !zoomIn)
			{
				CallCallback();
				return;
			}
			if (!zoomIn && zoomCameraModel.LastZoomBuildingType != buildingZoomType)
			{
				CallCallback();
				return;
			}
			zoomCameraModel.LastZoomBuildingType = buildingZoomType;
			OnZoomBegin(zoomIn);
			if (buildingZoomType == global::Kampai.Game.BuildingZoomType.TIKIBAR)
			{
				ZoomTikiBar(zoomIn);
			}
			else if (buildingZoomType == global::Kampai.Game.BuildingZoomType.STAGE)
			{
				global::Kampai.Game.StageBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.StageBuilding>(370);
				ZoomBuilding(zoomIn, byInstanceId);
			}
			else if (buildingZoomType == global::Kampai.Game.BuildingZoomType.ORDERBOARD)
			{
				global::Kampai.Game.OrderBoard byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.OrderBoard>(309);
				ZoomBuilding(zoomIn, byInstanceId2);
			}
		}

		private void ZoomTikiBar(bool zoomIn)
		{
			fadeBackgroundAudioSignal.Dispatch(!zoomIn, "Play_tikiBar_snapshotDuck_01");
			if (zoomIn)
			{
				ftueSignal.Dispatch();
				int toReenable = model.CurrentBehaviours;
				disableCameraSignal.Dispatch(toReenable);
				global::Kampai.Game.TikiBarBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TikiBarBuilding>(313);
				cameraAutoPanCompleteSignal.AddOnce(delegate
				{
					enableCameraSignal.Dispatch(toReenable);
					OnZoomEnd(zoomIn);
				});
				cameraAutoMoveToBuildingSignal.Dispatch(byInstanceId, new global::Kampai.Game.PanInstructions(byInstanceId));
			}
			else
			{
				OnZoomEnd(zoomIn);
			}
		}

		private void ZoomBuilding(bool zoomIn, global::Kampai.Game.ZoomableBuilding building)
		{
			fadeBackgroundAudioSignal.Dispatch(!zoomIn, "Play_tikiBar_snapshotDuck_01");
			int toReenable = model.CurrentBehaviours;
			disableCameraSignal.Dispatch(toReenable);
			if (zoomIn)
			{
				global::Kampai.Game.OrderBoard orderBoard = building as global::Kampai.Game.OrderBoard;
				if (orderBoard != null)
				{
					SetHitbox(false);
				}
				if (building is global::Kampai.Game.StageBuilding)
				{
					toReenable = 0;
				}
				zoomCameraModel.PreviousCameraPosition = mainCamera.transform.position;
				zoomCameraModel.PreviousCameraRotation = mainCamera.transform.localEulerAngles;
				zoomCameraModel.PreviousCameraFieldOfView = mainCamera.fieldOfView;
			}
			else if (building is global::Kampai.Game.StageBuilding)
			{
				stageService.HideStageBackdrop();
				toReenable = 3;
			}
			PositionTweenProperty position = new PositionTweenProperty((!zoomIn) ? zoomCameraModel.PreviousCameraPosition : zoomCameraModel.GetZoomedCameraPosition(building));
			AbstractTweenProperty abstractTweenProperty = null;
			abstractTweenProperty = ((!zoomIn) ? ((AbstractTweenProperty)new RotationTweenProperty(zoomCameraModel.PreviousCameraRotation)) : ((AbstractTweenProperty)new RotationQuaternionTweenProperty(zoomCameraModel.GetZoomedCameraRotation(building))));
			GoTweenFlow goTweenFlow = CreateFlow(position, abstractTweenProperty, (!zoomIn) ? zoomCameraModel.PreviousCameraFieldOfView : zoomCameraModel.GetZoomedFOV(building));
			goTweenFlow.setOnCompleteHandler(delegate
			{
				if (zoomBuildingSettings.EnableCamera)
				{
					enableCameraSignal.Dispatch(toReenable);
				}
				OnZoomEnd(zoomIn);
				global::Kampai.Game.OrderBoard orderBoard2 = building as global::Kampai.Game.OrderBoard;
				if (!zoomIn && orderBoard2 != null && orderBoard2.HarvestableCharacterDefinitionId != 0)
				{
					global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(orderBoard2.HarvestableCharacterDefinitionId);
					prestigeService.PostOrderCompletion(prestige);
				}
			});
			goTweenFlow.play();
		}

		private GoTweenFlow CreateFlow(AbstractTweenProperty position, AbstractTweenProperty rotation, float fieldOfView)
		{
			GoTweenConfig config = new GoTweenConfig().addTweenProperty(position).addTweenProperty(rotation).setEaseType(GoEaseType.SineOut);
			GoTweenConfig config2 = new GoTweenConfig().floatProp("fieldOfView", fieldOfView).setEaseType(GoEaseType.SineOut);
			GoTween tween = new GoTween(mainCamera, 1f, config2);
			GoTween tween2 = new GoTween(mainCamera.transform, 1f, config);
			return new GoTweenFlow().insert(0f, tween2).insert(0f, tween);
		}

		private void CallCallback()
		{
			if (zoomBuildingSettings.OnComplete != null)
			{
				zoomBuildingSettings.OnComplete();
			}
		}

		private void OnZoomBegin(bool zoomIn)
		{
			if (zoomIn)
			{
				cameraResetPanVelocitySignal.Dispatch();
				HideUI();
				SetHitbox(false);
			}
			playSoundFXSignal.Dispatch("Play_low_woosh_01");
			zoomCameraModel.ZoomInProgress = true;
		}

		private void OnZoomEnd(bool zoomIn)
		{
			zoomCameraModel.ZoomedIn = zoomIn;
			zoomCameraModel.ZoomInProgress = false;
			if (buildingZoomType == global::Kampai.Game.BuildingZoomType.ORDERBOARD && zoomIn)
			{
				ShowUI();
			}
			if (!zoomIn)
			{
				ShowUI();
				SetHitbox(true);
			}
			CallCallback();
		}

		private void HideUI()
		{
			closeSignal.Dispatch(null);
			ToggleUI(false);
		}

		private void ShowUI()
		{
			ToggleUI(true);
		}

		private void ToggleUI(bool enable)
		{
			if (!playerService.GetMinionPartyInstance().IsPartyHappening)
			{
				showHUDSignal.Dispatch(enable);
				showStoreSignal.Dispatch(enable);
			}
		}

		private void SetHitbox(bool enabled)
		{
			toggleHitboxSignal.Dispatch(buildingZoomType, enabled);
		}
	}
}
