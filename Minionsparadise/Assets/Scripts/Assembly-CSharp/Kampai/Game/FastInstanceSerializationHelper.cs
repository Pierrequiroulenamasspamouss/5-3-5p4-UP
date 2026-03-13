namespace Kampai.Game
{
	internal static class FastInstanceSerializationHelper
	{
		internal static void SerializeInstanceData(global::Newtonsoft.Json.JsonWriter jsonWriter, global::Kampai.Game.Instance instance)
		{
			jsonWriter.WritePropertyName("ID");
			jsonWriter.WriteValue(instance.ID);
			jsonWriter.WritePropertyName("Definition");
			jsonWriter.WriteValue(instance.Definition.ID);
		}
	}
}
