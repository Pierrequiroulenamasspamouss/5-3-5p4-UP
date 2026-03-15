namespace Kampai.Game.Trigger
{
	public class SaleSlotTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1171;
			}
		}

		public int slotCount { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.MarketplaceSaleSlot;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(slotCount);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			slotCount = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SLOTCOUNT":
				reader.Read();
				slotCount = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, SlotCount: {3}", GetType(), base.conditionOp, type, slotCount);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IPlayerService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (instance == null)
			{
				return false;
			}
			global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleSlot> byDefinitionId = instance.GetByDefinitionId<global::Kampai.Game.MarketplaceSaleSlot>(1000008094);
			return CheckUserMarketplaceSlotCount(byDefinitionId);
		}

		public bool CheckUserMarketplaceSlotCount(global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleSlot> slots)
		{
			if (slots == null || slots.Count == 0)
			{
				return false;
			}
			return TestOperator(slotCount, slots.Count);
		}
	}
}
