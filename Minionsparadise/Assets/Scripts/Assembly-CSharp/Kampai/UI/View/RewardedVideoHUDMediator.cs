namespace Kampai.UI.View
{
	public class RewardedVideoHUDMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		[Inject]
		public global::Kampai.UI.View.RewardedVideoHUDView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenRewardedAdWatchModalSignal openRewadedAdModalSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickModel { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			view.Init(positionService);
			view.ShowAdButton.ClickedSignal.AddListener(OnShowAdButtonClicked);
		}

		public override void OnRemove()
		{
			view.ShowAdButton.ClickedSignal.RemoveListener(OnShowAdButtonClicked);
		}

		internal void OnShowAdButtonClicked()
		{
			if (!pickModel.PanningCameraBlocked && !pickModel.ZoomingCameraBlocked && !zoomCameraModel.ZoomInProgress)
			{
				openRewadedAdModalSignal.Dispatch(view.AdPlacementInstance);
			}
		}
	}
}
