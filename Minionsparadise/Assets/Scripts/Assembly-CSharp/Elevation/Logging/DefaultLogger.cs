namespace Elevation.Logging
{
	public class DefaultLogger : global::Elevation.Logging.ILogger
	{
		private readonly global::Elevation.Logging.LogScope _scope;

		private readonly string _className;

		private global::System.Collections.Generic.Dictionary<string, global::System.Diagnostics.Stopwatch> _timers;

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

		public DefaultLogger(global::Elevation.Logging.LogScope scope, string className)
		{
			_scope = scope;
			_className = className;
		}

		public void Log(global::Elevation.Logging.LogEvent logEvent)
		{
			if (logEvent.TimerName != null)
			{
				if (_timers == null)
				{
					_timers = new global::System.Collections.Generic.Dictionary<string, global::System.Diagnostics.Stopwatch>();
				}
				global::System.Diagnostics.Stopwatch stopwatch;
				if (!logEvent.TimerStopped)
				{
					stopwatch = new global::System.Diagnostics.Stopwatch();
					stopwatch.Start();
					_timers[logEvent.TimerName] = stopwatch;
				}
				else if (_timers.TryGetValue(logEvent.TimerName, out stopwatch))
				{
					stopwatch.Stop();
					logEvent.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
				}
			}
			if (logEvent.Scope == global::Elevation.Logging.LogScope.Unknown)
			{
				logEvent.Scope = _scope;
			}
			if (logEvent.ClassName == null && _className != null)
			{
				logEvent.ClassName = _className;
			}
			global::Elevation.Logging.LogManager.Instance.WriteToTargets(logEvent);
		}
	}
}
