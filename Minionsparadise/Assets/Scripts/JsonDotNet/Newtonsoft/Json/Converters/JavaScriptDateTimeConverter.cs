namespace Newtonsoft.Json.Converters
{
	public class JavaScriptDateTimeConverter : global::Newtonsoft.Json.Converters.DateTimeConverterBase
	{
		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			long value2;
			if (value is global::System.DateTime)
			{
				global::System.DateTime dateTime = ((global::System.DateTime)value).ToUniversalTime();
				value2 = global::Newtonsoft.Json.JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTime);
			}
			else
			{
				if (!(value is global::System.DateTimeOffset))
				{
					throw new global::System.Exception("Expected date object value.");
				}
				value2 = global::Newtonsoft.Json.JsonConvert.ConvertDateTimeToJavaScriptTicks(((global::System.DateTimeOffset)value).ToUniversalTime().UtcDateTime);
			}
			writer.WriteStartConstructor("Date");
			writer.WriteValue(value2);
			writer.WriteEndConstructor();
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::System.Type type = (global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(objectType) ? global::System.Nullable.GetUnderlyingType(objectType) : objectType);
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				if (!global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(objectType))
				{
					throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot convert null value to {0}.", global::System.Globalization.CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.StartConstructor || string.Compare(reader.Value.ToString(), "Date", global::System.StringComparison.Ordinal) != 0)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected token or value when parsing date. Token: {0}, Value: {1}", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType, reader.Value));
			}
			reader.Read();
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.Integer)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected token parsing date. Expected Integer, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType));
			}
			long javaScriptTicks = (long)reader.Value;
			global::System.DateTime dateTime = global::Newtonsoft.Json.JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			reader.Read();
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.EndConstructor)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected token parsing date. Expected EndConstructor, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType));
			}
			if (type == typeof(global::System.DateTimeOffset))
			{
				return new global::System.DateTimeOffset(dateTime);
			}
			return dateTime;
		}
	}
}
