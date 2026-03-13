namespace Kampai.Util
{
	public class KampaiLoggerV2 : global::Elevation.Logging.DefaultLogger, global::Kampai.Util.IKampaiLogger
	{
		public KampaiLoggerV2(global::Elevation.Logging.LogScope scope, string className)
			: base(scope, className)
		{
		}

		public bool IsAllowedLevel(global::Kampai.Util.KampaiLogLevel level)
		{
			return true;
		}

		public void SetAllowedLevel(int level)
		{
		}

		public void Log(global::Kampai.Util.KampaiLogLevel level, string format, params object[] args)
		{
			LogIt(level, format, args);
		}

		public void Log(global::Kampai.Util.KampaiLogLevel level, string text)
		{
			LogIt(level, text);
		}

		public void Log(global::Kampai.Util.KampaiLogLevel level, bool toScreen, string text)
		{
			LogIt(level, text);
		}

		public void LogNullArgument()
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, "Null arguments");
		}

		public void Verbose(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Verbose, text);
		}

		public void Verbose(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Verbose, format, args);
		}

		public void Debug(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Debug, text);
		}

		public void Debug(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Debug, format, args);
		}

		public void Info(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Info, text);
		}

		public void Info(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Info, format, args);
		}

		public void Warning(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Warning, text);
		}

		public void Warning(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Warning, format, args);
		}

		public void Error(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, text);
		}

		public void Error(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, format, args);
		}

		public void EventStart(string eventName)
		{
			Log(global::Elevation.Logging.LogEvent.Debug(string.Format("EventStart: {0}", eventName)).StartTimer(eventName));
		}

		public void EventStop(string eventName)
		{
			Log(global::Elevation.Logging.LogEvent.Debug(string.Format("EventStop: {0}", eventName)).StopTimer(eventName));
		}

		public void LogEventList()
		{
		}

		protected void LogIt(global::Kampai.Util.KampaiLogLevel level, string text)
		{
			switch (level)
			{
			case global::Kampai.Util.KampaiLogLevel.Info:
				Log(global::Elevation.Logging.LogEvent.Info(text));
				break;
			case global::Kampai.Util.KampaiLogLevel.Debug:
				Log(global::Elevation.Logging.LogEvent.Debug(text));
				break;
			case global::Kampai.Util.KampaiLogLevel.Warning:
				Log(global::Elevation.Logging.LogEvent.Warn(text));
				break;
			case global::Kampai.Util.KampaiLogLevel.Error:
				Log(global::Elevation.Logging.LogEvent.Error(text));
				break;
			case global::Kampai.Util.KampaiLogLevel.Verbose:
				Log(global::Elevation.Logging.LogEvent.Trace(text));
				break;
			default:
				Log(global::Elevation.Logging.LogEvent.Error(text));
				break;
			}
		}

		protected void LogIt(global::Kampai.Util.KampaiLogLevel level, string format, params object[] args)
		{
			switch (level)
			{
			case global::Kampai.Util.KampaiLogLevel.Info:
				Log(global::Elevation.Logging.LogEvent.Info(format, args));
				break;
			case global::Kampai.Util.KampaiLogLevel.Debug:
				Log(global::Elevation.Logging.LogEvent.Debug(format, args));
				break;
			case global::Kampai.Util.KampaiLogLevel.Warning:
				Log(global::Elevation.Logging.LogEvent.Warn(format, args));
				break;
			case global::Kampai.Util.KampaiLogLevel.Error:
				Log(global::Elevation.Logging.LogEvent.Error(format, args));
				break;
			case global::Kampai.Util.KampaiLogLevel.Verbose:
				Log(global::Elevation.Logging.LogEvent.Trace(format, args));
				break;
			default:
				Log(global::Elevation.Logging.LogEvent.Error(format, args));
				break;
			}
		}

		public void Fatal(global::Kampai.Util.FatalCode code, string format, params object[] args)
		{
			Fatal(code, 0, format, args);
		}

		public void FatalNoThrow(global::Kampai.Util.FatalCode code, string format, params object[] args)
		{
			FatalNoThrow(code, 0, format, args);
		}

		public void Fatal(global::Kampai.Util.FatalCode code, int referencedId, string format, params object[] args)
		{
			FatalNoThrow(code, referencedId, format, args);
			throw new global::Kampai.Util.FatalException(code, referencedId, format, args);
		}

		public void FatalNullArgument(global::Kampai.Util.FatalCode code)
		{
			Fatal(code, "Null argument");
		}

		public void Fatal(global::Kampai.Util.FatalCode code)
		{
			Fatal(code, code.ToString());
		}

		public void FatalNoThrow(global::Kampai.Util.FatalCode code)
		{
			FatalNoThrow(code, 0, code.ToString());
		}

		public void Fatal(global::Kampai.Util.FatalCode code, int referencedId)
		{
			Fatal(code, referencedId, string.Empty);
		}

		public void FatalNoThrow(global::Kampai.Util.FatalCode code, int referencedId)
		{
			FatalNoThrow(code, referencedId, string.Empty);
		}

		public void FatalNoThrow(global::Kampai.Util.FatalCode code, int referencedId, string format, params object[] args)
		{
			string message = string.Format("[ERROR {0}-{1}] {2}", (int)code, referencedId, string.Format(format, args));
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary.Add("format", format);
			dictionary.Add("fatalCode", code);
			dictionary.Add("referencedId", referencedId);
			dictionary.Add("params", args);
			global::System.Collections.Generic.Dictionary<string, object> data = dictionary;
			Log(global::Elevation.Logging.LogEvent.Fatal(message).WithData(data).WithStackTrace());
		}

		public static global::Elevation.Logging.ILogger BuildingKampaiLogger(global::Elevation.Logging.LogScope scope, string className)
		{
			return new global::Kampai.Util.KampaiLoggerV2(scope, className);
		}
	}
}
