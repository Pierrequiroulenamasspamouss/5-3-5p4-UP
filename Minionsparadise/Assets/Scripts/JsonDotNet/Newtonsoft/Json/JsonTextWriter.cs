namespace Newtonsoft.Json
{
	public class JsonTextWriter : global::Newtonsoft.Json.JsonWriter
	{
		private readonly global::System.IO.TextWriter _writer;

		private global::Newtonsoft.Json.Utilities.Base64Encoder _base64Encoder;

		private char _indentChar;

		private int _indentation;

		private char _quoteChar;

		private bool _quoteName;

		private global::Newtonsoft.Json.Utilities.Base64Encoder Base64Encoder
		{
			get
			{
				if (_base64Encoder == null)
				{
					_base64Encoder = new global::Newtonsoft.Json.Utilities.Base64Encoder(_writer);
				}
				return _base64Encoder;
			}
		}

		public int Indentation
		{
			get
			{
				return _indentation;
			}
			set
			{
				if (value < 0)
				{
					throw new global::System.ArgumentException("Indentation value must be greater than 0.");
				}
				_indentation = value;
			}
		}

		public char QuoteChar
		{
			get
			{
				return _quoteChar;
			}
			set
			{
				if (value != '"' && value != '\'')
				{
					throw new global::System.ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
				}
				_quoteChar = value;
			}
		}

		public char IndentChar
		{
			get
			{
				return _indentChar;
			}
			set
			{
				_indentChar = value;
			}
		}

		public bool QuoteName
		{
			get
			{
				return _quoteName;
			}
			set
			{
				_quoteName = value;
			}
		}

		public JsonTextWriter(global::System.IO.TextWriter textWriter)
		{
			if (textWriter == null)
			{
				throw new global::System.ArgumentNullException("textWriter");
			}
			_writer = textWriter;
			_quoteChar = '"';
			_quoteName = true;
			_indentChar = ' ';
			_indentation = 2;
		}

		public override void Flush()
		{
			_writer.Flush();
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseOutput && _writer != null)
			{
				_writer.Close();
			}
		}

		public override void WriteStartObject()
		{
			base.WriteStartObject();
			_writer.Write("{");
		}

		public override void WriteStartArray()
		{
			base.WriteStartArray();
			_writer.Write("[");
		}

		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			_writer.Write("new ");
			_writer.Write(name);
			_writer.Write("(");
		}

		protected override void WriteEnd(global::Newtonsoft.Json.JsonToken token)
		{
			switch (token)
			{
			case global::Newtonsoft.Json.JsonToken.EndObject:
				_writer.Write("}");
				break;
			case global::Newtonsoft.Json.JsonToken.EndArray:
				_writer.Write("]");
				break;
			case global::Newtonsoft.Json.JsonToken.EndConstructor:
				_writer.Write(")");
				break;
			default:
				throw new global::Newtonsoft.Json.JsonWriterException("Invalid JsonToken: " + token);
			}
		}

		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			global::Newtonsoft.Json.Utilities.JavaScriptUtils.WriteEscapedJavaScriptString(_writer, name, _quoteChar, _quoteName);
			_writer.Write(':');
		}

		protected override void WriteIndent()
		{
			if (base.Formatting == global::Newtonsoft.Json.Formatting.Indented)
			{
				_writer.Write(global::System.Environment.NewLine);
				int num = base.Top * _indentation;
				for (int i = 0; i < num; i++)
				{
					_writer.Write(_indentChar);
				}
			}
		}

		protected override void WriteValueDelimiter()
		{
			_writer.Write(',');
		}

		protected override void WriteIndentSpace()
		{
			_writer.Write(' ');
		}

		private void WriteValueInternal(string value, global::Newtonsoft.Json.JsonToken token)
		{
			_writer.Write(value);
		}

		public override void WriteNull()
		{
			base.WriteNull();
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.Null, global::Newtonsoft.Json.JsonToken.Null);
		}

		public override void WriteUndefined()
		{
			base.WriteUndefined();
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.Undefined, global::Newtonsoft.Json.JsonToken.Undefined);
		}

		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			_writer.Write(json);
		}

		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			if (value == null)
			{
				WriteValueInternal(global::Newtonsoft.Json.JsonConvert.Null, global::Newtonsoft.Json.JsonToken.Null);
			}
			else
			{
				global::Newtonsoft.Json.Utilities.JavaScriptUtils.WriteEscapedJavaScriptString(_writer, value, _quoteChar, true);
			}
		}

		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Float);
		}

		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Float);
		}

		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Boolean);
		}

		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Float);
		}

		public override void WriteValue(global::System.DateTime value)
		{
			base.WriteValue(value);
			global::Newtonsoft.Json.JsonConvert.WriteDateTimeString(_writer, value);
		}

		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			if (value != null)
			{
				_writer.Write(_quoteChar);
				Base64Encoder.Encode(value, 0, value.Length);
				Base64Encoder.Flush();
				_writer.Write(_quoteChar);
			}
		}

		public override void WriteValue(global::System.DateTimeOffset value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Date);
		}

		public override void WriteValue(global::System.Guid value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.String);
		}

		public override void WriteValue(global::System.TimeSpan value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Date);
		}

		public override void WriteValue(global::System.Uri value)
		{
			base.WriteValue(value);
			WriteValueInternal(global::Newtonsoft.Json.JsonConvert.ToString(value), global::Newtonsoft.Json.JsonToken.Date);
		}

		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			_writer.Write("/*");
			_writer.Write(text);
			_writer.Write("*/");
		}

		public override void WriteWhitespace(string ws)
		{
			base.WriteWhitespace(ws);
			_writer.Write(ws);
		}
	}
}
