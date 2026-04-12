namespace Kampai.Game
{
	public class UnlinkAccountCommand : global::strange.extensions.command.impl.Command
	{
		private const string ACCOUNT_UNLINK_ENDPOINT = "rest/user/{0}/identity/unlink";

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UnlinkAccountCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject("game.server.host")]
		public string ServerUrl { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService FacebookService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.GAMECENTER)]
		public global::Kampai.Game.ISocialService GameCenterService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.GOOGLEPLAY)]
		public global::Kampai.Game.ISocialService GooglePlayService { get; set; }

		[Inject]
		public global::Kampai.Game.IdentityType UnlinkType { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
			string userID = userSession.UserID;
			global::Kampai.Game.UnlinkAccountRequest unlinkAccountRequest = new global::Kampai.Game.UnlinkAccountRequest();
			unlinkAccountRequest.identityType = UnlinkType.ToString().ToLower();
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal.AddListener(OnUnlinkAccountResponse);
			downloadService.Perform(requestFactory.Resource(ServerUrl + string.Format("rest/user/{0}/identity/unlink", userID)).WithHeaderParam("user_id", userSession.UserID).WithHeaderParam("session_key", userSession.SessionID)
				.WithContentType("application/json")
				.WithMethod("POST")
				.WithResponseSignal(signal)
				.WithEntity(unlinkAccountRequest));
		}

		private void OnUnlinkAccountResponse(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse httpResponse)
		{
			string text = UnlinkType.ToString();
			string empty = string.Empty;
			switch (httpResponse.Code)
			{
			case 200:
				logger.Debug(string.Format("Successfully unlinked user from identities with type {0}", text));
				LogOutSocialService(UnlinkType);
				RemoveSocialIdentities(UnlinkType);
				break;
			case 400:
				empty = "Invalid identity type";
				logger.Error(string.Format("{0}.  Failed to unlink user from identities with type {1}", empty, text));
				break;
			default:
				empty = "Unknown error";
				logger.Error(string.Format("{0}.  Failed to unlink user from identities with type {1}", empty, text));
				break;
			}
		}

		private void LogOutSocialService(global::Kampai.Game.IdentityType type)
		{
			global::System.Collections.Generic.Dictionary<global::Kampai.Game.IdentityType, global::Kampai.Game.ISocialService> dictionary = new global::System.Collections.Generic.Dictionary<global::Kampai.Game.IdentityType, global::Kampai.Game.ISocialService>();
			dictionary.Add(global::Kampai.Game.IdentityType.discord, FacebookService);
			dictionary.Add(global::Kampai.Game.IdentityType.gamecenter, GameCenterService);
			dictionary.Add(global::Kampai.Game.IdentityType.googleplay, GooglePlayService);
			global::System.Collections.Generic.Dictionary<global::Kampai.Game.IdentityType, global::Kampai.Game.ISocialService> dictionary2 = dictionary;
			global::Kampai.Game.ISocialService value;
			if (dictionary2.TryGetValue(type, out value) && value.isLoggedIn)
			{
				value.Logout();
			}
		}

		private void RemoveSocialIdentities(global::Kampai.Game.IdentityType type)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.UserIdentity> socialIdentities = userSessionService.UserSession.SocialIdentities;
			for (int num = socialIdentities.Count - 1; num >= 0; num--)
			{
				if (socialIdentities[num].Type == type)
				{
					socialIdentities.RemoveAt(num);
				}
			}
		}
	}
}
