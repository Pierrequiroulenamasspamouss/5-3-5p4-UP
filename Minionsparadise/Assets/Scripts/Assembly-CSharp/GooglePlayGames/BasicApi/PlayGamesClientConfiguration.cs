namespace GooglePlayGames.BasicApi
{
	public struct PlayGamesClientConfiguration
	{
		public class Builder
		{
			private bool mEnableSaveGames;

			private bool mRequireGooglePlus;

			private global::GooglePlayGames.BasicApi.InvitationReceivedDelegate mInvitationDelegate = delegate
			{
			};

			private global::GooglePlayGames.BasicApi.Multiplayer.MatchDelegate mMatchDelegate = delegate
			{
			};

			private string mRationale;

			public global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder EnableSavedGames()
			{
				mEnableSaveGames = true;
				return this;
			}

			public global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder RequireGooglePlus()
			{
				mRequireGooglePlus = true;
				return this;
			}

			public global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder WithInvitationDelegate(global::GooglePlayGames.BasicApi.InvitationReceivedDelegate invitationDelegate)
			{
				mInvitationDelegate = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(invitationDelegate);
				return this;
			}

			public global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder WithMatchDelegate(global::GooglePlayGames.BasicApi.Multiplayer.MatchDelegate matchDelegate)
			{
				mMatchDelegate = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(matchDelegate);
				return this;
			}

			public global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder WithPermissionRationale(string rationale)
			{
				mRationale = rationale;
				return this;
			}

			public global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration Build()
			{
				mRequireGooglePlus = global::GooglePlayGames.GameInfo.RequireGooglePlus();
				return new global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration(this);
			}

			internal bool HasEnableSaveGames()
			{
				return mEnableSaveGames;
			}

			internal bool HasRequireGooglePlus()
			{
				return mRequireGooglePlus;
			}

			internal global::GooglePlayGames.BasicApi.Multiplayer.MatchDelegate GetMatchDelegate()
			{
				return mMatchDelegate;
			}

			internal global::GooglePlayGames.BasicApi.InvitationReceivedDelegate GetInvitationDelegate()
			{
				return mInvitationDelegate;
			}

			internal string GetPermissionRationale()
			{
				return mRationale;
			}
		}

		public static readonly global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration DefaultConfiguration = new global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder().WithPermissionRationale("Select email address to send to this game or hit cancel to not share.").Build();

		private readonly bool mEnableSavedGames;

		private readonly bool mRequireGooglePlus;

		private readonly global::GooglePlayGames.BasicApi.InvitationReceivedDelegate mInvitationDelegate;

		private readonly global::GooglePlayGames.BasicApi.Multiplayer.MatchDelegate mMatchDelegate;

		private readonly string mPermissionRationale;

		public bool EnableSavedGames
		{
			get
			{
				return mEnableSavedGames;
			}
		}

		public bool RequireGooglePlus
		{
			get
			{
				return mRequireGooglePlus;
			}
		}

		public global::GooglePlayGames.BasicApi.InvitationReceivedDelegate InvitationDelegate
		{
			get
			{
				return mInvitationDelegate;
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.MatchDelegate MatchDelegate
		{
			get
			{
				return mMatchDelegate;
			}
		}

		public string PermissionRationale
		{
			get
			{
				return mPermissionRationale;
			}
		}

		private PlayGamesClientConfiguration(global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder builder)
		{
			mEnableSavedGames = builder.HasEnableSaveGames();
			mInvitationDelegate = builder.GetInvitationDelegate();
			mMatchDelegate = builder.GetMatchDelegate();
			mPermissionRationale = builder.GetPermissionRationale();
			mRequireGooglePlus = builder.HasRequireGooglePlus();
		}
	}
}
