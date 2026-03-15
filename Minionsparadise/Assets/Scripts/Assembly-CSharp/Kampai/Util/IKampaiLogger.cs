namespace Kampai.Util
{
	public interface IKampaiLogger
	{
		void Log(global::Kampai.Util.KampaiLogLevel level, string format, params object[] args);

		void Log(global::Kampai.Util.KampaiLogLevel level, string text);

		void Log(global::Kampai.Util.KampaiLogLevel level, bool toScreen, string text);

		void LogNullArgument();

		void Verbose(string text);

		void Verbose(string format, params object[] args);

		void Debug(string text);

		void Debug(string format, params object[] args);

		void Info(string text);

		void Info(string format, params object[] args);

		void Warning(string text);

		void Warning(string format, params object[] args);

		void Error(string text);

		void Error(string format, params object[] args);

		void Fatal(global::Kampai.Util.FatalCode code);

		void Fatal(global::Kampai.Util.FatalCode code, int referencedId);

		void Fatal(global::Kampai.Util.FatalCode code, string format, params object[] args);

		void Fatal(global::Kampai.Util.FatalCode code, int referencedId, string format, params object[] args);

		void FatalNullArgument(global::Kampai.Util.FatalCode code);

		void FatalNoThrow(global::Kampai.Util.FatalCode code);

		void FatalNoThrow(global::Kampai.Util.FatalCode code, int referencedId);

		void FatalNoThrow(global::Kampai.Util.FatalCode code, string format, params object[] args);

		void FatalNoThrow(global::Kampai.Util.FatalCode code, int referencedId, string format, params object[] args);

		void SetAllowedLevel(int level);

		bool IsAllowedLevel(global::Kampai.Util.KampaiLogLevel level);

		void EventStart(string eventName);

		void EventStop(string eventName);

		void LogEventList();
	}
}
