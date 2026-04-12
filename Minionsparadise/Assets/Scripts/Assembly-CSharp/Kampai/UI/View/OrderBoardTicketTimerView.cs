namespace Kampai.UI.View
{
	public class OrderBoardTicketTimerView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.DoubleConfirmButtonView RushButton;

		public global::UnityEngine.UI.Text GemCountText;

		public global::UnityEngine.UI.Text CountDownClockText;

		internal int rushCost;

		private bool inProgress;

		private int index;

		private int duration;

		private global::Kampai.Main.ILocalizationService localizationService;

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		internal void Init(global::Kampai.Main.ILocalizationService localization)
		{
			localizationService = localization;
		}

		internal void StartTimer(int index, int duration)
		{
			inProgress = true;
			this.index = -index;
			this.duration = duration;
			Update();
		}

		public void Update()
		{
			if (inProgress && duration >= 0 && timeEventService != null)
			{
				int timeRemaining = timeEventService.GetTimeRemaining(index);
				switch (timeRemaining)
				{
				case 0:
					inProgress = false;
					break;
				case -1:
					inProgress = false;
					return;
				}
				CountDownClockText.text = UIUtils.FormatTime(timeRemaining, localizationService);
				rushCost = timeEventService.CalculateRushCostForTimer(timeRemaining, global::Kampai.Game.RushActionType.COOLDOWN);
				GemCountText.text = rushCost.ToString();
			}
		}
	}
}
