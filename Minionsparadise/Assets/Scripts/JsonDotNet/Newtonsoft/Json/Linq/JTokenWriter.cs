namespace Newtonsoft.Json.Linq
{
	public class JTokenWriter : global::Newtonsoft.Json.JsonWriter
	{
		private global::Newtonsoft.Json.Linq.JContainer _token;

		private global::Newtonsoft.Json.Linq.JContainer _parent;

		private global::Newtonsoft.Json.Linq.JValue _value;

		public global::Newtonsoft.Json.Linq.JToken Token
		{
			get
			{
				if (_token != null)
				{
					return _token;
				}
				return _value;
			}
		}

		public JTokenWriter(global::Newtonsoft.Json.Linq.JContainer container)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(container, "container");
			_token = container;
			_parent = container;
		}

		public JTokenWriter()
		{
		}

		public override void Flush()
		{
		}

		public override void Close()
		{
			base.Close();
		}

		public override void WriteStartObject()
		{
			base.WriteStartObject();
			AddParent(new global::Newtonsoft.Json.Linq.JObject());
		}

		private void AddParent(global::Newtonsoft.Json.Linq.JContainer container)
		{
			if (_parent == null)
			{
				_token = container;
			}
			else
			{
				_parent.Add(container);
			}
			_parent = container;
		}

		private void RemoveParent()
		{
			_parent = _parent.Parent;
			if (_parent != null && _parent.Type == global::Newtonsoft.Json.Linq.JTokenType.Property)
			{
				_parent = _parent.Parent;
			}
		}

		public override void WriteStartArray()
		{
			base.WriteStartArray();
			AddParent(new global::Newtonsoft.Json.Linq.JArray());
		}

		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			AddParent(new global::Newtonsoft.Json.Linq.JConstructor(name));
		}

		protected override void WriteEnd(global::Newtonsoft.Json.JsonToken token)
		{
			RemoveParent();
		}

		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			AddParent(new global::Newtonsoft.Json.Linq.JProperty(name));
		}

		private void AddValue(object value, global::Newtonsoft.Json.JsonToken token)
		{
			AddValue(new global::Newtonsoft.Json.Linq.JValue(value), token);
		}

		internal void AddValue(global::Newtonsoft.Json.Linq.JValue value, global::Newtonsoft.Json.JsonToken token)
		{
			if (_parent != null)
			{
				_parent.Add(value);
				if (_parent.Type == global::Newtonsoft.Json.Linq.JTokenType.Property)
				{
					_parent = _parent.Parent;
				}
			}
			else
			{
				_value = value;
			}
		}

		public override void WriteNull()
		{
			base.WriteNull();
			AddValue(null, global::Newtonsoft.Json.JsonToken.Null);
		}

		public override void WriteUndefined()
		{
			base.WriteUndefined();
			AddValue(null, global::Newtonsoft.Json.JsonToken.Undefined);
		}

		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			AddValue(new global::Newtonsoft.Json.Linq.JRaw(json), global::Newtonsoft.Json.JsonToken.Raw);
		}

		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			AddValue(global::Newtonsoft.Json.Linq.JValue.CreateComment(text), global::Newtonsoft.Json.JsonToken.Comment);
		}

		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			AddValue(value ?? string.Empty, global::Newtonsoft.Json.JsonToken.String);
		}

		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Float);
		}

		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Float);
		}

		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Boolean);
		}

		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			AddValue(value.ToString(), global::Newtonsoft.Json.JsonToken.String);
		}

		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Integer);
		}

		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Float);
		}

		public override void WriteValue(global::System.DateTime value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Date);
		}

		public override void WriteValue(global::System.DateTimeOffset value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Date);
		}

		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.Bytes);
		}

		public override void WriteValue(global::System.TimeSpan value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.String);
		}

		public override void WriteValue(global::System.Guid value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.String);
		}

		public override void WriteValue(global::System.Uri value)
		{
			base.WriteValue(value);
			AddValue(value, global::Newtonsoft.Json.JsonToken.String);
		}
	}
}
