namespace Kampai.Common.Service.HealthMetrics
{
	public interface IClientHealthService
	{
		global::System.Collections.Generic.Dictionary<string, int> MeterEvents { get; }

		global::System.Collections.Generic.Dictionary<string, float> TimerEvents { get; }

		void MarkMeterEvent(string eventName);

		void MarkTimerEvent(string eventName, float duration);

		void ClearMeterEvents();

		void ClearTimerEvents();
	}
}
