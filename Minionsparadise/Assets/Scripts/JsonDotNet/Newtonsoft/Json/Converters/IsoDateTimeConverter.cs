namespace Newtonsoft.Json.Converters
{
	public class IsoDateTimeConverter : global::Newtonsoft.Json.Converters.DateTimeConverterBase
	{
		private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		private global::System.Globalization.DateTimeStyles _dateTimeStyles = global::System.Globalization.DateTimeStyles.RoundtripKind;

		private string _dateTimeFormat;

		private global::System.Globalization.CultureInfo _culture;

		public global::System.Globalization.DateTimeStyles DateTimeStyles
		{
			get
			{
				return _dateTimeStyles;
			}
			set
			{
				_dateTimeStyles = value;
			}
		}

		public string DateTimeFormat
		{
			get
			{
				return _dateTimeFormat ?? string.Empty;
			}
			set
			{
				_dateTimeFormat = global::Newtonsoft.Json.Utilities.StringUtils.NullEmptyString(value);
			}
		}

		public global::System.Globalization.CultureInfo Culture
		{
			get
			{
				return _culture ?? global::System.Globalization.CultureInfo.CurrentCulture;
			}
			set
			{
				_culture = value;
			}
		}

		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			string value2;
			if (value is global::System.DateTime)
			{
				global::System.DateTime dateTime = (global::System.DateTime)value;
				if ((_dateTimeStyles & global::System.Globalization.DateTimeStyles.AdjustToUniversal) == global::System.Globalization.DateTimeStyles.AdjustToUniversal || (_dateTimeStyles & global::System.Globalization.DateTimeStyles.AssumeUniversal) == global::System.Globalization.DateTimeStyles.AssumeUniversal)
				{
					dateTime = dateTime.ToUniversalTime();
				}
				value2 = dateTime.ToString(_dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", Culture);
			}
			else
			{
				if (!(value is global::System.DateTimeOffset))
				{
					throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, global::Newtonsoft.Json.Utilities.ReflectionUtils.GetObjectType(value)));
				}
				global::System.DateTimeOffset dateTimeOffset = (global::System.DateTimeOffset)value;
				if ((_dateTimeStyles & global::System.Globalization.DateTimeStyles.AdjustToUniversal) == global::System.Globalization.DateTimeStyles.AdjustToUniversal || (_dateTimeStyles & global::System.Globalization.DateTimeStyles.AssumeUniversal) == global::System.Globalization.DateTimeStyles.AssumeUniversal)
				{
					dateTimeOffset = dateTimeOffset.ToUniversalTime();
				}
				value2 = dateTimeOffset.ToString(_dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", Culture);
			}
			writer.WriteValue(value2);
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			bool flag = global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(objectType);
			global::System.Type type = (flag ? global::System.Nullable.GetUnderlyingType(objectType) : objectType);
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				if (!global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(objectType))
				{
					throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot convert null value to {0}.", global::System.Globalization.CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.String)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected token parsing date. Expected String, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType));
			}
			string text = reader.Value.ToString();
			if (string.IsNullOrEmpty(text) && flag)
			{
				return null;
			}
			if (type == typeof(global::System.DateTimeOffset))
			{
				if (!string.IsNullOrEmpty(_dateTimeFormat))
				{
					return global::System.DateTimeOffset.ParseExact(text, _dateTimeFormat, Culture, _dateTimeStyles);
				}
				return global::System.DateTimeOffset.Parse(text, Culture, _dateTimeStyles);
			}
			if (!string.IsNullOrEmpty(_dateTimeFormat))
			{
				return global::System.DateTime.ParseExact(text, _dateTimeFormat, Culture, _dateTimeStyles);
			}
			return global::System.DateTime.Parse(text, Culture, _dateTimeStyles);
		}
	}
}
