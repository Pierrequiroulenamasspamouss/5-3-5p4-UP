namespace Prime31
{
	public class Json
	{
		internal class ObjectDecoder
		{
			private global::System.Collections.Generic.Dictionary<string, global::System.Action<object, object>> _memberInfo;

			public static object decode<T>(string json, string rootElement = null) where T : new()
			{
				object obj = global::Prime31.Json.decode(json);
				if (obj == null)
				{
					return null;
				}
				return new global::Prime31.Json.ObjectDecoder().decode<T>(obj, rootElement);
			}

			private object decode<T>(object decodedJsonObject, string rootElement = null) where T : new()
			{
				if (rootElement != null)
				{
					global::System.Collections.IDictionary dictionary = decodedJsonObject as global::System.Collections.IDictionary;
					if (dictionary == null)
					{
						global::Prime31.Utils.logObject("A rootElement was requested (" + rootElement + ") but the json did not decode to a Dictionary. It decoded to: " + decodedJsonObject);
						return null;
					}
					if (!dictionary.Contains(rootElement))
					{
						global::Prime31.Utils.logObject("A rootElement was requested (" + rootElement + ") but does not exist in the decoded Dictionary");
						return null;
					}
					decodedJsonObject = dictionary[rootElement];
				}
				global::System.Type typeFromHandle = typeof(T);
				global::System.Collections.IList list = null;
				if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(global::System.Collections.Generic.List<>))
				{
					list = new T() as global::System.Collections.IList;
					typeFromHandle = list.GetType().GetGenericArguments()[0];
					if (!(decodedJsonObject is global::System.Collections.IList) || !decodedJsonObject.GetType().IsGenericType)
					{
						global::Prime31.Utils.logObject("A List was required but the json did not decode to a List. It decoded to: " + decodedJsonObject);
						return null;
					}
					{
						foreach (global::System.Collections.Generic.Dictionary<string, object> item in (global::System.Collections.IList)decodedJsonObject)
						{
							if (item == null)
							{
								global::Prime31.Utils.logObject("Aborted populating List because the json did not decode to a List of Dictionaries");
								return list;
							}
							list.Add(createAndPopulateObjectFromDictionary(typeFromHandle, item));
						}
						return list;
					}
				}
				return createAndPopulateObjectFromDictionary(typeFromHandle, decodedJsonObject as global::System.Collections.Generic.Dictionary<string, object>);
			}

			private global::System.Collections.Generic.Dictionary<string, global::System.Action<object, object>> getMemberInfoForObject(object obj)
			{
				if (_memberInfo == null)
				{
					_memberInfo = getMembersWithSetters(obj);
				}
				return _memberInfo;
			}

			private static global::System.Collections.Generic.Dictionary<string, global::System.Action<object, object>> getMembersWithSetters(object obj)
			{
				global::System.Collections.Generic.Dictionary<string, global::System.Action<object, object>> dictionary = new global::System.Collections.Generic.Dictionary<string, global::System.Action<object, object>>();
				global::System.Reflection.FieldInfo[] fields = obj.GetType().GetFields(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
				foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
				{
					if (fieldInfo.FieldType.Namespace.StartsWith("System"))
					{
						global::System.Reflection.FieldInfo theInfo = fieldInfo;
						global::System.Type theFieldType = fieldInfo.FieldType;
						dictionary[fieldInfo.Name] = delegate(object ownerObject, object val)
						{
							theInfo.SetValue(ownerObject, global::System.Convert.ChangeType(val, theFieldType));
						};
					}
				}
				global::System.Reflection.PropertyInfo[] properties = obj.GetType().GetProperties(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
				foreach (global::System.Reflection.PropertyInfo propertyInfo in properties)
				{
					if (propertyInfo.PropertyType.Namespace.StartsWith("System") && propertyInfo.CanWrite && propertyInfo.GetSetMethod(true) != null)
					{
						global::System.Reflection.PropertyInfo theInfo2 = propertyInfo;
						global::System.Type thePropertyType = propertyInfo.PropertyType;
						dictionary[propertyInfo.Name] = delegate(object ownerObject, object val)
						{
							theInfo2.SetValue(ownerObject, global::System.Convert.ChangeType(val, thePropertyType), null);
						};
					}
				}
				return dictionary;
			}

			public object createAndPopulateObjectFromDictionary(global::System.Type objectType, global::System.Collections.Generic.Dictionary<string, object> dict)
			{
				object obj = global::System.Activator.CreateInstance(objectType);
				global::System.Collections.Generic.Dictionary<string, global::System.Action<object, object>> memberInfoForObject = getMemberInfoForObject(obj);
				global::System.Collections.Generic.Dictionary<string, object>.KeyCollection keys = dict.Keys;
				foreach (string item in keys)
				{
					if (memberInfoForObject.ContainsKey(item))
					{
						try
						{
							memberInfoForObject[item](obj, dict[item]);
						}
						catch (global::System.Exception obj2)
						{
							global::Prime31.Utils.logObject(obj2);
						}
					}
				}
				return obj;
			}
		}

		internal class Deserializer
		{
			private enum JsonToken
			{
				None = 0,
				CurlyOpen = 1,
				CurlyClose = 2,
				SquaredOpen = 3,
				SquaredClose = 4,
				Colon = 5,
				Comma = 6,
				String = 7,
				Number = 8,
				True = 9,
				False = 10,
				Null = 11
			}

			private char[] charArray;

			private Deserializer(string json)
			{
				charArray = json.ToCharArray();
			}

			public static object deserialize(string json)
			{
				if (json != null)
				{
					global::Prime31.Json.Deserializer deserializer = new global::Prime31.Json.Deserializer(json);
					return deserializer.deserialize();
				}
				return null;
			}

			private object deserialize()
			{
				int index = 0;
				return parseValue(charArray, ref index);
			}

			protected object parseValue(char[] json, ref int index)
			{
				switch (lookAhead(json, index))
				{
				case global::Prime31.Json.Deserializer.JsonToken.String:
					return parseString(json, ref index);
				case global::Prime31.Json.Deserializer.JsonToken.Number:
					return parseNumber(json, ref index);
				case global::Prime31.Json.Deserializer.JsonToken.CurlyOpen:
					return parseObject(json, ref index);
				case global::Prime31.Json.Deserializer.JsonToken.SquaredOpen:
					return parseArray(json, ref index);
				case global::Prime31.Json.Deserializer.JsonToken.True:
					nextToken(json, ref index);
					return bool.Parse("TRUE");
				case global::Prime31.Json.Deserializer.JsonToken.False:
					nextToken(json, ref index);
					return bool.Parse("FALSE");
				case global::Prime31.Json.Deserializer.JsonToken.Null:
					nextToken(json, ref index);
					return null;
				default:
					return null;
				}
			}

			private global::System.Collections.IDictionary parseObject(char[] json, ref int index)
			{
				global::System.Collections.IDictionary dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
				nextToken(json, ref index);
				bool flag = false;
				while (!flag)
				{
					switch (lookAhead(json, index))
					{
					case global::Prime31.Json.Deserializer.JsonToken.None:
						return null;
					case global::Prime31.Json.Deserializer.JsonToken.Comma:
						nextToken(json, ref index);
						continue;
					case global::Prime31.Json.Deserializer.JsonToken.CurlyClose:
						nextToken(json, ref index);
						return dictionary;
					}
					string text = parseString(json, ref index);
					if (text == null)
					{
						return null;
					}
					global::Prime31.Json.Deserializer.JsonToken jsonToken = nextToken(json, ref index);
					if (jsonToken != global::Prime31.Json.Deserializer.JsonToken.Colon)
					{
						return null;
					}
					object value = parseValue(json, ref index);
					dictionary[text] = value;
				}
				return dictionary;
			}

			private global::System.Collections.IList parseArray(char[] json, ref int index)
			{
				global::System.Collections.Generic.List<object> list = new global::System.Collections.Generic.List<object>();
				nextToken(json, ref index);
				bool flag = false;
				while (!flag)
				{
					switch (lookAhead(json, index))
					{
					case global::Prime31.Json.Deserializer.JsonToken.None:
						return null;
					case global::Prime31.Json.Deserializer.JsonToken.Comma:
						nextToken(json, ref index);
						continue;
					case global::Prime31.Json.Deserializer.JsonToken.SquaredClose:
						break;
					default:
					{
						object item = parseValue(json, ref index);
						list.Add(item);
						continue;
					}
					}
					nextToken(json, ref index);
					break;
				}
				return list;
			}

			private string parseString(char[] json, ref int index)
			{
				string text = string.Empty;
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
							text += '"';
							continue;
						case '\\':
							text += '\\';
							continue;
						case '/':
							text += '/';
							continue;
						case 'b':
							text += '\b';
							continue;
						case 'f':
							text += '\f';
							continue;
						case 'n':
							text += '\n';
							continue;
						case 'r':
							text += '\r';
							continue;
						case 't':
							text += '\t';
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
						char[] array = new char[4];
						global::System.Array.Copy(json, index, array, 0, 4);
						uint utf = uint.Parse(new string(array), global::System.Globalization.NumberStyles.HexNumber);
						try
						{
							text += char.ConvertFromUtf32((int)utf);
						}
						catch (global::System.Exception)
						{
							char[] array2 = array;
							foreach (char c2 in array2)
							{
								text += c2;
							}
						}
						index += 4;
						continue;
					}
					default:
						text += c;
						continue;
					}
					break;
				}
				if (!flag)
				{
					return null;
				}
				return text;
			}

			private object parseNumber(char[] json, ref int index)
			{
				eatWhitespace(json, ref index);
				int lastIndexOfNumber = getLastIndexOfNumber(json, index);
				int num = lastIndexOfNumber - index + 1;
				char[] array = new char[num];
				global::System.Array.Copy(json, index, array, 0, num);
				index = lastIndexOfNumber + 1;
				string text = new string(array);
				long result;
				if (!text.Contains(".") && long.TryParse(text, global::System.Globalization.NumberStyles.Integer, global::System.Globalization.CultureInfo.InvariantCulture, out result))
				{
					return result;
				}
				return double.Parse(new string(array), global::System.Globalization.CultureInfo.InvariantCulture);
			}

			private int getLastIndexOfNumber(char[] json, int index)
			{
				int i;
				for (i = index; i < json.Length && "0123456789+-.eE".IndexOf(json[i]) != -1; i++)
				{
				}
				return i - 1;
			}

			private void eatWhitespace(char[] json, ref int index)
			{
				while (index < json.Length && " \t\n\r".IndexOf(json[index]) != -1)
				{
					index++;
				}
			}

			private global::Prime31.Json.Deserializer.JsonToken lookAhead(char[] json, int index)
			{
				int index2 = index;
				return nextToken(json, ref index2);
			}

			private global::Prime31.Json.Deserializer.JsonToken nextToken(char[] json, ref int index)
			{
				eatWhitespace(json, ref index);
				if (index == json.Length)
				{
					return global::Prime31.Json.Deserializer.JsonToken.None;
				}
				char c = json[index];
				index++;
				switch (c)
				{
				case '{':
					return global::Prime31.Json.Deserializer.JsonToken.CurlyOpen;
				case '}':
					return global::Prime31.Json.Deserializer.JsonToken.CurlyClose;
				case '[':
					return global::Prime31.Json.Deserializer.JsonToken.SquaredOpen;
				case ']':
					return global::Prime31.Json.Deserializer.JsonToken.SquaredClose;
				case ',':
					return global::Prime31.Json.Deserializer.JsonToken.Comma;
				case '"':
					return global::Prime31.Json.Deserializer.JsonToken.String;
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
					return global::Prime31.Json.Deserializer.JsonToken.Number;
				case ':':
					return global::Prime31.Json.Deserializer.JsonToken.Colon;
				default:
				{
					index--;
					int num = json.Length - index;
					if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
					{
						index += 5;
						return global::Prime31.Json.Deserializer.JsonToken.False;
					}
					if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
					{
						index += 4;
						return global::Prime31.Json.Deserializer.JsonToken.True;
					}
					if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
					{
						index += 4;
						return global::Prime31.Json.Deserializer.JsonToken.Null;
					}
					return global::Prime31.Json.Deserializer.JsonToken.None;
				}
				}
			}
		}

		internal class Serializer
		{
			private global::System.Text.StringBuilder _builder;

			private Serializer()
			{
				_builder = new global::System.Text.StringBuilder();
			}

			public static string serialize(object obj)
			{
				global::Prime31.Json.Serializer serializer = new global::Prime31.Json.Serializer();
				serializer.serializeObject(obj);
				return serializer._builder.ToString();
			}

			private void serializeObject(object value)
			{
				if (value == null)
				{
					_builder.Append("null");
					return;
				}
				if (value is string)
				{
					serializeString((string)value);
					return;
				}
				if (value is global::System.Collections.IList)
				{
					serializeIList((global::System.Collections.IList)value);
					return;
				}
				if (value is global::System.Collections.Generic.Dictionary<string, object>)
				{
					serializeDictionary((global::System.Collections.Generic.Dictionary<string, object>)value);
					return;
				}
				if (value is global::System.Collections.IDictionary)
				{
					serializeIDictionary((global::System.Collections.IDictionary)value);
					return;
				}
				if (value is bool)
				{
					_builder.Append(value.ToString().ToLower());
					return;
				}
				if (value.GetType().IsPrimitive)
				{
					_builder.Append(value);
					return;
				}
				if (value is global::System.DateTime)
				{
					global::System.DateTime value2 = new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc);
					double totalMilliseconds = ((global::System.DateTime)value).Subtract(value2).TotalMilliseconds;
					serializeString(global::System.Convert.ToString(totalMilliseconds, global::System.Globalization.CultureInfo.InvariantCulture));
					return;
				}
				try
				{
					serializeClass(value);
				}
				catch (global::System.Exception ex)
				{
					global::Prime31.Utils.logObject(string.Format("failed to serialize {0} with error: {1}", value, ex.Message));
				}
			}

			private void serializeIList(global::System.Collections.IList anArray)
			{
				_builder.Append("[");
				bool flag = true;
				for (int i = 0; i < anArray.Count; i++)
				{
					object value = anArray[i];
					if (!flag)
					{
						_builder.Append(", ");
					}
					serializeObject(value);
					flag = false;
				}
				_builder.Append("]");
			}

			private void serializeIDictionary(global::System.Collections.IDictionary dict)
			{
				_builder.Append("{");
				bool flag = true;
				foreach (object key in dict.Keys)
				{
					if (!flag)
					{
						_builder.Append(", ");
					}
					serializeString(key.ToString());
					_builder.Append(":");
					serializeObject(dict[key]);
					flag = false;
				}
				_builder.Append("}");
			}

			private void serializeDictionary(global::System.Collections.Generic.Dictionary<string, object> dict)
			{
				_builder.Append("{");
				bool flag = true;
				global::System.Collections.Generic.Dictionary<string, object>.KeyCollection keys = dict.Keys;
				foreach (string item in keys)
				{
					if (!flag)
					{
						_builder.Append(", ");
					}
					serializeString(item.ToString());
					_builder.Append(":");
					serializeObject(dict[item]);
					flag = false;
				}
				_builder.Append("}");
			}

			private void serializeString(string str)
			{
				_builder.Append("\"");
				char[] array = str.ToCharArray();
				foreach (char c in array)
				{
					switch (c)
					{
					case '"':
						_builder.Append("\\\"");
						continue;
					case '\\':
						_builder.Append("\\\\");
						continue;
					case '\b':
						_builder.Append("\\b");
						continue;
					case '\f':
						_builder.Append("\\f");
						continue;
					case '\n':
						_builder.Append("\\n");
						continue;
					case '\r':
						_builder.Append("\\r");
						continue;
					case '\t':
						_builder.Append("\\t");
						continue;
					}
					int num = global::System.Convert.ToInt32(c, global::System.Globalization.CultureInfo.InvariantCulture);
					if (num >= 32 && num <= 126)
					{
						_builder.Append(c);
					}
					else
					{
						_builder.Append("\\u" + global::System.Convert.ToString(num, 16).PadLeft(4, '0'));
					}
				}
				_builder.Append("\"");
			}

			private void serializeClass(object value)
			{
				_builder.Append("{");
				bool flag = true;
				global::System.Reflection.FieldInfo[] fields = value.GetType().GetFields(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
				foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
				{
					if (!fieldInfo.IsPrivate || !fieldInfo.Name.Contains("k__BackingField"))
					{
						if (!flag)
						{
							_builder.Append(", ");
						}
						serializeString(fieldInfo.Name);
						_builder.Append(":");
						serializeObject(fieldInfo.GetValue(value));
						flag = false;
					}
				}
				global::System.Reflection.PropertyInfo[] properties = value.GetType().GetProperties(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
				foreach (global::System.Reflection.PropertyInfo propertyInfo in properties)
				{
					if (!flag)
					{
						_builder.Append(", ");
					}
					serializeString(propertyInfo.Name);
					_builder.Append(":");
					serializeObject(propertyInfo.GetValue(value, null));
					flag = false;
				}
				_builder.Append("}");
			}
		}

		public static bool useSimpleJson = true;

		public static object decode(string json)
		{
			if (useSimpleJson)
			{
				return global::Prime31.SimpleJson.decode(json);
			}
			object obj = global::Prime31.Json.Deserializer.deserialize(json);
			if (obj == null)
			{
				global::Prime31.Utils.logObject("Something went wrong deserializing the json. We got a null return. Here is the json we tried to deserialize: " + json);
			}
			return obj;
		}

		public static T decode<T>(string json, string rootElement = null) where T : new()
		{
			if (useSimpleJson)
			{
				return global::Prime31.SimpleJson.decode<T>(json, rootElement);
			}
			return (T)global::Prime31.Json.ObjectDecoder.decode<T>(json, rootElement);
		}

		public static T decodeObject<T>(object jsonObject, string rootElement = null) where T : new()
		{
			return global::Prime31.SimpleJson.decodeObject<T>(jsonObject, rootElement);
		}

		public static string encode(object obj)
		{
			string text = ((!useSimpleJson) ? global::Prime31.Json.Serializer.serialize(obj) : global::Prime31.SimpleJson.encode(obj));
			if (text == null)
			{
				global::Prime31.Utils.logObject("Something went wrong serializing the object. We got a null return. Here is the object we tried to deserialize: ");
				global::Prime31.Utils.logObject(obj);
			}
			return text;
		}

		public static object jsonDecode(string json)
		{
			return decode(json);
		}

		public static string jsonEncode(object obj)
		{
			string text = global::Prime31.Json.Serializer.serialize(obj);
			if (text == null)
			{
				global::Prime31.Utils.logObject("Something went wrong serializing the object. We got a null return. Here is the object we tried to deserialize: ");
				global::Prime31.Utils.logObject(obj);
			}
			return text;
		}
	}
}
