namespace Kampai.Game
{
	public class ReengageNotificationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ScheduleNotificationSignal notificationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDevicePrefsService devicePrefsService { get; set; }

		public override void Execute()
		{
			if (!devicePrefsService.GetDevicePrefs().MinionsParadiseNotif)
			{
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.NotificationDefinition> all = definitionService.GetAll<global::Kampai.Game.NotificationDefinition>();
			for (int i = 0; i < all.Count; i++)
			{
				global::Kampai.Game.NotificationDefinition notificationDefinition = all[i];
				if (notificationDefinition.Type == global::Kampai.Game.NotificationType.Reengage.ToString())
				{
					notificationSignal.Dispatch(notificationDefinition);
				}
			}
		}
	}
}
