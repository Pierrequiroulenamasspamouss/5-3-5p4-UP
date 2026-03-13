namespace Kampai.UI.View
{
	public class WatchRewardedAdModalMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.WatchRewardedAdModalView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("WatchRewardedAdModalMediator") as global::Kampai.Util.IKampaiLogger;

		private string prefabName;

		private global::Kampai.Game.AdPlacementInstance adPlacementInstance;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IRewardedAdService rewardedAdService { get; set; }

		[Inject]
		public global::Kampai.UI.View.DeclineRewardedAdShowSignal declineRewadedAdShowSignal { get; set; }

		public override void OnRegister()
		{
			base.closeAllOtherMenuSignal.Dispatch(null);
			base.OnRegister();
			base.view.Init(localService);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.YesButton.onClick.AddListener(OnAcceptAd);
			base.view.NoButton.onClick.AddListener(OnDeclineAd);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			CleanupListeners();
		}

		private void CleanupListeners()
		{
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.YesButton.onClick.RemoveListener(OnAcceptAd);
			base.view.NoButton.onClick.RemoveListener(OnDeclineAd);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			prefabName = args.Get<string>();
			adPlacementInstance = args.Get<global::Kampai.Game.AdPlacementInstance>();
		}

		protected override void Close()
		{
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			base.view.Close();
		}

		private void OnMenuClose()
		{
			hideSkrimSignal.Dispatch("RewardedAdWatch");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, prefabName);
		}

		private void OnAcceptAd()
		{
			if (!base.view.IsAnimationPlaying("Close"))
			{
				rewardedAdService.ShowRewardedVideo(adPlacementInstance);
				base.view.Close(true);
			}
		}

		private void OnDeclineAd()
		{
			declineRewadedAdShowSignal.Dispatch(adPlacementInstance);
			base.view.Close();
		}
	}
}
