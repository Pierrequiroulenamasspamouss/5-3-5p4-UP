namespace Kampai.Game
{
	public class MarketplaceRefreshTimer : global::Kampai.Game.Instance<global::Kampai.Game.MarketplaceRefreshTimerDefinition>
	{
		public int UTCStartTime { get; set; }

		public MarketplaceRefreshTimer(global::Kampai.Game.MarketplaceRefreshTimerDefinition def)
			: base(def)
		{
			UTCStartTime = 0;
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "UTCSTARTTIME":
				reader.Read();
				UTCStartTime = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("UTCStartTime");
			writer.WriteValue(UTCStartTime);
		}
	}
}
