namespace Kampai.Util
{
	public abstract class FastJsonCreationConverter<T> : global::Kampai.Util.FastJsonConverter<T> where T : class, global::Kampai.Util.IFastJSONDeserializable
	{
		public virtual T ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return (T)null;
			}
			T val = Create();
			if (val == null)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException("No object created.");
			}
			val.Deserialize(reader, converters);
			return val;
		}

		public abstract T Create();
	}
}
