namespace Kampai.Main
{
	public class KampaiNativeTarget : global::Elevation.Logging.Targets.AsyncLoggingTarget
	{
		public KampaiNativeTarget(global::Elevation.Logging.LogLevel level)
			: base("Kampai.Native", level)
		{
		}

		protected override void Write(global::Elevation.Logging.LogEvent logEvent)
		{
			if (logEvent != null)
			{
				switch (logEvent.Level)
				{
				case global::Elevation.Logging.LogLevel.Trace:
				case global::Elevation.Logging.LogLevel.None:
					global::Kampai.Util.Native.LogVerbose(FormattedLogEvent(logEvent));
					break;
				case global::Elevation.Logging.LogLevel.Info:
					global::Kampai.Util.Native.LogInfo(FormattedLogEvent(logEvent));
					break;
				case global::Elevation.Logging.LogLevel.Debug:
					global::Kampai.Util.Native.LogDebug(FormattedLogEvent(logEvent));
					break;
				case global::Elevation.Logging.LogLevel.Warn:
					global::Kampai.Util.Native.LogWarning(FormattedLogEvent(logEvent));
					break;
				case global::Elevation.Logging.LogLevel.Error:
				case global::Elevation.Logging.LogLevel.Fatal:
					global::Kampai.Util.Native.LogError(FormattedLogEvent(logEvent));
					break;
				default:
					global::Kampai.Util.Native.LogError(FormattedLogEvent(logEvent));
					break;
				}
			}
		}

		public static global::Kampai.Main.KampaiNativeTarget Build(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			global::Kampai.Main.KampaiNativeTarget kampaiNativeTarget = new global::Kampai.Main.KampaiNativeTarget(global::Elevation.Logging.LogLevel.Trace);
			kampaiNativeTarget.UpdateConfig(config);
			return kampaiNativeTarget;
		}
	}
}
