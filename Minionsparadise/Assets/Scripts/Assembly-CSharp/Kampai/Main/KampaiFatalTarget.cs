namespace Kampai.Main
{
	public class KampaiFatalTarget : global::Elevation.Logging.Targets.ILoggingTarget
	{
		private global::strange.extensions.context.api.ICrossContextCapable _baseContext;

		private global::Kampai.Main.ILocalizationService _localService;

		private global::Kampai.Common.LogClientMetricsSignal _clientMetricsSignal;

		public string Name { get; set; }

		public global::Elevation.Logging.LogLevel Level { get; set; }

		public KampaiFatalTarget(global::strange.extensions.context.api.ICrossContextCapable baseContext, global::Kampai.Main.ILocalizationService localService, global::Kampai.Common.LogClientMetricsSignal clientMetricsSignal, string name)
		{
			Name = name;
			Level = global::Elevation.Logging.LogLevel.Fatal;
			_baseContext = baseContext;
			_localService = localService;
			_clientMetricsSignal = clientMetricsSignal;
		}

		public void WriteLogEvent(global::Elevation.Logging.LogEvent logEvent)
		{
			if (IsEnabled(logEvent))
			{
				global::System.Collections.Generic.Dictionary<string, object> data = logEvent.Data;
				global::Kampai.Util.FatalCode code = (global::Kampai.Util.FatalCode)(int)data["fatalCode"];
				int subcode = (int)data["referencedId"];
				object[] args = data["params"] as object[];
				string format = data["format"] as string;
				SendFatalTelemetry(code, subcode, format, args);
				ShowFatalView(code, subcode, args);
			}
		}

		private void ShowFatalView(global::Kampai.Util.FatalCode code, int subcode, params object[] args)
		{
			string code2 = string.Format("{0}-{1}", (int)code, subcode);
			string title;
			string message;
			if (global::Kampai.Util.GracefulErrors.IsGracefulError(code))
			{
				global::Kampai.Util.GracefulMessage gracefulError = global::Kampai.Util.GracefulErrors.GetGracefulError(code);
				title = _localService.GetString(gracefulError.Title);
				message = _localService.GetString(gracefulError.Description, args);
			}
			else if (global::Kampai.Util.Extensions.IsNetworkError(code))
			{
				title = string.Format(_localService.GetString("FatalNetworkTitle"));
				message = _localService.GetString("FatalNetworkMessage");
			}
			else
			{
				title = string.Format(_localService.GetString("FatalTitle"));
				message = _localService.GetString("FatalMessage");
			}
			string playerID = string.Empty;
			global::Kampai.Game.IPlayerService instance = _baseContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (instance != null && instance.IsPlayerInitialized())
			{
				playerID = instance.ID.ToString();
			}
			global::Kampai.Util.FatalView.SetFatalText(code2, message, title, playerID);
			_baseContext.injectionBinder.GetInstance<global::Kampai.Common.ReInitializeGameSignal>().Dispatch("Fatal");
		}

		private void SendFatalTelemetry(global::Kampai.Util.FatalCode code, int subcode, string format, params object[] args)
		{
			bool flag = false;
			try
			{
				string eventName = string.Format("AppFlow.Fatal.{0}", code);
				global::Kampai.Common.Service.HealthMetrics.IClientHealthService instance = _baseContext.injectionBinder.CrossContextBinder.GetInstance<global::Kampai.Common.Service.HealthMetrics.IClientHealthService>();
				if (instance != null)
				{
					instance.MarkMeterEvent(eventName);
					_clientMetricsSignal.Dispatch(true);
					flag = true;
				}
			}
			catch (global::System.Exception ex)
			{
				global::Kampai.Util.Native.LogError(string.Format("Unable to report fatal code metric: {0}", ex.Message));
				flag = false;
			}
			string text = global::System.Enum.GetName(typeof(global::Kampai.Util.FatalCode), code);
			if (string.IsNullOrEmpty(text))
			{
				text = "UNKNOWN";
			}
			string nameOfError = string.Format("{0}-{1}-{2}", text, (int)code, subcode);
			string errorDetails = (string.IsNullOrEmpty(format) ? string.Empty : ((args == null) ? string.Format("{0}, Userfacing: {1}", format, flag) : string.Format("{0}, {1}, Userfacing: {2}", format, args, flag)));
			try
			{
				global::Kampai.Common.ITelemetryService instance2 = _baseContext.injectionBinder.CrossContextBinder.GetInstance<global::Kampai.Common.ITelemetryService>();
				if (instance2 != null)
				{
					if (global::Kampai.Util.Extensions.IsNetworkError(code))
					{
						instance2.Send_Telemetry_EVT_GAME_ERROR_CONNECTIVITY(nameOfError, errorDetails, true);
					}
					else
					{
						instance2.Send_Telemetry_EVT_GAME_ERROR_GAMEPLAY(nameOfError, errorDetails, true);
					}
				}
			}
			catch (global::System.Exception ex2)
			{
				global::Kampai.Util.Native.LogError(string.Format("Unable to log telemetry fatal code: {0}", ex2.Message));
			}
		}

		public void Flush()
		{
		}

		public bool IsEnabled(global::Elevation.Logging.LogEvent logEvent)
		{
			return Level <= logEvent.Level;
		}

		public void UpdateConfig(global::System.Collections.Generic.Dictionary<string, object> config)
		{
		}
	}
}
