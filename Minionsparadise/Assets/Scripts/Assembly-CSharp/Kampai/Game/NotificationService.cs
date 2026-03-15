namespace Kampai.Game
{
	public class NotificationService : global::Kampai.Game.INotificationService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("NotificationService") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		public void Initialize()
		{
		}

		public void ScheduleLocalNotification(global::Kampai.Game.Notification notification)
		{
			if (!coppaService.Restricted())
			{
				string sound = null;
				if (!string.IsNullOrEmpty(notification.sound))
				{
					sound = notification.sound;
				}
				global::Kampai.Util.Native.ScheduleLocalNotification(notification.type, notification.secondsFromNow, notification.title, notification.text, notification.stackedTitle, notification.stackedText, string.Empty, sound, string.Empty, notification.badgeNumber);
			}
		}

		public void CancelLocalNotification(string type)
		{
			global::Kampai.Util.Native.CancelLocalNotification(type);
		}

		public void CancelAllNotifications()
		{
			global::Kampai.Util.Native.CancelAllLocalNotifications();
		}
	}
}
