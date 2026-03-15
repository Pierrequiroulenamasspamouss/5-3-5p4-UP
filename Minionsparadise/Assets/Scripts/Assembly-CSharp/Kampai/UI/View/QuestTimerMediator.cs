namespace Kampai.UI.View
{
	public class QuestTimerMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.QuestTimerView view { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.QuestDetailIDSignal questIdSignal { get; set; }

		[Inject]
		public global::Kampai.Game.QuestTimeoutSignal timeoutSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		public override void OnRegister()
		{
			view.timeEventService = timeEventService;
			view.playerService = playerService;
			view.localizationService = localizationService;
			questIdSignal.AddListener(EnableQuestTimer);
			timeoutSignal.AddListener(DisableQuestTimer);
		}

		public override void OnRemove()
		{
			questIdSignal.RemoveListener(EnableQuestTimer);
			timeoutSignal.RemoveListener(DisableQuestTimer);
		}

		private void EnableQuestTimer(int questId)
		{
			view.EnableQuestTimer(questId);
		}

		private void DisableQuestTimer(int questId)
		{
			view.DisableQuestTimer(questId);
		}
	}
}
