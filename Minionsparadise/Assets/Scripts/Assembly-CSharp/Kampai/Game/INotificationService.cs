namespace Kampai.Game
{
	public interface INotificationService
	{
		void Initialize();

		void ScheduleLocalNotification(global::Kampai.Game.Notification notification);

		void CancelLocalNotification(string type);

		void CancelAllNotifications();
	}
}
