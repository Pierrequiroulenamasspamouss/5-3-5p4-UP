namespace Kampai.Game
{
	public class SocialInitFailureCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialInitFailureCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.ISocialService socialService { get; set; }

		[Inject]
		public global::Kampai.Main.DisplayHindsightContentSignal displayHindsightContentSignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		public override void Execute()
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, socialService.type.ToString() + " Init Failed");
			if (!localPersistanceService.GetData("HindsightTriggeredAtGameLaunch").Equals("True"))
			{
				displayHindsightContentSignal.Dispatch(global::Kampai.Main.HindsightCampaign.Scope.game_launch);
				localPersistanceService.PutData("HindsightTriggeredAtGameLaunch", "True");
			}
		}
	}
}
