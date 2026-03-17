namespace Elevation.Logging.Targets
{
	public interface ILoggingTarget
	{
		string Name { get; }

		global::Elevation.Logging.LogLevel Level { get; }

		void WriteLogEvent(global::Elevation.Logging.LogEvent logEvent);

		void Flush();

		bool IsEnabled(global::Elevation.Logging.LogEvent logEvent);

		void UpdateConfig(global::System.Collections.Generic.Dictionary<string, object> config);
	}
}
