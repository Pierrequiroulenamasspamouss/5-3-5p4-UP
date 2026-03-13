namespace GooglePlayGames.BasicApi
{
	public interface IPlayGamesClient
	{
		void Authenticate(global::System.Action<bool> callback, bool silent);

		bool IsAuthenticated();

		void SignOut();

		string GetToken();

		string GetUserId();

		void LoadFriends(global::System.Action<bool> callback);

		string GetUserDisplayName();

		void GetIdToken(global::System.Action<string> idTokenCallback);

		string GetAccessToken();

		void GetServerAuthCode(string serverClientId, global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback);

		string GetUserEmail();

		void GetUserEmail(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback);

		string GetUserImageUrl();

		void GetPlayerStats(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, global::GooglePlayGames.BasicApi.PlayerStats> callback);

		void LoadUsers(string[] userIds, global::System.Action<global::UnityEngine.SocialPlatforms.IUserProfile[]> callback);

		global::GooglePlayGames.BasicApi.Achievement GetAchievement(string achievementId);

		void LoadAchievements(global::System.Action<global::GooglePlayGames.BasicApi.Achievement[]> callback);

		void UnlockAchievement(string achievementId, global::System.Action<bool> successOrFailureCalllback);

		void RevealAchievement(string achievementId, global::System.Action<bool> successOrFailureCalllback);

		void IncrementAchievement(string achievementId, int steps, global::System.Action<bool> successOrFailureCalllback);

		void SetStepsAtLeast(string achId, int steps, global::System.Action<bool> callback);

		void ShowAchievementsUI(global::System.Action<global::GooglePlayGames.BasicApi.UIStatus> callback);

		void ShowLeaderboardUI(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan span, global::System.Action<global::GooglePlayGames.BasicApi.UIStatus> callback);

		void LoadScores(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardStart start, int rowCount, global::GooglePlayGames.BasicApi.LeaderboardCollection collection, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan timeSpan, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback);

		void LoadMoreScores(global::GooglePlayGames.BasicApi.ScorePageToken token, int rowCount, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback);

		int LeaderboardMaxResults();

		void SubmitScore(string leaderboardId, long score, global::System.Action<bool> successOrFailureCalllback);

		void SubmitScore(string leaderboardId, long score, string metadata, global::System.Action<bool> successOrFailureCalllback);

		global::GooglePlayGames.BasicApi.Multiplayer.IRealTimeMultiplayerClient GetRtmpClient();

		global::GooglePlayGames.BasicApi.Multiplayer.ITurnBasedMultiplayerClient GetTbmpClient();

		global::GooglePlayGames.BasicApi.SavedGame.ISavedGameClient GetSavedGameClient();

		global::GooglePlayGames.BasicApi.Events.IEventsClient GetEventsClient();

		global::GooglePlayGames.BasicApi.Quests.IQuestsClient GetQuestsClient();

		void RegisterInvitationDelegate(global::GooglePlayGames.BasicApi.InvitationReceivedDelegate invitationDelegate);

		global::UnityEngine.SocialPlatforms.IUserProfile[] GetFriends();

		global::System.IntPtr GetApiClient();
	}
}
