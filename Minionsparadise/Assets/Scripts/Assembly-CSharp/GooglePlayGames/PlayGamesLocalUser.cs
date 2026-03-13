namespace GooglePlayGames
{
	public class PlayGamesLocalUser : global::GooglePlayGames.PlayGamesUserProfile, global::UnityEngine.SocialPlatforms.IUserProfile, global::UnityEngine.SocialPlatforms.ILocalUser
	{
		internal global::GooglePlayGames.PlayGamesPlatform mPlatform;

		private string emailAddress;

		private global::GooglePlayGames.BasicApi.PlayerStats mStats;

		public global::UnityEngine.SocialPlatforms.IUserProfile[] friends
		{
			get
			{
				return mPlatform.GetFriends();
			}
		}

		public bool authenticated
		{
			get
			{
				return mPlatform.IsAuthenticated();
			}
		}

		public bool underage
		{
			get
			{
				return true;
			}
		}

		public new string userName
		{
			get
			{
				string text = string.Empty;
				if (authenticated)
				{
					text = mPlatform.GetUserDisplayName();
					if (!base.userName.Equals(text))
					{
						ResetIdentity(text, mPlatform.GetUserId(), mPlatform.GetUserImageUrl());
					}
				}
				return text;
			}
		}

		public new string id
		{
			get
			{
				string text = string.Empty;
				if (authenticated)
				{
					text = mPlatform.GetUserId();
					if (!base.id.Equals(text))
					{
						ResetIdentity(mPlatform.GetUserDisplayName(), text, mPlatform.GetUserImageUrl());
					}
				}
				return text;
			}
		}

		[global::System.Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public string accessToken
		{
			get
			{
				return (!authenticated) ? string.Empty : mPlatform.GetAccessToken();
			}
		}

		public new bool isFriend
		{
			get
			{
				return true;
			}
		}

		public new global::UnityEngine.SocialPlatforms.UserState state
		{
			get
			{
				return global::UnityEngine.SocialPlatforms.UserState.Online;
			}
		}

		public new string AvatarURL
		{
			get
			{
				string text = string.Empty;
				if (authenticated)
				{
					text = mPlatform.GetUserImageUrl();
					if (!base.id.Equals(text))
					{
						ResetIdentity(mPlatform.GetUserDisplayName(), mPlatform.GetUserId(), text);
					}
				}
				return text;
			}
		}

		public string Email
		{
			get
			{
				if (authenticated && string.IsNullOrEmpty(emailAddress))
				{
					emailAddress = mPlatform.GetUserEmail();
					emailAddress = emailAddress ?? string.Empty;
				}
				return (!authenticated) ? string.Empty : emailAddress;
			}
		}

		internal PlayGamesLocalUser(global::GooglePlayGames.PlayGamesPlatform plaf)
			: base("localUser", string.Empty, string.Empty)
		{
			mPlatform = plaf;
			emailAddress = null;
			mStats = null;
		}

		public void Authenticate(global::System.Action<bool> callback)
		{
			mPlatform.Authenticate(callback);
		}

		public void Authenticate(global::System.Action<bool> callback, bool silent)
		{
			mPlatform.Authenticate(callback, silent);
		}

		public void LoadFriends(global::System.Action<bool> callback)
		{
			mPlatform.LoadFriends(this, callback);
		}

		[global::System.Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public void GetIdToken(global::System.Action<string> idTokenCallback)
		{
			if (authenticated)
			{
				mPlatform.GetIdToken(idTokenCallback);
			}
			else
			{
				idTokenCallback(null);
			}
		}

		public void GetStats(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, global::GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			if (mStats == null || !mStats.Valid)
			{
				mPlatform.GetPlayerStats(delegate(global::GooglePlayGames.BasicApi.CommonStatusCodes rc, global::GooglePlayGames.BasicApi.PlayerStats stats)
				{
					mStats = stats;
					callback(rc, stats);
				});
			}
			else
			{
				callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.Success, mStats);
			}
		}
	}
}
