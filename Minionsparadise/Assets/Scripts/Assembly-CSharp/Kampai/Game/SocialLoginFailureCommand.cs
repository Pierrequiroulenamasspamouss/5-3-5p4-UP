namespace Kampai.Game
{
	public class SocialLoginFailureCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialLoginFailureCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.ISocialService socialService { get; set; }

		public override void Execute()
		{
			logger.Debug("{0} Login Failed", socialService.type.ToString());
		}
	}
}
