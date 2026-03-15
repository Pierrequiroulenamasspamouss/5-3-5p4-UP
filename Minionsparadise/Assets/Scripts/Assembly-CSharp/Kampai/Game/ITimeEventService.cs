namespace Kampai.Game
{
	public interface ITimeEventService
	{
		float TimerScale { get; set; }

		bool AddEvent(int instanceId, int startTime, int eventTime, global::strange.extensions.signal.impl.Signal<int> timeEventSignal, global::Kampai.Game.TimeEventType eventType = global::Kampai.Game.TimeEventType.Default);

		void RushEvent(int instanceId);

		void RemoveEvent(int instanceId);

		int GetTimeRemaining(int instanceId);

		int GetEventDuration(int instanceId);

		int CalculateRushCostForTimer(int timerDurationInSecond, global::Kampai.Game.RushActionType rushActionType);

		bool HasEventID(int id);

		void SpeedUpTimers(int amount);

		void StartBuff(global::Kampai.Game.TimeEventType eventType, float buffMultipler, int buffStartTime);

		void StopBuff(global::Kampai.Game.TimeEventType eventType, int buffDuration);
	}
}
