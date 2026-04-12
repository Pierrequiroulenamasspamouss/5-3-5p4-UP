namespace Kampai.Game
{
	public class MarketplaceSaleSlot : global::Kampai.Game.Instance<global::Kampai.Game.MarketplaceSaleSlotDefinition>, global::System.IComparable<global::Kampai.Game.MarketplaceSaleSlot>
	{
		public enum State
		{
			LOCKED = 0,
			UNLOCKED = 1
		}

		public global::Kampai.Game.MarketplaceSaleSlot.State state { get; set; }

		public int itemId { get; set; }

		public int premiumCost { get; set; }

		public MarketplaceSaleSlot(global::Kampai.Game.MarketplaceSaleSlotDefinition definition)
			: base(definition)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STATE":
				reader.Read();
				state = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.MarketplaceSaleSlot.State>(reader);
				break;
			case "ITEMID":
				reader.Read();
				itemId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREMIUMCOST":
				reader.Read();
				premiumCost = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			writer.WritePropertyName("state");
			writer.WriteValue((int)state);
			writer.WritePropertyName("itemId");
			writer.WriteValue(itemId);
			writer.WritePropertyName("premiumCost");
			writer.WriteValue(premiumCost);
		}

		public int CompareTo(global::Kampai.Game.MarketplaceSaleSlot other)
		{
			if (other == null)
			{
				return -1;
			}
			return other.state - state;
		}
	}
}
