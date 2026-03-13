namespace Kampai.Game
{
	public class SocialLoginCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialLoginCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.ISocialService socialService { get; set; }

		[Inject]
		public global::Kampai.Util.Boxed<global::System.Action> callback { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginSuccessSignal loginSuccess { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginFailureSignal loginFailure { get; set; }

		public override void Execute()
		{
			logger.Debug("Social Login Command Called With {0}", socialService.type.ToString());
			if (!socialService.isLoggedIn)
			{
				socialService.Login(loginSuccess, loginFailure, callback.Value);
			}
			else
			{
				logger.Debug("Already logged on, you must log out first");
			}
		}
	}
}
