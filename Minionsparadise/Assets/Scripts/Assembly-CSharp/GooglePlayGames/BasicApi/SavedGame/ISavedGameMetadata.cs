namespace GooglePlayGames.BasicApi.SavedGame
{
	public interface ISavedGameMetadata
	{
		bool IsOpen { get; }

		string Filename { get; }

		string Description { get; }

		string CoverImageURL { get; }

		global::System.TimeSpan TotalTimePlayed { get; }

		global::System.DateTime LastModifiedTimestamp { get; }
	}
}
