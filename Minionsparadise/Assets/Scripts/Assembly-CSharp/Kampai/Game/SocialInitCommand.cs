namespace Kampai.Game
{
	public class SocialInitCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialInitCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.ISocialService socialService { get; set; }

		[Inject]
		public global::Kampai.Game.SocialInitSuccessSignal initSuccess { get; set; }

		[Inject]
		public global::Kampai.Game.SocialInitFailureSignal initFailure { get; set; }

		public override void Execute()
		{
			logger.Debug("Social Init Command Called With {0}", socialService.type.ToString());
			socialService.Init(initSuccess, initFailure);
		}
	}
}
