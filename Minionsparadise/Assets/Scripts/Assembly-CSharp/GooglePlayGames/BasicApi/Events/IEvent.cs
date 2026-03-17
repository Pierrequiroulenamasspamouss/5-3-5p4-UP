namespace GooglePlayGames.BasicApi.Events
{
	public interface IEvent
	{
		string Id { get; }

		string Name { get; }

		string Description { get; }

		string ImageUrl { get; }

		ulong CurrentCount { get; }

		global::GooglePlayGames.BasicApi.Events.EventVisibility Visibility { get; }
	}
}
