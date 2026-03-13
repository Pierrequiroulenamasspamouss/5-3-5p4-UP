namespace Kampai.Common
{
	public class TelemetryEvent
	{
		public SynergyTrackingEventType Type { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Common.TelemetryParameter> Parameters { get; set; }

		public TelemetryEvent(SynergyTrackingEventType type)
		{
			Type = type;
		}
	}
}
