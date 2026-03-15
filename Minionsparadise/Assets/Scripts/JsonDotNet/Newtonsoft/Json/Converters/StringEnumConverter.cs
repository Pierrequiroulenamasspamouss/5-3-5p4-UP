namespace Newtonsoft.Json.Converters
{
	public class StringEnumConverter : global::Newtonsoft.Json.JsonConverter
	{
		private readonly global::System.Collections.Generic.Dictionary<global::System.Type, global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, string>> _enumMemberNamesPerType = new global::System.Collections.Generic.Dictionary<global::System.Type, global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, string>>();

		public bool CamelCaseText { get; set; }

		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::System.Enum obj = (global::System.Enum)value;
			string text = obj.ToString("G");
			if (char.IsNumber(text[0]) || text[0] == '-')
			{
				writer.WriteValue(value);
				return;
			}
			global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, string> enumNameMap = GetEnumNameMap(obj.GetType());
			string second;
			enumNameMap.TryGetByFirst(text, out second);
			second = second ?? text;
			if (CamelCaseText)
			{
				second = global::Newtonsoft.Json.Utilities.StringUtils.ToCamelCase(second);
			}
			writer.WriteValue(second);
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
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.String)
			{
				global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, string> enumNameMap = GetEnumNameMap(type);
				string first;
				enumNameMap.TryGetBySecond(reader.Value.ToString(), out first);
				first = first ?? reader.Value.ToString();
				return global::System.Enum.Parse(type, first, true);
			}
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Integer)
			{
				return global::Newtonsoft.Json.Utilities.ConvertUtils.ConvertOrCast(reader.Value, global::System.Globalization.CultureInfo.InvariantCulture, type);
			}
			throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected token when parsing enum. Expected String or Integer, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType));
		}

		private global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, string> GetEnumNameMap(global::System.Type t)
		{
			global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, string> value;
			if (!_enumMemberNamesPerType.TryGetValue(t, out value))
			{
				lock (_enumMemberNamesPerType)
				{
					if (_enumMemberNamesPerType.TryGetValue(t, out value))
					{
						return value;
					}
					value = new global::Newtonsoft.Json.Utilities.BidirectionalDictionary<string, string>(global::System.StringComparer.OrdinalIgnoreCase, global::System.StringComparer.OrdinalIgnoreCase);
					global::System.Reflection.FieldInfo[] fields = t.GetFields();
					foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
					{
						string name = fieldInfo.Name;
						string text = global::System.Linq.Enumerable.SingleOrDefault(global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Cast<global::System.Runtime.Serialization.EnumMemberAttribute>(fieldInfo.GetCustomAttributes(typeof(global::System.Runtime.Serialization.EnumMemberAttribute), true)), (global::System.Runtime.Serialization.EnumMemberAttribute a) => a.Value)) ?? fieldInfo.Name;
						string first;
						if (value.TryGetBySecond(text, out first))
						{
							throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Enum name '{0}' already exists on enum '{1}'.", global::System.Globalization.CultureInfo.InvariantCulture, text, t.Name));
						}
						value.Add(name, text);
					}
					_enumMemberNamesPerType[t] = value;
				}
			}
			return value;
		}

		public override bool CanConvert(global::System.Type objectType)
		{
			global::System.Type type = (global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(objectType) ? global::System.Nullable.GetUnderlyingType(objectType) : objectType);
			return type.IsEnum;
		}
	}
}
