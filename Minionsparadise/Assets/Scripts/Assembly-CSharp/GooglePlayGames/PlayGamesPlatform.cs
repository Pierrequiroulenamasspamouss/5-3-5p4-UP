namespace GooglePlayGames
{
	public class PlayGamesPlatform : global::UnityEngine.SocialPlatforms.ISocialPlatform
	{
		private static volatile global::GooglePlayGames.PlayGamesPlatform sInstance;

		private static volatile bool sNearbyInitializePending;

		private static volatile global::GooglePlayGames.BasicApi.Nearby.INearbyConnectionClient sNearbyConnectionClient;

		private readonly global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration mConfiguration;

		private global::GooglePlayGames.PlayGamesLocalUser mLocalUser;

		private global::GooglePlayGames.BasicApi.IPlayGamesClient mClient;

		private string mDefaultLbUi;

		private global::System.Collections.Generic.Dictionary<string, string> mIdMap = new global::System.Collections.Generic.Dictionary<string, string>();

		public static bool DebugLogEnabled
		{
			get
			{
				return global::GooglePlayGames.OurUtils.Logger.DebugLogEnabled;
			}
			set
			{
				global::GooglePlayGames.OurUtils.Logger.DebugLogEnabled = value;
			}
		}

		public static global::GooglePlayGames.PlayGamesPlatform Instance
		{
			get
			{
				if (sInstance == null)
				{
					global::GooglePlayGames.OurUtils.Logger.d("Instance was not initialized, using default configuration.");
					InitializeInstance(global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration.DefaultConfiguration);
				}
				return sInstance;
			}
		}

		public static global::GooglePlayGames.BasicApi.Nearby.INearbyConnectionClient Nearby
		{
			get
			{
				if (sNearbyConnectionClient == null && !sNearbyInitializePending)
				{
					sNearbyInitializePending = true;
					InitializeNearby(null);
				}
				return sNearbyConnectionClient;
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.IRealTimeMultiplayerClient RealTime
		{
			get
			{
				return mClient.GetRtmpClient();
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.ITurnBasedMultiplayerClient TurnBased
		{
			get
			{
				return mClient.GetTbmpClient();
			}
		}

		public global::GooglePlayGames.BasicApi.SavedGame.ISavedGameClient SavedGame
		{
			get
			{
				return mClient.GetSavedGameClient();
			}
		}

		public global::GooglePlayGames.BasicApi.Events.IEventsClient Events
		{
			get
			{
				return mClient.GetEventsClient();
			}
		}

		public global::GooglePlayGames.BasicApi.Quests.IQuestsClient Quests
		{
			get
			{
				return mClient.GetQuestsClient();
			}
		}

		public global::UnityEngine.SocialPlatforms.ILocalUser localUser
		{
			get
			{
				return mLocalUser;
			}
		}

		internal PlayGamesPlatform(global::GooglePlayGames.BasicApi.IPlayGamesClient client)
		{
			mClient = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(client);
			mLocalUser = new global::GooglePlayGames.PlayGamesLocalUser(this);
			mConfiguration = global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration.DefaultConfiguration;
		}

		private PlayGamesPlatform(global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration configuration)
		{
			mLocalUser = new global::GooglePlayGames.PlayGamesLocalUser(this);
			mConfiguration = configuration;
		}

		public static void InitializeInstance(global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration configuration)
		{
			if (sInstance != null)
			{
				global::GooglePlayGames.OurUtils.Logger.w("PlayGamesPlatform already initialized. Ignoring this call.");
			}
			else
			{
				sInstance = new global::GooglePlayGames.PlayGamesPlatform(configuration);
			}
		}

		public static void InitializeNearby(global::System.Action<global::GooglePlayGames.BasicApi.Nearby.INearbyConnectionClient> callback)
		{
			global::UnityEngine.Debug.Log("Calling InitializeNearby!");
			if (sNearbyConnectionClient == null)
			{
				global::GooglePlayGames.NearbyConnectionClientFactory.Create(delegate(global::GooglePlayGames.BasicApi.Nearby.INearbyConnectionClient client)
				{
					global::UnityEngine.Debug.Log("Nearby Client Created!!");
					sNearbyConnectionClient = client;
					if (callback != null)
					{
						callback(client);
					}
					else
					{
						global::UnityEngine.Debug.Log("Initialize Nearby callback is null");
					}
				});
			}
			else if (callback != null)
			{
				global::UnityEngine.Debug.Log("Nearby Already initialized: calling callback directly");
				callback(sNearbyConnectionClient);
			}
			else
			{
				global::UnityEngine.Debug.Log("Nearby Already initialized");
			}
		}

		public static global::GooglePlayGames.PlayGamesPlatform Activate()
		{
			global::GooglePlayGames.OurUtils.Logger.d("Activating PlayGamesPlatform.");
			global::UnityEngine.Social.Active = Instance;
			global::GooglePlayGames.OurUtils.Logger.d("PlayGamesPlatform activated: " + global::UnityEngine.Social.Active);
			return Instance;
		}

		public global::System.IntPtr GetApiClient()
		{
			return mClient.GetApiClient();
		}

		public void AddIdMapping(string fromId, string toId)
		{
			mIdMap[fromId] = toId;
		}

		public void Authenticate(global::System.Action<bool> callback)
		{
			Authenticate(callback, false);
		}

		public void Authenticate(global::System.Action<bool> callback, bool silent)
		{
			if (mClient == null)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Creating platform-specific Play Games client.");
				mClient = global::GooglePlayGames.PlayGamesClientFactory.GetPlatformPlayGamesClient(mConfiguration);
			}
			mClient.Authenticate(callback, silent);
		}

		public void Authenticate(global::UnityEngine.SocialPlatforms.ILocalUser unused, global::System.Action<bool> callback)
		{
			Authenticate(callback, false);
		}

		public void Authenticate(global::UnityEngine.SocialPlatforms.ILocalUser user, global::System.Action<bool, string> callback)
		{
			Authenticate((bool success) =>
			{
				if (callback != null)
				{
					callback(success, string.Empty);
				}
			});
		}

		public bool IsAuthenticated()
		{
			return mClient != null && mClient.IsAuthenticated();
		}

		public void SignOut()
		{
			if (mClient != null)
			{
				mClient.SignOut();
			}
			mLocalUser = new global::GooglePlayGames.PlayGamesLocalUser(this);
		}

		public void LoadUsers(string[] userIds, global::System.Action<global::UnityEngine.SocialPlatforms.IUserProfile[]> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				callback(new global::UnityEngine.SocialPlatforms.IUserProfile[0]);
			}
			mClient.LoadUsers(userIds, callback);
		}

		public string GetUserId()
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				return "0";
			}
			return mClient.GetUserId();
		}

		public void GetIdToken(global::System.Action<string> idTokenCallback)
		{
			if (mClient != null)
			{
				mClient.GetIdToken(idTokenCallback);
				return;
			}
			global::GooglePlayGames.OurUtils.Logger.e("No client available, calling back with null.");
			idTokenCallback(null);
		}

		public string GetAccessToken()
		{
			if (mClient != null)
			{
				return mClient.GetAccessToken();
			}
			return null;
		}

		public void GetServerAuthCode(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback)
		{
			if (mClient != null && mClient.IsAuthenticated())
			{
				if (global::GooglePlayGames.GameInfo.WebClientIdInitialized())
				{
					mClient.GetServerAuthCode("247013943331-p01pqndt6uksnjbqv9vpv9qo2uj8ks7b.apps.googleusercontent.com", callback);
					return;
				}
				global::GooglePlayGames.OurUtils.Logger.e("GetServerAuthCode requires a webClientId.");
				callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.DeveloperError, string.Empty);
			}
			else
			{
				global::GooglePlayGames.OurUtils.Logger.e("GetServerAuthCode can only be called after authentication.");
				callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.SignInRequired, string.Empty);
			}
		}

		public string GetUserEmail()
		{
			if (mClient != null)
			{
				return mClient.GetUserEmail();
			}
			return null;
		}

		public void GetUserEmail(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback)
		{
			mClient.GetUserEmail(callback);
		}

		public void GetPlayerStats(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, global::GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			if (mClient != null && mClient.IsAuthenticated())
			{
				mClient.GetPlayerStats(callback);
				return;
			}
			global::GooglePlayGames.OurUtils.Logger.e("GetPlayerStats can only be called after authentication.");
			callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.SignInRequired, new global::GooglePlayGames.BasicApi.PlayerStats());
		}

		public global::GooglePlayGames.BasicApi.Achievement GetAchievement(string achievementId)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("GetAchievement can only be called after authentication.");
				return null;
			}
			return mClient.GetAchievement(achievementId);
		}

		public string GetUserDisplayName()
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("GetUserDisplayName can only be called after authentication.");
				return string.Empty;
			}
			return mClient.GetUserDisplayName();
		}

		public string GetUserImageUrl()
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("GetUserImageUrl can only be called after authentication.");
				return null;
			}
			return mClient.GetUserImageUrl();
		}

		public void ReportProgress(string achievementID, double progress, global::System.Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("ReportProgress can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			global::GooglePlayGames.OurUtils.Logger.d("ReportProgress, " + achievementID + ", " + progress);
			achievementID = MapId(achievementID);
			if (progress < 1E-06)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Progress 0.00 interpreted as request to reveal.");
				mClient.RevealAchievement(achievementID, callback);
				return;
			}
			bool flag = false;
			int num = 0;
			int num2 = 0;
			global::GooglePlayGames.BasicApi.Achievement achievement = mClient.GetAchievement(achievementID);
			if (achievement == null)
			{
				global::GooglePlayGames.OurUtils.Logger.w("Unable to locate achievement " + achievementID);
				global::GooglePlayGames.OurUtils.Logger.w("As a quick fix, assuming it's standard.");
				flag = false;
			}
			else
			{
				flag = achievement.IsIncremental;
				num = achievement.CurrentSteps;
				num2 = achievement.TotalSteps;
				global::GooglePlayGames.OurUtils.Logger.d("Achievement is " + ((!flag) ? "STANDARD" : "INCREMENTAL"));
				if (flag)
				{
					global::GooglePlayGames.OurUtils.Logger.d("Current steps: " + num + "/" + num2);
				}
			}
			if (flag)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " interpreted as incremental target (approximate).");
				if (progress >= 0.0 && progress <= 1.0)
				{
					global::GooglePlayGames.OurUtils.Logger.w("Progress " + progress + " is less than or equal to 1. You might be trying to use values in the range of [0,1], while values are expected to be within the range [0,100]. If you are using the latter, you can safely ignore this message.");
				}
				int num3 = (int)(progress / 100.0 * (double)num2);
				int num4 = num3 - num;
				global::GooglePlayGames.OurUtils.Logger.d("Target steps: " + num3 + ", cur steps:" + num);
				global::GooglePlayGames.OurUtils.Logger.d("Steps to increment: " + num4);
				if (num4 >= 0)
				{
					mClient.IncrementAchievement(achievementID, num4, callback);
				}
			}
			else if (progress >= 100.0)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " interpreted as UNLOCK.");
				mClient.UnlockAchievement(achievementID, callback);
			}
			else
			{
				global::GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " not enough to unlock non-incremental achievement.");
			}
		}

		public void IncrementAchievement(string achievementID, int steps, global::System.Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("IncrementAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				global::GooglePlayGames.OurUtils.Logger.d("IncrementAchievement: " + achievementID + ", steps " + steps);
				achievementID = MapId(achievementID);
				mClient.IncrementAchievement(achievementID, steps, callback);
			}
		}

		public void SetStepsAtLeast(string achievementID, int steps, global::System.Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("SetStepsAtLeast can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				global::GooglePlayGames.OurUtils.Logger.d("SetStepsAtLeast: " + achievementID + ", steps " + steps);
				achievementID = MapId(achievementID);
				mClient.SetStepsAtLeast(achievementID, steps, callback);
			}
		}

		public void LoadAchievementDescriptions(global::System.Action<global::UnityEngine.SocialPlatforms.IAchievementDescription[]> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("LoadAchievementDescriptions can only be called after authentication.");
				if (callback != null)
				{
					callback(null);
				}
				return;
			}
			mClient.LoadAchievements(delegate(global::GooglePlayGames.BasicApi.Achievement[] ach)
			{
				global::UnityEngine.SocialPlatforms.IAchievementDescription[] array = new global::UnityEngine.SocialPlatforms.IAchievementDescription[ach.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new global::GooglePlayGames.PlayGamesAchievement(ach[i]);
				}
				callback(array);
			});
		}

		public void LoadAchievements(global::System.Action<global::UnityEngine.SocialPlatforms.IAchievement[]> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("LoadAchievements can only be called after authentication.");
				callback(null);
			}
			mClient.LoadAchievements(delegate(global::GooglePlayGames.BasicApi.Achievement[] ach)
			{
				global::UnityEngine.SocialPlatforms.IAchievement[] array = new global::UnityEngine.SocialPlatforms.IAchievement[ach.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new global::GooglePlayGames.PlayGamesAchievement(ach[i]);
				}
				callback(array);
			});
		}

		public global::UnityEngine.SocialPlatforms.IAchievement CreateAchievement()
		{
			return new global::GooglePlayGames.PlayGamesAchievement();
		}

		public void ReportScore(long score, string board, global::System.Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				global::GooglePlayGames.OurUtils.Logger.d("ReportScore: score=" + score + ", board=" + board);
				string leaderboardId = MapId(board);
				mClient.SubmitScore(leaderboardId, score, callback);
			}
		}

		public void ReportScore(long score, string board, string metadata, global::System.Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			global::GooglePlayGames.OurUtils.Logger.d("ReportScore: score=" + score + ", board=" + board + " metadata=" + metadata);
			string leaderboardId = MapId(board);
			mClient.SubmitScore(leaderboardId, score, metadata, callback);
		}

		public void LoadScores(string leaderboardId, global::System.Action<global::UnityEngine.SocialPlatforms.IScore[]> callback)
		{
			LoadScores(leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardStart.PlayerCentered, mClient.LeaderboardMaxResults(), global::GooglePlayGames.BasicApi.LeaderboardCollection.Public, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan.AllTime, delegate(global::GooglePlayGames.BasicApi.LeaderboardScoreData scoreData)
			{
				callback(scoreData.Scores);
			});
		}

		public void LoadScores(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardStart start, int rowCount, global::GooglePlayGames.BasicApi.LeaderboardCollection collection, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan timeSpan, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				callback(new global::GooglePlayGames.BasicApi.LeaderboardScoreData(leaderboardId, global::GooglePlayGames.BasicApi.ResponseStatus.NotAuthorized));
			}
			else
			{
				mClient.LoadScores(leaderboardId, start, rowCount, collection, timeSpan, callback);
			}
		}

		public void LoadMoreScores(global::GooglePlayGames.BasicApi.ScorePageToken token, int rowCount, global::System.Action<global::GooglePlayGames.BasicApi.LeaderboardScoreData> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("LoadMoreScores can only be called after authentication.");
				callback(new global::GooglePlayGames.BasicApi.LeaderboardScoreData(token.LeaderboardId, global::GooglePlayGames.BasicApi.ResponseStatus.NotAuthorized));
			}
			else
			{
				mClient.LoadMoreScores(token, rowCount, callback);
			}
		}

		public global::UnityEngine.SocialPlatforms.ILeaderboard CreateLeaderboard()
		{
			return new global::GooglePlayGames.PlayGamesLeaderboard(mDefaultLbUi);
		}

		public void ShowAchievementsUI()
		{
			ShowAchievementsUI(null);
		}

		public void ShowAchievementsUI(global::System.Action<global::GooglePlayGames.BasicApi.UIStatus> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("ShowAchievementsUI can only be called after authentication.");
				return;
			}
			global::GooglePlayGames.OurUtils.Logger.d("ShowAchievementsUI callback is " + callback);
			mClient.ShowAchievementsUI(callback);
		}

		public void ShowLeaderboardUI()
		{
			global::GooglePlayGames.OurUtils.Logger.d("ShowLeaderboardUI with default ID");
			ShowLeaderboardUI(MapId(mDefaultLbUi), null);
		}

		public void ShowLeaderboardUI(string leaderboardId)
		{
			if (leaderboardId != null)
			{
				leaderboardId = MapId(leaderboardId);
			}
			mClient.ShowLeaderboardUI(leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan.AllTime, null);
		}

		public void ShowLeaderboardUI(string leaderboardId, global::System.Action<global::GooglePlayGames.BasicApi.UIStatus> callback)
		{
			ShowLeaderboardUI(leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan.AllTime, callback);
		}

		public void ShowLeaderboardUI(string leaderboardId, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan span, global::System.Action<global::GooglePlayGames.BasicApi.UIStatus> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("ShowLeaderboardUI can only be called after authentication.");
				if (callback != null)
				{
					callback(global::GooglePlayGames.BasicApi.UIStatus.NotAuthorized);
				}
			}
			else
			{
				global::GooglePlayGames.OurUtils.Logger.d("ShowLeaderboardUI, lbId=" + leaderboardId + " callback is " + callback);
				mClient.ShowLeaderboardUI(leaderboardId, span, callback);
			}
		}

		public void SetDefaultLeaderboardForUI(string lbid)
		{
			global::GooglePlayGames.OurUtils.Logger.d("SetDefaultLeaderboardForUI: " + lbid);
			if (lbid != null)
			{
				lbid = MapId(lbid);
			}
			mDefaultLbUi = lbid;
		}

		public void LoadFriends(global::UnityEngine.SocialPlatforms.ILocalUser user, global::System.Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			mClient.LoadFriends(callback);
		}

		public void LoadScores(global::UnityEngine.SocialPlatforms.ILeaderboard board, global::System.Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			global::GooglePlayGames.BasicApi.LeaderboardTimeSpan timeSpan;
			switch (board.timeScope)
			{
			case global::UnityEngine.SocialPlatforms.TimeScope.AllTime:
				timeSpan = global::GooglePlayGames.BasicApi.LeaderboardTimeSpan.AllTime;
				break;
			case global::UnityEngine.SocialPlatforms.TimeScope.Week:
				timeSpan = global::GooglePlayGames.BasicApi.LeaderboardTimeSpan.Weekly;
				break;
			case global::UnityEngine.SocialPlatforms.TimeScope.Today:
				timeSpan = global::GooglePlayGames.BasicApi.LeaderboardTimeSpan.Daily;
				break;
			default:
				timeSpan = global::GooglePlayGames.BasicApi.LeaderboardTimeSpan.AllTime;
				break;
			}
			((global::GooglePlayGames.PlayGamesLeaderboard)board).loading = true;
			global::GooglePlayGames.OurUtils.Logger.d(string.Concat("LoadScores, board=", board, " callback is ", callback));
			mClient.LoadScores(board.id, global::GooglePlayGames.BasicApi.LeaderboardStart.PlayerCentered, (board.range.count <= 0) ? mClient.LeaderboardMaxResults() : board.range.count, (board.userScope != global::UnityEngine.SocialPlatforms.UserScope.FriendsOnly) ? global::GooglePlayGames.BasicApi.LeaderboardCollection.Public : global::GooglePlayGames.BasicApi.LeaderboardCollection.Social, timeSpan, delegate(global::GooglePlayGames.BasicApi.LeaderboardScoreData scoreData)
			{
				HandleLoadingScores((global::GooglePlayGames.PlayGamesLeaderboard)board, scoreData, callback);
			});
		}

		public bool GetLoading(global::UnityEngine.SocialPlatforms.ILeaderboard board)
		{
			return board != null && board.loading;
		}

		public void RegisterInvitationDelegate(global::GooglePlayGames.BasicApi.InvitationReceivedDelegate deleg)
		{
			mClient.RegisterInvitationDelegate(deleg);
		}

		public string GetToken()
		{
			return mClient.GetToken();
		}

		internal void HandleLoadingScores(global::GooglePlayGames.PlayGamesLeaderboard board, global::GooglePlayGames.BasicApi.LeaderboardScoreData scoreData, global::System.Action<bool> callback)
		{
			bool flag = board.SetFromData(scoreData);
			if (flag && !board.HasAllScores() && scoreData.NextPageToken != null)
			{
				int rowCount = board.range.count - board.ScoreCount;
				mClient.LoadMoreScores(scoreData.NextPageToken, rowCount, delegate(global::GooglePlayGames.BasicApi.LeaderboardScoreData nextScoreData)
				{
					HandleLoadingScores(board, nextScoreData, callback);
				});
			}
			else
			{
				callback(flag);
			}
		}

		internal global::UnityEngine.SocialPlatforms.IUserProfile[] GetFriends()
		{
			if (!IsAuthenticated())
			{
				global::GooglePlayGames.OurUtils.Logger.d("Cannot get friends when not authenticated!");
				return new global::UnityEngine.SocialPlatforms.IUserProfile[0];
			}
			return mClient.GetFriends();
		}

		private string MapId(string id)
		{
			if (id == null)
			{
				return null;
			}
			if (mIdMap.ContainsKey(id))
			{
				string text = mIdMap[id];
				global::GooglePlayGames.OurUtils.Logger.d("Mapping alias " + id + " to ID " + text);
				return text;
			}
			return id;
		}
	}
}
