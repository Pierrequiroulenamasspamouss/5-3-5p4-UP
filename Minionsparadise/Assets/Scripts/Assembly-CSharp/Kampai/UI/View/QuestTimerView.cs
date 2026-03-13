namespace Kampai.UI.View
{
	public class QuestTimerView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Image timerEmpty;

		public global::UnityEngine.UI.Image timerFill;

		public global::UnityEngine.UI.Text timerText;

		internal global::Kampai.Game.ITimeEventService timeEventService;

		internal global::Kampai.Game.IPlayerService playerService;

		internal global::Kampai.Main.ILocalizationService localizationService;

		private int questId;

		private void Update()
		{
			UpdateTimer();
		}

		internal void EnableQuestTimer(int questId)
		{
			this.questId = questId;
			global::Kampai.Game.Quest byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Quest>(questId);
			bool flag = false;
			if (byInstanceId != null)
			{
				global::Kampai.Game.TimedQuestDefinition timedQuestDefinition = byInstanceId.GetActiveDefinition() as global::Kampai.Game.TimedQuestDefinition;
				global::Kampai.Game.LimitedQuestDefinition limitedQuestDefinition = byInstanceId.GetActiveDefinition() as global::Kampai.Game.LimitedQuestDefinition;
				flag = timedQuestDefinition != null || limitedQuestDefinition != null;
			}
			SetEnabled(flag);
		}

		internal void DisableQuestTimer(int questId)
		{
			if (this.questId == questId)
			{
				SetEnabled(false);
			}
		}

		private void UpdateTimer()
		{
			int timeRemaining = timeEventService.GetTimeRemaining(questId);
			if (timeRemaining < 0)
			{
				SetEnabled(false);
			}
			int eventDuration = timeEventService.GetEventDuration(questId);
			timerText.text = UIUtils.FormatTime(timeRemaining, localizationService);
			timerFill.fillAmount = (float)timeRemaining / (float)eventDuration;
		}

		private void SetEnabled(bool isEnabled)
		{
			GetComponent<global::UnityEngine.UI.Image>().enabled = isEnabled;
			timerEmpty.enabled = isEnabled;
			timerFill.enabled = isEnabled;
			timerText.enabled = isEnabled;
		}
	}
}
