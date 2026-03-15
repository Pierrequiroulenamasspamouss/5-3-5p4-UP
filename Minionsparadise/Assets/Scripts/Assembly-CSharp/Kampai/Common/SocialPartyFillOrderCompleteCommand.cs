namespace Kampai.Common
{
	public class SocialPartyFillOrderCompleteCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialPartyFillOrderCompleteCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.StuartTicketFilledSignal stuartTicketFilledSignal { get; set; }

		public override void Execute()
		{
			logger.Info("Fill Order Complete");
			stuartTicketFilledSignal.Dispatch();
		}
	}
}
