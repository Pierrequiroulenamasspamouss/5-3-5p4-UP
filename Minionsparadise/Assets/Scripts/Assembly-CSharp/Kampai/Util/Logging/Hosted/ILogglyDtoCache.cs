namespace Kampai.Util.Logging.Hosted
{
	public interface ILogglyDtoCache
	{
		void Initialize(global::Kampai.Game.IUserSessionService userSessionService, global::Kampai.Game.IConfigurationsService configurationsService);

		global::Kampai.Util.Logging.Hosted.LogglyDto GetCachedDto();

		void RefreshConfigurationValues();

		void RefreshUserSessionValues();

		void RefreshClientVersionValues();
	}
}
