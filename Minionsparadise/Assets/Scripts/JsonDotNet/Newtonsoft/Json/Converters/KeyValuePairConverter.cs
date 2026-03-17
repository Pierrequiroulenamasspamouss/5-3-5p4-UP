namespace Newtonsoft.Json.Converters
{
	public class KeyValuePairConverter : global::Newtonsoft.Json.JsonConverter
	{
		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::System.Type type = value.GetType();
			global::System.Reflection.PropertyInfo property = type.GetProperty("Key");
			global::System.Reflection.PropertyInfo property2 = type.GetProperty("Value");
			writer.WriteStartObject();
			writer.WritePropertyName("Key");
			serializer.Serialize(writer, global::Newtonsoft.Json.Utilities.ReflectionUtils.GetMemberValue(property, value));
			writer.WritePropertyName("Value");
			serializer.Serialize(writer, global::Newtonsoft.Json.Utilities.ReflectionUtils.GetMemberValue(property2, value));
			writer.WriteEndObject();
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::System.Collections.Generic.IList<global::System.Type> genericArguments = objectType.GetGenericArguments();
			global::System.Type objectType2 = genericArguments[0];
			global::System.Type objectType3 = genericArguments[1];
			reader.Read();
			reader.Read();
			object obj = serializer.Deserialize(reader, objectType2);
			reader.Read();
			reader.Read();
			object obj2 = serializer.Deserialize(reader, objectType3);
			reader.Read();
			return global::Newtonsoft.Json.Utilities.ReflectionUtils.CreateInstance(objectType, obj, obj2);
		}

		public override bool CanConvert(global::System.Type objectType)
		{
			if (objectType.IsValueType && objectType.IsGenericType)
			{
				return objectType.GetGenericTypeDefinition() == typeof(global::System.Collections.Generic.KeyValuePair<, >);
			}
			return false;
		}
	}
}
