namespace Kampai.Game
{
	public class GamePauseCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GamePauseCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.SavePlayerSignal savePlayerSignal { get; set; }

		[Inject]
		public global::Kampai.Common.LogClientMetricsSignal clientMetricsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReengageNotificationSignal reengageNotificationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ScheduleJobsCompleteNotificationsSignal scheduleJobsCompleteNotificationsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.IClientHealthService clientHealthService { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.Game.CancelBuildingMovementSignal cancelBuildingMovementSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IClientVersion clientVersion { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Splash.IBackgroundDownloadDlcService backgroundDownloadDlcService { get; set; }

		public override void Execute()
		{
			logger.EventStart("GamePauseCommand.Execute");
			global::Kampai.Util.Native.LogInfo(string.Format("AppPause, GamePauseCommand, realtimeSinceStartup: {0}", timeService.RealtimeSinceStartup()));
			playerDurationService.OnAppPause();
			reengageNotificationSignal.Dispatch();
			scheduleJobsCompleteNotificationsSignal.Dispatch();
			currencyService.PauseTransactionsHandling();
			savePlayerSignal.Dispatch(new global::Kampai.Util.Tuple<global::Kampai.Game.SaveLocation, string, bool>(global::Kampai.Game.SaveLocation.REMOTE, string.Empty, true));
			cancelBuildingMovementSignal.Dispatch(false);
			clientVersion.RemoveOverrideVersion();
			StopBackgroundDownloadDlc();
			clientHealthService.MarkTimerEvent("AppFlow.Pause", global::UnityEngine.Time.time);
			clientMetricsSignal.Dispatch(false);
			telemetryService.Send_Telemetry_EVT_USER_DATA_AT_APP_CLOSE();
			logger.EventStop("GamePauseCommand.Execute");
		}

		private void StopBackgroundDownloadDlc()
		{
			if (!backgroundDownloadDlcService.Stopped)
			{
				backgroundDownloadDlcService.Stop();
			}
		}
	}
}
