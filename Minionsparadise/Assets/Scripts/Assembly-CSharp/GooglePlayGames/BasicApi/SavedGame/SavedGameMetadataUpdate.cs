namespace GooglePlayGames.BasicApi.SavedGame
{
	public struct SavedGameMetadataUpdate
	{
		public struct Builder
		{
			internal bool mDescriptionUpdated;

			internal string mNewDescription;

			internal bool mCoverImageUpdated;

			internal byte[] mNewPngCoverImage;

			internal global::System.TimeSpan? mNewPlayedTime;

			public global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate.Builder WithUpdatedDescription(string description)
			{
				mNewDescription = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(description);
				mDescriptionUpdated = true;
				return this;
			}

			public global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate.Builder WithUpdatedPngCoverImage(byte[] newPngCoverImage)
			{
				mCoverImageUpdated = true;
				mNewPngCoverImage = newPngCoverImage;
				return this;
			}

			public global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate.Builder WithUpdatedPlayedTime(global::System.TimeSpan newPlayedTime)
			{
				if (newPlayedTime.TotalMilliseconds > 1.8446744073709552E+19)
				{
					throw new global::System.InvalidOperationException("Timespans longer than ulong.MaxValue milliseconds are not allowed");
				}
				mNewPlayedTime = newPlayedTime;
				return this;
			}

			public global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate Build()
			{
				return new global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate(this);
			}
		}

		private readonly bool mDescriptionUpdated;

		private readonly string mNewDescription;

		private readonly bool mCoverImageUpdated;

		private readonly byte[] mNewPngCoverImage;

		private readonly global::System.TimeSpan? mNewPlayedTime;

		public bool IsDescriptionUpdated
		{
			get
			{
				return mDescriptionUpdated;
			}
		}

		public string UpdatedDescription
		{
			get
			{
				return mNewDescription;
			}
		}

		public bool IsCoverImageUpdated
		{
			get
			{
				return mCoverImageUpdated;
			}
		}

		public byte[] UpdatedPngCoverImage
		{
			get
			{
				return mNewPngCoverImage;
			}
		}

		public bool IsPlayedTimeUpdated
		{
			get
			{
				return mNewPlayedTime.HasValue;
			}
		}

		public global::System.TimeSpan? UpdatedPlayedTime
		{
			get
			{
				return mNewPlayedTime;
			}
		}

		private SavedGameMetadataUpdate(global::GooglePlayGames.BasicApi.SavedGame.SavedGameMetadataUpdate.Builder builder)
		{
			mDescriptionUpdated = builder.mDescriptionUpdated;
			mNewDescription = builder.mNewDescription;
			mCoverImageUpdated = builder.mCoverImageUpdated;
			mNewPngCoverImage = builder.mNewPngCoverImage;
			mNewPlayedTime = builder.mNewPlayedTime;
		}
	}
}
