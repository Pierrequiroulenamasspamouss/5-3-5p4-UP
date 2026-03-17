namespace Kampai.Game
{
	public class Sale : global::Kampai.Game.Instance<global::Kampai.Game.SalePackDefinition>
	{
		public bool isDynamicSaleDefinition { get; set; }

		public int UTCUserStartTime { get; set; }

		public int Impressions { get; set; }

		public int UTCLastImpressionTime { get; set; }

		public bool Purchased { get; set; }

		public bool Started { get; set; }

		public bool Finished { get; set; }

		public bool Viewed { get; set; }

		public Sale(global::Kampai.Game.SalePackDefinition def)
			: base(def)
		{
			if (def.Type == global::Kampai.Game.SalePackType.Upsell)
			{
				isDynamicSaleDefinition = true;
			}
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ISDYNAMICSALEDEFINITION":
				reader.Read();
				isDynamicSaleDefinition = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "UTCUSERSTARTTIME":
				reader.Read();
				UTCUserStartTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "IMPRESSIONS":
				reader.Read();
				Impressions = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCLASTIMPRESSIONTIME":
				reader.Read();
				UTCLastImpressionTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PURCHASED":
				reader.Read();
				Purchased = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "STARTED":
				reader.Read();
				Started = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "FINISHED":
				reader.Read();
				Finished = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "VIEWED":
				reader.Read();
				Viewed = global::System.Convert.ToBoolean(reader.Value);
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
			writer.WritePropertyName("ID");
			writer.WriteValue(ID);
			writer.WritePropertyName("Definition");
			if (base.Definition.Type == global::Kampai.Game.SalePackType.Upsell)
			{
				base.Definition.Serialize(writer);
			}
			else
			{
				writer.WriteValue(base.Definition.ID);
			}
			writer.WritePropertyName("isDynamicSaleDefinition");
			writer.WriteValue(isDynamicSaleDefinition);
			writer.WritePropertyName("UTCUserStartTime");
			writer.WriteValue(UTCUserStartTime);
			writer.WritePropertyName("Impressions");
			writer.WriteValue(Impressions);
			writer.WritePropertyName("UTCLastImpressionTime");
			writer.WriteValue(UTCLastImpressionTime);
			writer.WritePropertyName("Purchased");
			writer.WriteValue(Purchased);
			writer.WritePropertyName("Started");
			writer.WriteValue(Started);
			writer.WritePropertyName("Finished");
			writer.WriteValue(Finished);
			writer.WritePropertyName("Viewed");
			writer.WriteValue(Viewed);
		}
	}
}
