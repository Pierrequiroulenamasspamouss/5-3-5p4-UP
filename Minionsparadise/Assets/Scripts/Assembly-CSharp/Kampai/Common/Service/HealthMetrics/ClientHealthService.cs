namespace Kampai.Common.Service.HealthMetrics
{
	public class ClientHealthService : global::Kampai.Common.Service.HealthMetrics.IClientHealthService
	{
		private static global::System.Collections.Generic.Dictionary<string, int> meterEvents = new global::System.Collections.Generic.Dictionary<string, int>();

		private static global::System.Collections.Generic.Dictionary<string, float> timerEvents = new global::System.Collections.Generic.Dictionary<string, float>();

		global::System.Collections.Generic.Dictionary<string, int> global::Kampai.Common.Service.HealthMetrics.IClientHealthService.MeterEvents
		{
			get
			{
				return meterEvents;
			}
		}

		global::System.Collections.Generic.Dictionary<string, float> global::Kampai.Common.Service.HealthMetrics.IClientHealthService.TimerEvents
		{
			get
			{
				return timerEvents;
			}
		}

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		void global::Kampai.Common.Service.HealthMetrics.IClientHealthService.MarkMeterEvent(string eventName)
		{
			if (global::Kampai.Util.ClientHealthUtil.isHealthMetricsEnabled(configurationsService, userSessionService))
			{
				int num = 0;
				if (meterEvents.ContainsKey(eventName))
				{
					num = meterEvents[eventName];
				}
				num++;
				meterEvents[eventName] = num;
			}
		}

		void global::Kampai.Common.Service.HealthMetrics.IClientHealthService.MarkTimerEvent(string eventName, float duration)
		{
			if (global::Kampai.Util.ClientHealthUtil.isHealthMetricsEnabled(configurationsService, userSessionService))
			{
				if (timerEvents.ContainsKey(eventName))
				{
					timerEvents[eventName] = duration;
				}
				else
				{
					timerEvents.Add(eventName, duration);
				}
			}
		}

		void global::Kampai.Common.Service.HealthMetrics.IClientHealthService.ClearMeterEvents()
		{
			meterEvents.Clear();
		}

		void global::Kampai.Common.Service.HealthMetrics.IClientHealthService.ClearTimerEvents()
		{
			timerEvents.Clear();
		}
	}
}
