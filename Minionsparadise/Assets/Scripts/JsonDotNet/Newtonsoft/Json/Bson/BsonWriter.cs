namespace Newtonsoft.Json.Bson
{
	public class BsonWriter : global::Newtonsoft.Json.JsonWriter
	{
		private readonly global::Newtonsoft.Json.Bson.BsonBinaryWriter _writer;

		private global::Newtonsoft.Json.Bson.BsonToken _root;

		private global::Newtonsoft.Json.Bson.BsonToken _parent;

		private string _propertyName;

		public global::System.DateTimeKind DateTimeKindHandling
		{
			get
			{
				return _writer.DateTimeKindHandling;
			}
			set
			{
				_writer.DateTimeKindHandling = value;
			}
		}

		public BsonWriter(global::System.IO.Stream stream)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(stream, "stream");
			_writer = new global::Newtonsoft.Json.Bson.BsonBinaryWriter(stream);
		}

		public override void Flush()
		{
			_writer.Flush();
		}

		protected override void WriteEnd(global::Newtonsoft.Json.JsonToken token)
		{
			base.WriteEnd(token);
			RemoveParent();
			if (base.Top == 0)
			{
				_writer.WriteToken(_root);
			}
		}

		public override void WriteComment(string text)
		{
			throw new global::Newtonsoft.Json.JsonWriterException("Cannot write JSON comment as BSON.");
		}

		public override void WriteStartConstructor(string name)
		{
			throw new global::Newtonsoft.Json.JsonWriterException("Cannot write JSON constructor as BSON.");
		}

		public override void WriteRaw(string json)
		{
			throw new global::Newtonsoft.Json.JsonWriterException("Cannot write raw JSON as BSON.");
		}

		public override void WriteRawValue(string json)
		{
			throw new global::Newtonsoft.Json.JsonWriterException("Cannot write raw JSON as BSON.");
		}

		public override void WriteStartArray()
		{
			base.WriteStartArray();
			AddParent(new global::Newtonsoft.Json.Bson.BsonArray());
		}

		public override void WriteStartObject()
		{
			base.WriteStartObject();
			AddParent(new global::Newtonsoft.Json.Bson.BsonObject());
		}

		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			_propertyName = name;
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseOutput && _writer != null)
			{
				_writer.Close();
			}
		}

		private void AddParent(global::Newtonsoft.Json.Bson.BsonToken container)
		{
			AddToken(container);
			_parent = container;
		}

		private void RemoveParent()
		{
			_parent = _parent.Parent;
		}

		private void AddValue(object value, global::Newtonsoft.Json.Bson.BsonType type)
		{
			AddToken(new global::Newtonsoft.Json.Bson.BsonValue(value, type));
		}

		internal void AddToken(global::Newtonsoft.Json.Bson.BsonToken token)
		{
			if (_parent != null)
			{
				if (_parent is global::Newtonsoft.Json.Bson.BsonObject)
				{
					((global::Newtonsoft.Json.Bson.BsonObject)_parent).Add(_propertyName, token);
					_propertyName = null;
				}
				else
				{
					((global::Newtonsoft.Json.Bson.BsonArray)_parent).Add(token);
				}
				return;
			}
			if (token.Type != global::Newtonsoft.Json.Bson.BsonType.Object && token.Type != global::Newtonsoft.Json.Bson.BsonType.Array)
			{
				throw new global::Newtonsoft.Json.JsonWriterException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error writing {0} value. BSON must start with an Object or Array.", global::System.Globalization.CultureInfo.InvariantCulture, token.Type));
			}
			_parent = token;
			_root = token;
		}

		public override void WriteNull()
		{
			base.WriteNull();
			AddValue(null, global::Newtonsoft.Json.Bson.BsonType.Null);
		}

		public override void WriteUndefined()
		{
			base.WriteUndefined();
			AddValue(null, global::Newtonsoft.Json.Bson.BsonType.Undefined);
		}

		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			if (value == null)
			{
				AddValue(null, global::Newtonsoft.Json.Bson.BsonType.Null);
			}
			else
			{
				AddToken(new global::Newtonsoft.Json.Bson.BsonString(value, true));
			}
		}

		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Integer);
		}

		public override void WriteValue(uint value)
		{
			if (value > int.MaxValue)
			{
				throw new global::Newtonsoft.Json.JsonWriterException("Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.");
			}
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Integer);
		}

		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Long);
		}

		public override void WriteValue(ulong value)
		{
			if (value > long.MaxValue)
			{
				throw new global::Newtonsoft.Json.JsonWriterException("Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.");
			}
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Long);
		}

		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Number);
		}

		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Number);
		}

		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Boolean);
		}

		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Integer);
		}

		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Integer);
		}

		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			AddToken(new global::Newtonsoft.Json.Bson.BsonString(value.ToString(), true));
		}

		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Integer);
		}

		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Integer);
		}

		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Number);
		}

		public override void WriteValue(global::System.DateTime value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Date);
		}

		public override void WriteValue(global::System.DateTimeOffset value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Date);
		}

		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Binary);
		}

		public override void WriteValue(global::System.Guid value)
		{
			base.WriteValue(value);
			AddToken(new global::Newtonsoft.Json.Bson.BsonString(value.ToString(), true));
		}

		public override void WriteValue(global::System.TimeSpan value)
		{
			base.WriteValue(value);
			AddToken(new global::Newtonsoft.Json.Bson.BsonString(value.ToString(), true));
		}

		public override void WriteValue(global::System.Uri value)
		{
			base.WriteValue(value);
			AddToken(new global::Newtonsoft.Json.Bson.BsonString(value.ToString(), true));
		}

		public void WriteObjectId(byte[] value)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new global::System.Exception("An object id must be 12 bytes");
			}
			AutoComplete(global::Newtonsoft.Json.JsonToken.Undefined);
			AddValue(value, global::Newtonsoft.Json.Bson.BsonType.Oid);
		}

		public void WriteRegex(string pattern, string options)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(pattern, "pattern");
			AutoComplete(global::Newtonsoft.Json.JsonToken.Undefined);
			AddToken(new global::Newtonsoft.Json.Bson.BsonRegex(pattern, options));
		}
	}
}
