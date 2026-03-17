namespace Newtonsoft.Json.Converters
{
	public class BsonObjectIdConverter : global::Newtonsoft.Json.JsonConverter
	{
		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Bson.BsonObjectId bsonObjectId = (global::Newtonsoft.Json.Bson.BsonObjectId)value;
			global::Newtonsoft.Json.Bson.BsonWriter bsonWriter = writer as global::Newtonsoft.Json.Bson.BsonWriter;
			if (bsonWriter != null)
			{
				bsonWriter.WriteObjectId(bsonObjectId.Value);
			}
			else
			{
				writer.WriteValue(bsonObjectId.Value);
			}
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.Bytes)
			{
				throw new global::Newtonsoft.Json.JsonSerializationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Expected Bytes but got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType));
			}
			byte[] value = (byte[])reader.Value;
			return new global::Newtonsoft.Json.Bson.BsonObjectId(value);
		}

		public override bool CanConvert(global::System.Type objectType)
		{
			return objectType == typeof(global::Newtonsoft.Json.Bson.BsonObjectId);
		}
	}
}
