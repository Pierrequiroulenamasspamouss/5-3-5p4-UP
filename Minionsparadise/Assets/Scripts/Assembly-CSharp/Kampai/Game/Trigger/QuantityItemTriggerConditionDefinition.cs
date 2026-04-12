namespace Kampai.Game.Trigger
{
	public class QuantityItemTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1168;
			}
		}

		public int itemDefId { get; set; }

		public uint quantity { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.QuantityItem;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(itemDefId);
			writer.Write(quantity);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			itemDefId = reader.ReadInt32();
			quantity = reader.ReadUInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "QUANTITY":
				reader.Read();
				quantity = global::System.Convert.ToUInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "ITEMDEFID":
				reader.Read();
				itemDefId = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, ItemDefID: {3}, Quantity: {4}", GetType(), base.conditionOp, type, itemDefId, quantity);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IPlayerService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (instance == null)
			{
				return false;
			}
			uint totalCountByDefinitionId = instance.GetTotalCountByDefinitionId(itemDefId);
			return TestOperator(quantity, totalCountByDefinitionId);
		}
	}
}
