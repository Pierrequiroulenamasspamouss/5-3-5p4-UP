namespace Kampai.Game
{
	public class ScheduleNotificationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.NotificationDefinition notification { get; set; }

		[Inject]
		public global::Kampai.Game.INotificationService service { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Notification notification = new global::Kampai.Game.Notification();
			notification.type = this.notification.Type;
			notification.secondsFromNow = this.notification.Seconds;
			notification.title = localService.GetString(this.notification.Title);
			notification.text = localService.GetString(this.notification.Text);
			notification.sound = this.notification.Sound;
			global::Kampai.Game.Notification notification2 = notification;
			notification2.stackedTitle = notification2.title;
			notification2.stackedText = notification2.text;
			switch ((global::Kampai.Game.NotificationType)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.NotificationType), this.notification.Type))
			{
			case global::Kampai.Game.NotificationType.MignetteCooldownComplete:
				notification2.type = this.notification.Track.ToString();
				break;
			case global::Kampai.Game.NotificationType.MarketplaceSaleComplete:
				notification2.type = string.Format("{0}_{1}", this.notification.Type, this.notification.Track);
				break;
			case global::Kampai.Game.NotificationType.DebugConsole:
				notification2.sound = "bob_booya";
				break;
			}
			if (string.IsNullOrEmpty(notification2.sound))
			{
				notification2.sound = string.Empty;
			}
			service.ScheduleLocalNotification(notification2);
		}
	}
}
