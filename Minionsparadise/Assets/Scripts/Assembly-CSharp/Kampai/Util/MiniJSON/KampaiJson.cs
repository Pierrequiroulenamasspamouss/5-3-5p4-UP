namespace Kampai.Util.MiniJSON
{
	public static class KampaiJson
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

			private const string WORD_BREAK = "{}[],:\"";

			private global::System.IO.StringReader json;

			private char PeekChar
			{
				get
				{
					int num = json.Peek();
					return (num != -1) ? global::System.Convert.ToChar(num) : '\0';
				}
			}

			private char NextChar
			{
				get
				{
					int num = json.Read();
					return (num != -1) ? global::System.Convert.ToChar(num) : '\0';
				}
			}

			private string NextWord
			{
				get
				{
					global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
					while (!IsWordBreak(PeekChar))
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

			private global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN NextToken
			{
				get
				{
					EatWhitespace();
					if (json.Peek() == -1)
					{
						return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.NONE;
					}
					switch (PeekChar)
					{
					case '{':
						return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.CURLY_OPEN;
					case '}':
						json.Read();
						return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.CURLY_CLOSE;
					case '[':
						return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.SQUARED_OPEN;
					case ']':
						json.Read();
						return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.SQUARED_CLOSE;
					case ',':
						json.Read();
						return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.COMMA;
					case '"':
						return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.STRING;
					case ':':
						return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.COLON;
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
						return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.NUMBER;
					default:
						switch (NextWord)
						{
						case "false":
							return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.FALSE;
						case "true":
							return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.TRUE;
						case "null":
							return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.NULL;
						default:
							return global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.NONE;
						}
					}
				}
			}

			private Parser(string jsonString)
			{
				json = new global::System.IO.StringReader(jsonString);
			}

			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			public static object Parse(string jsonString)
			{
				using (global::Kampai.Util.MiniJSON.KampaiJson.Parser parser = new global::Kampai.Util.MiniJSON.KampaiJson.Parser(jsonString))
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
					case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.COMMA:
						continue;
					case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.NONE:
						return null;
					case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.CURLY_CLOSE:
						return dictionary;
					}
					string text = ParseString();
					if (text == null)
					{
						return null;
					}
					if (NextToken != global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.COLON)
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
					global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN nextToken = NextToken;
					switch (nextToken)
					{
					case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.NONE:
						return null;
					case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.SQUARED_CLOSE:
						flag = false;
						break;
					default:
					{
						object item = ParseByToken(nextToken);
						list.Add(item);
						break;
					}
					case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.COMMA:
						break;
					}
				}
				return list;
			}

			private object ParseValue()
			{
				global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN nextToken = NextToken;
				return ParseByToken(nextToken);
			}

			private object ParseByToken(global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN token)
			{
				switch (token)
				{
				case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.STRING:
					return ParseString();
				case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.NUMBER:
					return ParseNumber();
				case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.CURLY_OPEN:
					return ParseObject();
				case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.SQUARED_OPEN:
					return ParseArray();
				case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.TRUE:
					return true;
				case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.FALSE:
					return false;
				case global::Kampai.Util.MiniJSON.KampaiJson.Parser.TOKEN.NULL:
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
							char[] array = new char[4];
							for (int i = 0; i < 4; i++)
							{
								array[i] = NextChar;
							}
							stringBuilder.Append((char)global::System.Convert.ToInt32(new string(array), 16));
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
				while (char.IsWhiteSpace(PeekChar))
				{
					json.Read();
					if (json.Peek() == -1)
					{
						break;
					}
				}
			}
		}

		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return global::Kampai.Util.MiniJSON.KampaiJson.Parser.Parse(json);
		}

		public static string Serialize(object obj)
		{
			return global::Kampai.Util.MiniJSON.Serializer.Serialize(obj);
		}
	}
}
