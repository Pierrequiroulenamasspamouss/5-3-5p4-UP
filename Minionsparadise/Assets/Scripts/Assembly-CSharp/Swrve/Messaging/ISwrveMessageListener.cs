namespace Swrve.Messaging
{
	public interface ISwrveMessageListener
	{
		void OnShow(global::Swrve.Messaging.SwrveMessageFormat format);

		void OnShowing(global::Swrve.Messaging.SwrveMessageFormat format);

		void OnDismiss(global::Swrve.Messaging.SwrveMessageFormat format);
	}
}
