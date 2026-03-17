namespace Kampai.Util
{
	public class TestLogger : global::Elevation.Logging.ILogger, global::Kampai.Util.IKampaiLogger
	{
		public bool OutputToDebugLog;

		private readonly global::Elevation.Logging.LogScope _scope;

		private readonly string _className;

		public global::Elevation.Logging.LogScope Scope
		{
			get
			{
				return _scope;
			}
		}

		public string ClassName
		{
			get
			{
				return _className;
			}
		}

		public TestLogger()
		{
			_scope = global::Elevation.Logging.LogScope.Default;
			_className = "Test";
		}

		public global::Kampai.Util.TestLogger EnableOutput()
		{
			OutputToDebugLog = true;
			return this;
		}

		public void Log(global::Kampai.Util.KampaiLogLevel level, string format, params object[] args)
		{
			if (OutputToDebugLog)
			{
				global::UnityEngine.Debug.Log(level.ToString() + ": " + string.Format(format, args));
			}
		}

		public void Log(global::Kampai.Util.KampaiLogLevel level, string text)
		{
			if (OutputToDebugLog)
			{
				global::UnityEngine.Debug.Log(level.ToString() + ": " + text);
			}
		}

		public void Log(global::Kampai.Util.KampaiLogLevel level, bool toScreen, string text)
		{
			Log(level, text);
		}

		public void Log(global::Elevation.Logging.LogEvent logEvent)
		{
			if (logEvent != null && OutputToDebugLog)
			{
				switch (logEvent.Level)
				{
				case global::Elevation.Logging.LogLevel.Trace:
				case global::Elevation.Logging.LogLevel.Debug:
				case global::Elevation.Logging.LogLevel.Info:
				case global::Elevation.Logging.LogLevel.None:
					global::UnityEngine.Debug.Log(logEvent.Message);
					break;
				case global::Elevation.Logging.LogLevel.Warn:
					global::UnityEngine.Debug.LogWarning(logEvent.Message);
					break;
				case global::Elevation.Logging.LogLevel.Error:
					global::UnityEngine.Debug.LogError(logEvent.Message);
					break;
				case global::Elevation.Logging.LogLevel.Fatal:
					global::UnityEngine.Debug.LogError("<color=blue>FATAL:</color>" + logEvent.Message);
					break;
				default:
					global::UnityEngine.Debug.LogError(logEvent.Message);
					break;
				}
			}
		}

		public void LogNullArgument()
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, "Null argument");
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

		public void Fatal(global::Kampai.Util.FatalCode code, string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, "FATAL " + code.ToString() + string.Format(format, args));
		}

		public void Fatal(global::Kampai.Util.FatalCode code, int referenceId, string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, "FATAL " + code.ToString() + " - " + referenceId + " " + string.Format(format, args));
		}

		public void FatalNullArgument(global::Kampai.Util.FatalCode code)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, "FATAL " + code.ToString() + " Null argument");
		}

		public void Fatal(global::Kampai.Util.FatalCode code)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, "FATAL " + code);
		}

		public void FatalNoThrow(global::Kampai.Util.FatalCode code)
		{
			Fatal(code);
		}

		public void FatalNoThrow(global::Kampai.Util.FatalCode code, int referencedId)
		{
			Fatal(code, referencedId);
		}

		public void FatalNoThrow(global::Kampai.Util.FatalCode code, string format, params object[] args)
		{
			Fatal(code, format, args);
		}

		public void FatalNoThrow(global::Kampai.Util.FatalCode code, int referencedId, string format, params object[] args)
		{
			Fatal(code, referencedId, format, args);
		}

		public void SetAllowedLevel(int level)
		{
		}

		public bool IsAllowedLevel(global::Kampai.Util.KampaiLogLevel level)
		{
			return true;
		}

		public virtual void Fatal(global::Kampai.Util.FatalCode code, int referencedId)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, "FATAL " + code.ToString() + ":" + referencedId);
		}

		public void EventStart(string eventName)
		{
		}

		public void EventStop(string eventName)
		{
		}

		public void LogEventList()
		{
		}

		public static global::Elevation.Logging.ILogger BuildingTestLogger(global::Elevation.Logging.LogScope scope, string className)
		{
			return new global::Kampai.Util.TestLogger();
		}
	}
}
