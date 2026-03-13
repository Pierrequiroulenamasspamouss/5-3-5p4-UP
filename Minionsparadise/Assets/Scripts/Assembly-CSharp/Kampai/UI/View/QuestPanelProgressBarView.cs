namespace Kampai.UI.View
{
	public class QuestPanelProgressBarView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text TimeRemainingText;

		private int endTime;

		private global::Kampai.Game.ITimeService timeService;

		private int timeRemaining;

		private global::Kampai.Main.ILocalizationService localizationService;

		internal void Init(int UTCEndTime, global::Kampai.Game.ITimeService timeService, global::Kampai.Main.ILocalizationService localizationService)
		{
			endTime = UTCEndTime;
			this.timeService = timeService;
			this.localizationService = localizationService;
		}

		public void Update()
		{
			if (timeService != null)
			{
				UpdateTime(timeService.CurrentTime());
			}
		}

		internal void UpdateTime(int currentTime)
		{
			timeRemaining = endTime - currentTime;
			TimeRemainingText.text = UIUtils.FormatTime(timeRemaining, localizationService);
		}
	}
}
