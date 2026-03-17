namespace Elevation.Logging.Targets
{
	public abstract class AsyncLoggingTarget : global::Elevation.Logging.Targets.BaseLoggingTarget, global::System.IDisposable
	{
		private readonly global::Elevation.Collections.Generic.LockFreeQueue<global::Elevation.Logging.LogEvent> _events;

		private global::System.Collections.Generic.List<global::Elevation.Logging.LogFilter> _filters;

		private readonly global::System.Threading.Thread _worker;

		private volatile bool _flushQueue;

		private volatile bool _isRunning;

		public int TimeoutMillis { get; protected set; }

		protected bool Disposed { get; private set; }

		protected AsyncLoggingTarget(string name, global::Elevation.Logging.LogLevel level, params global::Elevation.Logging.LogFilter[] filters)
			: base(name, level)
		{
			TimeoutMillis = -1;
			if (filters != null && filters.Length > 0)
			{
				_filters = new global::System.Collections.Generic.List<global::Elevation.Logging.LogFilter>(filters);
			}
			_events = new global::Elevation.Collections.Generic.LockFreeQueue<global::Elevation.Logging.LogEvent>();
			_flushQueue = false;
			_isRunning = true;
			_worker = new global::System.Threading.Thread(AsyncProcess)
			{
				Name = string.Format("{0}_thread", Name)
			};
			_worker.Start();
		}

		public override void WriteLogEvent(global::Elevation.Logging.LogEvent logEvent)
		{
			if (!Disposed)
			{
				_events.Enqueue(logEvent);
			}
		}

		public override void Flush()
		{
			if (!Disposed && !_flushQueue)
			{
				_flushQueue = true;
			}
		}

		public override bool IsEnabled(global::Elevation.Logging.LogEvent logEvent)
		{
			if (Disposed)
			{
				return false;
			}
			global::System.Collections.Generic.List<global::Elevation.Logging.LogFilter> filters = _filters;
			if (filters != null)
			{
				for (int i = 0; i < filters.Count; i++)
				{
					if (filters[i].IsMatch(logEvent))
					{
						return filters[i].Inclusive;
					}
				}
			}
			return Level <= logEvent.Level;
		}

		protected virtual void SetFilters(global::System.Collections.Generic.List<global::Elevation.Logging.LogFilter> filters)
		{
			if (!Disposed)
			{
				_filters = filters;
			}
		}

		protected void ClearFilters()
		{
			if (!Disposed)
			{
				_filters = null;
			}
		}

		public override void UpdateConfig(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			if (Disposed)
			{
				return;
			}
			base.UpdateConfig(config);
			ClearFilters();
			if (!config.ContainsKey("filters"))
			{
				return;
			}
			global::System.Collections.Generic.IEnumerable<object> enumerable = config["filters"] as global::System.Collections.Generic.IEnumerable<object>;
			if (enumerable == null)
			{
				return;
			}
			global::System.Collections.Generic.List<global::Elevation.Logging.LogFilter> list = new global::System.Collections.Generic.List<global::Elevation.Logging.LogFilter>();
			foreach (object item in enumerable)
			{
				global::System.Collections.Generic.Dictionary<string, object> dictionary = item as global::System.Collections.Generic.Dictionary<string, object>;
				if (dictionary != null)
				{
					string pattern = string.Empty;
					global::Elevation.Logging.FilterType type = global::Elevation.Logging.FilterType.Message;
					bool inclusive = true;
					if (dictionary.ContainsKey("pattern") && !string.IsNullOrEmpty(dictionary["pattern"].ToString()))
					{
						pattern = dictionary["pattern"] as string;
					}
					if (dictionary.ContainsKey("type") && !string.IsNullOrEmpty(dictionary["type"].ToString()))
					{
						type = (global::Elevation.Logging.FilterType)(int)global::System.Enum.Parse(typeof(global::Elevation.Logging.FilterType), dictionary["type"].ToString(), true);
					}
					if (dictionary.ContainsKey("inclusive") && !string.IsNullOrEmpty(dictionary["inclusive"].ToString()))
					{
						inclusive = dictionary["inclusive"].ToString().ToLower().Equals("true");
					}
					list.Add(new global::Elevation.Logging.LogFilter(pattern, type, inclusive));
				}
			}
			if (list.Count > 0)
			{
				SetFilters(list);
			}
		}

		protected abstract void Write(global::Elevation.Logging.LogEvent logEvent);

		protected virtual void AsyncProcess()
		{
			while (_isRunning)
			{
				global::Elevation.Logging.LogEvent logEvent;
				if (_flushQueue)
				{
					while ((logEvent = _events.Dequeue()) != null)
					{
						if (IsEnabled(logEvent))
						{
							Write(logEvent);
						}
					}
					_flushQueue = false;
				}
				logEvent = _events.Dequeue();
				if (logEvent != null && IsEnabled(logEvent))
				{
					Write(logEvent);
				}
				PostProcess();
				global::System.Threading.Thread.Sleep(1);
			}
		}

		protected virtual void PostProcess()
		{
		}

		public void Dispose()
		{
			Dispose(true);
			global::System.GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!Disposed && disposing && _isRunning)
			{
				_isRunning = false;
				_worker.Join();
				Disposed = true;
			}
		}
	}
}
