namespace Newtonsoft.Json.Bson
{
	public class BsonReader : global::Newtonsoft.Json.JsonReader
	{
		private enum BsonReaderState
		{
			Normal = 0,
			ReferenceStart = 1,
			ReferenceRef = 2,
			ReferenceId = 3,
			CodeWScopeStart = 4,
			CodeWScopeCode = 5,
			CodeWScopeScope = 6,
			CodeWScopeScopeObject = 7,
			CodeWScopeScopeEnd = 8
		}

		private class ContainerContext
		{
			public readonly global::Newtonsoft.Json.Bson.BsonType Type;

			public int Length;

			public int Position;

			public ContainerContext(global::Newtonsoft.Json.Bson.BsonType type)
			{
				Type = type;
			}
		}

		private const int MaxCharBytesSize = 128;

		private static readonly byte[] _seqRange1 = new byte[2] { 0, 127 };

		private static readonly byte[] _seqRange2 = new byte[2] { 194, 223 };

		private static readonly byte[] _seqRange3 = new byte[2] { 224, 239 };

		private static readonly byte[] _seqRange4 = new byte[2] { 240, 244 };

		private readonly global::System.IO.BinaryReader _reader;

		private readonly global::System.Collections.Generic.List<global::Newtonsoft.Json.Bson.BsonReader.ContainerContext> _stack;

		private byte[] _byteBuffer;

		private char[] _charBuffer;

		private global::Newtonsoft.Json.Bson.BsonType _currentElementType;

		private global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState _bsonReaderState;

		private global::Newtonsoft.Json.Bson.BsonReader.ContainerContext _currentContext;

		private bool _readRootValueAsArray;

		private bool _jsonNet35BinaryCompatibility;

		private global::System.DateTimeKind _dateTimeKindHandling;

		public bool JsonNet35BinaryCompatibility
		{
			get
			{
				return _jsonNet35BinaryCompatibility;
			}
			set
			{
				_jsonNet35BinaryCompatibility = value;
			}
		}

		public bool ReadRootValueAsArray
		{
			get
			{
				return _readRootValueAsArray;
			}
			set
			{
				_readRootValueAsArray = value;
			}
		}

		public global::System.DateTimeKind DateTimeKindHandling
		{
			get
			{
				return _dateTimeKindHandling;
			}
			set
			{
				_dateTimeKindHandling = value;
			}
		}

		public BsonReader(global::System.IO.Stream stream)
			: this(stream, false, global::System.DateTimeKind.Local)
		{
		}

		public BsonReader(global::System.IO.Stream stream, bool readRootValueAsArray, global::System.DateTimeKind dateTimeKindHandling)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(stream, "stream");
			_reader = new global::System.IO.BinaryReader(stream);
			_stack = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Bson.BsonReader.ContainerContext>();
			_readRootValueAsArray = readRootValueAsArray;
			_dateTimeKindHandling = dateTimeKindHandling;
		}

		private string ReadElement()
		{
			_currentElementType = ReadType();
			return ReadString();
		}

		public override byte[] ReadAsBytes()
		{
			Read();
			if (TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			if (TokenType == global::Newtonsoft.Json.JsonToken.Bytes)
			{
				return (byte[])Value;
			}
			throw new global::Newtonsoft.Json.JsonReaderException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error reading bytes. Expected bytes but got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, TokenType));
		}

		public override decimal? ReadAsDecimal()
		{
			Read();
			if (TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			if (TokenType == global::Newtonsoft.Json.JsonToken.Integer || TokenType == global::Newtonsoft.Json.JsonToken.Float)
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Float, global::System.Convert.ToDecimal(Value, global::System.Globalization.CultureInfo.InvariantCulture));
				return (decimal)Value;
			}
			throw new global::Newtonsoft.Json.JsonReaderException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error reading decimal. Expected a number but got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, TokenType));
		}

		public override global::System.DateTimeOffset? ReadAsDateTimeOffset()
		{
			Read();
			if (TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			if (TokenType == global::Newtonsoft.Json.JsonToken.Date)
			{
				SetToken(global::Newtonsoft.Json.JsonToken.Date, new global::System.DateTimeOffset((global::System.DateTime)Value));
				return (global::System.DateTimeOffset)Value;
			}
			throw new global::Newtonsoft.Json.JsonReaderException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error reading date. Expected bytes but got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, TokenType));
		}

		public override bool Read()
		{
			try
			{
				switch (_bsonReaderState)
				{
				case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.Normal:
					return ReadNormal();
				case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceStart:
				case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceRef:
				case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceId:
					return ReadReference();
				case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeStart:
				case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeCode:
				case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeScope:
				case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeScopeObject:
				case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeScopeEnd:
					return ReadCodeWScope();
				default:
					throw new global::Newtonsoft.Json.JsonReaderException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected state: {0}", global::System.Globalization.CultureInfo.InvariantCulture, _bsonReaderState));
				}
			}
			catch (global::System.IO.EndOfStreamException)
			{
				return false;
			}
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseInput && _reader != null)
			{
				_reader.Close();
			}
		}

		private bool ReadCodeWScope()
		{
			switch (_bsonReaderState)
			{
			case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeStart:
				SetToken(global::Newtonsoft.Json.JsonToken.PropertyName, "$code");
				_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeCode;
				return true;
			case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeCode:
				ReadInt32();
				SetToken(global::Newtonsoft.Json.JsonToken.String, ReadLengthString());
				_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeScope;
				return true;
			case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeScope:
			{
				if (base.CurrentState == global::Newtonsoft.Json.JsonReader.State.PostValue)
				{
					SetToken(global::Newtonsoft.Json.JsonToken.PropertyName, "$scope");
					return true;
				}
				SetToken(global::Newtonsoft.Json.JsonToken.StartObject);
				_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeScopeObject;
				global::Newtonsoft.Json.Bson.BsonReader.ContainerContext containerContext = new global::Newtonsoft.Json.Bson.BsonReader.ContainerContext(global::Newtonsoft.Json.Bson.BsonType.Object);
				PushContext(containerContext);
				containerContext.Length = ReadInt32();
				return true;
			}
			case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeScopeObject:
			{
				bool flag = ReadNormal();
				if (flag && TokenType == global::Newtonsoft.Json.JsonToken.EndObject)
				{
					_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeScopeEnd;
				}
				return flag;
			}
			case global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeScopeEnd:
				SetToken(global::Newtonsoft.Json.JsonToken.EndObject);
				_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.Normal;
				return true;
			default:
				throw new global::System.ArgumentOutOfRangeException();
			}
		}

		private bool ReadReference()
		{
			switch (base.CurrentState)
			{
			case global::Newtonsoft.Json.JsonReader.State.ObjectStart:
				SetToken(global::Newtonsoft.Json.JsonToken.PropertyName, "$ref");
				_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceRef;
				return true;
			case global::Newtonsoft.Json.JsonReader.State.Property:
				if (_bsonReaderState == global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceRef)
				{
					SetToken(global::Newtonsoft.Json.JsonToken.String, ReadLengthString());
					return true;
				}
				if (_bsonReaderState == global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceId)
				{
					SetToken(global::Newtonsoft.Json.JsonToken.Bytes, ReadBytes(12));
					return true;
				}
				throw new global::Newtonsoft.Json.JsonReaderException("Unexpected state when reading BSON reference: " + _bsonReaderState);
			case global::Newtonsoft.Json.JsonReader.State.PostValue:
				if (_bsonReaderState == global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceRef)
				{
					SetToken(global::Newtonsoft.Json.JsonToken.PropertyName, "$id");
					_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceId;
					return true;
				}
				if (_bsonReaderState == global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceId)
				{
					SetToken(global::Newtonsoft.Json.JsonToken.EndObject);
					_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.Normal;
					return true;
				}
				throw new global::Newtonsoft.Json.JsonReaderException("Unexpected state when reading BSON reference: " + _bsonReaderState);
			default:
				throw new global::Newtonsoft.Json.JsonReaderException("Unexpected state when reading BSON reference: " + base.CurrentState);
			}
		}

		private bool ReadNormal()
		{
			switch (base.CurrentState)
			{
			case global::Newtonsoft.Json.JsonReader.State.Start:
			{
				global::Newtonsoft.Json.JsonToken token2 = ((!_readRootValueAsArray) ? global::Newtonsoft.Json.JsonToken.StartObject : global::Newtonsoft.Json.JsonToken.StartArray);
				global::Newtonsoft.Json.Bson.BsonType type = ((!_readRootValueAsArray) ? global::Newtonsoft.Json.Bson.BsonType.Object : global::Newtonsoft.Json.Bson.BsonType.Array);
				SetToken(token2);
				global::Newtonsoft.Json.Bson.BsonReader.ContainerContext containerContext = new global::Newtonsoft.Json.Bson.BsonReader.ContainerContext(type);
				PushContext(containerContext);
				containerContext.Length = ReadInt32();
				return true;
			}
			case global::Newtonsoft.Json.JsonReader.State.Complete:
			case global::Newtonsoft.Json.JsonReader.State.Closed:
				return false;
			case global::Newtonsoft.Json.JsonReader.State.Property:
				ReadType(_currentElementType);
				return true;
			case global::Newtonsoft.Json.JsonReader.State.ObjectStart:
			case global::Newtonsoft.Json.JsonReader.State.ArrayStart:
			case global::Newtonsoft.Json.JsonReader.State.PostValue:
			{
				global::Newtonsoft.Json.Bson.BsonReader.ContainerContext currentContext = _currentContext;
				if (currentContext == null)
				{
					return false;
				}
				int num = currentContext.Length - 1;
				if (currentContext.Position < num)
				{
					if (currentContext.Type == global::Newtonsoft.Json.Bson.BsonType.Array)
					{
						ReadElement();
						ReadType(_currentElementType);
						return true;
					}
					SetToken(global::Newtonsoft.Json.JsonToken.PropertyName, ReadElement());
					return true;
				}
				if (currentContext.Position == num)
				{
					if (ReadByte() != 0)
					{
						throw new global::Newtonsoft.Json.JsonReaderException("Unexpected end of object byte value.");
					}
					PopContext();
					if (_currentContext != null)
					{
						MovePosition(currentContext.Length);
					}
					global::Newtonsoft.Json.JsonToken token = ((currentContext.Type == global::Newtonsoft.Json.Bson.BsonType.Object) ? global::Newtonsoft.Json.JsonToken.EndObject : global::Newtonsoft.Json.JsonToken.EndArray);
					SetToken(token);
					return true;
				}
				throw new global::Newtonsoft.Json.JsonReaderException("Read past end of current container context.");
			}
			default:
				throw new global::System.ArgumentOutOfRangeException();
			case global::Newtonsoft.Json.JsonReader.State.ConstructorStart:
			case global::Newtonsoft.Json.JsonReader.State.Constructor:
			case global::Newtonsoft.Json.JsonReader.State.Error:
			case global::Newtonsoft.Json.JsonReader.State.Finished:
				return false;
			}
		}

		private void PopContext()
		{
			_stack.RemoveAt(_stack.Count - 1);
			if (_stack.Count == 0)
			{
				_currentContext = null;
			}
			else
			{
				_currentContext = _stack[_stack.Count - 1];
			}
		}

		private void PushContext(global::Newtonsoft.Json.Bson.BsonReader.ContainerContext newContext)
		{
			_stack.Add(newContext);
			_currentContext = newContext;
		}

		private byte ReadByte()
		{
			MovePosition(1);
			return _reader.ReadByte();
		}

		private void ReadType(global::Newtonsoft.Json.Bson.BsonType type)
		{
			switch (type)
			{
			case global::Newtonsoft.Json.Bson.BsonType.Number:
				SetToken(global::Newtonsoft.Json.JsonToken.Float, ReadDouble());
				break;
			case global::Newtonsoft.Json.Bson.BsonType.String:
			case global::Newtonsoft.Json.Bson.BsonType.Symbol:
				SetToken(global::Newtonsoft.Json.JsonToken.String, ReadLengthString());
				break;
			case global::Newtonsoft.Json.Bson.BsonType.Object:
			{
				SetToken(global::Newtonsoft.Json.JsonToken.StartObject);
				global::Newtonsoft.Json.Bson.BsonReader.ContainerContext containerContext2 = new global::Newtonsoft.Json.Bson.BsonReader.ContainerContext(global::Newtonsoft.Json.Bson.BsonType.Object);
				PushContext(containerContext2);
				containerContext2.Length = ReadInt32();
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Array:
			{
				SetToken(global::Newtonsoft.Json.JsonToken.StartArray);
				global::Newtonsoft.Json.Bson.BsonReader.ContainerContext containerContext = new global::Newtonsoft.Json.Bson.BsonReader.ContainerContext(global::Newtonsoft.Json.Bson.BsonType.Array);
				PushContext(containerContext);
				containerContext.Length = ReadInt32();
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Binary:
				SetToken(global::Newtonsoft.Json.JsonToken.Bytes, ReadBinary());
				break;
			case global::Newtonsoft.Json.Bson.BsonType.Undefined:
				SetToken(global::Newtonsoft.Json.JsonToken.Undefined);
				break;
			case global::Newtonsoft.Json.Bson.BsonType.Oid:
			{
				byte[] value2 = ReadBytes(12);
				SetToken(global::Newtonsoft.Json.JsonToken.Bytes, value2);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Boolean:
			{
				bool flag = global::System.Convert.ToBoolean(ReadByte());
				SetToken(global::Newtonsoft.Json.JsonToken.Boolean, flag);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Date:
			{
				long javaScriptTicks = ReadInt64();
				global::System.DateTime dateTime = global::Newtonsoft.Json.JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
				global::System.DateTime dateTime2;
				switch (DateTimeKindHandling)
				{
				case global::System.DateTimeKind.Unspecified:
					dateTime2 = global::System.DateTime.SpecifyKind(dateTime, global::System.DateTimeKind.Unspecified);
					break;
				case global::System.DateTimeKind.Local:
					dateTime2 = dateTime.ToLocalTime();
					break;
				default:
					dateTime2 = dateTime;
					break;
				}
				SetToken(global::Newtonsoft.Json.JsonToken.Date, dateTime2);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Null:
				SetToken(global::Newtonsoft.Json.JsonToken.Null);
				break;
			case global::Newtonsoft.Json.Bson.BsonType.Regex:
			{
				string text = ReadString();
				string text2 = ReadString();
				string value = "/" + text + "/" + text2;
				SetToken(global::Newtonsoft.Json.JsonToken.String, value);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Reference:
				SetToken(global::Newtonsoft.Json.JsonToken.StartObject);
				_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.ReferenceStart;
				break;
			case global::Newtonsoft.Json.Bson.BsonType.Code:
				SetToken(global::Newtonsoft.Json.JsonToken.String, ReadLengthString());
				break;
			case global::Newtonsoft.Json.Bson.BsonType.CodeWScope:
				SetToken(global::Newtonsoft.Json.JsonToken.StartObject);
				_bsonReaderState = global::Newtonsoft.Json.Bson.BsonReader.BsonReaderState.CodeWScopeStart;
				break;
			case global::Newtonsoft.Json.Bson.BsonType.Integer:
				SetToken(global::Newtonsoft.Json.JsonToken.Integer, (long)ReadInt32());
				break;
			case global::Newtonsoft.Json.Bson.BsonType.TimeStamp:
			case global::Newtonsoft.Json.Bson.BsonType.Long:
				SetToken(global::Newtonsoft.Json.JsonToken.Integer, ReadInt64());
				break;
			default:
				throw new global::System.ArgumentOutOfRangeException("type", "Unexpected BsonType value: " + type);
			}
		}

		private byte[] ReadBinary()
		{
			int count = ReadInt32();
			global::Newtonsoft.Json.Bson.BsonBinaryType bsonBinaryType = (global::Newtonsoft.Json.Bson.BsonBinaryType)ReadByte();
			if (bsonBinaryType == global::Newtonsoft.Json.Bson.BsonBinaryType.Data && !_jsonNet35BinaryCompatibility)
			{
				count = ReadInt32();
			}
			return ReadBytes(count);
		}

		private string ReadString()
		{
			EnsureBuffers();
			global::System.Text.StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			while (true)
			{
				int num3 = num2;
				byte b;
				while (num3 < 128 && (b = _reader.ReadByte()) > 0)
				{
					_byteBuffer[num3++] = b;
				}
				int num4 = num3 - num2;
				num += num4;
				if (num3 < 128 && stringBuilder == null)
				{
					int chars = global::System.Text.Encoding.UTF8.GetChars(_byteBuffer, 0, num4, _charBuffer, 0);
					MovePosition(num + 1);
					return new string(_charBuffer, 0, chars);
				}
				int lastFullCharStop = GetLastFullCharStop(num3 - 1);
				int chars2 = global::System.Text.Encoding.UTF8.GetChars(_byteBuffer, 0, lastFullCharStop + 1, _charBuffer, 0);
				if (stringBuilder == null)
				{
					stringBuilder = new global::System.Text.StringBuilder(256);
				}
				stringBuilder.Append(_charBuffer, 0, chars2);
				if (lastFullCharStop < num4 - 1)
				{
					num2 = num4 - lastFullCharStop - 1;
					global::System.Array.Copy(_byteBuffer, lastFullCharStop + 1, _byteBuffer, 0, num2);
					continue;
				}
				if (num3 < 128)
				{
					break;
				}
				num2 = 0;
			}
			MovePosition(num + 1);
			return stringBuilder.ToString();
		}

		private string ReadLengthString()
		{
			int num = ReadInt32();
			MovePosition(num);
			string result = GetString(num - 1);
			_reader.ReadByte();
			return result;
		}

		private string GetString(int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			EnsureBuffers();
			global::System.Text.StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			do
			{
				int count = ((length - num > 128 - num2) ? (128 - num2) : (length - num));
				int num3 = _reader.BaseStream.Read(_byteBuffer, num2, count);
				if (num3 == 0)
				{
					throw new global::System.IO.EndOfStreamException("Unable to read beyond the end of the stream.");
				}
				num += num3;
				num3 += num2;
				if (num3 == length)
				{
					int chars = global::System.Text.Encoding.UTF8.GetChars(_byteBuffer, 0, num3, _charBuffer, 0);
					return new string(_charBuffer, 0, chars);
				}
				int lastFullCharStop = GetLastFullCharStop(num3 - 1);
				if (stringBuilder == null)
				{
					stringBuilder = new global::System.Text.StringBuilder(length);
				}
				int chars2 = global::System.Text.Encoding.UTF8.GetChars(_byteBuffer, 0, lastFullCharStop + 1, _charBuffer, 0);
				stringBuilder.Append(_charBuffer, 0, chars2);
				if (lastFullCharStop < num3 - 1)
				{
					num2 = num3 - lastFullCharStop - 1;
					global::System.Array.Copy(_byteBuffer, lastFullCharStop + 1, _byteBuffer, 0, num2);
				}
				else
				{
					num2 = 0;
				}
			}
			while (num < length);
			return stringBuilder.ToString();
		}

		private int GetLastFullCharStop(int start)
		{
			int num = start;
			int num2 = 0;
			for (; num >= 0; num--)
			{
				num2 = BytesInSequence(_byteBuffer[num]);
				switch (num2)
				{
				case 0:
					continue;
				default:
					num--;
					break;
				case 1:
					break;
				}
				break;
			}
			if (num2 == start - num)
			{
				return start;
			}
			return num;
		}

		private int BytesInSequence(byte b)
		{
			if (b <= _seqRange1[1])
			{
				return 1;
			}
			if (b >= _seqRange2[0] && b <= _seqRange2[1])
			{
				return 2;
			}
			if (b >= _seqRange3[0] && b <= _seqRange3[1])
			{
				return 3;
			}
			if (b >= _seqRange4[0] && b <= _seqRange4[1])
			{
				return 4;
			}
			return 0;
		}

		private void EnsureBuffers()
		{
			if (_byteBuffer == null)
			{
				_byteBuffer = new byte[128];
			}
			if (_charBuffer == null)
			{
				int maxCharCount = global::System.Text.Encoding.UTF8.GetMaxCharCount(128);
				_charBuffer = new char[maxCharCount];
			}
		}

		private double ReadDouble()
		{
			MovePosition(8);
			return _reader.ReadDouble();
		}

		private int ReadInt32()
		{
			MovePosition(4);
			return _reader.ReadInt32();
		}

		private long ReadInt64()
		{
			MovePosition(8);
			return _reader.ReadInt64();
		}

		private global::Newtonsoft.Json.Bson.BsonType ReadType()
		{
			MovePosition(1);
			return (global::Newtonsoft.Json.Bson.BsonType)_reader.ReadSByte();
		}

		private void MovePosition(int count)
		{
			_currentContext.Position += count;
		}

		private byte[] ReadBytes(int count)
		{
			MovePosition(count);
			return _reader.ReadBytes(count);
		}
	}
}
