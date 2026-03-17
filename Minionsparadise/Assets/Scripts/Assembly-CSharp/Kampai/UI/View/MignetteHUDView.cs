namespace Kampai.UI.View
{
	public class MignetteHUDView : global::strange.extensions.mediation.impl.View
	{
		public global::Kampai.UI.View.ButtonView CloseButton;

		public global::UnityEngine.UI.Text LabelScore;

		public global::UnityEngine.Transform DooberTarget;

		public global::Kampai.UI.View.KampaiImage CurrencyImage;

		public global::Kampai.UI.View.KampaiProgressBar ProgressBar;

		public global::UnityEngine.UI.Text TimeRemainingLabel;

		public global::UnityEngine.RectTransform CounterRect;

		public global::UnityEngine.UI.Text CounterLabel;

		public global::UnityEngine.Animation CountdownIntroAnimation;

		public global::UnityEngine.GameObject CurrencyPanel;

		private global::Kampai.Main.ILocalizationService localizationService;

		private int previousProgressValue = -1;

		private int previousTimerValue = -1;

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalAudioSignal { get; set; }

		internal void Init(global::Kampai.Main.ILocalizationService localizationService)
		{
			this.localizationService = localizationService;
		}

		public void SetCollectableImage(global::UnityEngine.Sprite image, global::UnityEngine.Sprite mask)
		{
			if (image != null)
			{
				CurrencyImage.sprite = image;
			}
			if (mask != null)
			{
				CurrencyImage.maskSprite = mask;
			}
		}

		public void ShowTimeProgressBar(bool enable)
		{
			if (ProgressBar.gameObject.activeSelf != enable)
			{
				ProgressBar.gameObject.SetActive(enable);
			}
		}

		public void ShowCounter(bool enable)
		{
			if (CounterRect.gameObject.activeSelf != enable)
			{
				CounterRect.gameObject.SetActive(enable);
			}
		}

		public void SetScore(int score)
		{
			LabelScore.text = score.ToString();
		}

		public void SetCounter(int counterValue)
		{
			CounterLabel.text = counterValue.ToString();
		}

		public void SetProgressRemainingText(float progressBarPct)
		{
			int num = global::UnityEngine.Mathf.FloorToInt(progressBarPct * 100f);
			if (num != previousProgressValue)
			{
				previousProgressValue = num;
				TimeRemainingLabel.text = string.Format("{0}%", num);
				ProgressBar.SetProgress(progressBarPct);
			}
		}

		public void SetTime(float timeRemainingInSeconds, float progressBarPct)
		{
			int num = (int)timeRemainingInSeconds;
			if (previousTimerValue != num)
			{
				previousTimerValue = num;
				TimeRemainingLabel.text = UIUtils.FormatTime(num, localizationService);
				if (num < 1)
				{
					progressBarPct = 0f;
				}
				ProgressBar.SetProgress(progressBarPct);
			}
		}

		public void StartCountdown()
		{
			CountdownIntroAnimation.Rewind();
			CountdownIntroAnimation.Play();
			globalAudioSignal.Dispatch("Play_mignette_countDown_01");
		}

		public void StartScorePresentationSequence()
		{
			CloseButton.gameObject.SetActive(false);
			CurrencyPanel.SetActive(false);
		}
	}
}
