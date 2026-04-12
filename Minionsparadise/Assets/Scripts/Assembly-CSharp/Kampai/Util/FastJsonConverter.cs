namespace Kampai.Util
{
	public interface FastJsonConverter<T> where T : class, global::Kampai.Util.IFastJSONDeserializable
	{
		T ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters);

		T Create();
	}
}
