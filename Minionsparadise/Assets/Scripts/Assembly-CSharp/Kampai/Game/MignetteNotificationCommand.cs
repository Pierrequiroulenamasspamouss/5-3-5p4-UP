namespace Kampai.Game
{
	public class MignetteNotificationCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MignetteNotificationCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public bool cancelNotification { get; set; }

		[Inject]
		public int buildingId { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CancelNotificationSignal cancelNotificationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ScheduleNotificationSignal scheduleNotificationSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.IBuildingWithCooldown byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.IBuildingWithCooldown>(buildingId);
			if (cancelNotification)
			{
				cancelNotificationSignal.Dispatch(byInstanceId.Definition.ID.ToString());
				return;
			}
			if (byInstanceId == null)
			{
				logger.Error("No IBuildingWithCooldown exists for buildingId: {0}", buildingId);
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.NotificationDefinition> all = definitionService.GetAll<global::Kampai.Game.NotificationDefinition>();
			foreach (global::Kampai.Game.NotificationDefinition item in all)
			{
				if (item.Type.Equals(global::Kampai.Game.NotificationType.MignetteCooldownComplete.ToString()))
				{
					cancelNotificationSignal.Dispatch(item.Track.ToString());
				}
			}
			foreach (global::Kampai.Game.NotificationDefinition item2 in all)
			{
				if (item2.Type.Equals(global::Kampai.Game.NotificationType.MignetteCooldownComplete.ToString()) && item2.Track == byInstanceId.Definition.ID)
				{
					item2.Seconds = byInstanceId.GetCooldown();
					scheduleNotificationSignal.Dispatch(item2);
				}
			}
		}
	}
}
