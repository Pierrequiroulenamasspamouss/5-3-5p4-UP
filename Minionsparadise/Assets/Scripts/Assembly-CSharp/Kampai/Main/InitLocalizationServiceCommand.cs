namespace Kampai.Main
{
	public class InitLocalizationServiceCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("InitLocalizationServiceCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Main.LocalizationServiceInitializedSignal localizationServiceInitializedSignal { get; set; }

		[Inject(global::Kampai.Main.LocalizationServices.EVENT)]
		public global::Kampai.Main.ILocalizationService localEventService { get; set; }

		[Inject]
		public global::Kampai.Splash.SplashProgressUpdateSignal splashProgressDoneSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDevicePrefsService devicePrefsService { get; set; }

		public override void Execute()
		{
			if (!localService.IsInitialized())
			{
				string language = devicePrefsService.GetDevicePrefs().Language;
				if (string.IsNullOrEmpty(language))
				{
					language = global::Kampai.Util.Native.GetDeviceLanguage();
				}
				logger.Info("Got language code {0}", language);
				localService.Initialize(language);
				localEventService.Initialize("EN-US");
				localizationServiceInitializedSignal.Dispatch();
				splashProgressDoneSignal.Dispatch(20, 5f);
			}
		}
	}
}
