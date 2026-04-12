namespace Kampai.Common.Service.HealthMetrics
{
	public interface ITapEventMetricsService
	{
		int Count { get; }

		void Mark();

		void Clear();
	}
}
