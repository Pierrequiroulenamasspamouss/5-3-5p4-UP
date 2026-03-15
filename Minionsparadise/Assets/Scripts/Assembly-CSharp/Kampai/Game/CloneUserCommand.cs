namespace Kampai.Game
{
	public class CloneUserCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CloneUserCommand") as global::Kampai.Util.IKampaiLogger;

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
			global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
			string text = string.Format("{0}/rest/gamestate/{1}/{2}", global::Kampai.Util.GameConstants.StaticConfig.SERVER_URL, UserId, userSession.UserID);
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, true, string.Format("Cloning: {0}", text));
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
			downloadService.Perform(requestFactory.Resource(text).WithHeaderParam("user_id", userSession.UserID).WithHeaderParam("session_key", userSession.SessionID)
				.WithResponseSignal(signal));
		}
	}
}
