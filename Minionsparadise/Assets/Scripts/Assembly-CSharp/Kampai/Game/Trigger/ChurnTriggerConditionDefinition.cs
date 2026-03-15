namespace Kampai.Game.Trigger
{
	public class ChurnTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1153;
			}
		}

		public float Value { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Churn;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Value);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Value = reader.ReadSingle();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "VALUE":
				reader.Read();
				Value = global::System.Convert.ToSingle(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, ChurnValue: {3}", GetType(), base.conditionOp, type, Value);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			float actualValue = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>().Churn();
			return TestOperator(Value, actualValue);
		}
	}
}
