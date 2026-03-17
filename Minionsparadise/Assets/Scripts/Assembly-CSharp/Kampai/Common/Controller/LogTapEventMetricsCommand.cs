namespace Kampai.Common.Controller
{
	public class LogTapEventMetricsCommand : global::strange.extensions.command.impl.Command
	{
		private const int MAX_INTERVALS = 5;

		private const int INTERVAL_LENGTH_SECONDS = 30;

		private const string TIMER_METRICS_RESOURCE = "/rest/healthMetrics/timers";

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LogTapEventMetricsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.ITapEventMetricsService tapEventMetricsService { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			if (global::Kampai.Util.ClientHealthUtil.isHealthMetricsEnabled(configService, userService))
			{
				routineRunner.StartCoroutine(PeriodicSendTapEventMetrics());
			}
		}

		private global::System.Collections.IEnumerator PeriodicSendTapEventMetrics()
		{
			for (int intervalCount = 1; intervalCount <= 5; intervalCount++)
			{
				yield return new global::UnityEngine.WaitForSeconds(30f);
				SendTapEventMetrics(intervalCount);
			}
		}

		private void SendTapEventMetrics(int intervalCount)
		{
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal.AddOnce(SendTapEventMetricsComplete);
			string key = string.Format("AppFlow.Tap.Interval{0}", intervalCount);
			int count = tapEventMetricsService.Count;
			global::System.Collections.Generic.Dictionary<string, float> dictionary = new global::System.Collections.Generic.Dictionary<string, float>();
			dictionary.Add(key, count);
			global::System.Collections.Generic.Dictionary<string, float> timerEvents = dictionary;
			global::Kampai.Common.Controller.ClientTimerMetricsDTO clientTimerMetricsDTO = new global::Kampai.Common.Controller.ClientTimerMetricsDTO();
			clientTimerMetricsDTO.timerEvents = timerEvents;
			global::Kampai.Common.Controller.ClientTimerMetricsDTO entity = clientTimerMetricsDTO;
			downloadService.Perform(requestFactory.Resource(global::Kampai.Util.GameConstants.Server.SERVER_URL + "/rest/healthMetrics/timers").WithContentType("application/json").WithMethod("POST")
				.WithEntity(entity)
				.WithResponseSignal(signal));
		}

		private void SendTapEventMetricsComplete(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Post Tap Event Metrics response: " + response.Code);
			if (response.Success)
			{
				tapEventMetricsService.Clear();
			}
			else
			{
				logger.Warning("SendTapEventMetricsComplete response failed");
			}
		}
	}
}
