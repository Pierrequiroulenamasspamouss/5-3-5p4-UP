namespace Kampai.Game
{
	public class QuestScriptInstance : global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable
	{
		[global::Newtonsoft.Json.JsonIgnore]
		public int QuestID { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public int QuestStepID { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public string QuestLocalizedKey { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public string Key { get; set; }

		public object Deserialize(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			reader.Skip();
			return null;
		}

		public void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			writer.WriteEndObject();
		}
	}
}
