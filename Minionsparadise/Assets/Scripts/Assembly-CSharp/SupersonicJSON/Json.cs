namespace SupersonicJSON
{
	public static class Json
	{
		private sealed class Parser : global::System.IDisposable
		{
			private enum TOKEN
			{
				NONE = 0,
				CURLY_OPEN = 1,
				CURLY_CLOSE = 2,
				SQUARED_OPEN = 3,
				SQUARED_CLOSE = 4,
				COLON = 5,
				COMMA = 6,
				STRING = 7,
				NUMBER = 8,
				TRUE = 9,
				FALSE = 10,
				NULL = 11
			}

			private const string WHITE_SPACE = " \t\n\r";

			private const string WORD_BREAK = " \t\n\r{}[],:\"";

			private global::System.IO.StringReader json;

			private char PeekChar
			{
				get
				{
					return global::System.Convert.ToChar(json.Peek());
				}
			}

			private char NextChar
			{
				get
				{
					return global::System.Convert.ToChar(json.Read());
				}
			}

			private string NextWord
			{
				get
				{
					global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
					while (" \t\n\r{}[],:\"".IndexOf(PeekChar) == -1)
					{
						stringBuilder.Append(NextChar);
						if (json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			private global::SupersonicJSON.Json.Parser.TOKEN NextToken
			{
				get
				{
					EatWhitespace();
					if (json.Peek() == -1)
					{
						return global::SupersonicJSON.Json.Parser.TOKEN.NONE;
					}
					switch (PeekChar)
					{
					case '{':
						return global::SupersonicJSON.Json.Parser.TOKEN.CURLY_OPEN;
					case '}':
						json.Read();
						return global::SupersonicJSON.Json.Parser.TOKEN.CURLY_CLOSE;
					case '[':
						return global::SupersonicJSON.Json.Parser.TOKEN.SQUARED_OPEN;
					case ']':
						json.Read();
						return global::SupersonicJSON.Json.Parser.TOKEN.SQUARED_CLOSE;
					case ',':
						json.Read();
						return global::SupersonicJSON.Json.Parser.TOKEN.COMMA;
					case '"':
						return global::SupersonicJSON.Json.Parser.TOKEN.STRING;
					case ':':
						return global::SupersonicJSON.Json.Parser.TOKEN.COLON;
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
						return global::SupersonicJSON.Json.Parser.TOKEN.NUMBER;
					default:
						switch (NextWord)
						{
						case "false":
							return global::SupersonicJSON.Json.Parser.TOKEN.FALSE;
						case "true":
							return global::SupersonicJSON.Json.Parser.TOKEN.TRUE;
						case "null":
							return global::SupersonicJSON.Json.Parser.TOKEN.NULL;
						default:
							return global::SupersonicJSON.Json.Parser.TOKEN.NONE;
						}
					}
				}
			}

			private Parser(string jsonString)
			{
				json = new global::System.IO.StringReader(jsonString);
			}

			public static object Parse(string jsonString)
			{
				using (global::SupersonicJSON.Json.Parser parser = new global::SupersonicJSON.Json.Parser(jsonString))
				{
					return parser.ParseValue();
				}
			}

			public void Dispose()
			{
				json.Dispose();
				json = null;
			}

			private global::System.Collections.Generic.Dictionary<string, object> ParseObject()
			{
				global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
				json.Read();
				while (true)
				{
					switch (NextToken)
					{
					case global::SupersonicJSON.Json.Parser.TOKEN.COMMA:
						continue;
					case global::SupersonicJSON.Json.Parser.TOKEN.NONE:
						return null;
					case global::SupersonicJSON.Json.Parser.TOKEN.CURLY_CLOSE:
						return dictionary;
					}
					string text = ParseString();
					if (text == null)
					{
						return null;
					}
					if (NextToken != global::SupersonicJSON.Json.Parser.TOKEN.COLON)
					{
						return null;
					}
					json.Read();
					dictionary[text] = ParseValue();
				}
			}

			private global::System.Collections.Generic.List<object> ParseArray()
			{
				global::System.Collections.Generic.List<object> list = new global::System.Collections.Generic.List<object>();
				json.Read();
				bool flag = true;
				while (flag)
				{
					global::SupersonicJSON.Json.Parser.TOKEN nextToken = NextToken;
					switch (nextToken)
					{
					case global::SupersonicJSON.Json.Parser.TOKEN.NONE:
						return null;
					case global::SupersonicJSON.Json.Parser.TOKEN.SQUARED_CLOSE:
						flag = false;
						break;
					default:
					{
						object item = ParseByToken(nextToken);
						list.Add(item);
						break;
					}
					case global::SupersonicJSON.Json.Parser.TOKEN.COMMA:
						break;
					}
				}
				return list;
			}

			private object ParseValue()
			{
				global::SupersonicJSON.Json.Parser.TOKEN nextToken = NextToken;
				return ParseByToken(nextToken);
			}

			private object ParseByToken(global::SupersonicJSON.Json.Parser.TOKEN token)
			{
				switch (token)
				{
				case global::SupersonicJSON.Json.Parser.TOKEN.STRING:
					return ParseString();
				case global::SupersonicJSON.Json.Parser.TOKEN.NUMBER:
					return ParseNumber();
				case global::SupersonicJSON.Json.Parser.TOKEN.CURLY_OPEN:
					return ParseObject();
				case global::SupersonicJSON.Json.Parser.TOKEN.SQUARED_OPEN:
					return ParseArray();
				case global::SupersonicJSON.Json.Parser.TOKEN.TRUE:
					return true;
				case global::SupersonicJSON.Json.Parser.TOKEN.FALSE:
					return false;
				case global::SupersonicJSON.Json.Parser.TOKEN.NULL:
					return null;
				default:
					return null;
				}
			}

			private string ParseString()
			{
				global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
				json.Read();
				bool flag = true;
				while (flag)
				{
					if (json.Peek() == -1)
					{
						flag = false;
						break;
					}
					char nextChar = NextChar;
					switch (nextChar)
					{
					case '"':
						flag = false;
						break;
					case '\\':
						if (json.Peek() == -1)
						{
							flag = false;
							break;
						}
						nextChar = NextChar;
						switch (nextChar)
						{
						case '"':
						case '/':
						case '\\':
							stringBuilder.Append(nextChar);
							break;
						case 'b':
							stringBuilder.Append('\b');
							break;
						case 'f':
							stringBuilder.Append('\f');
							break;
						case 'n':
							stringBuilder.Append('\n');
							break;
						case 'r':
							stringBuilder.Append('\r');
							break;
						case 't':
							stringBuilder.Append('\t');
							break;
						case 'u':
						{
							global::System.Text.StringBuilder stringBuilder2 = new global::System.Text.StringBuilder();
							for (int i = 0; i < 4; i++)
							{
								stringBuilder2.Append(NextChar);
							}
							stringBuilder.Append((char)global::System.Convert.ToInt32(stringBuilder2.ToString(), 16));
							break;
						}
						}
						break;
					default:
						stringBuilder.Append(nextChar);
						break;
					}
				}
				return stringBuilder.ToString();
			}

			private object ParseNumber()
			{
				string nextWord = NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long result;
					long.TryParse(nextWord, out result);
					return result;
				}
				double result2;
				double.TryParse(nextWord, out result2);
				return result2;
			}

			private void EatWhitespace()
			{
				while (" \t\n\r".IndexOf(PeekChar) != -1)
				{
					json.Read();
					if (json.Peek() == -1)
					{
						break;
					}
				}
			}
		}

		private sealed class Serializer
		{
			private global::System.Text.StringBuilder builder;

			private Serializer()
			{
				builder = new global::System.Text.StringBuilder();
			}

			public static string Serialize(object obj)
			{
				global::SupersonicJSON.Json.Serializer serializer = new global::SupersonicJSON.Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			private void SerializeValue(object value)
			{
				string str;
				global::System.Collections.IList anArray;
				global::System.Collections.IDictionary obj;
				if (value == null)
				{
					builder.Append("null");
				}
				else if ((str = value as string) != null)
				{
					SerializeString(str);
				}
				else if (value is bool)
				{
					builder.Append(value.ToString().ToLower());
				}
				else if ((anArray = value as global::System.Collections.IList) != null)
				{
					SerializeArray(anArray);
				}
				else if ((obj = value as global::System.Collections.IDictionary) != null)
				{
					SerializeObject(obj);
				}
				else if (value is char)
				{
					SerializeString(value.ToString());
				}
				else
				{
					SerializeOther(value);
				}
			}

			private void SerializeObject(global::System.Collections.IDictionary obj)
			{
				bool flag = true;
				builder.Append('{');
				foreach (object key in obj.Keys)
				{
					if (!flag)
					{
						builder.Append(',');
					}
					SerializeString(key.ToString());
					builder.Append(':');
					SerializeValue(obj[key]);
					flag = false;
				}
				builder.Append('}');
			}

			private void SerializeArray(global::System.Collections.IList anArray)
			{
				builder.Append('[');
				bool flag = true;
				foreach (object item in anArray)
				{
					if (!flag)
					{
						builder.Append(',');
					}
					SerializeValue(item);
					flag = false;
				}
				builder.Append(']');
			}

			private void SerializeString(string str)
			{
				builder.Append('"');
				char[] array = str.ToCharArray();
				char[] array2 = array;
				foreach (char c in array2)
				{
					switch (c)
					{
					case '"':
						builder.Append("\\\"");
						continue;
					case '\\':
						builder.Append("\\\\");
						continue;
					case '\b':
						builder.Append("\\b");
						continue;
					case '\f':
						builder.Append("\\f");
						continue;
					case '\n':
						builder.Append("\\n");
						continue;
					case '\r':
						builder.Append("\\r");
						continue;
					case '\t':
						builder.Append("\\t");
						continue;
					}
					int num = global::System.Convert.ToInt32(c);
					if (num >= 32 && num <= 126)
					{
						builder.Append(c);
					}
					else
					{
						builder.Append("\\u" + global::System.Convert.ToString(num, 16).PadLeft(4, '0'));
					}
				}
				builder.Append('"');
			}

			private void SerializeOther(object value)
			{
				if (value is float || value is int || value is uint || value is long || value is double || value is sbyte || value is byte || value is short || value is ushort || value is ulong || value is decimal)
				{
					builder.Append(value.ToString());
				}
				else
				{
					SerializeString(value.ToString());
				}
			}
		}

		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return global::SupersonicJSON.Json.Parser.Parse(json);
		}

		public static string Serialize(object obj)
		{
			return global::SupersonicJSON.Json.Serializer.Serialize(obj);
		}
	}
}
