namespace Kampai.Game
{
	public class Achievement : global::Kampai.Game.Instance<global::Kampai.Game.AchievementDefinition>
	{
		public int Progress { get; set; }

		public Achievement(global::Kampai.Game.AchievementDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PROGRESS":
				reader.Read();
				Progress = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("Progress");
			writer.WriteValue(Progress);
		}
	}
}
