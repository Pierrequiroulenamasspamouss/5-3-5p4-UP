namespace strange.extensions.dispatcher.api
{
	public interface ITriggerProvider
	{
		int Triggerables { get; }

		void AddTriggerable(global::strange.extensions.dispatcher.api.ITriggerable target);

		void RemoveTriggerable(global::strange.extensions.dispatcher.api.ITriggerable target);
	}
}
