namespace Kampai.Game
{
	public class NimbleOTCommand : global::strange.extensions.command.impl.Command
	{
		private const string NIMBLE_OT_RESOURCE = "/rest/healthMetrics/nimbleOT";

		private static int NIMBLE_POLL_INTERVAL = 120;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("NimbleOTCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject("game.server.host")]
		public string ServerUrl { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			routineRunner.StartCoroutine(PeriodicReloadConfigs());
		}

		private global::System.Collections.IEnumerator PeriodicReloadConfigs()
		{
			if (global::Kampai.Util.ClientHealthUtil.isHealthMetricsEnabled(configurationsService, userSessionService))
			{
				while (true)
				{
					pollNimbleOT();
					yield return new global::UnityEngine.WaitForSeconds(NIMBLE_POLL_INTERVAL);
				}
			}
			logger.Warning("Disabling operational telemetry due to configurations");
		}

		private void pollNimbleOT()
		{
			NimbleBridge_OperationalTelemetryEvent[] events = NimbleBridge_OperationalTelemetryDispatch.GetComponent().GetEvents("com.ea.nimble.network");
			string text = "{\"events\":[";
			int num = 0;
			NimbleBridge_OperationalTelemetryEvent[] array = events;
			foreach (NimbleBridge_OperationalTelemetryEvent nimbleBridge_OperationalTelemetryEvent in array)
			{
				text += nimbleBridge_OperationalTelemetryEvent.GetEventDictionary();
				if (num < events.Length - 1)
				{
					text += ",";
				}
				num++;
				nimbleBridge_OperationalTelemetryEvent.Dispose();
			}
			events = null;
			text += "]}";
			if (num > 0)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Sending " + num + " Nimble OT events");
				global::System.Text.UTF8Encoding uTF8Encoding = new global::System.Text.UTF8Encoding(false);
				byte[] bytes = uTF8Encoding.GetBytes(text.ToCharArray());
				downloadService.Perform(requestFactory.Resource(ServerUrl + "/rest/healthMetrics/nimbleOT").WithContentType("application/json").WithMethod("POST")
					.WithBody(bytes));
			}
		}
	}
}
