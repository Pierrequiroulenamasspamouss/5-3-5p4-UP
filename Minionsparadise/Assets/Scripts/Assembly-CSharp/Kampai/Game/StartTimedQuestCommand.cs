namespace Kampai.Game
{
	public class StartTimedQuestCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("StartTimedQuestCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.QuestTimeoutSignal timeoutSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TimedQuestNotificationSignal questNoteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public int questId { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Quest byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Quest>(questId);
			if (byInstanceId == null)
			{
				logger.Error("Quest doesn't exist for quest instance id: {0}", questId);
				return;
			}
			global::Kampai.Game.TimedQuestDefinition timedQuestDefinition = byInstanceId.GetActiveDefinition() as global::Kampai.Game.TimedQuestDefinition;
			if (timedQuestDefinition != null)
			{
				byInstanceId.UTCQuestStartTime = timeService.CurrentTime();
				questNoteSignal.Dispatch(byInstanceId.ID);
				timeEventService.AddEvent(byInstanceId.ID, byInstanceId.UTCQuestStartTime, timedQuestDefinition.Duration, timeoutSignal);
			}
		}
	}
}
