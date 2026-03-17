namespace Kampai.UI.View
{
	public abstract class AbstractWayFinderMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("AbstractWayFinderMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.OpenBuildingMenuSignal openBuildingMenuSignal;

		private global::Kampai.Game.View.BuildingManagerView buildingManagerView;

		private global::Kampai.UI.View.ButtonView goToButton;

		private int trackedId;

		public abstract global::Kampai.UI.View.IWayFinderView View { get; }

		[Inject]
		public global::Kampai.UI.IPositionService PositionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable GameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToInstanceSignal CameraAutoMoveToInstanceSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService LocalizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal RemoveWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITikiBarService TikiBarService { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel ZoomCameraModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateWayFinderPrioritySignal UpdateWayFinderPrioritySignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService DefinitionService { get; set; }

		[Inject]
		public global::Kampai.Common.TryHarvestBuildingSignal tryHarvestBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.TryCollectLeisurePointsSignal tryCollectLeisurePoints { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickModel { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel lairModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowActivitySpinnerSignal showActivitySpinnerSignal { get; set; }

		public override void OnRegister()
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = GameContext.injectionBinder;
			CameraAutoMoveToInstanceSignal = injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveToInstanceSignal>();
			openBuildingMenuSignal = injectionBinder.GetInstance<global::Kampai.Game.OpenBuildingMenuSignal>();
			buildingManagerView = injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER).GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.UI.View.WayFinderModal component = GetComponent<global::Kampai.UI.View.WayFinderModal>();
			goToButton = component.GoToButton;
			trackedId = component.Settings.TrackedId;
			global::Kampai.UI.View.AbstractWayFinderView abstractWayFinderView = View as global::Kampai.UI.View.AbstractWayFinderView;
			goToButton.ClickedSignal.AddListener(OnGoTo);
			abstractWayFinderView.UpdateWayFinderPrioritySignal.AddListener(UpdateWayFinderPriority);
			abstractWayFinderView.RemoveWayFinderSignal.AddListener(RemoveWayFinder);
			abstractWayFinderView.SimulateClickSignal.AddListener(OnGoTo);
			abstractWayFinderView.Init(PositionService, GameContext, logger, ZoomCameraModel, TikiBarService, PlayerService, LocalizationService, DefinitionService);
		}

		public override void OnRemove()
		{
			global::Kampai.UI.View.AbstractWayFinderView abstractWayFinderView = View as global::Kampai.UI.View.AbstractWayFinderView;
			abstractWayFinderView.Clear();
			goToButton.ClickedSignal.RemoveListener(OnGoTo);
			abstractWayFinderView.UpdateWayFinderPrioritySignal.RemoveListener(UpdateWayFinderPriority);
			abstractWayFinderView.RemoveWayFinderSignal.RemoveListener(RemoveWayFinder);
			abstractWayFinderView.SimulateClickSignal.RemoveListener(OnGoTo);
		}

		private void RemoveWayFinder()
		{
			RemoveWayFinderSignal.Dispatch(trackedId);
		}

		private void UpdateWayFinderPriority()
		{
			UpdateWayFinderPrioritySignal.Dispatch();
		}

		private void OnGoTo()
		{
			if (pickModel.activitySpinnerExists)
			{
				showActivitySpinnerSignal.Dispatch(false, global::UnityEngine.Vector3.zero);
				pickModel.activitySpinnerExists = false;
			}
			GoToClicked();
		}

		protected virtual void GoToClicked()
		{
			if (pickModel.PanningCameraBlocked || lairModel.goingToLair)
			{
				return;
			}
			global::Kampai.Game.Building byInstanceId = PlayerService.GetByInstanceId<global::Kampai.Game.Building>(trackedId);
			if (byInstanceId != null && (View.IsTargetObjectVisible() || byInstanceId.Definition.ID == 3022))
			{
				global::Kampai.Game.View.ScaffoldingBuildingObject scaffoldingBuildingObject = buildingManagerView.GetScaffoldingBuildingObject(trackedId);
				if (scaffoldingBuildingObject != null)
				{
					GameContext.injectionBinder.GetInstance<global::Kampai.Game.RevealBuildingSignal>().Dispatch(byInstanceId);
					return;
				}
				global::Kampai.Game.View.BuildingObject buildingObject = buildingManagerView.GetBuildingObject(trackedId);
				if (!(buildingObject != null))
				{
					return;
				}
				global::Kampai.Game.BuildingState state = byInstanceId.State;
				if (state == global::Kampai.Game.BuildingState.Harvestable || state == global::Kampai.Game.BuildingState.HarvestableAndWorking)
				{
					tryHarvestBuildingSignal.Dispatch(buildingObject, delegate
					{
					}, false);
					global::Kampai.Game.LeisureBuilding leisureBuilding = byInstanceId as global::Kampai.Game.LeisureBuilding;
					if (leisureBuilding != null)
					{
						tryCollectLeisurePoints.Dispatch(leisureBuilding);
					}
				}
				else
				{
					openBuildingMenuSignal.Dispatch(buildingObject, byInstanceId);
				}
			}
			else
			{
				PanToInstance();
			}
		}

		protected virtual void PanToInstance()
		{
			global::Kampai.Game.Instance byInstanceId = PlayerService.GetByInstanceId<global::Kampai.Game.Instance>(trackedId);
			if (byInstanceId != null && !ZoomCameraModel.ZoomedIn)
			{
				CameraAutoMoveToInstanceSignal.Dispatch(new global::Kampai.Game.PanInstructions(byInstanceId), new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(new global::Kampai.Game.ScreenPosition()));
			}
		}

		protected int GetTrackedId()
		{
			return trackedId;
		}
	}
}
