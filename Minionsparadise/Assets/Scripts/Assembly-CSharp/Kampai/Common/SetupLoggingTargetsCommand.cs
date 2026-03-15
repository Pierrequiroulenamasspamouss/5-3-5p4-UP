namespace Kampai.Common
{
	public class SetupLoggingTargetsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Util.IClientVersion clientVersion { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[Inject]
		public global::Kampai.Util.Logging.Hosted.ILogglyDtoCache logglyDtoCache { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject("game.server.environment")]
		public string serverEnvironment { get; set; }

		[Inject]
		public global::Kampai.Game.ConfigurationsLoadedSignal configurationsLoadedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UserSessionGrantedSignal userSessionGrantedSignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject]
		public global::Kampai.Common.AppEarlyPauseSignal appEarlyPauseSignal { get; set; }

		[Inject]
		public global::Kampai.Common.LogClientMetricsSignal clientMetricsSignal { get; set; }

		[Inject(global::Kampai.Util.BaseElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable baseContext { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		public override void Execute()
		{
			global::Elevation.Logging.LogManager.RegisterTarget("UnityEngine.Debug", global::Elevation.Logging.Targets.UnityEditorTarget.Build);
			global::Elevation.Logging.LogManager.RegisterTarget("Kampai.Fatal", BuildFatal);
			global::Elevation.Logging.LogManager.RegisterTarget("Kampai.Loggly", BuildLoggly);
			global::Elevation.Logging.LogManager.RegisterTarget("Kampai.Native", global::Kampai.Main.KampaiNativeTarget.Build);
			global::Elevation.Logging.LogManager.Instance.SetConfig(global::Kampai.Util.GameConstants.StaticConfig.LOGGING_CONFIG);
		}

		private global::Kampai.Main.KampaiFatalTarget BuildFatal(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			global::Kampai.Main.KampaiFatalTarget kampaiFatalTarget = new global::Kampai.Main.KampaiFatalTarget(baseContext, localService, clientMetricsSignal, "Kampai.Fatal");
			kampaiFatalTarget.UpdateConfig(config);
			return kampaiFatalTarget;
		}

		private global::Kampai.Main.KampaiLogglyTarget BuildLoggly(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			global::Kampai.Main.KampaiLogglyTarget kampaiLogglyTarget = new global::Kampai.Main.KampaiLogglyTarget("Kampai.Loggly", global::Elevation.Logging.LogLevel.Trace);
			kampaiLogglyTarget.Initialize(downloadService, requestFactory, userSessionService, configurationsService, serverEnvironment, clientVersion, logglyDtoCache, localPersistService, configurationsLoadedSignal, userSessionGrantedSignal, appEarlyPauseSignal);
			kampaiLogglyTarget.UpdateConfig(config);
			return kampaiLogglyTarget;
		}
	}
}
