namespace Kampai.Common
{
	public interface ISwrveService : global::Kampai.Common.IIapTelemetryService, global::Kampai.Common.ITelemetrySender
	{
		void UpdateResources();

		void SendUserStatsUpdate();
	}
}
