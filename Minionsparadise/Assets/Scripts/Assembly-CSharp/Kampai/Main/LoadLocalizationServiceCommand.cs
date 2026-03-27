namespace Kampai.Main
{
	public class LoadLocalizationServiceCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadLocalizationServiceCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject(global::Kampai.Main.LocalizationServices.EVENT)]
		public global::Kampai.Main.ILocalizationService localEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDevicePrefsService devicePrefsService { get; set; }

		[Inject]
		public global::Kampai.Game.SaveDevicePrefsSignal saveDevicePrefsSignal { get; set; }

		public override void Execute()
		{
			logger.Debug("Start loading localization Service");
			string language = devicePrefsService.GetDevicePrefs().Language;
			if ((language == "lolcat" || language == "minion") && (configurationService.GetConfigurations() == null || !configurationService.GetConfigurations().AprilsFool))
			{
				string deviceLanguage = global::Kampai.Util.Native.GetDeviceLanguage().ToLower();
				logger.Warning("Language {0} is disabled by server. Falling back to device language {1}", language, deviceLanguage);
				devicePrefsService.GetDevicePrefs().Language = deviceLanguage;
				saveDevicePrefsSignal.Dispatch();
				localService.Initialize(deviceLanguage);
			}
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
