namespace GooglePlayGames.BasicApi.Events
{
	public interface IEventsClient
	{
		void FetchAllEvents(global::GooglePlayGames.BasicApi.DataSource source, global::System.Action<global::GooglePlayGames.BasicApi.ResponseStatus, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Events.IEvent>> callback);

		void FetchEvent(global::GooglePlayGames.BasicApi.DataSource source, string eventId, global::System.Action<global::GooglePlayGames.BasicApi.ResponseStatus, global::GooglePlayGames.BasicApi.Events.IEvent> callback);

		void IncrementEvent(string eventId, uint stepsToIncrement);
	}
}
