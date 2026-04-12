namespace Newtonsoft.Json.Converters
{
	public abstract class CustomCreationConverter<T> : global::Newtonsoft.Json.JsonConverter
	{
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			throw new global::System.NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			T val = Create(objectType);
			if (val == null)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, val);
			return val;
		}

		public abstract T Create(global::System.Type objectType);

		public override bool CanConvert(global::System.Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}
	}
}
