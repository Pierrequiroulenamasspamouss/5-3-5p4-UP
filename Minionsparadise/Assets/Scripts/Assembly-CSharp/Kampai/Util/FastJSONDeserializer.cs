namespace Kampai.Util
{
	public static class FastJSONDeserializer
	{
		public static T Deserialize<T>(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters = null) where T : global::Kampai.Util.IFastJSONDeserializable, new()
		{
			T result = new T();
			result.Deserialize(reader, converters);
			return result;
		}

		public static T Deserialize<T>(string json, JsonConverters converters = null) where T : global::Kampai.Util.IFastJSONDeserializable, new()
		{
			using (global::System.IO.StringReader reader = new global::System.IO.StringReader(json))
			{
				using (global::Newtonsoft.Json.JsonTextReader reader2 = new global::Newtonsoft.Json.JsonTextReader(reader))
				{
					T result = new T();
					result.Deserialize(reader2, converters);
					return result;
				}
			}
		}

	public static T DeserializeFromFile<T>(string path, JsonConverters converters = null) where T : global::Kampai.Util.IFastJSONDeserializable, new()
	{
#if !UNITY_WEBPLAYER
#if !UNITY_WEBPLAYER
		using (global::System.IO.FileStream stream = global::System.IO.File.OpenRead(path))
#else
		using (global::System.IO.Stream stream = null)
#endif
		{
			using (global::System.IO.StreamReader reader = new global::System.IO.StreamReader(stream))
			{
				using (global::Newtonsoft.Json.JsonTextReader reader2 = new global::Newtonsoft.Json.JsonTextReader(reader))
				{
					T result = new T();
					result.Deserialize(reader2, converters);
					return result;
				}
			}
		}
#else
		return default(T);
#endif
	}
	}
}
