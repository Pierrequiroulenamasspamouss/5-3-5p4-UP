namespace Elevation.Logging
{
	public class LogEvent
	{
		private string _message;

		private object[] _parameters;

		private string _formattedMessage;

		public global::Elevation.Logging.LogLevel Level { get; set; }

		public global::Elevation.Logging.LogScope Scope { get; set; }

		public string ClassName { get; set; }

		public string MethodName { get; set; }

		public string TimerName { get; set; }

		public bool TimerStopped { get; set; }

		public double ElapsedTime { get; set; }

		public global::System.Collections.Generic.Dictionary<string, object> Data { get; set; }

		public global::System.Diagnostics.StackTrace StackTrace { get; private set; }

		public global::System.Exception Exception { get; set; }

		public int FatalCode { get; set; }

		public global::System.DateTime Timestamp { get; private set; }

		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value;
				_formattedMessage = null;
			}
		}

		public object[] Parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_parameters = value;
				_formattedMessage = null;
			}
		}

		public string FormattedMessage
		{
			get
			{
				if (_formattedMessage != null)
				{
					return _formattedMessage;
				}
				_formattedMessage = ((_parameters == null) ? _message : string.Format(_message, _parameters));
				return _formattedMessage;
			}
		}

		public string ClassAndMethodName
		{
			get
			{
				if (ClassName == null && MethodName == null)
				{
					return string.Empty;
				}
				if (ClassName == null)
				{
					return MethodName;
				}
				if (MethodName == null)
				{
					return ClassName;
				}
				return string.Join(".", new string[2] { ClassName, MethodName });
			}
		}

		public LogEvent()
		{
			Timestamp = global::System.DateTime.UtcNow;
			Level = global::Elevation.Logging.LogLevel.None;
			Scope = global::Elevation.Logging.LogScope.Unknown;
			ClassName = null;
			MethodName = null;
			TimerName = null;
			TimerStopped = false;
			ElapsedTime = -1.0;
			Data = null;
			StackTrace = null;
			Exception = null;
		}

		public static global::Elevation.Logging.LogEvent Trace(string message)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Trace;
			logEvent.Message = message;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Trace(string message, params object[] args)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Trace;
			logEvent.Message = message;
			logEvent.Parameters = args;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Debug(string message)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Debug;
			logEvent.Message = message;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Debug(string message, params object[] args)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Debug;
			logEvent.Message = message;
			logEvent.Parameters = args;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Info(string message)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Info;
			logEvent.Message = message;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Info(string message, params object[] args)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Info;
			logEvent.Message = message;
			logEvent.Parameters = args;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Warn(string message)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Warn;
			logEvent.Message = message;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Warn(string message, params object[] args)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Warn;
			logEvent.Message = message;
			logEvent.Parameters = args;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Error(string message)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Error;
			logEvent.Message = message;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Error(string message, params object[] args)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Error;
			logEvent.Message = message;
			logEvent.Parameters = args;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Fatal(string message)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Fatal;
			logEvent.Message = message;
			return logEvent;
		}

		public static global::Elevation.Logging.LogEvent Fatal(string message, params object[] args)
		{
			global::Elevation.Logging.LogEvent logEvent = new global::Elevation.Logging.LogEvent();
			logEvent.Level = global::Elevation.Logging.LogLevel.Fatal;
			logEvent.Message = message;
			logEvent.Parameters = args;
			return logEvent;
		}

		public global::Elevation.Logging.LogEvent FromScope(global::Elevation.Logging.LogScope scope)
		{
			Scope = scope;
			return this;
		}

		public global::Elevation.Logging.LogEvent FromClass(string className)
		{
			ClassName = className;
			return this;
		}

		public global::Elevation.Logging.LogEvent FromMethod(string methodName)
		{
			MethodName = methodName;
			return this;
		}

		public global::Elevation.Logging.LogEvent StartTimer(string timerName)
		{
			TimerName = timerName;
			TimerStopped = false;
			return this;
		}

		public global::Elevation.Logging.LogEvent StopTimer(string timerName)
		{
			TimerName = timerName;
			TimerStopped = true;
			return this;
		}

		public global::Elevation.Logging.LogEvent WithData(global::System.Collections.Generic.Dictionary<string, object> data)
		{
			Data = data;
			return this;
		}

		public global::Elevation.Logging.LogEvent WithException(global::System.Exception exception)
		{
			Exception = exception;
			return this;
		}

		public global::Elevation.Logging.LogEvent WithStackTrace()
		{
			StackTrace = new global::System.Diagnostics.StackTrace(1, true);
			return this;
		}

		public global::Elevation.Logging.LogEvent WithFatalCode(int code)
		{
			FatalCode = code;
			return this;
		}
	}
}
