namespace GooglePlayGames.BasicApi
{
	public class ScorePageToken
	{
		private string mId;

		private object mInternalObject;

		private global::GooglePlayGames.BasicApi.LeaderboardCollection mCollection;

		private global::GooglePlayGames.BasicApi.LeaderboardTimeSpan mTimespan;

		public global::GooglePlayGames.BasicApi.LeaderboardCollection Collection
		{
			get
			{
				return mCollection;
			}
		}

		public global::GooglePlayGames.BasicApi.LeaderboardTimeSpan TimeSpan
		{
			get
			{
				return mTimespan;
			}
		}

		public string LeaderboardId
		{
			get
			{
				return mId;
			}
		}

		internal object InternalObject
		{
			get
			{
				return mInternalObject;
			}
		}

		internal ScorePageToken(object internalObject, string id, global::GooglePlayGames.BasicApi.LeaderboardCollection collection, global::GooglePlayGames.BasicApi.LeaderboardTimeSpan timespan)
		{
			mInternalObject = internalObject;
			mId = id;
			mCollection = collection;
			mTimespan = timespan;
		}
	}
}
