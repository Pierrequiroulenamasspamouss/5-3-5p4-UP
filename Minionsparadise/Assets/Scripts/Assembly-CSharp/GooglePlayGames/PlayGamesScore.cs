namespace GooglePlayGames
{
	public class PlayGamesScore : global::UnityEngine.SocialPlatforms.IScore
	{
		private string mLbId;

		private long mValue;

		private ulong mRank;

		private string mPlayerId = string.Empty;

		private string mMetadata = string.Empty;

		private global::System.DateTime mDate = new global::System.DateTime(1970, 1, 1, 0, 0, 0);

		public string leaderboardID
		{
			get
			{
				return mLbId;
			}
			set
			{
				mLbId = value;
			}
		}

		public long value
		{
			get
			{
				return mValue;
			}
			set
			{
				mValue = value;
			}
		}

		public global::System.DateTime date
		{
			get
			{
				return mDate;
			}
		}

		public string formattedValue
		{
			get
			{
				return mValue.ToString();
			}
		}

		public string userID
		{
			get
			{
				return mPlayerId;
			}
		}

		public int rank
		{
			get
			{
				return (int)mRank;
			}
		}

		internal PlayGamesScore(global::System.DateTime date, string leaderboardId, ulong rank, string playerId, ulong value, string metadata)
		{
			mDate = date;
			mLbId = leaderboardID;
			mRank = rank;
			mPlayerId = playerId;
			mValue = (long)value;
			mMetadata = metadata;
		}

		public void ReportScore(global::System.Action<bool> callback)
		{
			global::GooglePlayGames.PlayGamesPlatform.Instance.ReportScore(mValue, mLbId, mMetadata, callback);
		}
	}
}
