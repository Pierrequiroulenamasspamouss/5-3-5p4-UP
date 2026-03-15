namespace Kampai.Common.Service.HealthMetrics
{
	public class TapEventMetricsService : global::Kampai.Common.Service.HealthMetrics.ITapEventMetricsService
	{
		private int count;

		int global::Kampai.Common.Service.HealthMetrics.ITapEventMetricsService.Count
		{
			get
			{
				return count;
			}
		}

		void global::Kampai.Common.Service.HealthMetrics.ITapEventMetricsService.Mark()
		{
			count++;
		}

		void global::Kampai.Common.Service.HealthMetrics.ITapEventMetricsService.Clear()
		{
			count = 0;
		}
	}
}
