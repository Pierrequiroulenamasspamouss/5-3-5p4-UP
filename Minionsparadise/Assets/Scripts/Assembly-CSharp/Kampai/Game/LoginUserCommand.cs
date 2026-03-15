namespace Kampai.Game
{
	public class LoginUserCommand : global::strange.extensions.command.impl.Command
	{
		public const string LOGIN_ENDPOINT = "/rest/user/session";

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoginUserCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public ILocalPersistanceService LocalPersistService { get; set; }

		[Inject]
		public IEncryptionService encryptionService { get; set; }

		[Inject]
		public global::Kampai.Game.RegisterUserSignal RegisterUserSignal { get; set; }

		[Inject("game.server.host")]
		public string ServerUrl { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService UserSessionService { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			logger.EventStart("LoginUserCommand.Execute");
			string text = LocalPersistService.GetData("UserID");
			string text2 = LocalPersistService.GetData("AnonymousSecret");
			string plainText = string.Empty;
			if (encryptionService.TryDecrypt(text2, "Kampai!", out plainText))
			{
				text2 = plainText;
			}
			string text3 = LocalPersistService.GetData("AnonymousID");
			if (encryptionService.TryDecrypt(text3, "Kampai!", out plainText))
			{
				text3 = plainText;
			}
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3))
			{
				global::Kampai.Util.TimeProfiler.StartSection("register");
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, true, "Registering new anonymous user");
				RegisterUserSignal.Dispatch();
			}
			else
			{
				global::Kampai.Util.TimeProfiler.StartSection("login");
				global::Kampai.Game.UserLoginRequest userLoginRequest = new global::Kampai.Game.UserLoginRequest();
				userLoginRequest.UserID = text;
				userLoginRequest.AnonymousSecret = text2;
				userLoginRequest.IdentityID = text3;
				global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
				signal.AddListener(UserSessionService.LoginRequestCallback);
				string text4 = ServerUrl + "/rest/user/session";
				downloadService.Perform(requestFactory.Resource(text4).WithContentType("application/json").WithMethod("POST")
					.WithEntity(userLoginRequest)
					.WithResponseSignal(signal));
				logger.Debug("LoginUserCommand: Using url {0}", text4);
			}
			logger.EventStop("LoginUserCommand.Execute");
		}
	}
}
