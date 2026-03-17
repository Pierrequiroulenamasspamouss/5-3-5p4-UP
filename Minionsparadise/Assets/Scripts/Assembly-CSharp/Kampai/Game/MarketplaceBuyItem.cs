namespace Kampai.Game
{
	public class MarketplaceBuyItem : global::Kampai.Game.Instance<global::Kampai.Game.MarketplaceItemDefinition>
	{
		public int BuyQuantity { get; set; }

		public int BuyPrice { get; set; }

		public bool BoughtFlag { get; set; }

		public MarketplaceBuyItem(global::Kampai.Game.MarketplaceItemDefinition definition)
			: base(definition)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BUYQUANTITY":
				reader.Read();
				BuyQuantity = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BUYPRICE":
				reader.Read();
				BuyPrice = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BOUGHTFLAG":
				reader.Read();
				BoughtFlag = global::System.Convert.ToBoolean(reader.Value);
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
			writer.WritePropertyName("BuyQuantity");
			writer.WriteValue(BuyQuantity);
			writer.WritePropertyName("BuyPrice");
			writer.WriteValue(BuyPrice);
			writer.WritePropertyName("BoughtFlag");
			writer.WriteValue(BoughtFlag);
		}
	}
}
