namespace GooglePlayGames.Native
{
	internal class UnsupportedSavedGamesClient : global::GooglePlayGames.BasicApi.SavedGame.ISavedGameClient
	{
		private readonly string mMessage;

		public UnsupportedSavedGamesClient(string message)
		{
			mMessage = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(message);
		}

		public void OpenWithAutomaticConflictResolution(string filename, global::GooglePlayGames.BasicApi.DataSource source, global::GooglePlayGames.BasicApi.SavedGame.ConflictResolutionStrategy resolutionStrategy, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> callback)
		{
			throw new global::System.NotImplementedException(mMessage);
		}

		public void OpenWithManualConflictResolution(string filename, global::GooglePlayGames.BasicApi.DataSource source, bool prefetchDataOnConflict, global::GooglePlayGames.BasicApi.SavedGame.ConflictCallback conflictCallback, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> completedCallback)
		{
			throw new global::System.NotImplementedException(mMessage);
		}

		public void ReadBinaryData(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata metadata, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, byte[]> completedCallback)
		{
			throw new global::System.NotImplementedException(mMessage);
		}

		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SelectUIStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> callback)
		{
			throw new global::System.NotImplementedException(mMessage);
		}

		public void CommitUpdate(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata metadata, global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata> callback)
		{
			throw new global::System.NotImplementedException(mMessage);
		}

		public void FetchAllSavedGames(global::GooglePlayGames.BasicApi.DataSource source, global::System.Action<global::GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata>> callback)
		{
			throw new global::System.NotImplementedException(mMessage);
		}

		public void Delete(global::GooglePlayGames.BasicApi.SavedGame.ISavedGameMetadata metadata)
		{
			throw new global::System.NotImplementedException(mMessage);
		}
	}
}
