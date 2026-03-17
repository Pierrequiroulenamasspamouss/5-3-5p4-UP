namespace Kampai.Common
{
	public class SetupSwrveCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupSwrveCommand") as global::Kampai.Util.IKampaiLogger;

		private static bool swrveVerboseLogEnabled = true;

		[Inject]
		public string kampaiUserID { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject(global::Kampai.Main.MainElement.MANAGER_PARENT)]
		public global::UnityEngine.GameObject managers { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Common.ISwrveService swrveService { get; set; }

		[Inject]
		public global::Kampai.Game.ABTestSignal abTestSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.ABTestResourcesUpdatedSignal abTestResourcesUpdatedSignal { get; set; }

		public override void Execute()
		{
			/*
#if UNITY_EDITOR
			logger.Info("Swrve is disabled in Unity Editor.");
			MoveForwardWithoutSwrve();
			return;
#endif
			*/
			if (configurationsService.isKillSwitchOn(global::Kampai.Game.KillSwitch.SWRVE))
			{
				logger.Info("Swrve is disabled by kill switch.");
				MoveForwardWithoutSwrve();
				return;
			}
			if (!telemetryService.SharingUsageEnabled())
			{
				logger.Info("Swrve is disabled because sharing usage is disabled");
				MoveForwardWithoutSwrve();
				return;
			}
			abTestSignal.AddOnce(SetSWRVEData);
			playerService.SWRVEGroup = global::Kampai.Util.ABTestModel.definitionVariants;
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("SwrvePrefab");
			gameObject.transform.parent = managers.transform;
			logger.Debug("SetupSwrveCommand:Execute swrveObject added to scene");
			Init(gameObject);
			swrveService.UpdateResources();
			telemetryService.AddTelemetrySender(swrveService);
			telemetryService.AddIapTelemetryService(swrveService);
			swrveService.SharingUsage(telemetryService.SharingUsageEnabled());
			swrveService.COPPACompliance();
			logger.Debug("SetupSwrveCommand.Execute SWRVEGroup: {0}", playerService.SWRVEGroup);
		}

		private void SetSWRVEData(global::Kampai.Util.ABTestCommand.GameMetaData dataArgs)
		{
			playerService.SWRVEGroup = dataArgs.definitionVariants;
		}

		private void MoveForwardWithoutSwrve()
		{
			abTestResourcesUpdatedSignal.Dispatch(false);
		}

		private void Init(global::UnityEngine.GameObject swrveObject)
		{
			SwrveComponent swrveComponent = swrveObject.AddComponent<SwrveComponent>();
			logger.Debug("Swrve: swrveAppIdType: {0}", global::Kampai.Util.GameConstants.StaticConfig.ENVIRONMENT);
			int result;
			if (!int.TryParse(global::Kampai.Util.GameConstants.StaticConfig.SWRVE_APP_ID, out result))
			{
				logger.Error("Invalid GameConstants.StaticConfig.SWRVE_APP_ID");
			}
			swrveComponent.FlushEventsOnApplicationQuit = true;
			swrveComponent.InitialiseOnStart = false;
			global::Swrve.SwrveConfig config = swrveComponent.Config;
			config.PushNotificationEnabled = false;
			config.UserId = kampaiUserID;
			config.Orientation = global::Swrve.Messaging.SwrveOrientation.Landscape;
			config.AutoDownloadCampaignsAndResources = false;
			
			// Force local server URLs
			config.EventsServer = "http://localhost:44732";
			config.ContentServer = "http://localhost:44732";

			InitSwrveLog();
			swrveComponent.Init(result, global::Kampai.Util.GameConstants.StaticConfig.SWRVE_API_KEY);
			logger.Debug("SetupSwrveCommand:Init SWRVE GameObject initialized");
		}

		private void InitSwrveLog()
		{
			SwrveLog.Verbose = swrveVerboseLogEnabled;
			SwrveLog.OnLog = OnSwrveLog;
		}

		private void OnSwrveLog(SwrveLog.SwrveLogType type, object message, string tag)
		{
			logger.Log(Convert(type), "Swrve: [{0}] {1}", tag, message);
		}

		private static global::Kampai.Util.KampaiLogLevel Convert(SwrveLog.SwrveLogType type)
		{
			switch (type)
			{
			case SwrveLog.SwrveLogType.Info:
				return global::Kampai.Util.KampaiLogLevel.Info;
			case SwrveLog.SwrveLogType.Warning:
				return global::Kampai.Util.KampaiLogLevel.Warning;
			case SwrveLog.SwrveLogType.Error:
				return global::Kampai.Util.KampaiLogLevel.Error;
			default:
				return global::Kampai.Util.KampaiLogLevel.Debug;
			}
		}
	}
}
