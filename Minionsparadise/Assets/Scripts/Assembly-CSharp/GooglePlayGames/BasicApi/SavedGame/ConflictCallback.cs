namespace GooglePlayGames.BasicApi.SavedGame
{
	public delegate void ConflictCallback(global::GooglePlayGames.BasicApi.SavedGame.IConflictResolver resolver, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata original, byte[] originalData, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata unmerged, byte[] unmergedData);
}
