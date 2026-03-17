namespace Kampai.UI.View
{
	public class ProgressBarView : global::Kampai.UI.View.WorldToGlassView
	{
		internal global::Kampai.UI.View.DoubleConfirmButtonView rushButton;

		internal global::strange.extensions.signal.impl.Signal OnShowSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::strange.extensions.signal.impl.Signal OnRemoveSignal = new global::strange.extensions.signal.impl.Signal();

		private global::UnityEngine.UI.Text timeRemainingText;

		private global::UnityEngine.UI.Text rushText;

		private global::UnityEngine.UI.Text percentageText;

		private global::UnityEngine.UI.Image fillImage;

		private global::UnityEngine.Vector2 fillPosition;

		private bool isTimerStopped;

		internal global::strange.extensions.signal.impl.Signal<int> OnTimerCompleteSignal { get; private set; }

		internal int startTime { get; private set; }

		internal int endTime { get; private set; }

		protected override string UIName
		{
			get
			{
				return "ResourceIcon";
			}
		}

		internal new void Init(global::Kampai.UI.IPositionService positionService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IPlayerService playerService, global::Kampai.Main.ILocalizationService localizationService)
		{
			base.Init(positionService, gameContext, logger, playerService, localizationService);
		}

		protected override void LoadModalData(global::Kampai.UI.View.WorldToGlassUIModal modal)
		{
			global::Kampai.UI.View.ProgressBarModal progressBarModal = modal as global::Kampai.UI.View.ProgressBarModal;
			if (progressBarModal == null)
			{
				logger.Error("Progress Bar modal doesn't exist!");
				return;
			}
			timeRemainingText = progressBarModal.timeRemainingText;
			rushText = progressBarModal.rushText;
			percentageText = progressBarModal.percentageText;
			fillImage = progressBarModal.fillImage;
			rushButton = progressBarModal.rushButton;
			global::Kampai.UI.View.ProgressBarSettings progressBarSettings = progressBarModal.Settings as global::Kampai.UI.View.ProgressBarSettings;
			OnTimerCompleteSignal = progressBarSettings.ConstructionCompleteSignal;
			startTime = progressBarSettings.StartTime;
			endTime = progressBarSettings.Duration + startTime;
		}

		internal void StartTime(int startTime, int endTime)
		{
			SetTimeRemainingText(endTime - startTime);
			this.startTime = startTime;
			this.endTime = endTime;
			fillPosition = fillImage.rectTransform.anchorMax;
		}

		internal void StopTime()
		{
			isTimerStopped = true;
		}

		protected override void OnHide()
		{
			base.OnHide();
			HideText();
		}

		protected override void OnShow()
		{
			OnShowSignal.Dispatch();
			base.OnShow();
			ShowText();
		}

		private void ShowText()
		{
			timeRemainingText.enabled = true;
			rushText.enabled = true;
			percentageText.enabled = true;
		}

		private void HideText()
		{
			timeRemainingText.enabled = false;
			rushText.enabled = false;
			percentageText.enabled = false;
		}

		internal override bool CanUpdate()
		{
			if (!base.CanUpdate())
			{
				return false;
			}
			if (isTimerStopped)
			{
				return false;
			}
			return true;
		}

		internal override void TargetObjectNullResponse()
		{
			logger.Warning("Removing ProgressBar with id: {0} since the target object does not exist", m_trackedId);
			OnRemoveSignal.Dispatch();
		}

		internal void UpdateTime(int timeRemaining)
		{
			int num = endTime - startTime;
			float num2 = 1f - (float)timeRemaining / (float)num;
			fillPosition.x = num2;
			fillImage.rectTransform.anchorMax = fillPosition;
			percentageText.text = string.Format("{0}%", (int)(num2 * 100f));
			SetTimeRemainingText(timeRemaining);
		}

		internal void SetTimeRemainingText(int time)
		{
			timeRemainingText.text = UIUtils.FormatTime(time, localizationService);
		}

		internal void SetRushCost(int rushCost)
		{
			rushText.text = string.Format("{0}", rushCost.ToString());
		}
	}
}
