namespace Kampai.Common
{
	public interface ITelemetrySender
	{
		void SendEvent(global::Kampai.Common.TelemetryEvent gameEvent);

		void COPPACompliance();

		void SharingUsage(bool enabled);
	}
}
