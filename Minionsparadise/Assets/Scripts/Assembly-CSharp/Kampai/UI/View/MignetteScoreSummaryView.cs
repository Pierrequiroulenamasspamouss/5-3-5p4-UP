namespace Kampai.UI.View
{
	public class MignetteScoreSummaryView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.GameObject MainContainer;

		public global::Kampai.UI.View.ButtonView ConfirmButton;

		public global::UnityEngine.UI.Text ConfirmButtonText;

		public global::Kampai.UI.View.AnimatedProgressBarViewObject ProgressBar;

		public global::UnityEngine.GameObject CollectionRewardPanel;

		public global::UnityEngine.GameObject ScorePanel;

		public global::Kampai.UI.View.KampaiImage ProgressBarCurrencyIcon;

		public global::Kampai.UI.View.KampaiImage GroupScoreCurrencyIcon;

		public global::UnityEngine.UI.Text TitleLabel;

		public global::UnityEngine.UI.Text ScoreTitleLabel;

		public global::UnityEngine.UI.Text ScoreAmountLabel;

		public global::UnityEngine.UI.Text TotalScoreLabel;

		public global::UnityEngine.UI.Text FunPointsAmount;

		public global::Kampai.UI.View.KampaiImage MignetteImage;

		public global::System.Collections.Generic.List<global::Kampai.UI.View.MignetteRuleViewObject> RuleDisplays;

		public global::UnityEngine.GameObject LockedOverlay;

		public global::UnityEngine.UI.Image LockedImage;

		public global::strange.extensions.signal.impl.Signal<global::UnityEngine.GameObject> CollectCurrencySignal = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.GameObject>();

		private int newProgressValue;

		private global::Kampai.UI.View.CollectionRewardIndicator rewardIndicatorReadyToCollect;

		private bool hasUnlockedBuildingReward;

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalAudioSignal { get; set; }

		protected override void Start()
		{
			base.Start();
			MainContainer.SetActive(false);
		}

		public void Init(global::Kampai.Main.ILocalizationService localizationService, int score, int collectionProgress, int collectionProgessBeforeReset, int maxScore, int xpReward, global::Kampai.Game.MignetteBuildingDefinition mignetteDefinition, bool showMignetteScoreIncrease, bool isMignetteUnlocked, bool hasUnlockedBuildingReward)
		{
			MainContainer.SetActive(true);
			base.Init();
			int num = 0;
			if (collectionProgress > 0)
			{
				num = collectionProgress - score;
				newProgressValue = collectionProgress;
			}
			else
			{
				num = collectionProgessBeforeReset - score;
				newProgressValue = collectionProgessBeforeReset;
			}
			this.hasUnlockedBuildingReward = hasUnlockedBuildingReward;
			ProgressBar.Init(num, maxScore);
			global::Kampai.UI.View.KampaiImage progressBarCurrencyIcon = ProgressBarCurrencyIcon;
			global::UnityEngine.Sprite sprite = UIUtils.LoadSpriteFromPath(mignetteDefinition.CollectableImage);
			GroupScoreCurrencyIcon.sprite = sprite;
			progressBarCurrencyIcon.sprite = sprite;
			global::Kampai.UI.View.KampaiImage progressBarCurrencyIcon2 = ProgressBarCurrencyIcon;
			sprite = UIUtils.LoadSpriteFromPath(mignetteDefinition.CollectableImageMask);
			GroupScoreCurrencyIcon.maskSprite = sprite;
			progressBarCurrencyIcon2.maskSprite = sprite;
			MignetteImage.sprite = UIUtils.LoadSpriteFromPath(mignetteDefinition.Image);
			MignetteImage.maskSprite = UIUtils.LoadSpriteFromPath(mignetteDefinition.Mask);
			TitleLabel.text = localizationService.GetString(mignetteDefinition.LocalizedKey);
			ScoreTitleLabel.text = localizationService.GetString("MignetteScoreSummary_Score");
			ScoreAmountLabel.text = score.ToString();
			TotalScoreLabel.text = localizationService.GetString("MignetteTotalScore");
			FunPointsAmount.text = xpReward.ToString();
			if (showMignetteScoreIncrease)
			{
				if (hasUnlockedBuildingReward)
				{
					ConfirmButtonText.text = localizationService.GetString("MignetteScoreSummary_CollectButton");
				}
				else
				{
					ConfirmButtonText.text = localizationService.GetString("MignetteScoreSummary_ConfirmButton");
				}
			}
			else
			{
				ConfirmButtonText.text = localizationService.GetString("MignetteScoreSummary_GoToButton");
			}
			LockedOverlay.SetActive(!isMignetteUnlocked);
			ConfirmButton.GetComponent<global::UnityEngine.UI.Button>().interactable = isMignetteUnlocked;
			ConfirmButton.gameObject.SetActive(!showMignetteScoreIncrease);
			ScorePanel.SetActive(showMignetteScoreIncrease);
			base.Open();
		}

		public void CreateRewardIndicator(int requiredPoints, uint rewardQuantity, int maxScore, string rewardImagePath, string rewardMaskImagePath, bool isReadyForCollection)
		{
			global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::Kampai.Util.KampaiResources.Load("cmp_MignetteReward");
			global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject);
			global::Kampai.UI.View.CollectionRewardIndicator component = gameObject2.GetComponent<global::Kampai.UI.View.CollectionRewardIndicator>();
			component.RewardLocLabel.text = UIUtils.FormatLargeNumber(requiredPoints);
			if (rewardQuantity > 1)
			{
				component.RewardCountLabel.text = UIUtils.FormatLargeNumber((int)rewardQuantity);
			}
			else
			{
				component.RewardCountLabel.enabled = false;
			}
			component.RewardImage.sprite = UIUtils.LoadSpriteFromPath(rewardImagePath);
			component.RewardImage.maskSprite = UIUtils.LoadSpriteFromPath(rewardMaskImagePath);
			global::UnityEngine.RectTransform component2 = CollectionRewardPanel.GetComponent<global::UnityEngine.RectTransform>();
			float width = component2.rect.width;
			global::UnityEngine.RectTransform component3 = gameObject2.GetComponent<global::UnityEngine.RectTransform>();
			component3.SetParent(component2, false);
			component3.localScale = gameObject.transform.localScale;
			component3.localPosition = global::UnityEngine.Vector3.zero;
			component3.anchoredPosition = new global::UnityEngine.Vector2(width * (float)requiredPoints / (float)maxScore, 0f);
			if (isReadyForCollection)
			{
				rewardIndicatorReadyToCollect = component;
			}
		}

		public void RefreshProgressBar(global::System.Action onComplete)
		{
			ProgressBar.AnimateToValue(newProgressValue);
			if (rewardIndicatorReadyToCollect != null)
			{
				StartCoroutine(WaitThenShowRewardIndicatorAndConfirmButton(onComplete));
			}
			else
			{
				StartCoroutine(WaitThenShowConfirmButton(onComplete));
			}
		}

		private global::System.Collections.IEnumerator WaitThenShowRewardIndicatorAndConfirmButton(global::System.Action onComplete)
		{
			yield return new global::UnityEngine.WaitForSeconds(ProgressBar.FillMainWaitTime + 1.2f);
			globalAudioSignal.Dispatch("Play_Mign_receivedAward_01");
			rewardIndicatorReadyToCollect.PlayCollectAnimation();
			yield return new global::UnityEngine.WaitForSeconds(1.6f);
			if (!hasUnlockedBuildingReward)
			{
				CollectCurrencySignal.Dispatch(rewardIndicatorReadyToCollect.RewardImage.gameObject);
				yield return new global::UnityEngine.WaitForSeconds(2f);
			}
			onComplete();
		}

		private global::System.Collections.IEnumerator WaitThenShowConfirmButton(global::System.Action onComplete)
		{
			yield return new global::UnityEngine.WaitForSeconds(ProgressBar.FillMainWaitTime + 0.3f);
			onComplete();
		}
	}
}
