namespace Kampai.Game
{
	public class Sticker : global::Kampai.Game.Instance<global::Kampai.Game.StickerDefinition>
	{
		public int UTCTimeEarned { get; set; }

		public bool isNew { get; set; }

		public Sticker(global::Kampai.Game.StickerDefinition def)
			: base(def)
		{
			isNew = true;
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ISNEW":
				reader.Read();
				isNew = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "UTCTIMEEARNED":
				reader.Read();
				UTCTimeEarned = global::System.Convert.ToInt32(reader.Value);
				break;
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
			writer.WritePropertyName("UTCTimeEarned");
			writer.WriteValue(UTCTimeEarned);
			writer.WritePropertyName("isNew");
			writer.WriteValue(isNew);
		}
	}
}
