namespace Kampai.UI.View
{
	public class RewardedVideoHUDView : global::Kampai.Util.KampaiView
	{
		private const string showAnimatorState = "anim_RewardedVideo_slideIn";

		private const string hideAnimatorState = "anim_RewardedVideo_slideOut";

		public global::Kampai.UI.View.ButtonView ShowAdButton;

		public global::UnityEngine.Animator PanelAnimator;

		public global::strange.extensions.signal.impl.Signal SlideOutAnimationCompleteSignal = new global::strange.extensions.signal.impl.Signal();

		public global::Kampai.Game.AdPlacementInstance AdPlacementInstance { get; private set; }

		public void Init(global::Kampai.UI.IPositionService positionService)
		{
			positionService.AddHUDElementToAvoid(base.gameObject);
		}

		public void InitPlacement(global::Kampai.Game.AdPlacementInstance instance)
		{
			AdPlacementInstance = instance;
		}

		public void PlayPanelAnimation(bool show)
		{
			PanelAnimator.Play((!show) ? "anim_RewardedVideo_slideOut" : "anim_RewardedVideo_slideIn");
		}

		public void OnSlideOutAnimationComplete()
		{
			SlideOutAnimationCompleteSignal.Dispatch();
		}
	}
}
