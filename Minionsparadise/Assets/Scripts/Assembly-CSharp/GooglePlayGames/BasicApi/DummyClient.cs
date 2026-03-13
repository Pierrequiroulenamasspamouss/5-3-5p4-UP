namespace GooglePlayGames.BasicApi
{
	public class DummyClient : global::GooglePlayGames.BasicApi.IPlayGamesClient
	{
		public void Authenticate(global::System.Action<bool> callback, bool silent)
		{
			LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public bool IsAuthenticated()
		{
			LogUsage();
			return false;
		}

		public void SignOut()
		{
			LogUsage();
		}

		public string GetAccessToken()
		{
			LogUsage();
			return "DummyAccessToken";
		}

		public void GetIdToken(global::System.Action<string> idTokenCallback)
		{
			LogUsage();
			if (idTokenCallback != null)
			{
				idTokenCallback("DummyIdToken");
			}
		}

		public string GetUserId()
		{
			LogUsage();
			return "DummyID";
		}

		public string GetToken()
		{
			return "DummyToken";
		}

		public void GetServerAuthCode(string serverClientId, global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.ApiNotConnected, "DummyServerAuthCode");
			}
		}

		public string GetUserEmail()
		{
			return string.Empty;
		}

		public void GetUserEmail(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.ApiNotConnected, null);
			}
		}

		public void GetPlayerStats(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, global::GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			LogUsage();
			callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.ApiNotConnected, new global::GooglePlayGames.BasicApi.PlayerStats());
		}

		public string GetUserDisplayName()
		{
			LogUsage();
			return "Player";
		}

		public string GetUserImageUrl()
		{
			LogUsage();
			return null;
		}

		public void LoadUsers(string[] userIds, global::System.Action<global::UnityEngine.SocialPlatforms.IUserProfile[]> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(null);
			}
		}

		public void LoadAchievements(global::System.Action<global::GooglePlayGames.BasicApi.Achievement[]> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(null);
			}
		}

		public global::GooglePlayGames.BasicApi.Achievement GetAchievement(string achId)
		{
			LogUsage();
			return null;
		}

		public void UnlockAchievement(string achId, global::System.Action<bool> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public void RevealAchievement(string achId, global::System.Action<bool> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public void IncrementAchievement(string achId, int steps, global::System.Action<bool> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public void SetStepsAtLeast(string achId, int steps, global::System.Action<bool> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public void ShowAchievementsUI(global::System.Action<global::GooglePlayGames.BasicApi.UIStatus> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(global::GooglePlayGames.BasicApi.UIStatus.VersionUpdateRequired);
			}
		}

		public void ShowLeaderboardUI(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan span, global::System.Action<global::GooglePlayGames.BasicApi.UIStatus> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(global::GooglePlayGames.BasicApi.UIStatus.VersionUpdateRequired);
			}
		}

		public int LeaderboardMaxResults()
		{
			return 25;
		}

		public void LoadScores(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardStart start, int rowCount, global::GooglePlayGames.BasicApi.LeaderboardCollection collection, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan timeSpan, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(new global::GooglePlayGames.BasicApi.LeaderboardScoreData(leaderboardId, global::GooglePlayGames.BasicApi.ResponseStatus.LicenseCheckFailed));
			}
		}

		public void LoadMoreScores(global::GooglePlayGames.BasicApi.ScorePageToken token, int rowCount, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(new global::GooglePlayGames.BasicApi.LeaderboardScoreData(token.LeaderboardId, global::GooglePlayGames.BasicApi.ResponseStatus.LicenseCheckFailed));
			}
		}

		public void SubmitScore(string leaderboardId, long score, global::System.Action<bool> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public void SubmitScore(string leaderboardId, long score, string metadata, global::System.Action<bool> callback)
		{
			LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.IRealTimeMultiplayerClient GetRtmpClient()
		{
			LogUsage();
			return null;
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.ITurnBasedMultiplayerClient GetTbmpClient()
		{
			LogUsage();
			return null;
		}

		public global::GooglePlayGames.BasicApi.SavedGame.ISavedGameClient GetSavedGameClient()
		{
			LogUsage();
			return null;
		}

		public global::GooglePlayGames.BasicApi.Events.IEventsClient GetEventsClient()
		{
			LogUsage();
			return null;
		}

		public global::GooglePlayGames.BasicApi.Quests.IQuestsClient GetQuestsClient()
		{
			LogUsage();
			return null;
		}

		public void RegisterInvitationDelegate(global::GooglePlayGames.BasicApi.InvitationReceivedDelegate invitationDelegate)
		{
			LogUsage();
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.Invitation GetInvitationFromNotification()
		{
			LogUsage();
			return null;
		}

		public bool HasInvitationFromNotification()
		{
			LogUsage();
			return false;
		}

		public void LoadFriends(global::System.Action<bool> callback)
		{
			LogUsage();
			callback(false);
		}

		public global::UnityEngine.SocialPlatforms.IUserProfile[] GetFriends()
		{
			LogUsage();
			return new global::UnityEngine.SocialPlatforms.IUserProfile[0];
		}

		public global::System.IntPtr GetApiClient()
		{
			LogUsage();
			return global::System.IntPtr.Zero;
		}

		private static void LogUsage()
		{
			global::GooglePlayGames.OurUtils.Logger.d("Received method call on DummyClient - using stub implementation.");
		}
	}
}
