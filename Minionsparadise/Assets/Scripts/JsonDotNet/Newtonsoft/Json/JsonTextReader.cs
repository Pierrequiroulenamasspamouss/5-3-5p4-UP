namespace Newtonsoft.Json
{
	public class JsonTextReader : global::Newtonsoft.Json.JsonReader, global::Newtonsoft.Json.IJsonLineInfo
	{
		private enum ReadType
		{
			Read = 0,
			ReadAsBytes = 1,
			ReadAsDecimal = 2,
			ReadAsDateTimeOffset = 3
		}

		private const int LineFeedValue = 10;

		private const int CarriageReturnValue = 13;

		private readonly global::System.IO.TextReader _reader;

		private readonly global::Newtonsoft.Json.Utilities.StringBuffer _buffer;

		private char? _lastChar;

		private int _currentLinePosition;

		private int _currentLineNumber;

		private bool _end;

		private global::Newtonsoft.Json.JsonTextReader.ReadType _readType;

		private global::System.Globalization.CultureInfo _culture;

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

		public int LineNumber
		{
			get
			{
				if (base.CurrentState == global::Newtonsoft.Json.JsonReader.State.Start)
				{
					return 0;
				}
				return _currentLineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				return _currentLinePosition;
			}
		}

		public JsonTextReader(global::System.IO.TextReader reader)
		{
			if (reader == null)
			{
				throw new global::System.ArgumentNullException("reader");
			}
			_reader = reader;
			_buffer = new global::Newtonsoft.Json.Utilities.StringBuffer(4096);
			_currentLineNumber = 1;
		}

		private void ParseString(char quote)
		{
			ReadStringIntoBuffer(quote);
			if (_readType == global::Newtonsoft.Json.JsonTextReader.ReadType.ReadAsBytes)
			{
				byte[] value;
				if (_buffer.Position == 0)
				{
					value = new byte[0];
				}
				else
				{
					value = global::System.Convert.FromBase64CharArray(_buffer.GetInternalBuffer(), 0, _buffer.Position);
					_buffer.Position = 0;
				}
				SetToken(global::Newtonsoft.Json.JsonToken.Bytes, value);
				return;
			}
			string text = _buffer.ToString();
			_buffer.Position = 0;
			if (text.StartsWith("/Date(", global::System.StringComparison.Ordinal) && text.EndsWith(")/", global::System.StringComparison.Ordinal))
			{
				ParseDate(text);
				return;
			}
			SetToken(global::Newtonsoft.Json.JsonToken.String, text);
			QuoteChar = quote;
		}

		private void ReadStringIntoBuffer(char quote)
		{
			while (true)
			{
				char c = MoveNext();
				switch (c)
				{
				case '\0':
					if (_end)
					{
						throw CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", quote, _currentLineNumber, _currentLinePosition);
					}
					_buffer.Append('\0');
					break;
				case '\\':
					if ((c = MoveNext()) != 0 || !_end)
					{
						switch (c)
						{
						case 'b':
							_buffer.Append('\b');
							break;
						case 't':
							_buffer.Append('\t');
							break;
						case 'n':
							_buffer.Append('\n');
							break;
						case 'f':
							_buffer.Append('\f');
							break;
						case 'r':
							_buffer.Append('\r');
							break;
						case '\\':
							_buffer.Append('\\');
							break;
						case '"':
						case '\'':
						case '/':
							_buffer.Append(c);
							break;
						case 'u':
						{
							char[] array = new char[4];
							for (int i = 0; i < array.Length; i++)
							{
								if ((c = MoveNext()) != 0 || !_end)
								{
									array[i] = c;
									continue;
								}
								throw CreateJsonReaderException("Unexpected end while parsing unicode character. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
							}
							char value = global::System.Convert.ToChar(int.Parse(new string(array), global::System.Globalization.NumberStyles.HexNumber, global::System.Globalization.NumberFormatInfo.InvariantInfo));
							_buffer.Append(value);
							break;
						}
						default:
							throw CreateJsonReaderException("Bad JSON escape sequence: {0}. Line {1}, position {2}.", "\\" + c, _currentLineNumber, _currentLinePosition);
						}
						break;
					}
					throw CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", quote, _currentLineNumber, _currentLinePosition);
				case '"':
				case '\'':
					if (c == quote)
					{
						return;
					}
					_buffer.Append(c);
					break;
				default:
					_buffer.Append(c);
					break;
				}
			}
		}

		private global::Newtonsoft.Json.JsonReaderException CreateJsonReaderException(string format, params object[] args)
		{
			string message = global::Newtonsoft.Json.Utilities.StringUtils.FormatWith(format, global::System.Globalization.CultureInfo.InvariantCulture, args);
			return new global::Newtonsoft.Json.JsonReaderException(message, null, _currentLineNumber, _currentLinePosition);
		}

		private global::System.TimeSpan ReadOffset(string offsetText)
		{
			bool flag = offsetText[0] == '-';
			int num = int.Parse(offsetText.Substring(1, 2), global::System.Globalization.NumberStyles.Integer, global::System.Globalization.CultureInfo.InvariantCulture);
			int num2 = 0;
			if (offsetText.Length >= 5)
			{
				num2 = int.Parse(offsetText.Substring(3, 2), global::System.Globalization.NumberStyles.Integer, global::System.Globalization.CultureInfo.InvariantCulture);
			}
			global::System.TimeSpan result = global::System.TimeSpan.FromHours(num) + global::System.TimeSpan.FromMinutes(num2);
			if (flag)
			{
				result = result.Negate();
			}
			return result;
		}

		private void ParseDate(string text)
		{
			string text2 = text.Substring(6, text.Length - 8);
			global::System.DateTimeKind dateTimeKind = global::System.DateTimeKind.Utc;
			int num = text2.IndexOf('+', 1);
			if (num == -1)
			{
				num = text2.IndexOf('-', 1);
			}
			global::System.TimeSpan timeSpan = global::System.TimeSpan.Zero;
			if (num != -1)
			{
				dateTimeKind = global::System.DateTimeKind.Local;
				timeSpan = ReadOffset(text2.Substring(num));
				text2 = text2.Substring(0, num);
			}
			long javaScriptTicks = long.Parse(text2, global::System.Globalization.NumberStyles.Integer, global::System.Globalization.CultureInfo.InvariantCulture);
			global::System.DateTime dateTime = global::Newtonsoft.Json.JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			if (_readType == global::Newtonsoft.Json.JsonTextReader.ReadType.ReadAsDateTimeOffset)
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Date, new global::System.DateTimeOffset(dateTime.Add(timeSpan).Ticks, timeSpan));
				return;
			}
			global::System.DateTime dateTime2;
			switch (dateTimeKind)
			{
			case global::System.DateTimeKind.Unspecified:
				dateTime2 = global::System.DateTime.SpecifyKind(dateTime.ToLocalTime(), global::System.DateTimeKind.Unspecified);
				break;
			case global::System.DateTimeKind.Local:
				dateTime2 = dateTime.ToLocalTime();
				break;
			default:
				dateTime2 = dateTime;
				break;
			}
			SetToken(global::Newtonsoft.Json.JsonToken.Date, dateTime2);
		}

		private char MoveNext()
		{
			int num = _reader.Read();
			switch (num)
			{
			case -1:
				_end = true;
				return '\0';
			case 13:
				if (_reader.Peek() == 10)
				{
					_reader.Read();
				}
				_currentLineNumber++;
				_currentLinePosition = 0;
				break;
			case 10:
				_currentLineNumber++;
				_currentLinePosition = 0;
				break;
			default:
				_currentLinePosition++;
				break;
			}
			return (char)num;
		}

		private bool HasNext()
		{
			return _reader.Peek() != -1;
		}

		private int PeekNext()
		{
			return _reader.Peek();
		}

		public override bool Read()
		{
			_readType = global::Newtonsoft.Json.JsonTextReader.ReadType.Read;
			return ReadInternal();
		}

		public override byte[] ReadAsBytes()
		{
			_readType = global::Newtonsoft.Json.JsonTextReader.ReadType.ReadAsBytes;
			do
			{
				if (!ReadInternal())
				{
					throw CreateJsonReaderException("Unexpected end when reading bytes: Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
				}
			}
			while (TokenType == global::Newtonsoft.Json.JsonToken.Comment);
			if (TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			if (TokenType == global::Newtonsoft.Json.JsonToken.Bytes)
			{
				return (byte[])Value;
			}
			if (TokenType == global::Newtonsoft.Json.JsonToken.StartArray)
			{
				global::System.Collections.Generic.List<byte> list = new global::System.Collections.Generic.List<byte>();
				while (ReadInternal())
				{
					switch (TokenType)
					{
					case global::Newtonsoft.Json.JsonToken.Integer:
						list.Add(global::System.Convert.ToByte(Value, global::System.Globalization.CultureInfo.InvariantCulture));
						break;
					case global::Newtonsoft.Json.JsonToken.EndArray:
					{
						byte[] array = list.ToArray();
						SetToken(global::Newtonsoft.Json.JsonToken.Bytes, array);
						return array;
					}
					default:
						throw CreateJsonReaderException("Unexpected token when reading bytes: {0}. Line {1}, position {2}.", TokenType, _currentLineNumber, _currentLinePosition);
					case global::Newtonsoft.Json.JsonToken.Comment:
						break;
					}
				}
				throw CreateJsonReaderException("Unexpected end when reading bytes: Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
			}
			throw CreateJsonReaderException("Unexpected token when reading bytes: {0}. Line {1}, position {2}.", TokenType, _currentLineNumber, _currentLinePosition);
		}

		public override decimal? ReadAsDecimal()
		{
			_readType = global::Newtonsoft.Json.JsonTextReader.ReadType.ReadAsDecimal;
			do
			{
				if (!ReadInternal())
				{
					throw CreateJsonReaderException("Unexpected end when reading decimal: Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
				}
			}
			while (TokenType == global::Newtonsoft.Json.JsonToken.Comment);
			if (TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			if (TokenType == global::Newtonsoft.Json.JsonToken.Float)
			{
				return (decimal?)Value;
			}
			decimal result;
			if (TokenType == global::Newtonsoft.Json.JsonToken.String && decimal.TryParse((string)Value, global::System.Globalization.NumberStyles.Number, Culture, out result))
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Float, result);
				return result;
			}
			throw CreateJsonReaderException("Unexpected token when reading decimal: {0}. Line {1}, position {2}.", TokenType, _currentLineNumber, _currentLinePosition);
		}

		public override global::System.DateTimeOffset? ReadAsDateTimeOffset()
		{
			_readType = global::Newtonsoft.Json.JsonTextReader.ReadType.ReadAsDateTimeOffset;
			do
			{
				if (!ReadInternal())
				{
					throw CreateJsonReaderException("Unexpected end when reading date: Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
				}
			}
			while (TokenType == global::Newtonsoft.Json.JsonToken.Comment);
			if (TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			if (TokenType == global::Newtonsoft.Json.JsonToken.Date)
			{
				return (global::System.DateTimeOffset)Value;
			}
			global::System.DateTimeOffset result;
			if (TokenType == global::Newtonsoft.Json.JsonToken.String && global::System.DateTimeOffset.TryParse((string)Value, Culture, global::System.Globalization.DateTimeStyles.None, out result))
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Date, result);
				return result;
			}
			throw CreateJsonReaderException("Unexpected token when reading date: {0}. Line {1}, position {2}.", TokenType, _currentLineNumber, _currentLinePosition);
		}

		private bool ReadInternal()
		{
			while (true)
			{
				char c;
				if (_lastChar.HasValue)
				{
					c = _lastChar.Value;
					_lastChar = null;
				}
				else
				{
					c = MoveNext();
				}
				if (c == '\0' && _end)
				{
					break;
				}
				switch (base.CurrentState)
				{
				case global::Newtonsoft.Json.JsonReader.State.Complete:
				case global::Newtonsoft.Json.JsonReader.State.Closed:
				case global::Newtonsoft.Json.JsonReader.State.Error:
					break;
				case global::Newtonsoft.Json.JsonReader.State.Start:
				case global::Newtonsoft.Json.JsonReader.State.Property:
				case global::Newtonsoft.Json.JsonReader.State.ArrayStart:
				case global::Newtonsoft.Json.JsonReader.State.Array:
				case global::Newtonsoft.Json.JsonReader.State.ConstructorStart:
				case global::Newtonsoft.Json.JsonReader.State.Constructor:
					return ParseValue(c);
				case global::Newtonsoft.Json.JsonReader.State.ObjectStart:
				case global::Newtonsoft.Json.JsonReader.State.Object:
					return ParseObject(c);
				case global::Newtonsoft.Json.JsonReader.State.PostValue:
					if (ParsePostValue(c))
					{
						return true;
					}
					break;
				default:
					throw CreateJsonReaderException("Unexpected state: {0}. Line {1}, position {2}.", base.CurrentState, _currentLineNumber, _currentLinePosition);
				}
			}
			return false;
		}

		private bool ParsePostValue(char currentChar)
		{
			do
			{
				switch (currentChar)
				{
				case '}':
					SetToken(global::Newtonsoft.Json.JsonToken.EndObject);
					return true;
				case ']':
					SetToken(global::Newtonsoft.Json.JsonToken.EndArray);
					return true;
				case ')':
					SetToken(global::Newtonsoft.Json.JsonToken.EndConstructor);
					return true;
				case '/':
					ParseComment();
					return true;
				case ',':
					SetStateBasedOnCurrent();
					return false;
				case '\t':
				case '\n':
				case '\r':
				case ' ':
					continue;
				}
				if (!char.IsWhiteSpace(currentChar))
				{
					throw CreateJsonReaderException("After parsing a value an unexpected character was encountered: {0}. Line {1}, position {2}.", currentChar, _currentLineNumber, _currentLinePosition);
				}
			}
			while ((currentChar = MoveNext()) != 0 || !_end);
			return false;
		}

		private bool ParseObject(char currentChar)
		{
			do
			{
				switch (currentChar)
				{
				case '}':
					SetToken(global::Newtonsoft.Json.JsonToken.EndObject);
					return true;
				case '/':
					ParseComment();
					return true;
				case '\t':
				case '\n':
				case '\r':
				case ' ':
					continue;
				}
				if (!char.IsWhiteSpace(currentChar))
				{
					return ParseProperty(currentChar);
				}
			}
			while ((currentChar = MoveNext()) != 0 || !_end);
			return false;
		}

		private bool ParseProperty(char firstChar)
		{
			char c = firstChar;
			char c2;
			if (ValidIdentifierChar(c))
			{
				c2 = '\0';
				c = ParseUnquotedProperty(c);
			}
			else
			{
				if (c != '"' && c != '\'')
				{
					throw CreateJsonReaderException("Invalid property identifier character: {0}. Line {1}, position {2}.", c, _currentLineNumber, _currentLinePosition);
				}
				c2 = c;
				ReadStringIntoBuffer(c2);
				c = MoveNext();
			}
			if (c != ':')
			{
				c = MoveNext();
				EatWhitespace(c, false, out c);
				if (c != ':')
				{
					throw CreateJsonReaderException("Invalid character after parsing property name. Expected ':' but got: {0}. Line {1}, position {2}.", c, _currentLineNumber, _currentLinePosition);
				}
			}
			SetToken(global::Newtonsoft.Json.JsonToken.PropertyName, _buffer.ToString());
			QuoteChar = c2;
			_buffer.Position = 0;
			return true;
		}

		private bool ValidIdentifierChar(char value)
		{
			if (!char.IsLetterOrDigit(value) && value != '_')
			{
				return value == '$';
			}
			return true;
		}

		private char ParseUnquotedProperty(char firstChar)
		{
			_buffer.Append(firstChar);
			char c;
			while ((c = MoveNext()) != 0 || !_end)
			{
				if (char.IsWhiteSpace(c) || c == ':')
				{
					return c;
				}
				if (ValidIdentifierChar(c))
				{
					_buffer.Append(c);
					continue;
				}
				throw CreateJsonReaderException("Invalid JavaScript property identifier character: {0}. Line {1}, position {2}.", c, _currentLineNumber, _currentLinePosition);
			}
			throw CreateJsonReaderException("Unexpected end when parsing unquoted property name. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
		}

		private bool ParseValue(char currentChar)
		{
			do
			{
				switch (currentChar)
				{
				case '"':
				case '\'':
					ParseString(currentChar);
					return true;
				case 't':
					ParseTrue();
					return true;
				case 'f':
					ParseFalse();
					return true;
				case 'n':
					if (HasNext())
					{
						switch ((char)(ushort)PeekNext())
						{
						case 'u':
							ParseNull();
							break;
						case 'e':
							ParseConstructor();
							break;
						default:
							throw CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", currentChar, _currentLineNumber, _currentLinePosition);
						}
						return true;
					}
					throw CreateJsonReaderException("Unexpected end. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
				case 'N':
					ParseNumberNaN();
					return true;
				case 'I':
					ParseNumberPositiveInfinity();
					return true;
				case '-':
					if (PeekNext() == 73)
					{
						ParseNumberNegativeInfinity();
					}
					else
					{
						ParseNumber(currentChar);
					}
					return true;
				case '/':
					ParseComment();
					return true;
				case 'u':
					ParseUndefined();
					return true;
				case '{':
					SetToken(global::Newtonsoft.Json.JsonToken.StartObject);
					return true;
				case '[':
					SetToken(global::Newtonsoft.Json.JsonToken.StartArray);
					return true;
				case '}':
					SetToken(global::Newtonsoft.Json.JsonToken.EndObject);
					return true;
				case ']':
					SetToken(global::Newtonsoft.Json.JsonToken.EndArray);
					return true;
				case ',':
					SetToken(global::Newtonsoft.Json.JsonToken.Undefined);
					return true;
				case ')':
					SetToken(global::Newtonsoft.Json.JsonToken.EndConstructor);
					return true;
				case '\t':
				case '\n':
				case '\r':
				case ' ':
					continue;
				}
				if (!char.IsWhiteSpace(currentChar))
				{
					if (char.IsNumber(currentChar) || currentChar == '-' || currentChar == '.')
					{
						ParseNumber(currentChar);
						return true;
					}
					throw CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", currentChar, _currentLineNumber, _currentLinePosition);
				}
			}
			while ((currentChar = MoveNext()) != 0 || !_end);
			return false;
		}

		private bool EatWhitespace(char initialChar, bool oneOrMore, out char finalChar)
		{
			bool result = false;
			char c = initialChar;
			while (c == ' ' || char.IsWhiteSpace(c))
			{
				result = true;
				c = MoveNext();
			}
			finalChar = c;
			if (oneOrMore)
			{
				return result;
			}
			return true;
		}

		private void ParseConstructor()
		{
			if (!MatchValue('n', "new", true))
			{
				return;
			}
			char finalChar = MoveNext();
			if (EatWhitespace(finalChar, true, out finalChar))
			{
				while (char.IsLetter(finalChar))
				{
					_buffer.Append(finalChar);
					finalChar = MoveNext();
				}
				EatWhitespace(finalChar, false, out finalChar);
				if (finalChar != '(')
				{
					throw CreateJsonReaderException("Unexpected character while parsing constructor: {0}. Line {1}, position {2}.", finalChar, _currentLineNumber, _currentLinePosition);
				}
				string value = _buffer.ToString();
				_buffer.Position = 0;
				SetToken(global::Newtonsoft.Json.JsonToken.StartConstructor, value);
			}
		}

		private void ParseNumber(char firstChar)
		{
			char c = firstChar;
			bool flag = false;
			do
			{
				if (IsSeperator(c))
				{
					flag = true;
					_lastChar = c;
				}
				else
				{
					_buffer.Append(c);
				}
			}
			while (!flag && ((c = MoveNext()) != 0 || !_end));
			string text = _buffer.ToString();
			bool flag2 = firstChar == '0' && !text.StartsWith("0.", global::System.StringComparison.OrdinalIgnoreCase);
			object value2;
			global::Newtonsoft.Json.JsonToken newToken;
			if (_readType == global::Newtonsoft.Json.JsonTextReader.ReadType.ReadAsDecimal)
			{
				if (flag2)
				{
					long value = (text.StartsWith("0x", global::System.StringComparison.OrdinalIgnoreCase) ? global::System.Convert.ToInt64(text, 16) : global::System.Convert.ToInt64(text, 8));
					value2 = global::System.Convert.ToDecimal(value);
				}
				else
				{
					value2 = decimal.Parse(text, global::System.Globalization.NumberStyles.Number | global::System.Globalization.NumberStyles.AllowExponent, global::System.Globalization.CultureInfo.InvariantCulture);
				}
				newToken = global::Newtonsoft.Json.JsonToken.Float;
			}
			else if (flag2)
			{
				value2 = (text.StartsWith("0x", global::System.StringComparison.OrdinalIgnoreCase) ? global::System.Convert.ToInt64(text, 16) : global::System.Convert.ToInt64(text, 8));
				newToken = global::Newtonsoft.Json.JsonToken.Integer;
			}
			else if (text.IndexOf(".", global::System.StringComparison.OrdinalIgnoreCase) != -1 || text.IndexOf("e", global::System.StringComparison.OrdinalIgnoreCase) != -1)
			{
				value2 = global::System.Convert.ToDouble(text, global::System.Globalization.CultureInfo.InvariantCulture);
				newToken = global::Newtonsoft.Json.JsonToken.Float;
			}
			else
			{
				try
				{
					value2 = global::System.Convert.ToInt64(text, global::System.Globalization.CultureInfo.InvariantCulture);
				}
				catch (global::System.OverflowException innerException)
				{
					throw new global::Newtonsoft.Json.JsonReaderException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("JSON integer {0} is too large or small for an Int64.", global::System.Globalization.CultureInfo.InvariantCulture, text), innerException);
				}
				newToken = global::Newtonsoft.Json.JsonToken.Integer;
			}
			_buffer.Position = 0;
			SetToken(newToken, value2);
		}

		private void ParseComment()
		{
			char c = MoveNext();
			if (c == '*')
			{
				while ((c = MoveNext()) != 0 || !_end)
				{
					if (c == '*')
					{
						if ((c = MoveNext()) != 0 || !_end)
						{
							if (c == '/')
							{
								break;
							}
							_buffer.Append('*');
							_buffer.Append(c);
						}
					}
					else
					{
						_buffer.Append(c);
					}
				}
				SetToken(global::Newtonsoft.Json.JsonToken.Comment, _buffer.ToString());
				_buffer.Position = 0;
				return;
			}
			throw CreateJsonReaderException("Error parsing comment. Expected: *. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
		}

		private bool MatchValue(char firstChar, string value)
		{
			char c = firstChar;
			int num = 0;
			while (c == value[num])
			{
				num++;
				if (num >= value.Length || ((c = MoveNext()) == '\0' && _end))
				{
					break;
				}
			}
			return num == value.Length;
		}

		private bool MatchValue(char firstChar, string value, bool noTrailingNonSeperatorCharacters)
		{
			bool flag = MatchValue(firstChar, value);
			if (!noTrailingNonSeperatorCharacters)
			{
				return flag;
			}
			int num = PeekNext();
			char c = ((num != -1) ? ((char)num) : '\0');
			return flag && (c == '\0' || IsSeperator(c));
		}

		private bool IsSeperator(char c)
		{
			switch (c)
			{
			case ',':
			case ']':
			case '}':
				return true;
			case '/':
				if (HasNext())
				{
					return PeekNext() == 42;
				}
				return false;
			case ')':
				if (base.CurrentState == global::Newtonsoft.Json.JsonReader.State.Constructor || base.CurrentState == global::Newtonsoft.Json.JsonReader.State.ConstructorStart)
				{
					return true;
				}
				break;
			case '\t':
			case '\n':
			case '\r':
			case ' ':
				return true;
			default:
				if (char.IsWhiteSpace(c))
				{
					return true;
				}
				break;
			}
			return false;
		}

		private void ParseTrue()
		{
			if (MatchValue('t', global::Newtonsoft.Json.JsonConvert.True, true))
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Boolean, true);
				return;
			}
			throw CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
		}

		private void ParseNull()
		{
			if (MatchValue('n', global::Newtonsoft.Json.JsonConvert.Null, true))
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Null);
				return;
			}
			throw CreateJsonReaderException("Error parsing null value. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
		}

		private void ParseUndefined()
		{
			if (MatchValue('u', global::Newtonsoft.Json.JsonConvert.Undefined, true))
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Undefined);
				return;
			}
			throw CreateJsonReaderException("Error parsing undefined value. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
		}

		private void ParseFalse()
		{
			if (MatchValue('f', global::Newtonsoft.Json.JsonConvert.False, true))
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Boolean, false);
				return;
			}
			throw CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
		}

		private void ParseNumberNegativeInfinity()
		{
			if (MatchValue('-', global::Newtonsoft.Json.JsonConvert.NegativeInfinity, true))
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Float, double.NegativeInfinity);
				return;
			}
			throw CreateJsonReaderException("Error parsing negative infinity value. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
		}

		private void ParseNumberPositiveInfinity()
		{
			if (MatchValue('I', global::Newtonsoft.Json.JsonConvert.PositiveInfinity, true))
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Float, double.PositiveInfinity);
				return;
			}
			throw CreateJsonReaderException("Error parsing positive infinity value. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
		}

		private void ParseNumberNaN()
		{
			if (MatchValue('N', global::Newtonsoft.Json.JsonConvert.NaN, true))
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Float, double.NaN);
				return;
			}
			throw CreateJsonReaderException("Error parsing NaN value. Line {0}, position {1}.", _currentLineNumber, _currentLinePosition);
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseInput && _reader != null)
			{
				_reader.Close();
			}
			if (_buffer != null)
			{
				_buffer.Clear();
			}
		}

		public bool HasLineInfo()
		{
			return true;
		}
	}
}
