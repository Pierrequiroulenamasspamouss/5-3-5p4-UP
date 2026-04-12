namespace Kampai.Game
{
	public class LinkAccountCommand : global::strange.extensions.command.impl.Command
	{
		public const string ACCOUNT_LINK_ENDPOINT = "/rest/v2/user/{0}/identity";

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LinkAccountCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public bool restartOnSuccess { get; set; }

		[Inject]
		public global::Kampai.Game.ISocialService socialService { get; set; }

		[Inject("game.server.host")]
		public string ServerUrl { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLogoutSignal socialLogout { get; set; }

		[Inject]
		public global::Kampai.Game.ReLinkAccountSignal relinkSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Main.ReloadGameSignal reloadGameSignal { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[Inject]
		public global::Kampai.Game.GooglePlayServerAuthCodeReceivedSignal googlePlayServerAuthCodeReceivedSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.GooglePlayService googlePlayService = socialService as global::Kampai.Game.GooglePlayService;
			if (googlePlayService != null && googlePlayService.isLoggedIn && googlePlayService.ServerAuthCode == null)
			{
				googlePlayServerAuthCodeReceivedSignal.AddOnce(OnGooglePlayServerAuthCodeReceived);
				googlePlayService.RequestServerAuthCode();
			}
			else
			{
				LinkAccount();
			}
		}

		private void OnGooglePlayServerAuthCodeReceived(bool success, global::Kampai.Game.ISocialService socialService)
		{
			LinkAccount();
		}

		private void LinkAccount()
		{
			global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
			string userID = userSession.UserID;
			global::Kampai.Game.AccountLinkRequest accountLinkRequest = new global::Kampai.Game.AccountLinkRequest();
			if (socialService.isLoggedIn)
			{
				accountLinkRequest.credentials = socialService.accessToken;
				accountLinkRequest.externalId = socialService.userID;
				accountLinkRequest.identityType = socialService.type.ToString().ToLower();
			}
			global::Kampai.Game.UserIdentity userIdentity = new global::Kampai.Game.UserIdentity();
			userIdentity.ExternalID = socialService.userID;
			userIdentity.Type = (global::Kampai.Game.IdentityType)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.IdentityType), socialService.type.ToString().ToLower());
			userSession.SocialIdentities.Add(userIdentity);
			logger.Debug("attempting to link type {0} for ID {1} ", accountLinkRequest.identityType, accountLinkRequest.externalId);
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal.AddListener(OnAccountLinkResponse);
			downloadService.Perform(requestFactory.Resource(ServerUrl + string.Format("/rest/v2/user/{0}/identity", userID)).WithHeaderParam("user_id", userSession.UserID).WithHeaderParam("session_key", userSession.SessionID)
				.WithContentType("application/json")
				.WithMethod("POST")
				.WithEntity(accountLinkRequest)
				.WithResponseSignal(signal));
		}

		private global::System.Collections.IEnumerator WaitAFrame(global::System.Action a)
		{
			yield return null;
			a();
		}

		private void OnAccountLinkResponse(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			string body = response.Body;
			global::Kampai.Game.AccountLinkErrorResponse error = null;
			try
			{
				error = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.AccountLinkErrorResponse>(body);
			}
			catch (global::System.Exception e)
			{
				HandleJsonException(e);
			}
			global::Kampai.Game.GooglePlayService googlePlayService = socialService as global::Kampai.Game.GooglePlayService;
			if (googlePlayService != null)
			{
				googlePlayService.ResetServerAuthCode();
			}
			if (response.Success)
			{
				global::Kampai.Game.UserIdentity item = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.UserIdentity>(body);
				userSessionService.UserSession.SocialIdentities.Add(item);
				if (restartOnSuccess)
				{
					reloadGameSignal.Dispatch();
				}
			}
			else if (error != null && error.error.responseCode == 409)
			{
				logger.Error("Social Account is already linked to an account");
				RemoveSocialIdentity();
				routineRunner.StartCoroutine(WaitAFrame(delegate
				{
					global::strange.extensions.signal.impl.Signal<bool> signal = new global::strange.extensions.signal.impl.Signal<bool>();
					signal.AddListener(delegate(bool result)
					{
						PopUpCallback(result, error.error.details.conflictUserId);
					});
					string descKey = localService.GetString("AccountConflictBody", localService.GetString(socialService.locKey));
					global::Kampai.UI.View.PopupConfirmationSetting type = new global::Kampai.UI.View.PopupConfirmationSetting("AccountConflictTitle", descKey, true, "img_char_Min_FeedbackChecklist01", signal, "AccountConflictKeep", "AccountConflictRestore");
					gameContext.injectionBinder.GetInstance<global::Kampai.Game.DisplayConfirmationSignal>().Dispatch(type);
				}));
				logger.Debug(body ?? "json is null");
			}
			else
			{
				RemoveSocialIdentity();
				socialLogout.Dispatch(socialService);
				logger.Error("Error Linking Social Account");
				logger.Debug(body ?? "json is null");
			}
		}

		private void HandleJsonException(global::System.Exception e)
		{
			logger.Info("OnAccountLinkResponse exception: {0}", e.Message);
		}

		private void PopUpCallback(bool result, string conflictId)
		{
			if (result)
			{
				relinkSignal.Dispatch(socialService, conflictId, false);
			}
			else
			{
				relinkSignal.Dispatch(socialService, conflictId, true);
			}
		}

		private void RemoveSocialIdentity()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.UserIdentity> socialIdentities = userSessionService.UserSession.SocialIdentities;
			for (int i = 0; i < socialIdentities.Count; i++)
			{
				if (socialIdentities[i].ExternalID == socialService.userID)
				{
					socialIdentities.RemoveAt(i);
					break;
				}
			}
		}
	}
}
