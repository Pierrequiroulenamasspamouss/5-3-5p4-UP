namespace Newtonsoft.Json.Serialization
{
	internal class JsonFormatterConverter : global::System.Runtime.Serialization.IFormatterConverter
	{
		private readonly global::Newtonsoft.Json.JsonSerializer _serializer;

		public JsonFormatterConverter(global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(serializer, "serializer");
			_serializer = serializer;
		}

		private T GetTokenValue<T>(object value)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(value, "value");
			global::Newtonsoft.Json.Linq.JValue jValue = (global::Newtonsoft.Json.Linq.JValue)value;
			return (T)global::System.Convert.ChangeType(jValue.Value, typeof(T), global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public object Convert(object value, global::System.Type type)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(value, "value");
			global::Newtonsoft.Json.Linq.JToken jToken = value as global::Newtonsoft.Json.Linq.JToken;
			if (jToken == null)
			{
				throw new global::System.ArgumentException("Value is not a JToken.", "value");
			}
			return _serializer.Deserialize(jToken.CreateReader(), type);
		}

		public object Convert(object value, global::System.TypeCode typeCode)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(value, "value");
			if (value is global::Newtonsoft.Json.Linq.JValue)
			{
				value = ((global::Newtonsoft.Json.Linq.JValue)value).Value;
			}
			return global::System.Convert.ChangeType(value, typeCode, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public bool ToBoolean(object value)
		{
			return GetTokenValue<bool>(value);
		}

		public byte ToByte(object value)
		{
			return GetTokenValue<byte>(value);
		}

		public char ToChar(object value)
		{
			return GetTokenValue<char>(value);
		}

		public global::System.DateTime ToDateTime(object value)
		{
			return GetTokenValue<global::System.DateTime>(value);
		}

		public decimal ToDecimal(object value)
		{
			return GetTokenValue<decimal>(value);
		}

		public double ToDouble(object value)
		{
			return GetTokenValue<double>(value);
		}

		public short ToInt16(object value)
		{
			return GetTokenValue<short>(value);
		}

		public int ToInt32(object value)
		{
			return GetTokenValue<int>(value);
		}

		public long ToInt64(object value)
		{
			return GetTokenValue<long>(value);
		}

		public sbyte ToSByte(object value)
		{
			return GetTokenValue<sbyte>(value);
		}

		public float ToSingle(object value)
		{
			return GetTokenValue<float>(value);
		}

		public string ToString(object value)
		{
			return GetTokenValue<string>(value);
		}

		public ushort ToUInt16(object value)
		{
			return GetTokenValue<ushort>(value);
		}

		public uint ToUInt32(object value)
		{
			return GetTokenValue<uint>(value);
		}

		public ulong ToUInt64(object value)
		{
			return GetTokenValue<ulong>(value);
		}
	}
}
