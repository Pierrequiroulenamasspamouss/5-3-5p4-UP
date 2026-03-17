namespace Swrve.Messaging
{
	public interface ISwrveTriggeredMessageListener
	{
		void OnMessageTriggered(global::Swrve.Messaging.SwrveMessage message);

		void DismissCurrentMessage();
	}
}
