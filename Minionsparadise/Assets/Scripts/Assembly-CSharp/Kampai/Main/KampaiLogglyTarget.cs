namespace Kampai.Main
{
#if !UNITY_WEBPLAYER
	public class KampaiLogglyTarget : global::Elevation.Logging.Targets.LogglyTarget
	{
		private const string TOKEN = "05946d11-d631-48e1-a74e-b344673d86f9";

		private const string TAG = "client.{0},{1}";

		private const int RETRY_ATTEMPTS = 1;

		private const int DEFAULT_SEND_RATE = 180;

		private const bool DEFAULT_KILL_SWITCH_STATE = false;

		private const int DEFAULT_MAX_BUFFER_SIZE_BYTES = 1048576;

		private const bool DEFAULT_LOGGING_ENABLED_STATE = true;

		private global::Kampai.Splash.IDownloadService downloadService;

		private global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory;

		private global::Kampai.Util.Logging.Hosted.ILogglyDtoCache logglyDtoCache;

		private global::Kampai.Util.Logging.Hosted.LogglyDto logglyDto;

		private bool newUser;

		private string logglyTag;

		public KampaiLogglyTarget(string name, global::Elevation.Logging.LogLevel level, string logFolder = null)
			: base(name, level, 180, 1048576, logFolder, "05946d11-d631-48e1-a74e-b344673d86f9", null)
		{
		}

		public void Initialize(global::Kampai.Splash.IDownloadService downloadService, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory, global::Kampai.Game.IUserSessionService userSessionService, global::Kampai.Game.IConfigurationsService configurationsService, string serverEnvironment, global::Kampai.Util.IClientVersion clientVersion, global::Kampai.Util.Logging.Hosted.ILogglyDtoCache dtoCache, ILocalPersistanceService localPersistService, global::Kampai.Game.ConfigurationsLoadedSignal configurationsLoadedSignal, global::Kampai.Game.UserSessionGrantedSignal userSessionGrantedSignal, global::Kampai.Common.AppEarlyPauseSignal earlyPauseSignal)
		{
			this.downloadService = downloadService;
			this.requestFactory = requestFactory;
			logglyDtoCache = dtoCache;
			logglyTag = string.Format("client.{0},{1}", serverEnvironment, clientVersion.GetClientPlatform());
			logglyDtoCache.Initialize(userSessionService, configurationsService);
			logglyDtoCache.RefreshClientVersionValues();
			logglyDtoCache.RefreshConfigurationValues();
			logglyDtoCache.RefreshUserSessionValues();
			string data = localPersistService.GetData("UserID");
			newUser = string.IsNullOrEmpty(data);
			SetupCache();
			configurationsLoadedSignal.AddListener(OnConfigurationsLoaded);
			userSessionGrantedSignal.AddListener(OnUserSessionGranted);
			earlyPauseSignal.AddListener(AppEarlyPaused);
		}

		protected override void SendRequest(byte[] bytes)
		{
			downloadService.Perform(requestFactory.Resource(_url).WithHeaderParam("X-LOGGLY-TAG", logglyTag).WithContentType("application/json")
				.WithMethod("POST")
				.WithBody(bytes)
				.WithGZip(true)
				.WithRetry(true, 1));
		}

		protected override void SerializeProperties(global::System.Text.StringBuilder sb, global::Elevation.Logging.LogEvent logEvent)
		{
			if (logglyDto != null)
			{
				SerializeProperty(sb, "userId", logglyDto.UserId);
				SerializeProperty(sb, "clientVersion", logglyDto.ClientVersion);
				SerializeProperty(sb, "clientDeviceType", logglyDto.ClientDeviceType);
				SerializeProperty(sb, "clientPlatform", logglyDto.ClientPlatform);
				SerializeProperty(sb, "newUser", logglyDto.NewUser);
				SerializeProperty(sb, "synergyId", logglyDto.SynergyId);
				SerializeProperty(sb, "configUrl", logglyDto.ConfigUrl);
				SerializeProperty(sb, "configVariant", logglyDto.ConfigVariant);
				SerializeProperty(sb, "definitionId", logglyDto.DefinitionId);
			}
		}

		protected void SerializeProperty(global::System.Text.StringBuilder sb, string name, object value)
		{
			sb.AppendFormat("\"{0}\":\"{1}\",", name, value);
		}

		public void OnConfigurationsLoaded(bool init)
		{
			if (!base.Disposed && logglyDtoCache != null)
			{
				logglyDtoCache.RefreshConfigurationValues();
				SetupCache();
			}
		}

		public void AppEarlyPaused()
		{
			if (!base.Disposed)
			{
				Flush();
			}
		}

		public void OnUserSessionGranted()
		{
			if (!base.Disposed && logglyDtoCache != null)
			{
				logglyDtoCache.RefreshUserSessionValues();
				SetupCache();
				Flush();
			}
		}

		private void SetupCache()
		{
			logglyDto = logglyDtoCache.GetCachedDto();
			logglyDto.NewUser = newUser.ToString();
		}

		public override void UpdateConfig(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			if (!base.Disposed)
			{
				base.UpdateConfig(config);
			}
		}
	}
#else
	public class KampaiLogglyTarget : global::Elevation.Logging.Targets.LogglyTarget
	{
		public KampaiLogglyTarget(string name, global::Elevation.Logging.LogLevel level, string logFolder = null)
			: base(name, level, 0, 0, logFolder, string.Empty, null)
		{
		}

		public void Initialize(global::Kampai.Splash.IDownloadService downloadService, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory, global::Kampai.Game.IUserSessionService userSessionService, global::Kampai.Game.IConfigurationsService configurationsService, string serverEnvironment, global::Kampai.Util.IClientVersion clientVersion, global::Kampai.Util.Logging.Hosted.ILogglyDtoCache dtoCache, ILocalPersistanceService localPersistService, global::Kampai.Game.ConfigurationsLoadedSignal configurationsLoadedSignal, global::Kampai.Game.UserSessionGrantedSignal userSessionGrantedSignal, global::Kampai.Common.AppEarlyPauseSignal earlyPauseSignal)
		{
		}

		protected override void SendRequest(byte[] bytes)
		{
		}

		public override void UpdateConfig(global::System.Collections.Generic.Dictionary<string, object> config)
		{
		}

		protected override void SerializeProperties(global::System.Text.StringBuilder sb, global::Elevation.Logging.LogEvent logEvent)
		{
		}
	}
#endif
}
