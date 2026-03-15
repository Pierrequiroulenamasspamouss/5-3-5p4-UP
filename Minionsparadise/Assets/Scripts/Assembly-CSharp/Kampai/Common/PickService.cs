namespace Kampai.Common
{
	public class PickService : global::Kampai.Common.IPickService
	{
		private global::UnityEngine.RaycastHit hit;

		private global::UnityEngine.Vector3 inputPosition;

		private int input;

		private bool startedOnHUD;

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera camera { get; set; }

		[Inject]
		public global::Kampai.Game.ICameraControlsService cameraControlsService { get; set; }

		[Inject]
		public global::Kampai.Game.ShowHiddenBuildingsSignal showHiddenBuildingsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.MinionPickSignal minionSignal { get; set; }

		[Inject]
		public global::Kampai.Common.EnvironmentalMignetteTappedSignal environmentalMignetteTappedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.BuildingPickSignal buildingSignal { get; set; }

		[Inject]
		public global::Kampai.Common.VillainIslandMessageSignal villainIslandMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Common.LairEnvironmentElementClickedSignal lairEnvironmentElementClickedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.MagnetFingerPickSignal magnetFingerSignal { get; set; }

		[Inject]
		public global::Kampai.Common.DragAndDropPickSignal dragAndDropSignal { get; set; }

		[Inject]
		public global::Kampai.Common.LandExpansionPickSignal landExpansionSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.ITapEventMetricsService tapEventMetricsService { get; set; }

		[Inject]
		public global::Kampai.Common.TikiBarViewPickSignal tikiBarViewPickSignal { get; set; }

		[Inject]
		public global::Kampai.Common.DeselectAllMinionsSignal deselectAllMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.MoveBuildMenuSignal moveBuildMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimedSocialEventService timedSocialEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowActivitySpinnerSignal showActivitySpinnerSignal { get; set; }

		public void OnGameInput(global::UnityEngine.Vector3 inputPosition, int input, bool pressed)
		{
			if (!model.Enabled || model.PanningCameraBlocked)
			{
				return;
			}
			this.inputPosition = inputPosition;
			this.input = input;
			bool flag = pressed && IsPointerOverUIObject(inputPosition);
			bool previousPressState = model.PreviousPressState;
			bool flag2 = model.InvalidateMovement;
			if (!flag2 && flag)
			{
				model.InvalidateMovement = flag;
				flag2 = flag;
			}
			if (input == 0)
			{
				cameraControlsService.OnGameInput(inputPosition, 0);
			}
			if (pressed && !flag2 && !zoomCameraModel.ZoomedIn)
			{
				showHiddenBuildingsSignal.Dispatch();
			}
			if (!previousPressState && pressed && flag2)
			{
				startedOnHUD = true;
			}
			if (model.CurrentMode == global::Kampai.Common.PickControllerModel.Mode.DragAndDrop)
			{
				model.InvalidateMovement = false;
				flag2 = false;
				startedOnHUD = false;
			}
			if (!previousPressState && pressed && !flag2 && model.CurrentMode == global::Kampai.Common.PickControllerModel.Mode.None)
			{
				cameraControlsService.OnGameInput(inputPosition, input);
				TouchStart();
			}
			else if (previousPressState && pressed)
			{
				if (!startedOnHUD)
				{
					cameraControlsService.OnGameInput(inputPosition, input);
				}
				else
				{
					cameraControlsService.OnGameInput(inputPosition, 0);
				}
				if (!flag2)
				{
					TouchHold();
				}
				else if (model.activitySpinnerExists)
				{
					model.activitySpinnerExists = false;
					showActivitySpinnerSignal.Dispatch(false, global::UnityEngine.Vector3.zero);
				}
			}
			else if (previousPressState && !pressed)
			{
				EndTouch();
			}
			model.PreviousPressState = pressed;
			if ((input & 2) != 0 && zoomCameraModel.ZoomedIn)
			{
				ZoomOutOfBuilding();
			}
		}

		private bool IsPointerOverUIObject(global::UnityEngine.Vector3 position)
		{
			global::UnityEngine.EventSystems.PointerEventData pointerEventData = new global::UnityEngine.EventSystems.PointerEventData(global::UnityEngine.EventSystems.EventSystem.current);
			pointerEventData.position = position;
			global::System.Collections.Generic.List<global::UnityEngine.EventSystems.RaycastResult> list = new global::System.Collections.Generic.List<global::UnityEngine.EventSystems.RaycastResult>();
			global::UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointerEventData, list);
			return list.Count > 0;
		}

		private void EndTouch()
		{
			if (!model.DetectedMovement)
			{
				float num = global::UnityEngine.Mathf.Abs(inputPosition.magnitude - model.StartTouchPosition.magnitude);
				if (num >= 15f)
				{
					model.DetectedMovement = true;
				}
				else if (!model.InvalidateMovement)
				{
					moveBuildMenuSignal.Dispatch(false);
				}
			}
			TouchEnd();
		}

		private void Reset()
		{
			model.StartHitObject = null;
			model.EndHitObject = null;
			model.InvalidateMovement = false;
			model.Blocked = false;
			model.HeldTimer = 0f;
			model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.None;
			model.DetectedMovement = false;
			model.ValidLocation = true;
			startedOnHUD = false;
		}

		private void TouchStart()
		{
			model.StartTouchPosition = inputPosition;
			model.StartTouchTimeMs = global::UnityEngine.Time.time;
			global::UnityEngine.Ray ray = camera.ScreenPointToRay(inputPosition);
			if (global::UnityEngine.Physics.Raycast(ray, out hit))
			{
				model.StartHitObject = hit.collider.gameObject;
			}
			if (zoomCameraModel.ZoomedIn)
			{
				if (zoomCameraModel.LastZoomBuildingType == global::Kampai.Game.BuildingZoomType.TIKIBAR)
				{
					model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.TikiBarView;
					return;
				}
				if (zoomCameraModel.LastZoomBuildingType == global::Kampai.Game.BuildingZoomType.STAGE)
				{
					model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.StageView;
					return;
				}
			}
			if (!(model.StartHitObject != null))
			{
				return;
			}
			switch (model.StartHitObject.layer)
			{
			case 9:
			case 14:
				TouchStartHitBuilding();
				break;
			case 8:
				model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.Minion;
				break;
			case 11:
				model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.EnvironmentalMignette;
				break;
			case 12:
				if (!model.SelectedBuilding.HasValue)
				{
					model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.LandExpansion;
				}
				break;
			case 15:
				model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.VillainIsland;
				break;
			case 16:
				model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.VillainLair;
				break;
			case 10:
			case 13:
				break;
			}
		}

		private void TouchHold()
		{
			if (!model.DetectedMovement)
			{
				float num = global::UnityEngine.Mathf.Abs(inputPosition.magnitude - model.StartTouchPosition.magnitude);
				if (num >= 15f)
				{
					model.DetectedMovement = true;
				}
			}
			if (!model.Blocked)
			{
				model.Blocked = true;
			}
			model.HeldTimer += global::UnityEngine.Time.deltaTime;
			int type = 2;
			switch (model.CurrentMode)
			{
			case global::Kampai.Common.PickControllerModel.Mode.TikiBarView:
			case global::Kampai.Common.PickControllerModel.Mode.StageView:
				if (model.DetectedMovement && !timedSocialEventService.isRewardCutscene())
				{
					ZoomOutOfBuilding();
				}
				break;
			case global::Kampai.Common.PickControllerModel.Mode.Building:
				buildingSignal.Dispatch(type, inputPosition);
				break;
			case global::Kampai.Common.PickControllerModel.Mode.DragAndDrop:
				dragAndDropSignal.Dispatch(type, inputPosition, global::Kampai.Game.DragOffsetType.NONE);
				break;
			case global::Kampai.Common.PickControllerModel.Mode.MagnetFinger:
				if (model.ValidLocation && !model.SelectedBuilding.HasValue)
				{
					magnetFingerSignal.Dispatch(type, inputPosition);
				}
				break;
			case global::Kampai.Common.PickControllerModel.Mode.None:
			case global::Kampai.Common.PickControllerModel.Mode.Minion:
				HandleNoneAndMinion();
				break;
			case global::Kampai.Common.PickControllerModel.Mode.EnvironmentalMignette:
			case global::Kampai.Common.PickControllerModel.Mode.LandExpansion:
				break;
			}
		}

		private void HandleNoneAndMinion()
		{
			if (playerService.GetHighestFtueCompleted() >= 9 && model.HeldTimer > 1f && !model.DetectedMovement && input == 1 && !model.SelectedBuilding.HasValue)
			{
				model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.MagnetFinger;
				magnetFingerSignal.Dispatch(1, inputPosition);
				if (model.ValidLocation)
				{
					global::UnityEngine.Vector3 groundPosition = gameContext.injectionBinder.GetInstance<global::Kampai.Game.View.CameraUtils>().GroundPlaneRaycast(inputPosition);
					MoveMinion(groundPosition);
				}
			}
		}

		private void TouchEnd()
		{
			deselectAllMinionsSignal.Dispatch();
			if (model.minionMoveIndicator != null)
			{
				global::UnityEngine.Object.Destroy(model.minionMoveIndicator);
			}
			if (!model.InvalidateMovement)
			{
				global::UnityEngine.Ray ray = camera.ScreenPointToRay(inputPosition);
				if (global::UnityEngine.Physics.Raycast(ray, out hit))
				{
					model.EndHitObject = hit.collider.gameObject;
					if (model.CurrentMode != global::Kampai.Common.PickControllerModel.Mode.DragAndDrop && (model.EndHitObject.name == "StampAlbum" || model.EndHitObject.name == "Shelve"))
					{
						model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.TikiBarView;
					}
				}
				CheckCases();
			}
			tapEventMetricsService.Mark();
			Reset();
		}

		private void CheckCases()
		{
			switch (model.CurrentMode)
			{
			case global::Kampai.Common.PickControllerModel.Mode.Building:
				buildingSignal.Dispatch(3, inputPosition);
				break;
			case global::Kampai.Common.PickControllerModel.Mode.DragAndDrop:
				dragAndDropSignal.Dispatch(3, inputPosition, global::Kampai.Game.DragOffsetType.NONE);
				break;
			case global::Kampai.Common.PickControllerModel.Mode.MagnetFinger:
				magnetFingerSignal.Dispatch(3, inputPosition);
				break;
			case global::Kampai.Common.PickControllerModel.Mode.EnvironmentalMignette:
				if (model.EndHitObject != null)
				{
					environmentalMignetteTappedSignal.Dispatch(model.EndHitObject);
				}
				break;
			case global::Kampai.Common.PickControllerModel.Mode.LandExpansion:
				landExpansionSignal.Dispatch(3, inputPosition);
				break;
			case global::Kampai.Common.PickControllerModel.Mode.TikiBarView:
				TikiBarViewClick();
				break;
			case global::Kampai.Common.PickControllerModel.Mode.StageView:
				StageViewClick();
				break;
			case global::Kampai.Common.PickControllerModel.Mode.VillainIsland:
				if (model.EndHitObject != null)
				{
					VillainIslandClick();
				}
				break;
			case global::Kampai.Common.PickControllerModel.Mode.VillainLair:
				VillainLairClick();
				break;
			case global::Kampai.Common.PickControllerModel.Mode.None:
			case global::Kampai.Common.PickControllerModel.Mode.Minion:
				NoneClick();
				break;
			}
		}

		private void ZoomOutOfBuilding()
		{
			if (zoomCameraModel.ZoomedIn)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.OUT, zoomCameraModel.LastZoomBuildingType));
			}
		}

		private void TikiBarViewClick()
		{
			if (!model.DetectedMovement)
			{
				global::UnityEngine.GameObject endHitObject = model.EndHitObject;
				if (endHitObject != null)
				{
					tikiBarViewPickSignal.Dispatch(endHitObject);
				}
			}
		}

		public void SkipStagePerformance()
		{
			showHUDSignal.Dispatch(true);
			showStoreSignal.Dispatch(true);
			ZoomOutOfBuilding();
		}

		private void StageViewClick()
		{
			if (timedSocialEventService.isRewardCutscene())
			{
				SkipStagePerformance();
			}
			else
			{
				ZoomOutOfBuilding();
			}
		}

		private void VillainIslandClick()
		{
			if (!model.DetectedMovement)
			{
				villainIslandMessageSignal.Dispatch(true);
			}
		}

		private void NoneClick()
		{
			if (model.DetectedMovement)
			{
				return;
			}
			global::UnityEngine.GameObject startHitObject = model.StartHitObject;
			global::UnityEngine.GameObject endHitObject = model.EndHitObject;
			if (startHitObject != null && endHitObject != null && startHitObject == endHitObject)
			{
				minionSignal.Dispatch(model.EndHitObject);
				return;
			}
			global::UnityEngine.Vector3 xZProjection = gameContext.injectionBinder.GetInstance<global::Kampai.Game.View.CameraUtils>().GroundPlaneRaycast(inputPosition);
			global::Kampai.Util.Point p = new global::Kampai.Util.Point
			{
				XZProjection = xZProjection
			};
			global::Kampai.Game.Environment instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.Environment>();
			if (!instance.Contains(p) || !instance.IsWalkable(p.x, p.y))
			{
				model.ValidLocation = false;
			}
		}

		private void MoveMinion(global::UnityEngine.Vector3 groundPosition)
		{
			if (model.MinionMoveToIndicator == null)
			{
				model.MinionMoveToIndicator = global::Kampai.Util.KampaiResources.Load("MinionMoveToIndicator");
			}
			if (model.minionMoveIndicator != null)
			{
				global::UnityEngine.Object.Destroy(model.minionMoveIndicator);
			}
			model.minionMoveIndicator = global::UnityEngine.Object.Instantiate(model.MinionMoveToIndicator) as global::UnityEngine.GameObject;
			model.minionMoveIndicator.transform.position = groundPosition;
		}

		private void TouchStartHitBuilding()
		{
			if (model.SelectedBuilding.HasValue)
			{
				global::UnityEngine.Ray ray = camera.ScreenPointToRay(inputPosition);
				if (global::UnityEngine.Physics.Raycast(ray, out hit, float.PositiveInfinity, 16384))
				{
					model.StartHitObject = hit.collider.gameObject;
				}
				global::Kampai.Game.View.BuildingDefinitionObject component = model.StartHitObject.GetComponent<global::Kampai.Game.View.BuildingDefinitionObject>();
				if (component != null)
				{
					int? selectedBuilding = model.SelectedBuilding;
					if ((selectedBuilding.GetValueOrDefault() == component.ID && selectedBuilding.HasValue) || (component.ID == 0 && model.SelectedBuilding == -1))
					{
						model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.DragAndDrop;
						dragAndDropSignal.Dispatch(1, inputPosition, global::Kampai.Game.DragOffsetType.NONE);
					}
				}
			}
			else
			{
				model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.Building;
			}
		}

		private void VillainLairClick()
		{
			lairEnvironmentElementClickedSignal.Dispatch(model.StartHitObject.GetInstanceID());
		}

		public void SetIgnoreInstanceInput(int instanceId, bool isIgnored)
		{
			model.SetIgnoreInstance(instanceId, isIgnored);
		}

		public global::Kampai.Common.PickState GetPickState()
		{
			global::Kampai.Common.PickState pickState = new global::Kampai.Common.PickState();
			pickState.MinionsSelected = new global::System.Collections.Generic.List<int>(model.SelectedMinions.Keys);
			return pickState;
		}
	}
}
