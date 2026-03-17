namespace Newtonsoft.Json.Bson
{
	internal class BsonBinaryWriter
	{
		private static readonly global::System.Text.Encoding Encoding = global::System.Text.Encoding.UTF8;

		private readonly global::System.IO.BinaryWriter _writer;

		private byte[] _largeByteBuffer;

		private int _maxChars;

		public global::System.DateTimeKind DateTimeKindHandling { get; set; }

		public BsonBinaryWriter(global::System.IO.Stream stream)
		{
			DateTimeKindHandling = global::System.DateTimeKind.Utc;
			_writer = new global::System.IO.BinaryWriter(stream);
		}

		public void Flush()
		{
			_writer.Flush();
		}

		public void Close()
		{
			_writer.Close();
		}

		public void WriteToken(global::Newtonsoft.Json.Bson.BsonToken t)
		{
			CalculateSize(t);
			WriteTokenInternal(t);
		}

		private void WriteTokenInternal(global::Newtonsoft.Json.Bson.BsonToken t)
		{
			switch (t.Type)
			{
			case global::Newtonsoft.Json.Bson.BsonType.Object:
			{
				global::Newtonsoft.Json.Bson.BsonObject bsonObject = (global::Newtonsoft.Json.Bson.BsonObject)t;
				_writer.Write(bsonObject.CalculatedSize);
				foreach (global::Newtonsoft.Json.Bson.BsonProperty item in bsonObject)
				{
					_writer.Write((sbyte)item.Value.Type);
					WriteString((string)item.Name.Value, item.Name.ByteCount, null);
					WriteTokenInternal(item.Value);
				}
				_writer.Write((byte)0);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Array:
			{
				global::Newtonsoft.Json.Bson.BsonArray bsonArray = (global::Newtonsoft.Json.Bson.BsonArray)t;
				_writer.Write(bsonArray.CalculatedSize);
				int num2 = 0;
				foreach (global::Newtonsoft.Json.Bson.BsonToken item2 in bsonArray)
				{
					_writer.Write((sbyte)item2.Type);
					WriteString(num2.ToString(global::System.Globalization.CultureInfo.InvariantCulture), global::Newtonsoft.Json.Utilities.MathUtils.IntLength(num2), null);
					WriteTokenInternal(item2);
					num2++;
				}
				_writer.Write((byte)0);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Integer:
			{
				global::Newtonsoft.Json.Bson.BsonValue bsonValue4 = (global::Newtonsoft.Json.Bson.BsonValue)t;
				_writer.Write(global::System.Convert.ToInt32(bsonValue4.Value, global::System.Globalization.CultureInfo.InvariantCulture));
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Long:
			{
				global::Newtonsoft.Json.Bson.BsonValue bsonValue5 = (global::Newtonsoft.Json.Bson.BsonValue)t;
				_writer.Write(global::System.Convert.ToInt64(bsonValue5.Value, global::System.Globalization.CultureInfo.InvariantCulture));
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Number:
			{
				global::Newtonsoft.Json.Bson.BsonValue bsonValue6 = (global::Newtonsoft.Json.Bson.BsonValue)t;
				_writer.Write(global::System.Convert.ToDouble(bsonValue6.Value, global::System.Globalization.CultureInfo.InvariantCulture));
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.String:
			{
				global::Newtonsoft.Json.Bson.BsonString bsonString = (global::Newtonsoft.Json.Bson.BsonString)t;
				WriteString((string)bsonString.Value, bsonString.ByteCount, bsonString.CalculatedSize - 4);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Boolean:
			{
				global::Newtonsoft.Json.Bson.BsonValue bsonValue7 = (global::Newtonsoft.Json.Bson.BsonValue)t;
				_writer.Write((bool)bsonValue7.Value);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Date:
			{
				global::Newtonsoft.Json.Bson.BsonValue bsonValue3 = (global::Newtonsoft.Json.Bson.BsonValue)t;
				long num = 0L;
				if (bsonValue3.Value is global::System.DateTime)
				{
					global::System.DateTime dateTime = (global::System.DateTime)bsonValue3.Value;
					if (DateTimeKindHandling == global::System.DateTimeKind.Utc)
					{
						dateTime = dateTime.ToUniversalTime();
					}
					else if (DateTimeKindHandling == global::System.DateTimeKind.Local)
					{
						dateTime = dateTime.ToLocalTime();
					}
					num = global::Newtonsoft.Json.JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTime, false);
				}
				else
				{
					global::System.DateTimeOffset dateTimeOffset = (global::System.DateTimeOffset)bsonValue3.Value;
					num = global::Newtonsoft.Json.JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.UtcDateTime, dateTimeOffset.Offset);
				}
				_writer.Write(num);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Binary:
			{
				global::Newtonsoft.Json.Bson.BsonValue bsonValue2 = (global::Newtonsoft.Json.Bson.BsonValue)t;
				byte[] array = (byte[])bsonValue2.Value;
				_writer.Write(array.Length);
				_writer.Write((byte)0);
				_writer.Write(array);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Oid:
			{
				global::Newtonsoft.Json.Bson.BsonValue bsonValue = (global::Newtonsoft.Json.Bson.BsonValue)t;
				byte[] buffer = (byte[])bsonValue.Value;
				_writer.Write(buffer);
				break;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Regex:
			{
				global::Newtonsoft.Json.Bson.BsonRegex bsonRegex = (global::Newtonsoft.Json.Bson.BsonRegex)t;
				WriteString((string)bsonRegex.Pattern.Value, bsonRegex.Pattern.ByteCount, null);
				WriteString((string)bsonRegex.Options.Value, bsonRegex.Options.ByteCount, null);
				break;
			}
			default:
				throw new global::System.ArgumentOutOfRangeException("t", global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected token when writing BSON: {0}", global::System.Globalization.CultureInfo.InvariantCulture, t.Type));
			case global::Newtonsoft.Json.Bson.BsonType.Undefined:
			case global::Newtonsoft.Json.Bson.BsonType.Null:
				break;
			}
		}

		private void WriteString(string s, int byteCount, int? calculatedlengthPrefix)
		{
			if (calculatedlengthPrefix.HasValue)
			{
				_writer.Write(calculatedlengthPrefix.Value);
			}
			if (s != null)
			{
				if (_largeByteBuffer == null)
				{
					_largeByteBuffer = new byte[256];
					_maxChars = 256 / Encoding.GetMaxByteCount(1);
				}
				if (byteCount <= 256)
				{
					Encoding.GetBytes(s, 0, s.Length, _largeByteBuffer, 0);
					_writer.Write(_largeByteBuffer, 0, byteCount);
				}
				else
				{
					int num = 0;
					int num2 = s.Length;
					while (num2 > 0)
					{
						int num3 = ((num2 > _maxChars) ? _maxChars : num2);
						int bytes = Encoding.GetBytes(s, num, num3, _largeByteBuffer, 0);
						_writer.Write(_largeByteBuffer, 0, bytes);
						num += num3;
						num2 -= num3;
					}
				}
			}
			_writer.Write((byte)0);
		}

		private int CalculateSize(int stringByteCount)
		{
			return stringByteCount + 1;
		}

		private int CalculateSizeWithLength(int stringByteCount, bool includeSize)
		{
			int num = ((!includeSize) ? 1 : 5);
			return num + stringByteCount;
		}

		private int CalculateSize(global::Newtonsoft.Json.Bson.BsonToken t)
		{
			switch (t.Type)
			{
			case global::Newtonsoft.Json.Bson.BsonType.Object:
			{
				global::Newtonsoft.Json.Bson.BsonObject bsonObject = (global::Newtonsoft.Json.Bson.BsonObject)t;
				int num4 = 4;
				foreach (global::Newtonsoft.Json.Bson.BsonProperty item in bsonObject)
				{
					int num5 = 1;
					num5 += CalculateSize(item.Name);
					num5 += CalculateSize(item.Value);
					num4 += num5;
				}
				return bsonObject.CalculatedSize = num4 + 1;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Array:
			{
				global::Newtonsoft.Json.Bson.BsonArray bsonArray = (global::Newtonsoft.Json.Bson.BsonArray)t;
				int num2 = 4;
				int num3 = 0;
				foreach (global::Newtonsoft.Json.Bson.BsonToken item2 in bsonArray)
				{
					num2++;
					num2 += CalculateSize(global::Newtonsoft.Json.Utilities.MathUtils.IntLength(num3));
					num2 += CalculateSize(item2);
					num3++;
				}
				num2++;
				bsonArray.CalculatedSize = num2;
				return bsonArray.CalculatedSize;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Integer:
				return 4;
			case global::Newtonsoft.Json.Bson.BsonType.Long:
				return 8;
			case global::Newtonsoft.Json.Bson.BsonType.Number:
				return 8;
			case global::Newtonsoft.Json.Bson.BsonType.String:
			{
				global::Newtonsoft.Json.Bson.BsonString bsonString = (global::Newtonsoft.Json.Bson.BsonString)t;
				string text = (string)bsonString.Value;
				bsonString.ByteCount = ((text != null) ? Encoding.GetByteCount(text) : 0);
				bsonString.CalculatedSize = CalculateSizeWithLength(bsonString.ByteCount, bsonString.IncludeLength);
				return bsonString.CalculatedSize;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Boolean:
				return 1;
			case global::Newtonsoft.Json.Bson.BsonType.Undefined:
			case global::Newtonsoft.Json.Bson.BsonType.Null:
				return 0;
			case global::Newtonsoft.Json.Bson.BsonType.Date:
				return 8;
			case global::Newtonsoft.Json.Bson.BsonType.Binary:
			{
				global::Newtonsoft.Json.Bson.BsonValue bsonValue = (global::Newtonsoft.Json.Bson.BsonValue)t;
				byte[] array = (byte[])bsonValue.Value;
				bsonValue.CalculatedSize = 5 + array.Length;
				return bsonValue.CalculatedSize;
			}
			case global::Newtonsoft.Json.Bson.BsonType.Oid:
				return 12;
			case global::Newtonsoft.Json.Bson.BsonType.Regex:
			{
				global::Newtonsoft.Json.Bson.BsonRegex bsonRegex = (global::Newtonsoft.Json.Bson.BsonRegex)t;
				int num = 0;
				num += CalculateSize(bsonRegex.Pattern);
				num += CalculateSize(bsonRegex.Options);
				bsonRegex.CalculatedSize = num;
				return bsonRegex.CalculatedSize;
			}
			default:
				throw new global::System.ArgumentOutOfRangeException("t", global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected token when writing BSON: {0}", global::System.Globalization.CultureInfo.InvariantCulture, t.Type));
			}
		}
	}
}
