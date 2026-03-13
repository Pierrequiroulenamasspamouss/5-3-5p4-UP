namespace Prime31
{
	public class SimpleJson
	{
		private const int TOKEN_NONE = 0;

		private const int TOKEN_CURLY_OPEN = 1;

		private const int TOKEN_CURLY_CLOSE = 2;

		private const int TOKEN_SQUARED_OPEN = 3;

		private const int TOKEN_SQUARED_CLOSE = 4;

		private const int TOKEN_COLON = 5;

		private const int TOKEN_COMMA = 6;

		private const int TOKEN_STRING = 7;

		private const int TOKEN_NUMBER = 8;

		private const int TOKEN_TRUE = 9;

		private const int TOKEN_FALSE = 10;

		private const int TOKEN_NULL = 11;

		private const int BUILDER_CAPACITY = 2000;

		private static global::Prime31.IJsonSerializerStrategy _currentJsonSerializerStrategy;

		private static global::Prime31.PocoJsonSerializerStrategy _pocoJsonSerializerStrategy;

		public static global::Prime31.IJsonSerializerStrategy currentJsonSerializerStrategy
		{
			get
			{
				return _currentJsonSerializerStrategy ?? (_currentJsonSerializerStrategy = pocoJsonSerializerStrategy);
			}
			set
			{
				_currentJsonSerializerStrategy = value;
			}
		}

		public static global::Prime31.PocoJsonSerializerStrategy pocoJsonSerializerStrategy
		{
			get
			{
				return _pocoJsonSerializerStrategy ?? (_pocoJsonSerializerStrategy = new global::Prime31.PocoJsonSerializerStrategy());
			}
		}

		public static string encode(object obj)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder(2000);
			return (!serializeValue(currentJsonSerializerStrategy, obj, stringBuilder)) ? null : stringBuilder.ToString();
		}

		public static bool tryDeserializeObject(string json, out object obj)
		{
			bool success = true;
			if (json != null)
			{
				char[] json2 = json.ToCharArray();
				int index = 0;
				obj = parseValue(json2, ref index, ref success);
			}
			else
			{
				obj = null;
			}
			return success;
		}

		public static object decode(string json)
		{
			object obj;
			if (tryDeserializeObject(json, out obj))
			{
				return obj;
			}
			global::Prime31.Utils.logObject("Something went wrong deserializing the json. We got a null return. Here is the json we tried to deserialize: " + json);
			return null;
		}

		private static object decode(string json, global::System.Type type)
		{
			return decode(json, type, null);
		}

		public static T decode<T>(string json, string rootElement = null)
		{
			return (T)decode(json, typeof(T), rootElement);
		}

		private static object decode(string json, global::System.Type type, string rootElement = null)
		{
			object obj = decode(json);
			if (type == null || (obj != null && obj.GetType().IsAssignableFrom(type)))
			{
				return obj;
			}
			if (rootElement != null)
			{
				if (obj is global::Prime31.JsonObject)
				{
					global::Prime31.JsonObject jsonObject = obj as global::Prime31.JsonObject;
					if (jsonObject.ContainsKey(rootElement))
					{
						obj = jsonObject[rootElement];
					}
					else
					{
						global::Prime31.Utils.logObject(string.Format("A rootElement was requested ({0})  but does not exist in the decoded Dictionary", rootElement));
					}
				}
				else
				{
					global::Prime31.Utils.logObject(string.Format("A rootElement was requested ({0}) but the decoded object is not a Dictionary. It is a {1}", rootElement, obj.GetType()));
				}
			}
			return currentJsonSerializerStrategy.deserializeObject(obj, type);
		}

		public static T decodeObject<T>(object jsonObject, string rootElement = null)
		{
			global::System.Type typeFromHandle = typeof(T);
			if (typeFromHandle == null || (jsonObject != null && jsonObject.GetType().IsAssignableFrom(typeFromHandle)))
			{
				return (T)jsonObject;
			}
			if (rootElement != null)
			{
				if (jsonObject is global::System.Collections.Generic.Dictionary<string, object>)
				{
					global::System.Collections.Generic.Dictionary<string, object> dictionary = jsonObject as global::System.Collections.Generic.Dictionary<string, object>;
					if (dictionary.ContainsKey(rootElement))
					{
						jsonObject = dictionary[rootElement];
					}
					else
					{
						global::Prime31.Utils.logObject(string.Format("A rootElement was requested ({0})  but does not exist in the decoded Dictionary", rootElement));
					}
				}
				else
				{
					global::Prime31.Utils.logObject(string.Format("A rootElement was requested ({0}) but the decoded object is not a Dictionary. It is a {1}", rootElement, jsonObject.GetType()));
				}
			}
			return (T)currentJsonSerializerStrategy.deserializeObject(jsonObject, typeFromHandle);
		}

		protected static global::System.Collections.Generic.IDictionary<string, object> parseObject(char[] json, ref int index, ref bool success)
		{
			global::System.Collections.Generic.IDictionary<string, object> dictionary = new global::Prime31.JsonObject();
			nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				switch (lookAhead(json, index))
				{
				case 0:
					success = false;
					return null;
				case 6:
					nextToken(json, ref index);
					continue;
				case 2:
					nextToken(json, ref index);
					return dictionary;
				}
				string key = parseString(json, ref index, ref success);
				if (!success)
				{
					success = false;
					return null;
				}
				int num = nextToken(json, ref index);
				if (num != 5)
				{
					success = false;
					return null;
				}
				object value = parseValue(json, ref index, ref success);
				if (!success)
				{
					success = false;
					return null;
				}
				dictionary[key] = value;
			}
			return dictionary;
		}

		protected static global::Prime31.JsonArray parseArray(char[] json, ref int index, ref bool success)
		{
			global::Prime31.JsonArray jsonArray = new global::Prime31.JsonArray();
			nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				switch (lookAhead(json, index))
				{
				case 0:
					success = false;
					return null;
				case 6:
					nextToken(json, ref index);
					continue;
				case 4:
					break;
				default:
				{
					object item = parseValue(json, ref index, ref success);
					if (!success)
					{
						return null;
					}
					jsonArray.Add(item);
					continue;
				}
				}
				nextToken(json, ref index);
				break;
			}
			return jsonArray;
		}

		protected static object parseValue(char[] json, ref int index, ref bool success)
		{
			switch (lookAhead(json, index))
			{
			case 7:
				return parseString(json, ref index, ref success);
			case 8:
				return parseNumber(json, ref index, ref success);
			case 1:
				return parseObject(json, ref index, ref success);
			case 3:
				return parseArray(json, ref index, ref success);
			case 9:
				nextToken(json, ref index);
				return true;
			case 10:
				nextToken(json, ref index);
				return false;
			case 11:
				nextToken(json, ref index);
				return null;
			default:
				success = false;
				return null;
			}
		}

		protected static string parseString(char[] json, ref int index, ref bool success)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder(2000);
			eatWhitespace(json, ref index);
			char c = json[index++];
			bool flag = false;
			while (!flag && index != json.Length)
			{
				c = json[index++];
				switch (c)
				{
				case '"':
					flag = true;
					break;
				case '\\':
				{
					if (index == json.Length)
					{
						break;
					}
					switch (json[index++])
					{
					case '"':
						stringBuilder.Append('"');
						continue;
					case '\\':
						stringBuilder.Append('\\');
						continue;
					case '/':
						stringBuilder.Append('/');
						continue;
					case 'b':
						stringBuilder.Append('\b');
						continue;
					case 'f':
						stringBuilder.Append('\f');
						continue;
					case 'n':
						stringBuilder.Append('\n');
						continue;
					case 'r':
						stringBuilder.Append('\r');
						continue;
					case 't':
						stringBuilder.Append('\t');
						continue;
					case 'u':
						break;
					default:
						continue;
					}
					int num = json.Length - index;
					if (num < 4)
					{
						break;
					}
					uint result;
					if (!(success = uint.TryParse(new string(json, index, 4), global::System.Globalization.NumberStyles.HexNumber, global::System.Globalization.CultureInfo.InvariantCulture, out result)))
					{
						return "";
					}
					if (55296 <= result && result <= 56319)
					{
						index += 4;
						num = json.Length - index;
						uint result2;
						if (num < 6 || !(new string(json, index, 2) == "\\u") || !uint.TryParse(new string(json, index + 2, 4), global::System.Globalization.NumberStyles.HexNumber, global::System.Globalization.CultureInfo.InvariantCulture, out result2) || 56320 > result2 || result2 > 57343)
						{
							success = false;
							return "";
						}
						stringBuilder.Append((char)result);
						stringBuilder.Append((char)result2);
						index += 6;
					}
					else
					{
						stringBuilder.Append(char.ConvertFromUtf32((int)result));
						index += 4;
					}
					continue;
				}
				default:
					stringBuilder.Append(c);
					continue;
				}
				break;
			}
			if (!flag)
			{
				success = false;
				return null;
			}
			return stringBuilder.ToString();
		}

		protected static object parseNumber(char[] json, ref int index, ref bool success)
		{
			eatWhitespace(json, ref index);
			int lastIndexOfNumber = getLastIndexOfNumber(json, index);
			int length = lastIndexOfNumber - index + 1;
			string text = new string(json, index, length);
			object result2;
			if (text.IndexOf(".", global::System.StringComparison.OrdinalIgnoreCase) != -1 || text.IndexOf("e", global::System.StringComparison.OrdinalIgnoreCase) != -1)
			{
				double result;
				success = double.TryParse(new string(json, index, length), global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out result);
				result2 = result;
			}
			else
			{
				long result3;
				success = long.TryParse(new string(json, index, length), global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture, out result3);
				result2 = result3;
			}
			index = lastIndexOfNumber + 1;
			return result2;
		}

		protected static int getLastIndexOfNumber(char[] json, int index)
		{
			int i;
			for (i = index; i < json.Length && "0123456789+-.eE".IndexOf(json[i]) != -1; i++)
			{
			}
			return i - 1;
		}

		protected static void eatWhitespace(char[] json, ref int index)
		{
			while (index < json.Length && " \t\n\r\b\f".IndexOf(json[index]) != -1)
			{
				index++;
			}
		}

		protected static int lookAhead(char[] json, int index)
		{
			int index2 = index;
			return nextToken(json, ref index2);
		}

		protected static int nextToken(char[] json, ref int index)
		{
			eatWhitespace(json, ref index);
			if (index == json.Length)
			{
				return 0;
			}
			char c = json[index];
			index++;
			switch (c)
			{
			case '{':
				return 1;
			case '}':
				return 2;
			case '[':
				return 3;
			case ']':
				return 4;
			case ',':
				return 6;
			case '"':
				return 7;
			case '-':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return 8;
			case ':':
				return 5;
			default:
			{
				index--;
				int num = json.Length - index;
				if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
				{
					index += 5;
					return 10;
				}
				if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
				{
					index += 4;
					return 9;
				}
				if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
				{
					index += 4;
					return 11;
				}
				return 0;
			}
			}
		}

		protected static bool serializeValue(global::Prime31.IJsonSerializerStrategy jsonSerializerStrategy, object value, global::System.Text.StringBuilder builder)
		{
			bool flag = true;
			if (value is string)
			{
				flag = serializeString((string)value, builder);
			}
			else if (value is global::System.Collections.Generic.IDictionary<string, object>)
			{
				global::System.Collections.Generic.IDictionary<string, object> dictionary = (global::System.Collections.Generic.IDictionary<string, object>)value;
				flag = serializeObject(jsonSerializerStrategy, dictionary.Keys, dictionary.Values, builder);
			}
			else if (value is global::System.Collections.Generic.IDictionary<string, string>)
			{
				global::System.Collections.Generic.IDictionary<string, string> dictionary2 = (global::System.Collections.Generic.IDictionary<string, string>)value;
				flag = serializeObject(jsonSerializerStrategy, dictionary2.Keys, dictionary2.Values, builder);
			}
			else if (value is global::System.Collections.IDictionary)
			{
				global::System.Collections.IDictionary dictionary3 = (global::System.Collections.IDictionary)value;
				flag = serializeObject(jsonSerializerStrategy, dictionary3.Keys, dictionary3.Values, builder);
			}
			else if (value is global::System.Collections.IEnumerable)
			{
				flag = serializeArray(jsonSerializerStrategy, (global::System.Collections.IEnumerable)value, builder);
			}
			else if (isNumeric(value))
			{
				flag = serializeNumber(value, builder);
			}
			else if (value is bool)
			{
				builder.Append((!(bool)value) ? "false" : "true");
			}
			else if (value == null)
			{
				builder.Append("null");
			}
			else
			{
				object output;
				flag = jsonSerializerStrategy.serializeNonPrimitiveObject(value, out output);
				if (flag)
				{
					serializeValue(jsonSerializerStrategy, output, builder);
				}
			}
			return flag;
		}

		protected static bool serializeObject(global::Prime31.IJsonSerializerStrategy jsonSerializerStrategy, global::System.Collections.IEnumerable keys, global::System.Collections.IEnumerable values, global::System.Text.StringBuilder builder)
		{
			builder.Append("{");
			global::System.Collections.IEnumerator enumerator = keys.GetEnumerator();
			global::System.Collections.IEnumerator enumerator2 = values.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				object current = enumerator.Current;
				object current2 = enumerator2.Current;
				if (!flag)
				{
					builder.Append(",");
				}
				if (current is string)
				{
					serializeString((string)current, builder);
				}
				else if (!serializeValue(jsonSerializerStrategy, current2, builder))
				{
					return false;
				}
				builder.Append(":");
				if (!serializeValue(jsonSerializerStrategy, current2, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		protected static bool serializeArray(global::Prime31.IJsonSerializerStrategy jsonSerializerStrategy, global::System.Collections.IEnumerable anArray, global::System.Text.StringBuilder builder)
		{
			builder.Append("[");
			bool flag = true;
			foreach (object item in anArray)
			{
				if (!flag)
				{
					builder.Append(",");
				}
				if (!serializeValue(jsonSerializerStrategy, item, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("]");
			return true;
		}

		protected static bool serializeString(string aString, global::System.Text.StringBuilder builder)
		{
			builder.Append("\"");
			char[] array = aString.ToCharArray();
			foreach (char c in array)
			{
				switch (c)
				{
				case '"':
					builder.Append("\\\"");
					break;
				case '\\':
					builder.Append("\\\\");
					break;
				case '\b':
					builder.Append("\\b");
					break;
				case '\f':
					builder.Append("\\f");
					break;
				case '\n':
					builder.Append("\\n");
					break;
				case '\r':
					builder.Append("\\r");
					break;
				case '\t':
					builder.Append("\\t");
					break;
				default:
					builder.Append(c);
					break;
				}
			}
			builder.Append("\"");
			return true;
		}

		protected static bool serializeNumber(object number, global::System.Text.StringBuilder builder)
		{
			if (number is long)
			{
				builder.Append(((long)number).ToString(global::System.Globalization.CultureInfo.InvariantCulture));
			}
			else if (number is ulong)
			{
				builder.Append(((ulong)number).ToString(global::System.Globalization.CultureInfo.InvariantCulture));
			}
			else if (number is int)
			{
				builder.Append(((int)number).ToString(global::System.Globalization.CultureInfo.InvariantCulture));
			}
			else if (number is uint)
			{
				builder.Append(((uint)number).ToString(global::System.Globalization.CultureInfo.InvariantCulture));
			}
			else if (number is decimal)
			{
				builder.Append(((decimal)number).ToString(global::System.Globalization.CultureInfo.InvariantCulture));
			}
			else if (number is float)
			{
				builder.Append(((float)number).ToString(global::System.Globalization.CultureInfo.InvariantCulture));
			}
			else
			{
				builder.Append(global::System.Convert.ToDouble(number, global::System.Globalization.CultureInfo.InvariantCulture).ToString("r", global::System.Globalization.CultureInfo.InvariantCulture));
			}
			return true;
		}

		protected static bool isNumeric(object value)
		{
			if (value is sbyte)
			{
				return true;
			}
			if (value is byte)
			{
				return true;
			}
			if (value is short)
			{
				return true;
			}
			if (value is ushort)
			{
				return true;
			}
			if (value is int)
			{
				return true;
			}
			if (value is uint)
			{
				return true;
			}
			if (value is long)
			{
				return true;
			}
			if (value is ulong)
			{
				return true;
			}
			if (value is float)
			{
				return true;
			}
			if (value is double)
			{
				return true;
			}
			if (value is decimal)
			{
				return true;
			}
			return false;
		}
	}
}
