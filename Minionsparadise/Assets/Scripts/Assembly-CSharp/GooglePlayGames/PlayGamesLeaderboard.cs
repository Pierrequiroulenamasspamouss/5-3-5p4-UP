namespace GooglePlayGames
{
	public class PlayGamesLeaderboard : global::UnityEngine.SocialPlatforms.ILeaderboard
	{
		private string mId;

		private global::UnityEngine.SocialPlatforms.UserScope mUserScope;

		private global::UnityEngine.SocialPlatforms.Range mRange;

		private global::UnityEngine.SocialPlatforms.TimeScope mTimeScope;

		private string[] mFilteredUserIds;

		private bool mLoading;

		private global::UnityEngine.SocialPlatforms.IScore mLocalUserScore;

		private uint mMaxRange;

		private global::System.Collections.Generic.List<global::GooglePlayGames.PlayGamesScore> mScoreList = new global::System.Collections.Generic.List<global::GooglePlayGames.PlayGamesScore>();

		private string mTitle;

		public bool loading
		{
			get
			{
				return mLoading;
			}
			internal set
			{
				mLoading = value;
			}
		}

		public string id
		{
			get
			{
				return mId;
			}
			set
			{
				mId = value;
			}
		}

		public global::UnityEngine.SocialPlatforms.UserScope userScope
		{
			get
			{
				return mUserScope;
			}
			set
			{
				mUserScope = value;
			}
		}

		public global::UnityEngine.SocialPlatforms.Range range
		{
			get
			{
				return mRange;
			}
			set
			{
				mRange = value;
			}
		}

		public global::UnityEngine.SocialPlatforms.TimeScope timeScope
		{
			get
			{
				return mTimeScope;
			}
			set
			{
				mTimeScope = value;
			}
		}

		public global::UnityEngine.SocialPlatforms.IScore localUserScore
		{
			get
			{
				return mLocalUserScore;
			}
		}

		public uint maxRange
		{
			get
			{
				return mMaxRange;
			}
		}

		public global::UnityEngine.SocialPlatforms.IScore[] scores
		{
			get
			{
				global::GooglePlayGames.PlayGamesScore[] array = new global::GooglePlayGames.PlayGamesScore[mScoreList.Count];
				mScoreList.CopyTo(array);
				return array;
			}
		}

		public string title
		{
			get
			{
				return mTitle;
			}
		}

		public int ScoreCount
		{
			get
			{
				return mScoreList.Count;
			}
		}

		public PlayGamesLeaderboard(string id)
		{
			mId = id;
		}

		public void SetUserFilter(string[] userIDs)
		{
			mFilteredUserIds = userIDs;
		}

		public void LoadScores(global::System.Action<bool> callback)
		{
			global::GooglePlayGames.PlayGamesPlatform.Instance.LoadScores(this, callback);
		}

		internal bool SetFromData(global::GooglePlayGames.BasicApi.LeaderboardScoreData data)
		{
			if (data.Valid)
			{
				global::UnityEngine.Debug.Log("Setting leaderboard from: " + data);
				SetMaxRange(data.ApproximateCount);
				SetTitle(data.Title);
				SetLocalUserScore((global::GooglePlayGames.PlayGamesScore)data.PlayerScore);
				global::UnityEngine.SocialPlatforms.IScore[] array = data.Scores;
				foreach (global::UnityEngine.SocialPlatforms.IScore score in array)
				{
					AddScore((global::GooglePlayGames.PlayGamesScore)score);
				}
				mLoading = data.Scores.Length == 0 || HasAllScores();
			}
			return data.Valid;
		}

		internal void SetMaxRange(ulong val)
		{
			mMaxRange = (uint)val;
		}

		internal void SetTitle(string value)
		{
			mTitle = value;
		}

		internal void SetLocalUserScore(global::GooglePlayGames.PlayGamesScore score)
		{
			mLocalUserScore = score;
		}

		internal int AddScore(global::GooglePlayGames.PlayGamesScore score)
		{
			if (mFilteredUserIds == null || mFilteredUserIds.Length == 0)
			{
				mScoreList.Add(score);
			}
			else
			{
				string[] array = mFilteredUserIds;
				foreach (string text in array)
				{
					if (text.Equals(score.userID))
					{
						return mScoreList.Count;
					}
				}
				mScoreList.Add(score);
			}
			return mScoreList.Count;
		}

		internal bool HasAllScores()
		{
			return mScoreList.Count >= mRange.count || mScoreList.Count >= maxRange;
		}
	}
}
