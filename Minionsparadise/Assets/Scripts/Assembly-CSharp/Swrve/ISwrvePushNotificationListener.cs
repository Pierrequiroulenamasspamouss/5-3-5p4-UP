namespace Swrve
{
	public interface ISwrvePushNotificationListener
	{
		void OnNotificationReceived(global::System.Collections.Generic.Dictionary<string, object> notificationJson);

		void OnOpenedFromPushNotification(global::System.Collections.Generic.Dictionary<string, object> notificationJson);
	}
}
