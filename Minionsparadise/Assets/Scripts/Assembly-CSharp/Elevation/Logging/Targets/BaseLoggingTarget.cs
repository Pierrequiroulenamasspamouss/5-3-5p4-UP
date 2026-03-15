namespace Elevation.Logging.Targets
{
	public abstract class BaseLoggingTarget : global::Elevation.Logging.Targets.ILoggingTarget
	{
		public string Name { get; protected set; }

		public global::Elevation.Logging.LogLevel Level { get; protected set; }

		protected BaseLoggingTarget(string name, global::Elevation.Logging.LogLevel level)
		{
			Name = name;
			Level = level;
		}

		public abstract void WriteLogEvent(global::Elevation.Logging.LogEvent logEvent);

		public virtual void Flush()
		{
		}

		public virtual bool IsEnabled(global::Elevation.Logging.LogEvent logEvent)
		{
			if (logEvent != null)
			{
				return Level <= logEvent.Level;
			}
			return false;
		}

		public virtual void UpdateConfig(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			if (config.ContainsKey("level"))
			{
				Level = (global::Elevation.Logging.LogLevel)(int)global::System.Enum.Parse(typeof(global::Elevation.Logging.LogLevel), config["level"].ToString(), true);
			}
		}

		protected virtual string FormattedLogEvent(global::Elevation.Logging.LogEvent logEvent)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder(128);
			stringBuilder.AppendFormat("{0} {1,-5} [{2}] {3}", logEvent.Timestamp, logEvent.Level, logEvent.Scope, logEvent.ClassName);
			if (logEvent.MethodName != null)
			{
				stringBuilder.AppendFormat(".{0}", logEvent.MethodName);
			}
			stringBuilder.AppendFormat(": {0}", logEvent.FormattedMessage);
			if (logEvent.ElapsedTime > 0.0)
			{
				stringBuilder.AppendFormat(" Elapsed: {0:0.000}s", logEvent.ElapsedTime);
			}
			if (logEvent.Data != null)
			{
				stringBuilder.AppendFormat(" Data: {0}", logEvent.Data);
			}
			if (logEvent.StackTrace != null)
			{
				stringBuilder.AppendFormat("\nStackTrace: {0}", logEvent.StackTrace.ToString());
			}
			if (logEvent.Exception != null)
			{
				stringBuilder.AppendFormat("\nException: {0}", logEvent.Exception.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
