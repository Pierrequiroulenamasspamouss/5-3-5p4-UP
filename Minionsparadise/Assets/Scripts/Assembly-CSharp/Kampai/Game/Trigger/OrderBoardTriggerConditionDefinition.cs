namespace Kampai.Game.Trigger
{
	public class OrderBoardTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1161;
			}
		}

		public int duration { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.OrderBoard;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(duration);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			duration = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "DURATION":
				reader.Read();
				duration = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IOrderBoardService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IOrderBoardService>();
			if (instance == null)
			{
				return false;
			}
			int longestIdleOrderDuration = instance.GetLongestIdleOrderDuration();
			return TestOperator(duration, longestIdleOrderDuration);
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, duration: {3}", GetType(), base.conditionOp, type, duration);
		}

		public override global::Kampai.Game.Transaction.TransactionDefinition GetDynamicTriggerTransaction(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IOrderBoardService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IOrderBoardService>();
			if (instance == null)
			{
				return null;
			}
			return instance.GetLongestIdleOrderTransaction();
		}
	}
}
