namespace Kampai.UI.View
{
	public class WorldProgressPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("WorldProgressPanelMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.WorldProgressPanelView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel model { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayWorldProgressSignal displayWorldProgressSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWorldProgressSignal removeWorldProgressSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowMoveBuildingMenuSignal showMoveBuildingMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal createWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayFinderSignal { get; set; }

		public override void OnRegister()
		{
			view.Init(logger);
			displayWorldProgressSignal.AddListener(DisplayWorldProgress);
			removeWorldProgressSignal.AddListener(RemoveWorldProgress);
			showMoveBuildingMenuSignal.AddListener(ShowMoveBuildingMenu);
		}

		public override void OnRemove()
		{
			view.Cleanup();
			displayWorldProgressSignal.RemoveListener(DisplayWorldProgress);
			removeWorldProgressSignal.RemoveListener(RemoveWorldProgress);
			showMoveBuildingMenuSignal.RemoveListener(ShowMoveBuildingMenu);
		}

		private void ShowMoveBuildingMenu(bool show, global::Kampai.UI.View.MoveBuildingSetting setting)
		{
			if (show)
			{
				createWayFinderSignal.Dispatch(new global::Kampai.UI.View.WayFinderSettings(setting.TrackedId));
				global::UnityEngine.GameObject type = view.CreateOrUpdateMoveBuildingMenu(setting);
				closeAllMenuSignal.Dispatch(type);
				hideAllWayFindersSignal.Dispatch();
				model.BuildingDragMode = true;
			}
			else
			{
				removeWayFinderSignal.Dispatch(setting.TrackedId);
				showAllWayFindersSignal.Dispatch();
				view.RemoveMoveBuildingMenu();
				model.BuildingDragMode = false;
			}
		}

		private void DisplayWorldProgress(global::Kampai.UI.View.ProgressBarSettings settings)
		{
			view.CreateProgressBar(settings);
		}

		private void RemoveWorldProgress(int trackedId)
		{
			view.RemoveProgressBar(trackedId);
		}
	}
}
