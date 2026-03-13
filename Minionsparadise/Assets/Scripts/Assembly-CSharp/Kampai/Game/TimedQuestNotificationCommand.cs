namespace Kampai.Game
{
	public class TimedQuestNotificationCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("TimedQuestNotificationCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ScheduleNotificationSignal notificationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDevicePrefsService devicePrefsService { get; set; }

		[Inject]
		public int questId { get; set; }

		public override void Execute()
		{
			if (devicePrefsService.GetDevicePrefs().EventNotif)
			{
				global::Kampai.Game.NotificationDefinition notificationDefinition = definitionService.Get<global::Kampai.Game.NotificationDefinition>(10012);
				global::Kampai.Game.Quest byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Quest>(questId);
				if (byInstanceId == null)
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Error, string.Format("Quest instance id {0} does not exist in the player service", questId));
				}
				global::Kampai.Game.TimedQuestDefinition timedQuestDefinition = definitionService.Get<global::Kampai.Game.TimedQuestDefinition>(byInstanceId.GetActiveDefinition().ID);
				global::Kampai.Game.NotificationDefinition notificationDefinition2 = new global::Kampai.Game.NotificationDefinition();
				notificationDefinition2.ID = questId;
				notificationDefinition2.Type = notificationDefinition.Type;
				notificationDefinition2.Seconds = timedQuestDefinition.PushNoteWarningTime;
				notificationDefinition2.Title = notificationDefinition.Title;
				notificationDefinition2.Text = notificationDefinition.Text;
				notificationSignal.Dispatch(notificationDefinition2);
			}
		}
	}
}
