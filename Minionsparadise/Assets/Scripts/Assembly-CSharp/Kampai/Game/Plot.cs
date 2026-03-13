namespace Kampai.Game
{
	public abstract class Plot<T> : global::Kampai.Game.Instance<T>, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Game.Plot, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable where T : global::Kampai.Game.PlotDefinition
	{
		global::Kampai.Game.PlotDefinition global::Kampai.Game.Plot.Definition
		{
			get
			{
				return base.Definition;
			}
		}

		public global::Kampai.Game.Location Location { get; set; }

		public Plot(T definition)
			: base(definition)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "LOCATION":
				reader.Read();
				Location = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
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
			if (Location != null)
			{
				writer.WritePropertyName("Location");
				writer.WriteStartObject();
				writer.WritePropertyName("x");
				writer.WriteValue(Location.x);
				writer.WritePropertyName("y");
				writer.WriteValue(Location.y);
				writer.WriteEndObject();
			}
		}
	}
	public interface Plot : global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		new global::Kampai.Game.PlotDefinition Definition { get; }
	}
}
