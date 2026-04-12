namespace Kampai.Util
{
	public interface IFastJSONDeserializable
	{
		object Deserialize(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters = null);
	}
}
