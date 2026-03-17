namespace Kampai.Game.Trigger
{
	public class SessionCountTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1173;
			}
		}

		public int Count { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.SessionCount;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Count);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Count = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "COUNT":
				reader.Read();
				Count = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, Seconds: {3}", GetType(), base.conditionOp, type, Count);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.IPlayerService instance = injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			return TestOperator(Count, (int)instance.GetQuantity(global::Kampai.Game.StaticItem.PLAYER_SESSION_COUNT));
		}
	}
}
