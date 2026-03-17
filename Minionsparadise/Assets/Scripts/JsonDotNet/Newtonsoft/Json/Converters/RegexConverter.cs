namespace Newtonsoft.Json.Converters
{
	public class RegexConverter : global::Newtonsoft.Json.JsonConverter
	{
		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::System.Text.RegularExpressions.Regex regex = (global::System.Text.RegularExpressions.Regex)value;
			global::Newtonsoft.Json.Bson.BsonWriter bsonWriter = writer as global::Newtonsoft.Json.Bson.BsonWriter;
			if (bsonWriter != null)
			{
				WriteBson(bsonWriter, regex);
			}
			else
			{
				WriteJson(writer, regex);
			}
		}

		private bool HasFlag(global::System.Text.RegularExpressions.RegexOptions options, global::System.Text.RegularExpressions.RegexOptions flag)
		{
			return (options & flag) == flag;
		}

		private void WriteBson(global::Newtonsoft.Json.Bson.BsonWriter writer, global::System.Text.RegularExpressions.Regex regex)
		{
			string text = null;
			if (HasFlag(regex.Options, global::System.Text.RegularExpressions.RegexOptions.IgnoreCase))
			{
				text += "i";
			}
			if (HasFlag(regex.Options, global::System.Text.RegularExpressions.RegexOptions.Multiline))
			{
				text += "m";
			}
			if (HasFlag(regex.Options, global::System.Text.RegularExpressions.RegexOptions.Singleline))
			{
				text += "s";
			}
			text += "u";
			if (HasFlag(regex.Options, global::System.Text.RegularExpressions.RegexOptions.ExplicitCapture))
			{
				text += "x";
			}
			writer.WriteRegex(regex.ToString(), text);
		}

		private void WriteJson(global::Newtonsoft.Json.JsonWriter writer, global::System.Text.RegularExpressions.Regex regex)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("Pattern");
			writer.WriteValue(regex.ToString());
			writer.WritePropertyName("Options");
			writer.WriteValue(regex.Options);
			writer.WriteEndObject();
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Bson.BsonReader bsonReader = reader as global::Newtonsoft.Json.Bson.BsonReader;
			if (bsonReader != null)
			{
				return ReadBson(bsonReader);
			}
			return ReadJson(reader);
		}

		private object ReadBson(global::Newtonsoft.Json.Bson.BsonReader reader)
		{
			string text = (string)reader.Value;
			int num = text.LastIndexOf("/");
			string pattern = text.Substring(1, num - 1);
			string text2 = text.Substring(num + 1);
			global::System.Text.RegularExpressions.RegexOptions regexOptions = global::System.Text.RegularExpressions.RegexOptions.None;
			string text3 = text2;
			for (int i = 0; i < text3.Length; i++)
			{
				switch (text3[i])
				{
				case 'i':
					regexOptions |= global::System.Text.RegularExpressions.RegexOptions.IgnoreCase;
					break;
				case 'm':
					regexOptions |= global::System.Text.RegularExpressions.RegexOptions.Multiline;
					break;
				case 's':
					regexOptions |= global::System.Text.RegularExpressions.RegexOptions.Singleline;
					break;
				case 'x':
					regexOptions |= global::System.Text.RegularExpressions.RegexOptions.ExplicitCapture;
					break;
				}
			}
			return new global::System.Text.RegularExpressions.Regex(pattern, regexOptions);
		}

		private global::System.Text.RegularExpressions.Regex ReadJson(global::Newtonsoft.Json.JsonReader reader)
		{
			reader.Read();
			reader.Read();
			string pattern = (string)reader.Value;
			reader.Read();
			reader.Read();
			int options = global::System.Convert.ToInt32(reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
			reader.Read();
			return new global::System.Text.RegularExpressions.Regex(pattern, (global::System.Text.RegularExpressions.RegexOptions)options);
		}

		public override bool CanConvert(global::System.Type objectType)
		{
			return objectType == typeof(global::System.Text.RegularExpressions.Regex);
		}
	}
}
