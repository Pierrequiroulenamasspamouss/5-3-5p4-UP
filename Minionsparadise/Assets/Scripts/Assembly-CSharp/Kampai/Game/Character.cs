namespace Kampai.Game
{
	public abstract class Character<T> : global::Kampai.Game.Instance<T>, global::Kampai.Game.Character, global::Kampai.Game.Instance, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable where T : global::Kampai.Game.Definition
	{
		public string Name { get; set; }

		protected Character(T definition)
			: base(definition)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NAME":
				reader.Read();
				Name = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
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
			if (Name != null)
			{
				writer.WritePropertyName("Name");
				writer.WriteValue(Name);
			}
		}
	}
	public interface Character : global::Kampai.Game.Instance, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		string Name { get; set; }
	}
}
