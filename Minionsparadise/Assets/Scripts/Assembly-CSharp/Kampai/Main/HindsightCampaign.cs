namespace Kampai.Main
{
	public class HindsightCampaign : global::Kampai.Game.Instance<global::Kampai.Main.HindsightCampaignDefinition>
	{
		public enum Scope
		{
			unknown = 0,
			game_launch = 1,
			message_in_a_bottle = 2
		}

		public enum DismissType
		{
			ACCEPTED = 0,
			DECLINED = 1
		}

		public enum Platform
		{
			all = 0,
			ios = 1,
			android = 2
		}

		public int ViewCount { get; set; }

		public HindsightCampaign(global::Kampai.Main.HindsightCampaignDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "VIEWCOUNT":
				reader.Read();
				ViewCount = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("ViewCount");
			writer.WriteValue(ViewCount);
		}
	}
}
