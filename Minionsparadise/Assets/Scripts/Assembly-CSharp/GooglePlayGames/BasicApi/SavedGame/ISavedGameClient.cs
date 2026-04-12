namespace GooglePlayGames.BasicApi.SavedGame
{
	public interface ISavedGameClient
	{
		void OpenWithAutomaticConflictResolution(string filename, global::GooglePlayGames.BasicApi.DataSource source, global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy resolutionStrategy, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> callback);

		void OpenWithManualConflictResolution(string filename, global::GooglePlayGames.BasicApi.DataSource source, bool prefetchDataOnConflict, global::GooglePlayGames.BasicApi.SavedGame.ConflictCallback conflictCallback, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> completedCallback);

		void ReadBinaryData(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata metadata, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, byte[]> completedCallback);

		void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> callback);

		void CommitUpdate(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata metadata, global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> callback);

		void FetchAllSavedGames(global::GooglePlayGames.BasicApi.DataSource source, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata>> callback);

		void Delete(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata metadata);
	}
}
