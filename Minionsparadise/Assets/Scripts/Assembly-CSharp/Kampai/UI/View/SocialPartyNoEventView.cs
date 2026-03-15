namespace Kampai.UI.View
{
	public class SocialPartyNoEventView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.UI.Text TitleText;

		public global::UnityEngine.UI.Text NoEventDescriptionText;

		public global::UnityEngine.UI.Text UpcomingEventDescriptionText;

		public global::UnityEngine.UI.Text YesButtonText;

		public global::Kampai.UI.View.ButtonView YesButton;

		public global::UnityEngine.UI.Text remainingTime;

		private global::Kampai.Game.ITimedSocialEventService timedSocialEventService;

		private global::Kampai.Main.ILocalizationService localService;

		private global::Kampai.Game.ITimeService timeService;

		private int startTime;

		public override void Init()
		{
			base.Init();
			base.Open();
			global::Kampai.Game.TimedSocialEventDefinition nextSocialEvent = timedSocialEventService.GetNextSocialEvent();
			if (nextSocialEvent != null)
			{
				startTime = nextSocialEvent.StartTime;
				NoEventDescriptionText.gameObject.SetActive(false);
				UpcomingEventDescriptionText.text = localService.GetString("socialpartyupcomingeventdescription");
			}
			else
			{
				UpcomingEventDescriptionText.gameObject.SetActive(false);
				remainingTime.gameObject.SetActive(false);
				NoEventDescriptionText.text = localService.GetString("socialpartynoeventdescription");
			}
		}

		public void SetServices(global::Kampai.Game.ITimedSocialEventService timedSocialEventService, global::Kampai.Game.ITimeService timeService, global::Kampai.Main.ILocalizationService localService)
		{
			this.timedSocialEventService = timedSocialEventService;
			this.timeService = timeService;
			this.localService = localService;
		}

		public void Update()
		{
			int num = startTime - timeService.CurrentTime();
			if (num > 0)
			{
				remainingTime.text = UIUtils.FormatSocialTime(num);
			}
		}
	}
}
