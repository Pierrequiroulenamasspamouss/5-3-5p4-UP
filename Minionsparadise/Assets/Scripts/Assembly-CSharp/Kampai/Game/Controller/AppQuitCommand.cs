namespace Kampai.Game.Controller
{
	public class AppQuitCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("AppQuitCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.IClientHealthService clientHealthService { get; set; }

		[Inject]
		public global::Kampai.Common.LogClientMetricsSignal logClientMetricsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SavePlayerSignal savePlayerSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			logger.EventStart("AppQuitCommand.Execute");
			savePlayerSignal.Dispatch(new global::Kampai.Util.Tuple<global::Kampai.Game.SaveLocation, string, bool>(global::Kampai.Game.SaveLocation.REMOTE, string.Empty, true));
			telemetryService.Send_Telemetry_EVT_USER_DATA_AT_APP_CLOSE();
			clientHealthService.MarkTimerEvent("AppFlow.Quit", global::UnityEngine.Time.time);
			logClientMetricsSignal.Dispatch(false);
			global::Kampai.Util.MediaClient.Stop();
			logger.EventStop("AppQuitCommand.Execute");
		}
	}
}
