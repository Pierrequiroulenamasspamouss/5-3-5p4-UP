namespace Kampai.Common
{
	public class LogTelemetryCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Common.TelemetryEvent gameEvent { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			telemetryService.LogGameEvent(gameEvent);
		}
	}
}
