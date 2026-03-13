namespace strange.extensions.dispatcher.api
{
	public interface ITriggerable
	{
		bool Trigger<T>(object data);

		bool Trigger(object key, object data);
	}
}
