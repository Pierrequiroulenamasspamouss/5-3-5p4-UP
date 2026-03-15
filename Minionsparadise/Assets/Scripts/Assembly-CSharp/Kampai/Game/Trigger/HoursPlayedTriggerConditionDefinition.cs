namespace Kampai.Game.Trigger
{
	public class HoursPlayedTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1158;
			}
		}

		public float Hours { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.HoursPlayed;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Hours);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Hours = reader.ReadSingle();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "HOURS":
				reader.Read();
				Hours = global::System.Convert.ToSingle(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, Seconds: {3}", GetType(), base.conditionOp, type, Hours);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.IPlayerDurationService instance = injectionBinder.GetInstance<global::Kampai.Game.IPlayerDurationService>();
			return TestOperator(Hours, (float)instance.TotalGamePlaySeconds / 3600f);
		}
	}
}
