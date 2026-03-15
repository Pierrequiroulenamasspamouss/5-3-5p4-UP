namespace Elevation.Logging.Targets
{
	public class ConsoleTarget : global::Elevation.Logging.Targets.AsyncLoggingTarget
	{
		public ConsoleTarget(global::Elevation.Logging.LogLevel level, params global::Elevation.Logging.LogFilter[] filters)
			: base("Console", level, filters)
		{
		}

		protected override void Write(global::Elevation.Logging.LogEvent logEvent)
		{
			global::System.Console.Error.WriteLine(FormattedLogEvent(logEvent));
		}

		public static global::Elevation.Logging.Targets.ConsoleTarget Build(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			global::Elevation.Logging.Targets.ConsoleTarget consoleTarget = new global::Elevation.Logging.Targets.ConsoleTarget(global::Elevation.Logging.LogLevel.None);
			consoleTarget.UpdateConfig(config);
			return consoleTarget;
		}
	}
}
