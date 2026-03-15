namespace Kampai.Game.Trigger
{
	public class DaysSinceInstallTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1156;
			}
		}

		public int Days { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.DaysSinceInstall;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Days);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Days = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "DAYS":
				reader.Read();
				Days = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, Seconds: {3}", GetType(), base.conditionOp, type, Days);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.IPlayerService instance = injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			global::Kampai.Game.ITimeService instance2 = injectionBinder.GetInstance<global::Kampai.Game.ITimeService>();
			int actualValue = (instance2.CurrentTime() - instance.FirstGameStartUTC) / 86400;
			return TestOperator(Days, actualValue);
		}
	}
}
