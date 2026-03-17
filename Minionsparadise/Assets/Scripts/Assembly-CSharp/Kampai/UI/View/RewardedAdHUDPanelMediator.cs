namespace Kampai.UI.View
{
	public class RewardedAdHUDPanelMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RewardedAdHUDPanelMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.UI.View.RewardedVideoHUDView rewardedVideoHUDView;

		private global::Kampai.Game.AdPlacementName placementName = global::Kampai.Game.AdPlacementName.HUD;

		[Inject]
		public global::Kampai.UI.View.RewardedAdHUDPanelView view { get; set; }

		[Inject]
		public global::Kampai.Game.IRewardedAdService rewardedAdService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.RewardedAdRewardSignal rewardedAdRewardSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenRewardedAdDailyRewardPickerModalSignal openRewardedAdDailyRewardPickerModalSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AdPlacementActivityStateChangedSignal adPlacementActivityStateChangedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateAdHUDSignal updateAdHUDSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			view.Init();
			rewardedAdRewardSignal.AddListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.AddListener(OnAdPlacementActivityStateChanged);
			updateAdHUDSignal.AddListener(UpdateHud);
			UpdateHud();
		}

		public override void OnRemove()
		{
			rewardedAdRewardSignal.RemoveListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.RemoveListener(OnAdPlacementActivityStateChanged);
			updateAdHUDSignal.RemoveListener(UpdateHud);
			SubscribeOnHudButtonHidingAnimationSignal(false);
		}

		private void SubscribeOnHudButtonHidingAnimationSignal(bool subscribe)
		{
			if (subscribe)
			{
				if (rewardedVideoHUDView != null)
				{
					rewardedVideoHUDView.SlideOutAnimationCompleteSignal.AddListener(OnHUDButtonSlideOutAnimationComplete);
				}
			}
			else if (rewardedVideoHUDView != null)
			{
				rewardedVideoHUDView.SlideOutAnimationCompleteSignal.RemoveListener(OnHUDButtonSlideOutAnimationComplete);
			}
		}

		private void OnAdPlacementActivityStateChanged(global::Kampai.Game.AdPlacementInstance placement, bool enabled)
		{
			UpdateHud();
		}

		private void UpdateHud()
		{
			bool flag = rewardedAdService.IsPlacementActive(placementName);
			if (!flag)
			{
				logger.Debug("Ads: placement '{0}' is disabled.", placementName);
			}
			bool flag2 = false;
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			if (minionPartyInstance != null)
			{
				flag2 = minionPartyInstance.IsPartyReady || minionPartyInstance.IsPartyHappening;
			}
			EnableHudButton(flag && !flag2, rewardedAdService.GetPlacementInstance(placementName));
		}

		private void EnableHudButton(bool enable, global::Kampai.Game.AdPlacementInstance instance)
		{
			if (enable && instance != null)
			{
				if (rewardedVideoHUDView != null)
				{
					rewardedVideoHUDView.gameObject.SetActive(true);
					rewardedVideoHUDView.PlayPanelAnimation(true);
				}
				else
				{
					rewardedVideoHUDView = CreateRewardedVideoHudButton();
				}
				rewardedVideoHUDView.InitPlacement(instance);
			}
			else if (rewardedVideoHUDView != null)
			{
				SubscribeOnHudButtonHidingAnimationSignal(true);
				rewardedVideoHUDView.PlayPanelAnimation(false);
			}
		}

		private void OnHUDButtonSlideOutAnimationComplete()
		{
			SubscribeOnHudButtonHidingAnimationSignal(false);
			rewardedVideoHUDView.gameObject.SetActive(false);
		}

		private void OnRewardedAdReward(global::Kampai.Game.AdPlacementInstance placement)
		{
			if (placement.Definition.Name == placementName)
			{
				global::Kampai.Game.OnTheGlassDailyRewardDefinition onTheGlassDailyRewardDefinition = placement.Definition as global::Kampai.Game.OnTheGlassDailyRewardDefinition;
				if (onTheGlassDailyRewardDefinition != null)
				{
					openRewardedAdDailyRewardPickerModalSignal.Dispatch(placement, onTheGlassDailyRewardDefinition);
				}
			}
		}

		private global::Kampai.UI.View.RewardedVideoHUDView CreateRewardedVideoHudButton()
		{
			global::Kampai.UI.View.RewardedVideoHUDView rewardedVideoHUDView = BuildRewardedVideoHudControl();
			rewardedVideoHUDView.transform.SetParent(view.transform, false);
			rewardedVideoHUDView.transform.localScale = new global::UnityEngine.Vector3(1f, 1f, 1f);
			return rewardedVideoHUDView;
		}

		private global::Kampai.UI.View.RewardedVideoHUDView BuildRewardedVideoHudControl()
		{
			string path = "HUD_RewardedVideo";
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load(path) as global::UnityEngine.GameObject;
			if (gameObject == null)
			{
				logger.Error("Unable to load Rewarded Video HUD prefab.");
				return null;
			}
			global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject);
			return gameObject2.GetComponent<global::Kampai.UI.View.RewardedVideoHUDView>();
		}
	}
}
