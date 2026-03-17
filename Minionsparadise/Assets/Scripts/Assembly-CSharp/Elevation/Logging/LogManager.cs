namespace Elevation.Logging
{
	public class LogManager
	{
		private static readonly global::Elevation.Logging.LogManager _instance = new global::Elevation.Logging.LogManager();

		private readonly global::System.Collections.Generic.List<global::Elevation.Logging.Targets.ILoggingTarget> _targets;

		private readonly object _targetLock = new object();

		private readonly global::System.Collections.Generic.Dictionary<string, global::Elevation.Logging.ILogger> _loggers;

		private global::Elevation.Logging.Factory<global::Elevation.Logging.Targets.ILoggingTarget, global::System.Collections.Generic.Dictionary<string, object>> _targetFactory;

		private global::System.Func<global::Elevation.Logging.LogScope, string, global::Elevation.Logging.ILogger> _loggerBuilder;

		public static global::Elevation.Logging.LogManager Instance
		{
			get
			{
				return _instance;
			}
		}

		private LogManager()
		{
			_targets = new global::System.Collections.Generic.List<global::Elevation.Logging.Targets.ILoggingTarget>();
			_loggers = new global::System.Collections.Generic.Dictionary<string, global::Elevation.Logging.ILogger>(16);
			_targetFactory = new global::Elevation.Logging.Factory<global::Elevation.Logging.Targets.ILoggingTarget, global::System.Collections.Generic.Dictionary<string, object>>();
			_loggerBuilder = (global::Elevation.Logging.LogScope scope, string className) => new global::Elevation.Logging.DefaultLogger(scope, className);
			_targetFactory.Register("file", global::Elevation.Logging.Targets.FileTarget.Build);
			_targetFactory.Register("console", global::Elevation.Logging.Targets.ConsoleTarget.Build);
			_targetFactory.Register("null", (global::System.Collections.Generic.Dictionary<string, object> config) => new global::Elevation.Logging.Targets.NullTarget());
		}

		public void SetConfig(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			if (config == null)
			{
				return;
			}
			global::System.Collections.Generic.List<global::Elevation.Logging.Targets.ILoggingTarget> list = new global::System.Collections.Generic.List<global::Elevation.Logging.Targets.ILoggingTarget>(_targets);
			foreach (string key in config.Keys)
			{
				global::Elevation.Logging.Targets.ILoggingTarget loggingTarget = GetTarget(key);
				global::System.Collections.Generic.Dictionary<string, object> dictionary = config[key] as global::System.Collections.Generic.Dictionary<string, object>;
				if (dictionary == null)
				{
					continue;
				}
				try
				{
					if (loggingTarget == null)
					{
						loggingTarget = CreateTarget(key, dictionary);
						AddTarget(loggingTarget);
					}
					else
					{
						loggingTarget.UpdateConfig(dictionary);
						list.Remove(loggingTarget);
					}
				}
				catch (global::System.Exception arg)
				{
					global::System.Console.Error.WriteLine("Error processing config for target: {0}.\n{1}", key, arg);
					if (loggingTarget != null)
					{
						RemoveTarget(loggingTarget);
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				RemoveTarget(list[i]);
			}
		}

		public void AddTarget(global::Elevation.Logging.Targets.ILoggingTarget target)
		{
			lock (_targetLock)
			{
				_targets.Add(target);
			}
		}

		public void Shutdown()
		{
			lock (_targetLock)
			{
				for (int i = 0; i < _targets.Count; i++)
				{
					ShutdownTarget(_targets[i]);
				}
				_targets.Clear();
			}
		}

		private void ShutdownTarget(global::Elevation.Logging.Targets.ILoggingTarget target)
		{
			global::System.IDisposable disposable = target as global::System.IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		public void WriteToTargets(global::Elevation.Logging.LogEvent logEvent)
		{
			lock (_targetLock)
			{
				for (int i = 0; i < _targets.Count; i++)
				{
					_targets[i].WriteLogEvent(logEvent);
				}
			}
		}

		public static void RegisterLogger(global::System.Func<global::Elevation.Logging.LogScope, string, global::Elevation.Logging.ILogger> builder)
		{
			_instance._loggerBuilder = builder;
		}

		public static global::Elevation.Logging.ILogger GetDefaultLogger()
		{
			return _instance.GetLogger(global::Elevation.Logging.LogScope.Default, null);
		}

		public static global::Elevation.Logging.ILogger GetClassLogger(global::Elevation.Logging.LogScope scope, string className)
		{
			return _instance.GetLogger(scope, className);
		}

		public static global::Elevation.Logging.ILogger GetClassLogger(string className)
		{
			return _instance.GetLogger(global::Elevation.Logging.LogScope.Default, className);
		}

		private global::Elevation.Logging.ILogger GetLogger(global::Elevation.Logging.LogScope scope, string className)
		{
			string text = scope.ToString();
			string key = string.Join(".", new string[2]
			{
				text,
				(className != null) ? className : string.Empty
			});
			global::Elevation.Logging.ILogger value;
			if (_loggers.TryGetValue(key, out value))
			{
				return value;
			}
			value = _loggerBuilder(scope, className);
			_loggers[key] = value;
			return value;
		}

		public static void RegisterTarget(string name, global::System.Func<global::System.Collections.Generic.Dictionary<string, object>, global::Elevation.Logging.Targets.ILoggingTarget> builder)
		{
			_instance._targetFactory.Register(name, builder);
		}

		private global::Elevation.Logging.Targets.ILoggingTarget GetTarget(string targetName)
		{
			lock (_targetLock)
			{
				for (int i = 0; i < _targets.Count; i++)
				{
					if (string.Compare(_targets[i].Name, targetName, global::System.StringComparison.OrdinalIgnoreCase) == 0)
					{
						return _targets[i];
					}
				}
			}
			return null;
		}

		private global::Elevation.Logging.Targets.ILoggingTarget CreateTarget(string name, global::System.Collections.Generic.Dictionary<string, object> config)
		{
			if (_targetFactory == null)
			{
				return null;
			}
			return _targetFactory.Create(name, config);
		}

		private void RemoveTarget(global::Elevation.Logging.Targets.ILoggingTarget target)
		{
			lock (_targetLock)
			{
				for (int i = 0; i < _targets.Count; i++)
				{
					if (string.Compare(_targets[i].Name, target.Name, global::System.StringComparison.OrdinalIgnoreCase) == 0)
					{
						ShutdownTarget(_targets[i]);
						_targets.RemoveAt(i);
						break;
					}
				}
			}
		}
	}
}
