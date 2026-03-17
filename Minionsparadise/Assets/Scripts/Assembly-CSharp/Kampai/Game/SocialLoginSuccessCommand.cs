namespace Kampai.Game
{
	public class SocialLoginSuccessCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialLoginSuccessCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.ISocialService socialService { get; set; }

		[Inject]
		public global::Kampai.Game.SocialInitSuccessSignal socialInitSuccess { get; set; }

		public override void Execute()
		{
			logger.Debug("Social Login Success");
			socialService.SendLoginTelemetry("Game Screen");
			socialInitSuccess.Dispatch(socialService);
		}
	}
}
