namespace Kampai.Game
{
	public class CloneUserFromEnvCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CloneUserFromEnvCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public string FromEnvironment { get; set; }

		[Inject]
		public long UserId { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Kampai.Main.ReloadGameSignal reloadGameSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, true, string.Format("Cloning {0} From {1}", UserId, FromEnvironment));
			FetchGamestate();
		}

		private void FetchGamestate()
		{
			global::Kampai.Game.UserSession gameStateRequestSession = GetGameStateRequestSession();
			string text = string.Format("https://kampai-{0}.appspot.com/rest/gamestate/{1}", FromEnvironment, UserId);
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, true, string.Format("Cloning: {0} using {1}:{2}", text, gameStateRequestSession.UserID, gameStateRequestSession.SessionID));
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal.AddListener(delegate(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
			{
				if (response.Success)
				{
					PostGamestate(response.Body);
				}
				else
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Error, true, string.Format("Server request failed: [{0}]{1}", response.Code, response.Body));
				}
			});
			downloadService.Perform(requestFactory.Resource(text).WithHeaderParam("user_id", gameStateRequestSession.UserID).WithHeaderParam("session_key", gameStateRequestSession.SessionID)
				.WithResponseSignal(signal));
		}

		private global::Kampai.Game.UserSession GetGameStateRequestSession()
		{
			global::Kampai.Game.UserSession userSession = new global::Kampai.Game.UserSession();
			if (FromEnvironment == "prod")
			{
				userSession.UserID = "4816488934408192";
				userSession.SessionID = "mSqL48RwjvpUoorT-T7Yb4D5IsA_YYZgaaquZY2qESQ";
			}
			else if (FromEnvironment == "stage")
			{
				userSession.UserID = "5165625081069568";
				userSession.SessionID = "WZdzsiUDlMN5gdWIt0ogSjenlImZNY-ZNXEqVKSSgBo";
			}
			else
			{
				global::Kampai.Game.UserSession userSession2 = userSessionService.UserSession;
				userSession.UserID = userSession2.UserID;
				userSession.SessionID = global::Kampai.Util.GameConstants.StaticConfig.SECRET_KEY;
			}
			return userSession;
		}

		private void PostGamestate(string gamestate)
		{
			global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
			string text = string.Format("{0}/rest/gamestate/{1}?forceSave=true", global::Kampai.Util.GameConstants.StaticConfig.SERVER_URL, userSession.UserID);
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, true, string.Format("Cloning to: {0}", text));
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal.AddListener(delegate(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
			{
				if (response.Success)
				{
					reloadGameSignal.Dispatch();
				}
				else
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Error, true, string.Format("Server request failed: [{0}]{1}", response.Code, response.Body));
				}
			});
			downloadService.Perform(requestFactory.Resource(text).WithMethod("POST").WithBody(global::System.Text.Encoding.UTF8.GetBytes(gamestate))
				.WithHeaderParam("user_id", userSession.UserID)
				.WithHeaderParam("session_key", userSession.SessionID)
				.WithContentType("text/plain")
				.WithResponseSignal(signal));
		}
	}
}
