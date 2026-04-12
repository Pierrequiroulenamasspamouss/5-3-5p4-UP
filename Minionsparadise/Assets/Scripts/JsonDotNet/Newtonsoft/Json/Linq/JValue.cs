namespace Newtonsoft.Json.Linq
{
	public class JValue : global::Newtonsoft.Json.Linq.JToken, global::System.IEquatable<global::Newtonsoft.Json.Linq.JValue>, global::System.IFormattable, global::System.IComparable, global::System.IComparable<global::Newtonsoft.Json.Linq.JValue>
	{
		private global::Newtonsoft.Json.Linq.JTokenType _valueType;

		private object _value;

		public override bool HasValues
		{
			get
			{
				return false;
			}
		}

		public override global::Newtonsoft.Json.Linq.JTokenType Type
		{
			get
			{
				return _valueType;
			}
		}

		public new object Value
		{
			get
			{
				return _value;
			}
			set
			{
				global::System.Type type = ((_value != null) ? _value.GetType() : null);
				global::System.Type type2 = ((value != null) ? value.GetType() : null);
				if (type != type2)
				{
					_valueType = GetValueType(_valueType, value);
				}
				_value = value;
			}
		}

		internal JValue(object value, global::Newtonsoft.Json.Linq.JTokenType type)
		{
			_value = value;
			_valueType = type;
		}

		public JValue(global::Newtonsoft.Json.Linq.JValue other)
			: this(other.Value, other.Type)
		{
		}

		public JValue(long value)
			: this(value, global::Newtonsoft.Json.Linq.JTokenType.Integer)
		{
		}

		public JValue(ulong value)
			: this(value, global::Newtonsoft.Json.Linq.JTokenType.Integer)
		{
		}

		public JValue(double value)
			: this(value, global::Newtonsoft.Json.Linq.JTokenType.Float)
		{
		}

		public JValue(global::System.DateTime value)
			: this(value, global::Newtonsoft.Json.Linq.JTokenType.Date)
		{
		}

		public JValue(bool value)
			: this(value, global::Newtonsoft.Json.Linq.JTokenType.Boolean)
		{
		}

		public JValue(string value)
			: this(value, global::Newtonsoft.Json.Linq.JTokenType.String)
		{
		}

		public JValue(global::System.Guid value)
			: this(value, global::Newtonsoft.Json.Linq.JTokenType.String)
		{
		}

		public JValue(global::System.Uri value)
			: this(value, global::Newtonsoft.Json.Linq.JTokenType.String)
		{
		}

		public JValue(global::System.TimeSpan value)
			: this(value, global::Newtonsoft.Json.Linq.JTokenType.String)
		{
		}

		public JValue(object value)
			: this(value, GetValueType(null, value))
		{
		}

		internal override bool DeepEquals(global::Newtonsoft.Json.Linq.JToken node)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = node as global::Newtonsoft.Json.Linq.JValue;
			if (jValue == null)
			{
				return false;
			}
			return ValuesEquals(this, jValue);
		}

		private static int Compare(global::Newtonsoft.Json.Linq.JTokenType valueType, object objA, object objB)
		{
			if (objA == null && objB == null)
			{
				return 0;
			}
			if (objA != null && objB == null)
			{
				return 1;
			}
			if (objA == null && objB != null)
			{
				return -1;
			}
			switch (valueType)
			{
			case global::Newtonsoft.Json.Linq.JTokenType.Integer:
				if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
				{
					return global::System.Convert.ToDecimal(objA, global::System.Globalization.CultureInfo.InvariantCulture).CompareTo(global::System.Convert.ToDecimal(objB, global::System.Globalization.CultureInfo.InvariantCulture));
				}
				if (objA is float || objB is float || objA is double || objB is double)
				{
					return CompareFloat(objA, objB);
				}
				return global::System.Convert.ToInt64(objA, global::System.Globalization.CultureInfo.InvariantCulture).CompareTo(global::System.Convert.ToInt64(objB, global::System.Globalization.CultureInfo.InvariantCulture));
			case global::Newtonsoft.Json.Linq.JTokenType.Float:
				return CompareFloat(objA, objB);
			case global::Newtonsoft.Json.Linq.JTokenType.Comment:
			case global::Newtonsoft.Json.Linq.JTokenType.String:
			case global::Newtonsoft.Json.Linq.JTokenType.Raw:
			{
				string text = global::System.Convert.ToString(objA, global::System.Globalization.CultureInfo.InvariantCulture);
				string strB = global::System.Convert.ToString(objB, global::System.Globalization.CultureInfo.InvariantCulture);
				return text.CompareTo(strB);
			}
			case global::Newtonsoft.Json.Linq.JTokenType.Boolean:
			{
				bool flag = global::System.Convert.ToBoolean(objA, global::System.Globalization.CultureInfo.InvariantCulture);
				bool value3 = global::System.Convert.ToBoolean(objB, global::System.Globalization.CultureInfo.InvariantCulture);
				return flag.CompareTo(value3);
			}
			case global::Newtonsoft.Json.Linq.JTokenType.Date:
			{
				if (objA is global::System.DateTime)
				{
					global::System.DateTime dateTime = global::System.Convert.ToDateTime(objA, global::System.Globalization.CultureInfo.InvariantCulture);
					global::System.DateTime value2 = global::System.Convert.ToDateTime(objB, global::System.Globalization.CultureInfo.InvariantCulture);
					return dateTime.CompareTo(value2);
				}
				if (!(objB is global::System.DateTimeOffset))
				{
					throw new global::System.ArgumentException("Object must be of type DateTimeOffset.");
				}
				global::System.DateTimeOffset dateTimeOffset = (global::System.DateTimeOffset)objA;
				global::System.DateTimeOffset other = (global::System.DateTimeOffset)objB;
				return dateTimeOffset.CompareTo(other);
			}
			case global::Newtonsoft.Json.Linq.JTokenType.Bytes:
			{
				if (!(objB is byte[]))
				{
					throw new global::System.ArgumentException("Object must be of type byte[].");
				}
				byte[] array = objA as byte[];
				byte[] array2 = objB as byte[];
				if (array == null)
				{
					return -1;
				}
				if (array2 == null)
				{
					return 1;
				}
				return global::Newtonsoft.Json.Utilities.MiscellaneousUtils.ByteArrayCompare(array, array2);
			}
			case global::Newtonsoft.Json.Linq.JTokenType.Guid:
			{
				if (!(objB is global::System.Guid))
				{
					throw new global::System.ArgumentException("Object must be of type Guid.");
				}
				global::System.Guid guid = (global::System.Guid)objA;
				global::System.Guid value4 = (global::System.Guid)objB;
				return guid.CompareTo(value4);
			}
			case global::Newtonsoft.Json.Linq.JTokenType.Uri:
			{
				if (!(objB is global::System.Uri))
				{
					throw new global::System.ArgumentException("Object must be of type Uri.");
				}
				global::System.Uri uri = (global::System.Uri)objA;
				global::System.Uri uri2 = (global::System.Uri)objB;
				return global::System.Collections.Generic.Comparer<string>.Default.Compare(uri.ToString(), uri2.ToString());
			}
			case global::Newtonsoft.Json.Linq.JTokenType.TimeSpan:
			{
				if (!(objB is global::System.TimeSpan))
				{
					throw new global::System.ArgumentException("Object must be of type TimeSpan.");
				}
				global::System.TimeSpan timeSpan = (global::System.TimeSpan)objA;
				global::System.TimeSpan value = (global::System.TimeSpan)objB;
				return timeSpan.CompareTo(value);
			}
			default:
				throw global::Newtonsoft.Json.Utilities.MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected value type: {0}", global::System.Globalization.CultureInfo.InvariantCulture, valueType));
			}
		}

		private static int CompareFloat(object objA, object objB)
		{
			double d = global::System.Convert.ToDouble(objA, global::System.Globalization.CultureInfo.InvariantCulture);
			double num = global::System.Convert.ToDouble(objB, global::System.Globalization.CultureInfo.InvariantCulture);
			if (global::Newtonsoft.Json.Utilities.MathUtils.ApproxEquals(d, num))
			{
				return 0;
			}
			return d.CompareTo(num);
		}

		internal override global::Newtonsoft.Json.Linq.JToken CloneToken()
		{
			return new global::Newtonsoft.Json.Linq.JValue(this);
		}

		public static global::Newtonsoft.Json.Linq.JValue CreateComment(string value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value, global::Newtonsoft.Json.Linq.JTokenType.Comment);
		}

		public static global::Newtonsoft.Json.Linq.JValue CreateString(string value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value, global::Newtonsoft.Json.Linq.JTokenType.String);
		}

		private static global::Newtonsoft.Json.Linq.JTokenType GetValueType(global::Newtonsoft.Json.Linq.JTokenType? current, object value)
		{
			if (value == null)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Null;
			}
			if (value == global::System.DBNull.Value)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Null;
			}
			if (value is string)
			{
				return GetStringValueType(current);
			}
			if (value is long || value is int || value is short || value is sbyte || value is ulong || value is uint || value is ushort || value is byte)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Integer;
			}
			if (value is global::System.Enum)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Integer;
			}
			if (value is double || value is float || value is decimal)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Float;
			}
			if (value is global::System.DateTime)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Date;
			}
			if (value is global::System.DateTimeOffset)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Date;
			}
			if (value is byte[])
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Bytes;
			}
			if (value is bool)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Boolean;
			}
			if (value is global::System.Guid)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Guid;
			}
			if (value is global::System.Uri)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Uri;
			}
			if (value is global::System.TimeSpan)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.TimeSpan;
			}
			throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Could not determine JSON object type for type {0}.", global::System.Globalization.CultureInfo.InvariantCulture, value.GetType()));
		}

		private static global::Newtonsoft.Json.Linq.JTokenType GetStringValueType(global::Newtonsoft.Json.Linq.JTokenType? current)
		{
			if (!current.HasValue)
			{
				return global::Newtonsoft.Json.Linq.JTokenType.String;
			}
			global::Newtonsoft.Json.Linq.JTokenType value = current.Value;
			if (value == global::Newtonsoft.Json.Linq.JTokenType.Comment || value == global::Newtonsoft.Json.Linq.JTokenType.String || value == global::Newtonsoft.Json.Linq.JTokenType.Raw)
			{
				return current.Value;
			}
			return global::Newtonsoft.Json.Linq.JTokenType.String;
		}

		public override void WriteTo(global::Newtonsoft.Json.JsonWriter writer, params global::Newtonsoft.Json.JsonConverter[] converters)
		{
			switch (_valueType)
			{
			case global::Newtonsoft.Json.Linq.JTokenType.Comment:
				writer.WriteComment(_value.ToString());
				return;
			case global::Newtonsoft.Json.Linq.JTokenType.Raw:
				writer.WriteRawValue((_value != null) ? _value.ToString() : null);
				return;
			case global::Newtonsoft.Json.Linq.JTokenType.Null:
				writer.WriteNull();
				return;
			case global::Newtonsoft.Json.Linq.JTokenType.Undefined:
				writer.WriteUndefined();
				return;
			}
			global::Newtonsoft.Json.JsonConverter matchingConverter;
			if (_value != null && (matchingConverter = global::Newtonsoft.Json.JsonSerializer.GetMatchingConverter(converters, _value.GetType())) != null)
			{
				matchingConverter.WriteJson(writer, _value, new global::Newtonsoft.Json.JsonSerializer());
				return;
			}
			switch (_valueType)
			{
			case global::Newtonsoft.Json.Linq.JTokenType.Integer:
				writer.WriteValue(global::System.Convert.ToInt64(_value, global::System.Globalization.CultureInfo.InvariantCulture));
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Float:
				writer.WriteValue(global::System.Convert.ToDouble(_value, global::System.Globalization.CultureInfo.InvariantCulture));
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.String:
				writer.WriteValue((_value != null) ? _value.ToString() : null);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Boolean:
				writer.WriteValue(global::System.Convert.ToBoolean(_value, global::System.Globalization.CultureInfo.InvariantCulture));
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Date:
				if (_value is global::System.DateTimeOffset)
				{
					writer.WriteValue((global::System.DateTimeOffset)_value);
				}
				else
				{
					writer.WriteValue(global::System.Convert.ToDateTime(_value, global::System.Globalization.CultureInfo.InvariantCulture));
				}
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Bytes:
				writer.WriteValue((byte[])_value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Guid:
			case global::Newtonsoft.Json.Linq.JTokenType.Uri:
			case global::Newtonsoft.Json.Linq.JTokenType.TimeSpan:
				writer.WriteValue((_value != null) ? _value.ToString() : null);
				break;
			default:
				throw global::Newtonsoft.Json.Utilities.MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", _valueType, "Unexpected token type.");
			}
		}

		internal override int GetDeepHashCode()
		{
			int num = ((_value != null) ? _value.GetHashCode() : 0);
			return _valueType.GetHashCode() ^ num;
		}

		private static bool ValuesEquals(global::Newtonsoft.Json.Linq.JValue v1, global::Newtonsoft.Json.Linq.JValue v2)
		{
			if (v1 != v2)
			{
				if (v1._valueType == v2._valueType)
				{
					return Compare(v1._valueType, v1._value, v2._value) == 0;
				}
				return false;
			}
			return true;
		}

		public bool Equals(global::Newtonsoft.Json.Linq.JValue other)
		{
			if (other == null)
			{
				return false;
			}
			return ValuesEquals(this, other);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = obj as global::Newtonsoft.Json.Linq.JValue;
			if (jValue != null)
			{
				return Equals(jValue);
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			if (_value == null)
			{
				return 0;
			}
			return _value.GetHashCode();
		}

		public override string ToString()
		{
			if (_value == null)
			{
				return string.Empty;
			}
			return _value.ToString();
		}

		public string ToString(string format)
		{
			return ToString(format, global::System.Globalization.CultureInfo.CurrentCulture);
		}

		public string ToString(global::System.IFormatProvider formatProvider)
		{
			return ToString(null, formatProvider);
		}

		public string ToString(string format, global::System.IFormatProvider formatProvider)
		{
			if (_value == null)
			{
				return string.Empty;
			}
			global::System.IFormattable formattable = _value as global::System.IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(format, formatProvider);
			}
			return _value.ToString();
		}

		int global::System.IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			object objB = ((obj is global::Newtonsoft.Json.Linq.JValue) ? ((global::Newtonsoft.Json.Linq.JValue)obj).Value : obj);
			return Compare(_valueType, _value, objB);
		}

		public int CompareTo(global::Newtonsoft.Json.Linq.JValue obj)
		{
			if (obj == null)
			{
				return 1;
			}
			return Compare(_valueType, _value, obj._value);
		}
	}
}
