namespace Kampai.Main
{
	public class AppStartCompleteCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("AppStartCompleteCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.DisplayHindsightContentSignal displayHindsightContentSignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		[Inject]
		public global::Kampai.Main.GameLoadedModel gameLoadedModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.Util.DeviceInformation deviceInformation { get; set; }

		public override void Execute()
		{
			currencyService.RefreshCatalog();
			localPersistanceService.PutData("ExternalLinkOpened", "False");
			if (!localPersistanceService.GetData("SocialInProgress").Equals("True"))
			{
				displayHindsightContentSignal.Dispatch(global::Kampai.Main.HindsightCampaign.Scope.game_launch);
				localPersistanceService.PutData("HindsightTriggeredAtGameLaunch", "True");
			}
			else
			{
				localPersistanceService.PutData("HindsightTriggeredAtGameLaunch", "False");
			}
			showAllWayFindersSignal.Dispatch();
			gameLoadedModel.gameLoaded = true;
			global::UnityEngine.Application.targetFrameRate = new global::Kampai.Util.DeviceCapabilities().GetTargetFrameRate(logger, global::UnityEngine.Application.platform, deviceInformation);
			long num = TimeUtil.CurrentTimeMillis() - global::Kampai.Util.Native.GetAppStartupTime();
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, true, string.Format("App Started: {0}", ((float)num / 1000f).ToString()));
			gameLoadedModel.coldStartTime = num;
		}
	}
}
