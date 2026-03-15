namespace Kampai.Game
{
	public class MarketplaceSaleItem : global::Kampai.Game.Instance<global::Kampai.Game.MarketplaceItemDefinition>, global::System.IComparable<global::Kampai.Game.MarketplaceSaleItem>
	{
		public enum State
		{
			PENDING = 0,
			SOLD = 1
		}

		public global::Kampai.Game.MarketplaceSaleItem.State state { get; set; }

		public int QuantitySold { get; set; }

		public int SalePrice { get; set; }

		public int SaleStartTime { get; set; }

		public int LengthOfSale { get; set; }

		public MarketplaceSaleItem(global::Kampai.Game.MarketplaceItemDefinition definition)
			: base(definition)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STATE":
				reader.Read();
				state = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.MarketplaceSaleItem.State>(reader);
				break;
			case "QUANTITYSOLD":
				reader.Read();
				QuantitySold = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SALEPRICE":
				reader.Read();
				SalePrice = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SALESTARTTIME":
				reader.Read();
				SaleStartTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LENGTHOFSALE":
				reader.Read();
				LengthOfSale = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("QuantitySold");
			writer.WriteValue(QuantitySold);
			writer.WritePropertyName("SalePrice");
			writer.WriteValue(SalePrice);
			writer.WritePropertyName("SaleStartTime");
			writer.WriteValue(SaleStartTime);
			writer.WritePropertyName("LengthOfSale");
			writer.WriteValue(LengthOfSale);
		}

		public int CompareTo(global::Kampai.Game.MarketplaceSaleItem other)
		{
			if (other == null)
			{
				return -1;
			}
			return other.state - state;
		}
	}
}
