namespace Kampai.Game
{
	public class SpecialEventItem : global::Kampai.Game.Item
	{
		public bool HasEnded { get; set; }

		public SpecialEventItem(global::Kampai.Game.SpecialEventItemDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "HASENDED":
				reader.Read();
				HasEnded = global::System.Convert.ToBoolean(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
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
			writer.WritePropertyName("HasEnded");
			writer.WriteValue(HasEnded);
		}
	}
}
