namespace GooglePlayGames.BasicApi
{
	public class LeaderboardScoreData
	{
		private string mId;

		private global::GooglePlayGames.BasicApi.ResponseStatus mStatus;

		private ulong mApproxCount;

		private string mTitle;

		private global::UnityEngine.SocialPlatforms.IScore mPlayerScore;

		private global::GooglePlayGames.BasicApi.ScorePageToken mPrevPage;

		private global::GooglePlayGames.BasicApi.ScorePageToken mNextPage;

		private global::System.Collections.Generic.List<global::GooglePlayGames.PlayGamesScore> mScores = new global::System.Collections.Generic.List<global::GooglePlayGames.PlayGamesScore>();

		public bool Valid
		{
			get
			{
				return mStatus == global::GooglePlayGames.BasicApi.ResponseStatus.Success || mStatus == global::GooglePlayGames.BasicApi.ResponseStatus.SuccessWithStale;
			}
		}

		public global::GooglePlayGames.BasicApi.ResponseStatus Status
		{
			get
			{
				return mStatus;
			}
			internal set
			{
				mStatus = value;
			}
		}

		public ulong ApproximateCount
		{
			get
			{
				return mApproxCount;
			}
			internal set
			{
				mApproxCount = value;
			}
		}

		public string Title
		{
			get
			{
				return mTitle;
			}
			internal set
			{
				mTitle = value;
			}
		}

		public string Id
		{
			get
			{
				return mId;
			}
			internal set
			{
				mId = value;
			}
		}

		public global::UnityEngine.SocialPlatforms.IScore PlayerScore
		{
			get
			{
				return mPlayerScore;
			}
			internal set
			{
				mPlayerScore = value;
			}
		}

		public global::UnityEngine.SocialPlatforms.IScore[] Scores
		{
			get
			{
				return mScores.ToArray();
			}
		}

		public global::GooglePlayGames.BasicApi.ScorePageToken PrevPageToken
		{
			get
			{
				return mPrevPage;
			}
			internal set
			{
				mPrevPage = value;
			}
		}

		public global::GooglePlayGames.BasicApi.ScorePageToken NextPageToken
		{
			get
			{
				return mNextPage;
			}
			internal set
			{
				mNextPage = value;
			}
		}

		internal LeaderboardScoreData(string leaderboardId)
		{
			mId = leaderboardId;
		}

		internal LeaderboardScoreData(string leaderboardId, global::GooglePlayGames.BasicApi.ResponseStatus status)
		{
			mId = leaderboardId;
			mStatus = status;
		}

		internal int AddScore(global::GooglePlayGames.PlayGamesScore score)
		{
			mScores.Add(score);
			return mScores.Count;
		}

		public override string ToString()
		{
			return string.Format("[LeaderboardScoreData: mId={0},  mStatus={1}, mApproxCount={2}, mTitle={3}]", mId, mStatus, mApproxCount, mTitle);
		}
	}
}
