namespace Newtonsoft.Json.Linq
{
	public abstract class JToken : global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken>, global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken>, global::System.Collections.IEnumerable, global::Newtonsoft.Json.IJsonLineInfo, global::System.ICloneable
	{
		private global::Newtonsoft.Json.Linq.JContainer _parent;

		private global::Newtonsoft.Json.Linq.JToken _previous;

		private global::Newtonsoft.Json.Linq.JToken _next;

		private static global::Newtonsoft.Json.Linq.JTokenEqualityComparer _equalityComparer;

		private int? _lineNumber;

		private int? _linePosition;

		public static global::Newtonsoft.Json.Linq.JTokenEqualityComparer EqualityComparer
		{
			get
			{
				if (_equalityComparer == null)
				{
					_equalityComparer = new global::Newtonsoft.Json.Linq.JTokenEqualityComparer();
				}
				return _equalityComparer;
			}
		}

		public global::Newtonsoft.Json.Linq.JContainer Parent
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return _parent;
			}
			internal set
			{
				_parent = value;
			}
		}

		public global::Newtonsoft.Json.Linq.JToken Root
		{
			get
			{
				global::Newtonsoft.Json.Linq.JContainer parent = Parent;
				if (parent == null)
				{
					return this;
				}
				while (parent.Parent != null)
				{
					parent = parent.Parent;
				}
				return parent;
			}
		}

		public abstract global::Newtonsoft.Json.Linq.JTokenType Type { get; }

		public abstract bool HasValues { get; }

		public global::Newtonsoft.Json.Linq.JToken Next
		{
			get
			{
				return _next;
			}
			internal set
			{
				_next = value;
			}
		}

		public global::Newtonsoft.Json.Linq.JToken Previous
		{
			get
			{
				return _previous;
			}
			internal set
			{
				_previous = value;
			}
		}

		public virtual global::Newtonsoft.Json.Linq.JToken this[object key]
		{
			get
			{
				throw new global::System.InvalidOperationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot access child value on {0}.", global::System.Globalization.CultureInfo.InvariantCulture, GetType()));
			}
			set
			{
				throw new global::System.InvalidOperationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot set child value on {0}.", global::System.Globalization.CultureInfo.InvariantCulture, GetType()));
			}
		}

		public virtual global::Newtonsoft.Json.Linq.JToken First
		{
			get
			{
				throw new global::System.InvalidOperationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot access child value on {0}.", global::System.Globalization.CultureInfo.InvariantCulture, GetType()));
			}
		}

		public virtual global::Newtonsoft.Json.Linq.JToken Last
		{
			get
			{
				throw new global::System.InvalidOperationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot access child value on {0}.", global::System.Globalization.CultureInfo.InvariantCulture, GetType()));
			}
		}

		global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken> global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken>.this[object key]
		{
			get
			{
				return this[key];
			}
		}

		int global::Newtonsoft.Json.IJsonLineInfo.LineNumber
		{
			get
			{
				return _lineNumber ?? 0;
			}
		}

		int global::Newtonsoft.Json.IJsonLineInfo.LinePosition
		{
			get
			{
				return _linePosition ?? 0;
			}
		}

		internal abstract global::Newtonsoft.Json.Linq.JToken CloneToken();

		internal abstract bool DeepEquals(global::Newtonsoft.Json.Linq.JToken node);

		public static bool DeepEquals(global::Newtonsoft.Json.Linq.JToken t1, global::Newtonsoft.Json.Linq.JToken t2)
		{
			if (t1 != t2)
			{
				if (t1 != null && t2 != null)
				{
					return t1.DeepEquals(t2);
				}
				return false;
			}
			return true;
		}

		internal JToken()
		{
		}

		public void AddAfterSelf(object content)
		{
			if (_parent == null)
			{
				throw new global::System.InvalidOperationException("The parent is missing.");
			}
			int num = _parent.IndexOfItem(this);
			_parent.AddInternal(num + 1, content);
		}

		public void AddBeforeSelf(object content)
		{
			if (_parent == null)
			{
				throw new global::System.InvalidOperationException("The parent is missing.");
			}
			int index = _parent.IndexOfItem(this);
			_parent.AddInternal(index, content);
		}

		public global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> Ancestors()
		{
			for (global::Newtonsoft.Json.Linq.JToken parent = Parent; parent != null; parent = parent.Parent)
			{
				yield return parent;
			}
		}

		public global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> AfterSelf()
		{
			if (Parent != null)
			{
				for (global::Newtonsoft.Json.Linq.JToken o = Next; o != null; o = o.Next)
				{
					yield return o;
				}
			}
		}

		public global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> BeforeSelf()
		{
			for (global::Newtonsoft.Json.Linq.JToken o = Parent.First; o != this; o = o.Next)
			{
				yield return o;
			}
		}

		public virtual T Value<T>(object key)
		{
			global::Newtonsoft.Json.Linq.JToken token = this[key];
			return token.Convert<global::Newtonsoft.Json.Linq.JToken, T>();
		}

		public virtual global::Newtonsoft.Json.Linq.JEnumerable<global::Newtonsoft.Json.Linq.JToken> Children()
		{
			return global::Newtonsoft.Json.Linq.JEnumerable<global::Newtonsoft.Json.Linq.JToken>.Empty;
		}

		public global::Newtonsoft.Json.Linq.JEnumerable<T> Children<T>() where T : global::Newtonsoft.Json.Linq.JToken
		{
			return new global::Newtonsoft.Json.Linq.JEnumerable<T>(global::System.Linq.Enumerable.OfType<T>(Children()));
		}

		public virtual global::System.Collections.Generic.IEnumerable<T> Values<T>()
		{
			throw new global::System.InvalidOperationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot access child value on {0}.", global::System.Globalization.CultureInfo.InvariantCulture, GetType()));
		}

		public void Remove()
		{
			if (_parent == null)
			{
				throw new global::System.InvalidOperationException("The parent is missing.");
			}
			_parent.RemoveItem(this);
		}

		public void Replace(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (_parent == null)
			{
				throw new global::System.InvalidOperationException("The parent is missing.");
			}
			_parent.ReplaceItem(this, value);
		}

		public abstract void WriteTo(global::Newtonsoft.Json.JsonWriter writer, params global::Newtonsoft.Json.JsonConverter[] converters);

		public override string ToString()
		{
			return ToString(global::Newtonsoft.Json.Formatting.Indented);
		}

		public string ToString(global::Newtonsoft.Json.Formatting formatting, params global::Newtonsoft.Json.JsonConverter[] converters)
		{
			using (global::System.IO.StringWriter stringWriter = new global::System.IO.StringWriter(global::System.Globalization.CultureInfo.InvariantCulture))
			{
				global::Newtonsoft.Json.JsonTextWriter jsonTextWriter = new global::Newtonsoft.Json.JsonTextWriter(stringWriter);
				jsonTextWriter.Formatting = formatting;
				WriteTo(jsonTextWriter, converters);
				return stringWriter.ToString();
			}
		}

		private static global::Newtonsoft.Json.Linq.JValue EnsureValue(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				throw new global::System.ArgumentNullException("value");
			}
			if (value is global::Newtonsoft.Json.Linq.JProperty)
			{
				value = ((global::Newtonsoft.Json.Linq.JProperty)value).Value;
			}
			return value as global::Newtonsoft.Json.Linq.JValue;
		}

		private static string GetType(global::Newtonsoft.Json.Linq.JToken token)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(token, "token");
			if (token is global::Newtonsoft.Json.Linq.JProperty)
			{
				token = ((global::Newtonsoft.Json.Linq.JProperty)token).Value;
			}
			return token.Type.ToString();
		}

		private static bool IsNullable(global::Newtonsoft.Json.Linq.JToken o)
		{
			if (o.Type != global::Newtonsoft.Json.Linq.JTokenType.Undefined)
			{
				return o.Type == global::Newtonsoft.Json.Linq.JTokenType.Null;
			}
			return true;
		}

		private static bool ValidateFloat(global::Newtonsoft.Json.Linq.JToken o, bool nullable)
		{
			if (o.Type != global::Newtonsoft.Json.Linq.JTokenType.Float && o.Type != global::Newtonsoft.Json.Linq.JTokenType.Integer)
			{
				if (nullable)
				{
					return IsNullable(o);
				}
				return false;
			}
			return true;
		}

		private static bool ValidateInteger(global::Newtonsoft.Json.Linq.JToken o, bool nullable)
		{
			if (o.Type != global::Newtonsoft.Json.Linq.JTokenType.Integer)
			{
				if (nullable)
				{
					return IsNullable(o);
				}
				return false;
			}
			return true;
		}

		private static bool ValidateDate(global::Newtonsoft.Json.Linq.JToken o, bool nullable)
		{
			if (o.Type != global::Newtonsoft.Json.Linq.JTokenType.Date)
			{
				if (nullable)
				{
					return IsNullable(o);
				}
				return false;
			}
			return true;
		}

		private static bool ValidateBoolean(global::Newtonsoft.Json.Linq.JToken o, bool nullable)
		{
			if (o.Type != global::Newtonsoft.Json.Linq.JTokenType.Boolean)
			{
				if (nullable)
				{
					return IsNullable(o);
				}
				return false;
			}
			return true;
		}

		private static bool ValidateString(global::Newtonsoft.Json.Linq.JToken o)
		{
			if (o.Type != global::Newtonsoft.Json.Linq.JTokenType.String && o.Type != global::Newtonsoft.Json.Linq.JTokenType.Comment && o.Type != global::Newtonsoft.Json.Linq.JTokenType.Raw)
			{
				return IsNullable(o);
			}
			return true;
		}

		private static bool ValidateBytes(global::Newtonsoft.Json.Linq.JToken o)
		{
			if (o.Type != global::Newtonsoft.Json.Linq.JTokenType.Bytes)
			{
				return IsNullable(o);
			}
			return true;
		}

		public static explicit operator bool(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateBoolean(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Boolean.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (bool)jValue.Value;
		}

		public static explicit operator global::System.DateTimeOffset(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateDate(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to DateTimeOffset.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (global::System.DateTimeOffset)jValue.Value;
		}

		public static explicit operator bool?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateBoolean(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Boolean.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (bool?)jValue.Value;
		}

		public static explicit operator long(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Int64.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (long)jValue.Value;
		}

		public static explicit operator global::System.DateTime?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateDate(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to DateTime.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (global::System.DateTime?)jValue.Value;
		}

		public static explicit operator global::System.DateTimeOffset?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateDate(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to DateTimeOffset.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (global::System.DateTimeOffset?)jValue.Value;
		}

		public static explicit operator decimal?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateFloat(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Decimal.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return global::System.Convert.ToDecimal(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator double?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateFloat(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Double.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (double?)jValue.Value;
		}

		public static explicit operator int(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Int32.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return global::System.Convert.ToInt32(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator short(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Int16.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return global::System.Convert.ToInt16(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator ushort(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to UInt16.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return global::System.Convert.ToUInt16(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator int?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Int32.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return global::System.Convert.ToInt32(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator short?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Int16.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return global::System.Convert.ToInt16(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator ushort?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to UInt16.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return (ushort)global::System.Convert.ToInt16(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator global::System.DateTime(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateDate(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to DateTime.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (global::System.DateTime)jValue.Value;
		}

		public static explicit operator long?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Int64.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (long?)jValue.Value;
		}

		public static explicit operator float?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateFloat(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Single.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return global::System.Convert.ToSingle(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator decimal(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateFloat(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Decimal.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return global::System.Convert.ToDecimal(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator uint?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to UInt32.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (uint?)jValue.Value;
		}

		public static explicit operator ulong?(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, true))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to UInt64.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (ulong?)jValue.Value;
		}

		public static explicit operator double(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateFloat(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Double.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (double)jValue.Value;
		}

		public static explicit operator float(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateFloat(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to Single.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return global::System.Convert.ToSingle(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator string(global::Newtonsoft.Json.Linq.JToken value)
		{
			if (value == null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateString(jValue))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to String.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (string)jValue.Value;
		}

		public static explicit operator uint(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to UInt32.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return global::System.Convert.ToUInt32(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator ulong(global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateInteger(jValue, false))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to UInt64.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return global::System.Convert.ToUInt64(jValue.Value, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static explicit operator byte[](global::Newtonsoft.Json.Linq.JToken value)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateBytes(jValue))
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not convert {0} to byte array.", global::System.Globalization.CultureInfo.InvariantCulture, GetType(value)));
			}
			return (byte[])jValue.Value;
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(bool value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(global::System.DateTimeOffset value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(bool? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(long value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(global::System.DateTime? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(global::System.DateTimeOffset? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(decimal? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(double? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(short value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(ushort value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(int value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(int? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(global::System.DateTime value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(long? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(float? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(decimal value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(short? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(ushort? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(uint? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(ulong? value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(double value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(float value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(string value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(uint value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(ulong value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		public static implicit operator global::Newtonsoft.Json.Linq.JToken(byte[] value)
		{
			return new global::Newtonsoft.Json.Linq.JValue(value);
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return ((global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken>)this).GetEnumerator();
		}

		global::System.Collections.Generic.IEnumerator<global::Newtonsoft.Json.Linq.JToken> global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken>.GetEnumerator()
		{
			return Children().GetEnumerator();
		}

		internal abstract int GetDeepHashCode();

		public global::Newtonsoft.Json.JsonReader CreateReader()
		{
			return new global::Newtonsoft.Json.Linq.JTokenReader(this);
		}

		internal static global::Newtonsoft.Json.Linq.JToken FromObjectInternal(object o, global::Newtonsoft.Json.JsonSerializer jsonSerializer)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(o, "o");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			using (global::Newtonsoft.Json.Linq.JTokenWriter jTokenWriter = new global::Newtonsoft.Json.Linq.JTokenWriter())
			{
				jsonSerializer.Serialize(jTokenWriter, o);
				return jTokenWriter.Token;
			}
		}

		public static global::Newtonsoft.Json.Linq.JToken FromObject(object o)
		{
			return FromObjectInternal(o, new global::Newtonsoft.Json.JsonSerializer());
		}

		public static global::Newtonsoft.Json.Linq.JToken FromObject(object o, global::Newtonsoft.Json.JsonSerializer jsonSerializer)
		{
			return FromObjectInternal(o, jsonSerializer);
		}

		public T ToObject<T>()
		{
			return ToObject<T>(new global::Newtonsoft.Json.JsonSerializer());
		}

		public T ToObject<T>(global::Newtonsoft.Json.JsonSerializer jsonSerializer)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			using (global::Newtonsoft.Json.Linq.JTokenReader reader = new global::Newtonsoft.Json.Linq.JTokenReader(this))
			{
				return jsonSerializer.Deserialize<T>(reader);
			}
		}

		public static global::Newtonsoft.Json.Linq.JToken ReadFrom(global::Newtonsoft.Json.JsonReader reader)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.None && !reader.Read())
			{
				throw new global::System.Exception("Error reading JToken from JsonReader.");
			}
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.StartObject)
			{
				return global::Newtonsoft.Json.Linq.JObject.Load(reader);
			}
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.StartArray)
			{
				return global::Newtonsoft.Json.Linq.JArray.Load(reader);
			}
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.PropertyName)
			{
				return global::Newtonsoft.Json.Linq.JProperty.Load(reader);
			}
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.StartConstructor)
			{
				return global::Newtonsoft.Json.Linq.JConstructor.Load(reader);
			}
			if (!global::Newtonsoft.Json.JsonReader.IsStartToken(reader.TokenType))
			{
				return new global::Newtonsoft.Json.Linq.JValue(reader.Value);
			}
			throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error reading JToken from JsonReader. Unexpected token: {0}", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType));
		}

		public static global::Newtonsoft.Json.Linq.JToken Parse(string json)
		{
			global::Newtonsoft.Json.JsonReader reader = new global::Newtonsoft.Json.JsonTextReader(new global::System.IO.StringReader(json));
			return Load(reader);
		}

		public static global::Newtonsoft.Json.Linq.JToken Load(global::Newtonsoft.Json.JsonReader reader)
		{
			return ReadFrom(reader);
		}

		internal void SetLineInfo(global::Newtonsoft.Json.IJsonLineInfo lineInfo)
		{
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
			}
		}

		internal void SetLineInfo(int lineNumber, int linePosition)
		{
			_lineNumber = lineNumber;
			_linePosition = linePosition;
		}

		bool global::Newtonsoft.Json.IJsonLineInfo.HasLineInfo()
		{
			if (_lineNumber.HasValue)
			{
				return _linePosition.HasValue;
			}
			return false;
		}

		public global::Newtonsoft.Json.Linq.JToken SelectToken(string path)
		{
			return SelectToken(path, false);
		}

		public global::Newtonsoft.Json.Linq.JToken SelectToken(string path, bool errorWhenNoMatch)
		{
			global::Newtonsoft.Json.Linq.JPath jPath = new global::Newtonsoft.Json.Linq.JPath(path);
			return jPath.Evaluate(this, errorWhenNoMatch);
		}

		object global::System.ICloneable.Clone()
		{
			return DeepClone();
		}

		public global::Newtonsoft.Json.Linq.JToken DeepClone()
		{
			return CloneToken();
		}
	}
}
