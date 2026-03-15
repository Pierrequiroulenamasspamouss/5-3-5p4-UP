namespace Kampai.Game
{
	public class RegisterUserCommand : global::strange.extensions.command.impl.Command
	{
		public const string REGISTER_ENDPOINT = "/rest/user/register";

		[Inject("game.server.host")]
		public string ServerUrl { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Kampai.Game.ISynergyService synergyService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.UserRegisterRequest userRegisterRequest = new global::Kampai.Game.UserRegisterRequest();
			userRegisterRequest.SynergyID = synergyService.userID;
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal.AddListener(userSessionService.RegisterRequestCallback);
			downloadService.Perform(requestFactory.Resource(ServerUrl + "/rest/user/register").WithContentType("application/json").WithMethod("POST")
				.WithEntity(userRegisterRequest)
				.WithResponseSignal(signal));
		}
	}
}
