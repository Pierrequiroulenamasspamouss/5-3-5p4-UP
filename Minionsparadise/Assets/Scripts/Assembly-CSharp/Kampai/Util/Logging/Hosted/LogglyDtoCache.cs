namespace Kampai.Util.Logging.Hosted
{
	public class LogglyDtoCache : global::Kampai.Util.Logging.Hosted.ILogglyDtoCache
	{
		public enum DtoProperty
		{
			ClientVersion = 0,
			ClientDeviceType = 1,
			ClientPlatform = 2,
			ConfigUrl = 3,
			ConfigVariant = 4,
			DefinitionId = 5,
			UserId = 6,
			SynergyId = 7
		}

		private const string DEFAULT_VALUE = "unknown";

		private global::Kampai.Game.IUserSessionService userSessionService;

		private global::Kampai.Game.IConfigurationsService configurationsService;

		private readonly global::System.Collections.Generic.Dictionary<global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty, global::Kampai.Util.Logging.Hosted.CachedValue<string>> cache;

		private readonly global::Kampai.Util.Logging.Hosted.CachedValue<global::System.Collections.Generic.IList<string>> definitionVariantsCache;

		[Inject]
		public global::Kampai.Util.IClientVersion clientVersion { get; set; }

		public LogglyDtoCache()
		{
			cache = new global::System.Collections.Generic.Dictionary<global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty, global::Kampai.Util.Logging.Hosted.CachedValue<string>>();
			global::System.Func<string> valueSetter = () => (clientVersion != null) ? clientVersion.GetClientVersion() : null;
			cache.Add(global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ClientVersion, new global::Kampai.Util.Logging.Hosted.CachedValue<string>(valueSetter));
			global::System.Func<string> valueSetter2 = () => (clientVersion != null) ? clientVersion.GetClientDeviceType() : null;
			cache.Add(global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ClientDeviceType, new global::Kampai.Util.Logging.Hosted.CachedValue<string>(valueSetter2));
			global::System.Func<string> valueSetter3 = () => (clientVersion != null) ? clientVersion.GetClientPlatform() : null;
			cache.Add(global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ClientPlatform, new global::Kampai.Util.Logging.Hosted.CachedValue<string>(valueSetter3));
			global::System.Func<string> valueSetter4 = () => (configurationsService != null) ? configurationsService.GetConfigURL() : null;
			cache.Add(global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ConfigUrl, new global::Kampai.Util.Logging.Hosted.CachedValue<string>(valueSetter4));
			global::System.Func<string> valueSetter5 = () => (configurationsService != null) ? configurationsService.GetConfigVariant() : null;
			cache.Add(global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ConfigVariant, new global::Kampai.Util.Logging.Hosted.CachedValue<string>(valueSetter5));
			global::System.Func<string> valueSetter6 = delegate
			{
				if (configurationsService != null)
				{
					global::Kampai.Game.ConfigurationDefinition configurations = configurationsService.GetConfigurations();
					if (configurations != null)
					{
						return configurations.definitionId;
					}
				}
				return (string)null;
			};
			cache.Add(global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.DefinitionId, new global::Kampai.Util.Logging.Hosted.CachedValue<string>(valueSetter6));
			global::System.Func<string> valueSetter7 = delegate
			{
				if (userSessionService != null)
				{
					global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
					if (userSession != null)
					{
						return userSession.UserID;
					}
				}
				return (string)null;
			};
			cache.Add(global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.UserId, new global::Kampai.Util.Logging.Hosted.CachedValue<string>(valueSetter7));
			global::System.Func<string> valueSetter8 = delegate
			{
				if (userSessionService != null)
				{
					global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
					if (userSession != null)
					{
						return userSession.SynergyID;
					}
				}
				return (string)null;
			};
			cache.Add(global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.SynergyId, new global::Kampai.Util.Logging.Hosted.CachedValue<string>(valueSetter8));
			global::System.Func<global::System.Collections.Generic.IList<string>> valueSetter9 = delegate
			{
				string text = string.Empty;
				if (configurationsService != null)
				{
					text = configurationsService.GetDefinitionVariants();
				}
				return (text != null) ? text.Split(new char[1] { '_' }, global::System.StringSplitOptions.RemoveEmptyEntries) : null;
			};
			definitionVariantsCache = new global::Kampai.Util.Logging.Hosted.CachedValue<global::System.Collections.Generic.IList<string>>(valueSetter9);
		}

		void global::Kampai.Util.Logging.Hosted.ILogglyDtoCache.Initialize(global::Kampai.Game.IUserSessionService userSessionService, global::Kampai.Game.IConfigurationsService configurationsService)
		{
			this.userSessionService = userSessionService;
			this.configurationsService = configurationsService;
		}

		global::Kampai.Util.Logging.Hosted.LogglyDto global::Kampai.Util.Logging.Hosted.ILogglyDtoCache.GetCachedDto()
		{
			global::Kampai.Util.Logging.Hosted.LogglyDto logglyDto = new global::Kampai.Util.Logging.Hosted.LogglyDto();
			logglyDto.ClientVersion = cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ClientVersion].GetValue() ?? "unknown";
			logglyDto.ClientDeviceType = cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ClientDeviceType].GetValue() ?? "unknown";
			logglyDto.ClientPlatform = cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ClientPlatform].GetValue() ?? "unknown";
			logglyDto.UserId = cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.UserId].GetValue() ?? "unknown";
			logglyDto.SynergyId = cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.SynergyId].GetValue() ?? "unknown";
			logglyDto.ConfigUrl = cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ConfigUrl].GetValue() ?? "unknown";
			logglyDto.ConfigVariant = cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ConfigVariant].GetValue() ?? "unknown";
			logglyDto.DefinitionId = cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.DefinitionId].GetValue() ?? "unknown";
			logglyDto.DefinitionVariants = definitionVariantsCache.GetValue() ?? new string[1] { "unknown" };
			return logglyDto;
		}

		void global::Kampai.Util.Logging.Hosted.ILogglyDtoCache.RefreshConfigurationValues()
		{
			cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ConfigUrl].SetFresh(false);
			cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ConfigVariant].SetFresh(false);
			cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.DefinitionId].SetFresh(false);
			definitionVariantsCache.SetFresh(false);
		}

		void global::Kampai.Util.Logging.Hosted.ILogglyDtoCache.RefreshUserSessionValues()
		{
			cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.UserId].SetFresh(false);
			cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.SynergyId].SetFresh(false);
		}

		void global::Kampai.Util.Logging.Hosted.ILogglyDtoCache.RefreshClientVersionValues()
		{
			cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ClientVersion].SetFresh(false);
			cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ClientDeviceType].SetFresh(false);
			cache[global::Kampai.Util.Logging.Hosted.LogglyDtoCache.DtoProperty.ClientPlatform].SetFresh(false);
		}
	}
}
