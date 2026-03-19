namespace Kampai.Game
{
	public class FacebookService : global::Kampai.Game.ISocialService
	{
		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> _initSuccessSignal;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> _initFailSignal;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> _loginSuccessSignal;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> _loginFailureSignal;

		private global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> _inviteSignalSuccess;

		private global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> _inviteSignalFailure;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> _getFriendsSignalSuccess;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> _getFriendsSignalFailure;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("FacebookService") as global::Kampai.Util.IKampaiLogger;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> success = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService>();

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> failure = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService>();

		private bool killSwitchFlag;

		private bool _useServerLogin = false;
		private string _serverAccessToken = string.Empty;
		private string _serverUserID = string.Empty;

		[Inject]
		public ILocalPersistanceService localPersistence { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		public global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.FBUser> friends { get; set; }

		public global::System.Collections.Generic.Dictionary<string, global::UnityEngine.Texture> userPictures { get; set; }

		public string LoginSource { get; set; }

		public bool isLoggedIn
		{
			get
			{
				if (_useServerLogin) return !string.IsNullOrEmpty(_serverAccessToken);
				return global::Discord.Unity.FB.IsLoggedIn;
			}
		}

		public bool isKillSwitchEnabled
		{
			get
			{
				return killSwitchFlag;
			}
		}

		public string userID
		{
			get
			{
				if (_useServerLogin) return _serverUserID;
				return (!isLoggedIn) ? string.Empty : global::Discord.Unity.AccessToken.CurrentAccessToken.UserId;
			}
		}

		public string userName
		{
			get
			{
				return string.Empty;
			}
		}

		public global::Kampai.Game.SocialServices type
		{
			get
			{
				return global::Kampai.Game.SocialServices.FACEBOOK;
			}
		}

		public string accessToken
		{
			get
			{
				if (_useServerLogin) return _serverAccessToken;
				return (!isLoggedIn) ? string.Empty : global::Discord.Unity.AccessToken.CurrentAccessToken.TokenString;
			}
		}

		public global::System.DateTime tokenExpiry
		{
			get
			{
				return (!isLoggedIn) ? global::System.DateTime.MinValue : global::Discord.Unity.AccessToken.CurrentAccessToken.ExpirationTime;
			}
		}

		public string locKey
		{
			get
			{
				return "AccountTypeFacebook";
			}
		}

		public void Login(global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> successSignal, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> failureSignal, global::System.Action callback)
		{
			logger.Debug("Discord: Login Source = {0}", LoginSource ?? "N/A");
			if (!isKillSwitchEnabled)
			{
				if (localPersistence.GetData("SocialInProgress") != "True")
				{
					localPersistence.PutData("SocialInProgress", "True");
					_loginSuccessSignal = successSignal;
					_loginFailureSignal = failureSignal;
					if (_useServerLogin)
					{
						routineRunner.StartCoroutine(LogInViaServer());
					}
					else
					{
						routineRunner.StartCoroutine(LogInWithReadPermissions("public_profile", "user_friends"));
					}
				}
				else
				{
					logger.Warning("Discord: Ignoring login attempt as another one is already in progress.");
				}
			}
			else
			{
				failureSignal.Dispatch(this);
			}
		}

		private global::System.Collections.IEnumerator LogInWithReadPermissions(params string[] permissions)
		{
			yield return null;
			global::Discord.Unity.FB.LogInWithReadPermissions(permissions, AuthCallback);
		}

		private global::System.Collections.IEnumerator LogInViaServer()
		{
			string uid = localPersistence.GetData("UserID");
			if (string.IsNullOrEmpty(uid)) { uid = "1000000000"; }
			global::UnityEngine.Application.OpenURL(global::Kampai.Util.GameConstants.Server.CDN_METADATA_URL + "/auth/discord/login?uid=" + uid);
			logger.Info("Discord: Opened Server Discord Login for {0}. Polling for token...", uid);
			bool solved = false;
			while (!solved)
			{
				yield return new global::UnityEngine.WaitForSeconds(2f);
				global::UnityEngine.WWW www = new global::UnityEngine.WWW(global::Kampai.Util.GameConstants.Server.CDN_METADATA_URL + "/auth/discord/status?uid=" + uid);
				yield return www;
				if (string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text))
				{
					global::System.Collections.Generic.Dictionary<string, object> result = global::Discord.MiniJSON.Json.Deserialize(www.text) as global::System.Collections.Generic.Dictionary<string, object>;
					if (result != null && result.ContainsKey("status") && result["status"].ToString() == "success")
					{
						_serverAccessToken = result["token"].ToString();
						_serverUserID = result["uid"].ToString();
						solved = true;
					}
				}
			}
			localPersistence.PutData("SocialInProgress", "False");
			localPersistence.PutData("Discord_AccessToken", _serverAccessToken);
			localPersistence.PutData("Discord_UserID", _serverUserID);
			_loginSuccessSignal.Dispatch(this);
		}

		public void Init(global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> successSignal, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> failureSignal)
		{
			logger.Debug("Discord: Init Called");
			updateKillSwitchFlag();
			_initSuccessSignal = successSignal;
			_initFailSignal = failureSignal;
			friends = new global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.FBUser>();
			userPictures = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.Texture>();
			try
			{
				if (!global::Discord.Unity.FB.IsInitialized)
				{
					global::Discord.Unity.FB.Init(global::Kampai.Util.GameConstants.Discord.APP_ID, true, true, true, false, true, null, "en_US", null, SetInit);
				}
				else
				{
					SetInit();
				}
			}
			catch (global::System.NotImplementedException)
			{
				logger.Warning("Discord SDK implies NotImplementedException. Falling back to Server Login.");
				_useServerLogin = true;
				_initSuccessSignal.Dispatch(this);
			}

			_serverAccessToken = localPersistence.GetData("Discord_AccessToken");
			_serverUserID = localPersistence.GetData("Discord_UserID");
			if (!string.IsNullOrEmpty(_serverAccessToken))
			{
				_useServerLogin = true;
				logger.Info("Discord: Restored Server Login for {0}", _serverUserID);
			}

			localPersistence.PutData("SocialInProgress", "False");
		}

		private void downloadFriendsSuccess(global::Kampai.Game.ISocialService service)
		{
			success.RemoveListener(downloadFriendsSuccess);
			failure.RemoveListener(downloadFriendsFailure);
		}

		private void downloadFriendsFailure(global::Kampai.Game.ISocialService service)
		{
			success.RemoveListener(downloadFriendsSuccess);
			failure.RemoveListener(downloadFriendsFailure);
		}

		public void FriendInvite(string message, string title, string data, int maxRecipients, global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> successSignal, global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> failureSignal)
		{
			SendRequest(message, title, data, null, maxRecipients, successSignal, failureSignal);
		}

		public void SendRequest(string message, string title, string data, global::System.Collections.Generic.IList<string> ids, global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> successSignal, global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> failureSignal)
		{
			SendRequest(message, title, data, ids, 100, successSignal, failureSignal);
		}

		public void SendRequestToAll(string message, string title, string data, global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> successSignal, global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> failureSignal)
		{
			SendRequest(message, title, data, (friends == null) ? null : friends.Keys, 100, successSignal, failureSignal);
		}

		public void SendRequest(string message, string title, string data, global::System.Collections.Generic.IEnumerable<string> ids, int maxRecipients, global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> successSignal, global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<string>> failureSignal)
		{
			logger.Debug("Discord: FriendInvite");
			if (!isKillSwitchEnabled && isLoggedIn)
			{
				_inviteSignalSuccess = successSignal;
				_inviteSignalFailure = failureSignal;
				global::Discord.Unity.FB.AppRequest(message, ids, null, null, maxRecipients, data, title, AppRequestCallback);
				return;
			}
			if (!isLoggedIn)
			{
				logger.Error("Discord: FriendInvite failed. Please log in first.");
			}
			if (failureSignal != null)
			{
				failureSignal.Dispatch(null);
			}
		}

		private void AppRequestCallback(global::Discord.Unity.IAppRequestResult result)
		{
			string text = ((result == null) ? null : result.Error);
			global::System.Collections.Generic.List<string> list = ((result == null || result.To == null) ? null : new global::System.Collections.Generic.List<string>(result.To));
			if (result == null || result.Cancelled || !string.IsNullOrEmpty(text))
			{
				if (result == null)
				{
					logger.Error("Discord: AppRequest with no result");
				}
				else
				{
					logger.Error("Discord: AppRequest failure = {0}", (!string.IsNullOrEmpty(text)) ? text : "Cancelled");
				}
				if (_inviteSignalFailure != null)
				{
					_inviteSignalFailure.Dispatch(list);
				}
				return;
			}
			logger.Debug("Discord: AppRequest result = {0}", result.RawResult);
			global::System.Collections.Generic.IDictionary<string, object> resultDictionary = result.ResultDictionary;
			if (resultDictionary == null || !resultDictionary.ContainsKey("request") || list == null || list.Count == 0)
			{
				logger.Error("Discord: AppRequest cancelled due to bad response.");
				if (_inviteSignalFailure != null)
				{
					_inviteSignalFailure.Dispatch(list);
				}
				return;
			}
			logger.Debug("Discord: AppRequest succeeded for = {0}", string.Join(",", list.ToArray()));
			if (_inviteSignalSuccess != null)
			{
				_inviteSignalSuccess.Dispatch(list);
			}
		}

		public void GetUserInfo()
		{
			global::Discord.Unity.FB.API("/me?fields=id,first_name", global::Discord.Unity.HttpMethod.GET, GetUserInfoCallback);
		}

		private void GetUserInfoCallback(global::Discord.Unity.IGraphResult result)
		{
			string text = ((result == null) ? null : result.Error);
			if (result == null || result.Cancelled || !string.IsNullOrEmpty(text))
			{
				if (result == null)
				{
					logger.Error("Discord: GetUserInfo with no result");
					return;
				}
				logger.Error("Discord: GetUserInfo failure = {0}", (!string.IsNullOrEmpty(text)) ? text : "Cancelled");
			}
			else
			{
				logger.Debug("Discord: GetUserInfo result = {0}", result.RawResult);
			}
		}

		public void DownloadFriends(int friendLimit, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> success, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.ISocialService> failure)
		{
			logger.Debug("Discord: DownloadFriends");
			if (!isKillSwitchEnabled)
			{
				_getFriendsSignalFailure = failure;
				_getFriendsSignalSuccess = success;
				global::Discord.Unity.FB.API(string.Format("me/friends?fields=name,id&limit={0}&access_token={1}", friendLimit, global::Discord.Unity.AccessToken.CurrentAccessToken.TokenString), global::Discord.Unity.HttpMethod.GET, DownloadFriendsCallback);
			}
			else
			{
				failure.Dispatch(this);
			}
		}

		private void DownloadFriendsCallback(global::Discord.Unity.IGraphResult result)
		{
			string text = ((result == null) ? null : result.Error);
			if (result == null || result.Cancelled || !string.IsNullOrEmpty(text))
			{
				if (result == null)
				{
					logger.Error("Discord: DownloadFriends with no result");
				}
				else
				{
					logger.Error("Discord: DownloadFriends failure = {0}", (!string.IsNullOrEmpty(text)) ? text : "Cancelled");
				}
				if (_getFriendsSignalFailure != null)
				{
					_getFriendsSignalFailure.Dispatch(this);
				}
				return;
			}
			logger.Debug("Discord: DownloadFriends result = {0}", result.RawResult);
			global::System.Collections.Generic.IDictionary<string, object> resultDictionary = result.ResultDictionary;
			global::System.Collections.Generic.List<object> list = ((resultDictionary == null || !resultDictionary.ContainsKey("data")) ? null : (resultDictionary["data"] as global::System.Collections.Generic.List<object>));
			if (list == null)
			{
				logger.Error("Discord: DownloadFriends result doesn't have any valid data.");
				if (_getFriendsSignalFailure != null)
				{
					_getFriendsSignalFailure.Dispatch(this);
				}
				return;
			}
			foreach (global::System.Collections.Generic.Dictionary<string, object> item in list)
			{
				string text2 = item["id"] as string;
				if (!friends.ContainsKey(text2))
				{
					friends.Add(text2, new global::Kampai.Game.FBUser(item["name"] as string, text2));
				}
			}
			if (_getFriendsSignalSuccess != null)
			{
				_getFriendsSignalSuccess.Dispatch(this);
			}
		}

		public global::Kampai.Game.FBUser GetFriend(string fbid)
		{
			if (friends != null && friends.ContainsKey(fbid))
			{
				return friends[fbid];
			}
			return null;
		}

		public global::System.Collections.IEnumerator DownloadUserPicture(string id, global::strange.extensions.signal.impl.Signal<string> callback = null)
		{
			string url = string.Format("https://graph.discord.com/{0}/picture?width=256&height=256", id);
			logger.Info("Discord: Download user picture URL: {0}", url);
			global::UnityEngine.WWW www = new global::UnityEngine.WWW(url);
			yield return www;
			if (!string.IsNullOrEmpty(www.error) || www.texture == null)
			{
				logger.Warning("Discord: Download picture failed with error {0}", www.error);
			}
			else
			{
				global::UnityEngine.Texture texture = www.texture;
				if (texture.width > 8 && texture.height > 8)
				{
					userPictures[id] = texture;
					if (friends.ContainsKey(id))
					{
						friends[id].SetTexture(texture, global::UnityEngine.Vector2.zero);
					}
				}
			}
			if (callback != null)
			{
				callback.Dispatch(id);
			}
		}

		public global::UnityEngine.Texture GetUserPicture(string id)
		{
			return (userPictures == null || !userPictures.ContainsKey(id)) ? null : userPictures[id];
		}

		public void Logout()
		{
			logger.Debug("Discord: Logout");
			global::Discord.Unity.FB.LogOut();
			_serverAccessToken = string.Empty;
			_serverUserID = string.Empty;
			localPersistence.PutData("Discord_AccessToken", "");
			localPersistence.PutData("Discord_UserID", "");
			friends.Clear();
		}

		private void SetInit()
		{
			logger.Debug("Discord: Set Init Called");
			if (global::Discord.Unity.FB.IsInitialized)
			{
				global::Discord.Unity.FB.ActivateApp();
				_initSuccessSignal.Dispatch(this);
				logger.Info("Is Logged In Discord: {0}", isLoggedIn.ToString());
				if (isLoggedIn)
				{
					global::Discord.Unity.AccessToken currentAccessToken = global::Discord.Unity.AccessToken.CurrentAccessToken;
					logger.Info("Discord UserID: {0}", currentAccessToken.UserId);
					logger.Info("Access Token: {0}", currentAccessToken.TokenString);
					logger.Info("Access Expiry: {0} ", currentAccessToken.ExpirationTime.ToString());
					success.AddListener(downloadFriendsSuccess);
					failure.AddListener(downloadFriendsFailure);
					DownloadFriends(100, success, failure);
				}
			}
			else
			{
				_initFailSignal.Dispatch(this);
			}
		}

		private void AuthCallback(global::Discord.Unity.ILoginResult result)
		{
			localPersistence.PutData("SocialInProgress", "False");
			logger.Debug("Discord: Auth result = {0}", result.RawResult);
			if (isLoggedIn)
			{
				global::Discord.Unity.AccessToken currentAccessToken = global::Discord.Unity.AccessToken.CurrentAccessToken;
				logger.Debug("********** FB **********");
				logger.Debug(currentAccessToken.UserId);
				logger.Debug(currentAccessToken.TokenString);
				_loginSuccessSignal.Dispatch(this);
				GetUserInfo();
				success.AddListener(downloadFriendsSuccess);
				failure.AddListener(downloadFriendsFailure);
				DownloadFriends(100, success, failure);
			}
			else
			{
				string text = ((result == null) ? null : result.Error);
				logger.Error("Discord: Auth failure = {0}", (!string.IsNullOrEmpty(text) || !result.Cancelled) ? text : "Cancelled");
				global::Discord.Unity.FB.Init(global::Kampai.Util.GameConstants.Discord.APP_ID, true, true, true, false, true, null, "en_US", null, SetInit);
				localPersistence.PutData("SocialInProgress", "False");
				_loginFailureSignal.Dispatch(this);
			}
		}

		public void updateKillSwitchFlag()
		{
			killSwitchFlag = configurationsService.isKillSwitchOn(global::Kampai.Game.KillSwitch.FACEBOOK);
		}

		public void SendLoginTelemetry(string loginLocation)
		{
			telemetryService.Send_Telemetry_EVT_EBISU_LOGIN_FACEBOOK(loginLocation, LoginSource);
		}

		public void incrementAchievement(string achievementID, float percentComplete)
		{
		}

		public void ShowAchievements()
		{
		}
	}
}
