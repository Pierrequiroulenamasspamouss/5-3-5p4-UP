namespace Elevation.Logging.Targets
{
	public class NullTarget : global::Elevation.Logging.Targets.ILoggingTarget
	{
		public string Name { get; private set; }

		public global::Elevation.Logging.LogLevel Level { get; private set; }

		public NullTarget()
		{
			Name = "Null";
			Level = global::Elevation.Logging.LogLevel.None;
		}

		public void WriteLogEvent(global::Elevation.Logging.LogEvent logEvent)
		{
		}

		public void Flush()
		{
		}

		public bool IsEnabled(global::Elevation.Logging.LogEvent logEvent)
		{
			return false;
		}

		public void UpdateConfig(global::System.Collections.Generic.Dictionary<string, object> config)
		{
		}
	}
}
