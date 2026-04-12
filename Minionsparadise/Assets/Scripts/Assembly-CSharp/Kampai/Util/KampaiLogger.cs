namespace Kampai.Util
{
	public class KampaiLogger : global::Kampai.Util.IKampaiLogger
	{
		private global::Kampai.Util.KampaiLogLevel allowedLevel;

		[Inject]
		public global::Kampai.UI.View.LogToScreenSignal logToScreenSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject(global::Kampai.Util.BaseElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable baseContext { get; set; }

		public virtual bool IsAllowedLevel(global::Kampai.Util.KampaiLogLevel level)
		{
			return allowedLevel <= level;
		}

		public virtual void SetAllowedLevel(int level)
		{
			allowedLevel = (global::Kampai.Util.KampaiLogLevel)level;
			Log(global::Kampai.Util.KampaiLogLevel.Info, "Set log level: {0}", allowedLevel);
		}

		public virtual void Log(global::Kampai.Util.KampaiLogLevel level, string format, params object[] args)
		{
			string text = string.Format(format, args);
			LogIt(level, text);
		}

		public virtual void Log(global::Kampai.Util.KampaiLogLevel level, string text)
		{
			LogIt(level, text);
		}

		public virtual void Log(global::Kampai.Util.KampaiLogLevel level, bool toScreen, string text)
		{
			if (toScreen)
			{
				logToScreenSignal.Dispatch(text);
			}
			LogIt(level, text);
		}

		public virtual void LogNullArgument()
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, "Null arguments");
		}

		public virtual void Verbose(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Verbose, text);
		}

		public virtual void Verbose(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Verbose, format, args);
		}

		public virtual void Debug(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Debug, text);
		}

		public virtual void Debug(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Debug, format, args);
		}

		public virtual void Info(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Info, text);
		}

		public virtual void Info(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Info, format, args);
		}

		public virtual void Warning(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Warning, text);
		}

		public virtual void Warning(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Warning, format, args);
		}

		public virtual void Error(string text)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, text);
		}

		public virtual void Error(string format, params object[] args)
		{
			Log(global::Kampai.Util.KampaiLogLevel.Error, format, args);
		}

		public void EventStart(string eventName)
		{
			LogIt(global::Kampai.Util.KampaiLogLevel.Debug, string.Format("EventStart: {0}", eventName));
		}

		public void EventStop(string eventName)
		{
			LogIt(global::Kampai.Util.KampaiLogLevel.Debug, string.Format("EventStop: {0}", eventName));
		}

		public void LogEventList()
		{
		}

        protected virtual void LogIt(global::Kampai.Util.KampaiLogLevel level, string text, bool isFatal = false)
        {
            if (IsAllowedLevel(level))
            {
                // --- HIJACK UNITY LOGS ---
                switch (level)
                {
                    case global::Kampai.Util.KampaiLogLevel.Info:
                        global::UnityEngine.Debug.Log("<color=cyan>[V1-INFO]</color> " + text);
                        global::Kampai.Util.Native.LogInfo(text);
                        break;
                    case global::Kampai.Util.KampaiLogLevel.Debug:
                        global::UnityEngine.Debug.Log("<color=silver>[V1-DEBUG]</color> " + text);
                        global::Kampai.Util.Native.LogDebug(text);
                        break;
                    case global::Kampai.Util.KampaiLogLevel.Warning:
                        global::UnityEngine.Debug.LogWarning("<color=orange>[V1-WARN]</color> " + text);
                        global::Kampai.Util.Native.LogWarning(text);
                        break;
                    case global::Kampai.Util.KampaiLogLevel.Error:
                        global::UnityEngine.Debug.LogError("<color=red>[V1-ERROR]</color> " + (isFatal ? "[FATAL] " : "") + text);
                        global::Kampai.Util.Native.LogError(text);
                        break;
                    default:
                        global::UnityEngine.Debug.Log("<color=white>[V1-VERBOSE]</color> " + text);
                        global::Kampai.Util.Native.LogVerbose(text);
                        break;
                }
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

		public virtual void FatalNoThrow(global::Kampai.Util.FatalCode code, int referencedId, string format, params object[] args)
		{
			string text = string.Format("[ERROR {0}-{1}] {2}", (int)code, referencedId, string.Format(format, args));
            // --- HIJACK DU CRASH FATAL ---
            global::UnityEngine.Debug.LogError("<color=red><b>[FATAL CRASH TRIGGERED]</b></color> Code: " + code.ToString() + " | Reason: " + text);
            // -----------------------------
            string text2 = new global::System.Diagnostics.StackTrace(1, true).ToString();
			LogIt(global::Kampai.Util.KampaiLogLevel.Error, text, true);
			LogIt(global::Kampai.Util.KampaiLogLevel.Error, text2, true);
			string code2 = string.Format("{0}-{1}", (int)code, referencedId);
			string title;
			string message;
			if (global::Kampai.Util.GracefulErrors.IsGracefulError(code))
			{
				global::Kampai.Util.GracefulMessage gracefulError = global::Kampai.Util.GracefulErrors.GetGracefulError(code);
				title = localService.GetString(gracefulError.Title);
				message = localService.GetString(gracefulError.Description, args);
			}
			else if (code.IsNetworkError())
			{
				title = string.Format(localService.GetString("FatalNetworkTitle"));
				message = localService.GetString("FatalNetworkMessage");
			}
			else
			{
				title = string.Format(localService.GetString("FatalTitle"));
				message = localService.GetString("FatalMessage");
			}
			string playerID = string.Empty;
			global::Kampai.Game.IPlayerService instance = baseContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (instance != null && instance.IsPlayerInitialized())
			{
				playerID = instance.ID.ToString();
			}
			global::Kampai.Util.FatalView.SetFatalText(code2, message, title, playerID);
			baseContext.injectionBinder.GetInstance<global::Kampai.Common.ReInitializeGameSignal>().Dispatch("Fatal");
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
	}
}
