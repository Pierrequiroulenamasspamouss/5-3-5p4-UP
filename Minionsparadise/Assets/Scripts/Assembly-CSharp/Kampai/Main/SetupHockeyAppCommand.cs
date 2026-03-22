namespace Kampai.Main
{
	public class SetupHockeyAppCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupHockeyAppCommand") as global::Kampai.Util.IKampaiLogger;

		private string HockeyAppId = global::Kampai.Util.GameConstants.StaticConfig.HOCKEY_APP_ID;

		[Inject(global::Kampai.Main.MainElement.MANAGER_PARENT)]
		public global::UnityEngine.GameObject managers { get; set; }

		[Inject]
		public ILocalPersistanceService persistanceService { get; set; }

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.IClientHealthService clientHealthService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			// De-integrated HockeyApp: Skip initialization to avoid ClassNotFoundException and potential hangs on Android.
			return;
#if !UNITY_ANDROID || UNITY_EDITOR
			return;
#else
			logger.EventStart("SetupHockeyAppCommand.Execute");
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("HockeyApp");
			gameObject.SetActive(false);
			string userId = persistanceService.GetData("UserID");
			gameObject.name = "HockeyAppUnityAndroid";
			HockeyAppAndroid hockeyAppAndroid = gameObject.AddComponent<HockeyAppAndroid>();
			hockeyAppAndroid.packageID = global::Kampai.Util.Native.BundleIdentifier;
			hockeyAppAndroid.exceptionLogging = true;
			hockeyAppAndroid.appID = HockeyAppId;
			hockeyAppAndroid.userId = userId;
			hockeyAppAndroid.crashReportCallback = delegate
			{
				clientHealthService.MarkMeterEvent("AppFlow.Crash");
			};
			hockeyAppAndroid.autoUpload = true;
			hockeyAppAndroid.telemetryService = telemetryService;
			gameObject.SetActive(true);
			logger.EventStop("SetupHockeyAppCommand.Execute");
#endif
		}
	}
}
