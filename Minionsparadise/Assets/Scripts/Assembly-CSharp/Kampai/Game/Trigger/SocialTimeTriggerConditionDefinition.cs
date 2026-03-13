namespace Kampai.Game.Trigger
{
	public class SocialTimeTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1175;
			}
		}

		public int Seconds { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.SocialTime;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Seconds);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Seconds = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SECONDS":
				reader.Read();
				Seconds = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, Seconds: {3}", GetType(), base.conditionOp, type, Seconds);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.ITimedSocialEventService instance = injectionBinder.GetInstance<global::Kampai.Game.ITimedSocialEventService>();
			global::Kampai.Util.IKampaiLogger kampaiLogger = global::Elevation.Logging.LogManager.GetClassLogger("SocialTimeTriggerConditionDefinition") as global::Kampai.Util.IKampaiLogger;
			global::Kampai.Game.ITimeService instance2 = injectionBinder.GetInstance<global::Kampai.Game.ITimeService>();
			global::Kampai.Game.TimedSocialEventDefinition currentSocialEvent = instance.GetCurrentSocialEvent();
			if (currentSocialEvent == null)
			{
				kampaiLogger.Info("No social order available.");
				return false;
			}
			long num = instance.GetCurrentSocialEvent().FinishTime;
			global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0, global::System.DateTimeKind.Utc).AddSeconds(num).ToLocalTime();
			global::System.DateTime dateTime2 = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0, global::System.DateTimeKind.Utc).AddSeconds(instance2.CurrentTime()).ToLocalTime();
			global::System.TimeSpan timeSpan = dateTime.Subtract(dateTime2);
			if (dateTime2 >= dateTime)
			{
				kampaiLogger.Info("Event is already over.");
				return false;
			}
			return TestOperator(Seconds, timeSpan.TotalSeconds);
		}
	}
}
