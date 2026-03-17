namespace Swrve.Messaging
{
	public interface ISwrveMessageAnimator
	{
		void InitMessage(global::Swrve.Messaging.SwrveMessageFormat format);

		void AnimateMessage(global::Swrve.Messaging.SwrveMessageFormat format);

		void AnimateButton(global::Swrve.Messaging.SwrveButton button);

		void AnimateButtonPressed(global::Swrve.Messaging.SwrveButton button);

		bool IsMessageDismissed(global::Swrve.Messaging.SwrveMessageFormat format);
	}
}
