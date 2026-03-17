namespace Kampai.Game.Trigger
{
	[global::Kampai.Util.RequiresJsonConverter]
	public abstract class TriggerConditionDefinition : global::Kampai.Game.Definition, global::Kampai.Game.Trigger.IIsTriggerable
	{
		public override int TypeCode
		{
			get
			{
				return 1020;
			}
		}

		public global::Kampai.Game.Trigger.TriggerConditionOperator conditionOp { get; set; }

		public abstract global::Kampai.Game.Trigger.TriggerConditionType.Identifier type { get; }

		public TriggerConditionDefinition()
		{
			conditionOp = global::Kampai.Game.Trigger.TriggerConditionOperator.Invalid;
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, conditionOp);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			conditionOp = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.Trigger.TriggerConditionOperator>(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "CONDITIONOP":
				reader.Read();
				conditionOp = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.Trigger.TriggerConditionOperator>(reader);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public abstract bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext);

		public virtual global::Kampai.Game.Transaction.TransactionDefinition GetDynamicTriggerTransaction(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			return null;
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}", GetType(), conditionOp, type);
		}

		protected bool TestOperator<T>(T testingValue, T actualValue) where T : global::System.IComparable<T>
		{
			bool result = false;
			switch (conditionOp)
			{
			case global::Kampai.Game.Trigger.TriggerConditionOperator.Equal:
				result = actualValue.CompareTo(testingValue) == 0;
				break;
			case global::Kampai.Game.Trigger.TriggerConditionOperator.NotEqual:
				result = actualValue.CompareTo(testingValue) != 0;
				break;
			case global::Kampai.Game.Trigger.TriggerConditionOperator.Greater:
				result = actualValue.CompareTo(testingValue) > 0;
				break;
			case global::Kampai.Game.Trigger.TriggerConditionOperator.Less:
				result = actualValue.CompareTo(testingValue) < 0;
				break;
			case global::Kampai.Game.Trigger.TriggerConditionOperator.LessEqual:
				result = actualValue.CompareTo(testingValue) <= 0;
				break;
			case global::Kampai.Game.Trigger.TriggerConditionOperator.GreaterEqual:
				result = actualValue.CompareTo(testingValue) >= 0;
				break;
			}
			return result;
		}
	}
}
