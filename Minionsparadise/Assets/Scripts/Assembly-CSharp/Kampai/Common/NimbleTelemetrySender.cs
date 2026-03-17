namespace Kampai.Common
{
	public class NimbleTelemetrySender : global::Kampai.Common.ITelemetrySender
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("NimbleTelemetrySender") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		public NimbleTelemetrySender()
		{
			NimbleBridge_Log.GetComponent();
		}

		public virtual void COPPACompliance()
		{
			string text = null;
			global::System.DateTime birthdate;
			if (!coppaService.GetBirthdate(out birthdate))
			{
				birthdate = global::System.DateTime.Now;
			}
			text = DateAsString(birthdate.Year, birthdate.Month);
			NimbleBridge_Tracking.GetComponent().AddCustomSessionValue("ageGateDob", text);
		}

		public void SharingUsage(bool enabled)
		{
			logger.Info("=======================================================================================================================================================================================");
			logger.Info("=======================================================================================================================================================================================");
			logger.Info("#                                                                                                                                                                                     #");
			logger.Info("#                  setting Nimble sharingusage to {0}                                                                                                                             #", enabled);
			logger.Info("#                                                                                                                                                                                     #");
			logger.Info("=======================================================================================================================================================================================");
			global::System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				NimbleBridge_Tracking.GetComponent().SetEnabled(enabled);
			}, null);
		}

		protected string DateAsString(int year, int month)
		{
			return year + "-" + ((month >= 10) ? month.ToString() : ("0" + month));
		}

		public virtual void SendEvent(global::Kampai.Common.TelemetryEvent gameEvent)
		{
			NimbleBridge_Tracking.GetComponent().LogEvent("SYNERGYTRACKING::CUSTOM", getNimbleParameters(gameEvent));
		}

		public static global::System.Collections.Generic.Dictionary<string, string> getNimbleParameters(global::Kampai.Common.TelemetryEvent telemetryEvent)
		{
			global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
			dictionary.Add("eventType", ((int)telemetryEvent.Type).ToString());
			for (int i = 0; i < telemetryEvent.Parameters.Count; i++)
			{
				global::Kampai.Common.TelemetryParameter telemetryParameter = telemetryEvent.Parameters[i];
				if (telemetryParameter.keyType.Length > 0)
				{
					dictionary.Add("keyType" + (i + 1).ToString().PadLeft(2, '0'), ((int)global::System.Enum.Parse(typeof(SynergyTrackingEventKey), telemetryParameter.keyType)).ToString());
					dictionary.Add("keyValue" + (i + 1).ToString().PadLeft(2, '0'), (telemetryParameter.value != null) ? telemetryParameter.value.ToString() : string.Empty);
				}
			}
			return dictionary;
		}
	}
}
