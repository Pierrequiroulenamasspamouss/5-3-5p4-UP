namespace Elevation.Logging
{
	public interface ILogger
	{
		global::Elevation.Logging.LogScope Scope { get; }

		string ClassName { get; }

		void Log(global::Elevation.Logging.LogEvent logEvent);
	}
}
