namespace Elevation.Logging.Targets
{
#if !UNITY_WEBPLAYER
	public abstract class LogglyTarget : global::Elevation.Logging.Targets.BufferedTarget
	{
		protected readonly string _url;

		private global::System.DateTime _lastRollTime;

		private int _sendRate;

		protected LogglyTarget(string name, global::Elevation.Logging.LogLevel level, int sendRateSeconds, int maxBufferSize, string logFolder, string token, string tag, params global::Elevation.Logging.LogFilter[] filters)
			: base(name, level, sendRateSeconds * 500, maxBufferSize, logFolder, filters)
		{
			_lastRollTime = global::System.DateTime.UtcNow;
			_sendRate = sendRateSeconds;
			if (tag != null)
			{
				_url = string.Format("https://logs-01.loggly.com/bulk/{0}/tag/{1}", token, tag);
			}
			else
			{
				_url = string.Format("https://logs-01.loggly.com/bulk/{0}", token);
			}
		}

		protected override void PostProcess()
		{
			if ((global::System.DateTime.UtcNow - _lastRollTime).TotalSeconds > (double)_sendRate && base.CurrentFileSize > 0)
			{
				RollLogFiles();
			}
		}

		protected override void RollLogFiles()
		{
			base.RollLogFiles();
			_lastRollTime = global::System.DateTime.UtcNow;
		}

		protected override void BatchProcess(global::System.IO.FileInfo buffer)
		{
			byte[] array = null;
			using (global::System.IO.FileStream fileStream = global::System.IO.File.OpenRead(buffer.FullName))
			{
				int num = 0;
				int num2 = (int)fileStream.Length;
				array = new byte[num2];
				while (num2 > 0)
				{
					int num3 = fileStream.Read(array, num, num2);
					if (num3 == 0)
					{
						break;
					}
					num += num3;
					num2 -= num3;
				}
			}
			if (array != null && array.Length > 0)
			{
				SendRequest(array);
			}
		}

		protected abstract void SendRequest(byte[] bytes);

		public override void UpdateConfig(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			if (!base.Disposed)
			{
				base.UpdateConfig(config);
				if (config.ContainsKey("sendRateSeconds"))
				{
					_sendRate = global::System.Convert.ToInt32(config["sendRateSeconds"]);
					base.TimeoutMillis = _sendRate * 500;
				}
			}
		}

		protected virtual void SerializeProperties(global::System.Text.StringBuilder sb, global::Elevation.Logging.LogEvent logEvent)
		{
		}

		private void SerializeStandardProperties(global::System.Text.StringBuilder sb, global::Elevation.Logging.LogEvent logEvent)
		{
			sb.AppendFormat("\"timestamp\":\"{0}\",", logEvent.Timestamp.ToString("o"));
			sb.AppendFormat("\"logLevel\":\"{0}\",", logEvent.Level.ToString().ToUpper());
			sb.AppendFormat("\"scope\":\"{0}\",", logEvent.Scope);
			if (logEvent.ClassName != null)
			{
				sb.AppendFormat("\"class\":\"{0}\",", logEvent.ClassName);
			}
			if (logEvent.MethodName != null)
			{
				sb.AppendFormat("\"method\":\"{0}\",", logEvent.MethodName);
			}
			if (logEvent.ElapsedTime > 0.0)
			{
				sb.AppendFormat("\"elapsedTime\":{0},", logEvent.ElapsedTime);
			}
			if (logEvent.StackTrace != null)
			{
				sb.AppendFormat("\"stacktrace\":");
				SerializeString(sb, logEvent.StackTrace.ToString());
				sb.Append(',');
			}
			if (logEvent.Exception != null)
			{
				sb.AppendFormat("\"exception\":");
				SerializeString(sb, logEvent.Exception.ToString());
				sb.Append(',');
			}
			sb.AppendFormat("\"message\":\"{0}\"", logEvent.FormattedMessage);
		}

		protected void SerializeString(global::System.Text.StringBuilder sb, string str)
		{
			sb.Append('"');
			foreach (char c in str)
			{
				switch (c)
				{
				case '"':
					sb.Append("\\\"");
					continue;
				case '\\':
					sb.Append("\\\\");
					continue;
				case '\b':
					sb.Append("\\b");
					continue;
				case '\f':
					sb.Append("\\f");
					continue;
				case '\n':
					sb.Append("\\n");
					continue;
				case '\r':
					sb.Append("\\r");
					continue;
				case '\t':
					sb.Append("\\t");
					continue;
				}
				int num = global::System.Convert.ToInt32(c);
				if (num >= 32 && num <= 126)
				{
					sb.Append(c);
					continue;
				}
				sb.Append("\\u");
				sb.Append(num.ToString("x4"));
			}
			sb.Append('"');
		}

		protected override string FormattedLogEvent(global::Elevation.Logging.LogEvent logEvent)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder(128);
			stringBuilder.Append('{');
			SerializeProperties(stringBuilder, logEvent);
			SerializeStandardProperties(stringBuilder, logEvent);
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}
	}
#else
	public abstract class LogglyTarget : global::Elevation.Logging.Targets.BufferedTarget
	{
		protected LogglyTarget(string name, global::Elevation.Logging.LogLevel level, int sendRateSeconds, int maxBufferSize, string logFolder, string token, string tag, params global::Elevation.Logging.LogFilter[] filters)
			: base(name, level, 0, 0, logFolder, filters)
		{
		}

		protected abstract void SendRequest(byte[] bytes);

		protected override void BatchProcess(global::System.IO.FileInfo buffer)
		{
		}

		protected virtual void SerializeProperties(global::System.Text.StringBuilder sb, global::Elevation.Logging.LogEvent logEvent)
		{
		}
	}
#endif
}
