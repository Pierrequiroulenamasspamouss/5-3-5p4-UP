public class UpdateUserCommand : global::strange.extensions.command.impl.Command
{
	public const string USER_UPDATE_ENDPOINT = "/rest/user/{0}";

	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UpdateUserCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public string synergyID { get; set; }

	[Inject("game.server.host")]
	public string ServerUrl { get; set; }

	[Inject]
	public global::Kampai.Splash.IDownloadService downloadService { get; set; }

	[Inject]
	public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

	[Inject]
	public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

	public override void Execute()
	{
		if (string.IsNullOrEmpty(synergyID))
		{
			logger.Error("Failed to update user with new synergyID. Provided synergyID is null or empty.");
			return;
		}
		global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
		if (userSession != null)
		{
			string userID = userSession.UserID;
			global::Kampai.Game.UserUpdateRequest userUpdateRequest = new global::Kampai.Game.UserUpdateRequest();
			userUpdateRequest.SynergyID = synergyID;
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal.AddListener(delegate(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
			{
				userSessionService.UserUpdateRequestCallback(synergyID, response);
			});
			downloadService.Perform(requestFactory.Resource(ServerUrl + string.Format("/rest/user/{0}", userID)).WithHeaderParam("user_id", userID).WithHeaderParam("session_key", userSession.SessionID)
				.WithContentType("application/json")
				.WithMethod("POST")
				.WithEntity(userUpdateRequest)
				.WithResponseSignal(signal));
		}
		else
		{
			logger.Error("Failed to update user with new synergyID {0}. No user session yet.", synergyID);
		}
	}
}
