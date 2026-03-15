namespace Kampai.Main
{
	public class LoadLocalizationServiceCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadLocalizationServiceCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject(global::Kampai.Main.LocalizationServices.EVENT)]
		public global::Kampai.Main.ILocalizationService localEventService { get; set; }

		public override void Execute()
		{
			logger.Debug("Start loading localization Service");
			logger.EventStart("LoadLocalizationServiceCommand.Execute");
			if (localService.IsInitialized())
			{
				localService.Update();
			}
			else
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_LOAD_LOCALIZATION, "Localization service hasn't been initialized yet.");
			}
			if (localEventService.IsInitialized())
			{
				localEventService.Update();
			}
			else
			{
				logger.Error("Event Localization service hasn't been initialized yet.");
			}
			logger.EventStop("LoadLocalizationServiceCommand.Execute");
		}
	}
}
