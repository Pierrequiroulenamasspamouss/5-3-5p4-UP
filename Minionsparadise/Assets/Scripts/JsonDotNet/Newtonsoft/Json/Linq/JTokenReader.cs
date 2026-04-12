namespace Newtonsoft.Json.Linq
{
	public class JTokenReader : global::Newtonsoft.Json.JsonReader, global::Newtonsoft.Json.IJsonLineInfo
	{
		private readonly global::Newtonsoft.Json.Linq.JToken _root;

		private global::Newtonsoft.Json.Linq.JToken _parent;

		private global::Newtonsoft.Json.Linq.JToken _current;

		private bool IsEndElement
		{
			get
			{
				return _current == _parent;
			}
		}

		int global::Newtonsoft.Json.IJsonLineInfo.LineNumber
		{
			get
			{
				if (base.CurrentState == global::Newtonsoft.Json.JsonReader.State.Start)
				{
					return 0;
				}
				global::Newtonsoft.Json.IJsonLineInfo jsonLineInfo = (IsEndElement ? null : _current);
				if (jsonLineInfo != null)
				{
					return jsonLineInfo.LineNumber;
				}
				return 0;
			}
		}

		int global::Newtonsoft.Json.IJsonLineInfo.LinePosition
		{
			get
			{
				if (base.CurrentState == global::Newtonsoft.Json.JsonReader.State.Start)
				{
					return 0;
				}
				global::Newtonsoft.Json.IJsonLineInfo jsonLineInfo = (IsEndElement ? null : _current);
				if (jsonLineInfo != null)
				{
					return jsonLineInfo.LinePosition;
				}
				return 0;
			}
		}

		public JTokenReader(global::Newtonsoft.Json.Linq.JToken token)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(token, "token");
			_root = token;
			_current = token;
		}

		public override byte[] ReadAsBytes()
		{
			Read();
			if (TokenType == global::Newtonsoft.Json.JsonToken.String)
			{
				string text = (string)Value;
				byte[] value = ((text.Length == 0) ? new byte[0] : global::System.Convert.FromBase64String(text));
				SetToken(global::Newtonsoft.Json.JsonToken.Bytes, value);
			}
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
			if (base.CurrentState != global::Newtonsoft.Json.JsonReader.State.Start)
			{
				global::Newtonsoft.Json.Linq.JContainer jContainer = _current as global::Newtonsoft.Json.Linq.JContainer;
				if (jContainer != null && _parent != jContainer)
				{
					return ReadInto(jContainer);
				}
				return ReadOver(_current);
			}
			SetToken(_current);
			return true;
		}

		private bool ReadOver(global::Newtonsoft.Json.Linq.JToken t)
		{
			if (t == _root)
			{
				return ReadToEnd();
			}
			global::Newtonsoft.Json.Linq.JToken next = t.Next;
			if (next == null || next == t || t == t.Parent.Last)
			{
				if (t.Parent == null)
				{
					return ReadToEnd();
				}
				return SetEnd(t.Parent);
			}
			_current = next;
			SetToken(_current);
			return true;
		}

		private bool ReadToEnd()
		{
			return false;
		}

		private global::Newtonsoft.Json.JsonToken? GetEndToken(global::Newtonsoft.Json.Linq.JContainer c)
		{
			switch (c.Type)
			{
			case global::Newtonsoft.Json.Linq.JTokenType.Object:
				return global::Newtonsoft.Json.JsonToken.EndObject;
			case global::Newtonsoft.Json.Linq.JTokenType.Array:
				return global::Newtonsoft.Json.JsonToken.EndArray;
			case global::Newtonsoft.Json.Linq.JTokenType.Constructor:
				return global::Newtonsoft.Json.JsonToken.EndConstructor;
			case global::Newtonsoft.Json.Linq.JTokenType.Property:
				return null;
			default:
				throw global::Newtonsoft.Json.Utilities.MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", c.Type, "Unexpected JContainer type.");
			}
		}

		private bool ReadInto(global::Newtonsoft.Json.Linq.JContainer c)
		{
			global::Newtonsoft.Json.Linq.JToken first = c.First;
			if (first == null)
			{
				return SetEnd(c);
			}
			SetToken(first);
			_current = first;
			_parent = c;
			return true;
		}

		private bool SetEnd(global::Newtonsoft.Json.Linq.JContainer c)
		{
			global::Newtonsoft.Json.JsonToken? endToken = GetEndToken(c);
			if (endToken.HasValue)
			{
				SetToken(endToken.Value);
				_current = c;
				_parent = c;
				return true;
			}
			return ReadOver(c);
		}

		private void SetToken(global::Newtonsoft.Json.Linq.JToken token)
		{
			switch (token.Type)
			{
			case global::Newtonsoft.Json.Linq.JTokenType.Object:
				SetToken(global::Newtonsoft.Json.JsonToken.StartObject);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Array:
				SetToken(global::Newtonsoft.Json.JsonToken.StartArray);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Constructor:
				SetToken(global::Newtonsoft.Json.JsonToken.StartConstructor);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Property:
				SetToken(global::Newtonsoft.Json.JsonToken.PropertyName, ((global::Newtonsoft.Json.Linq.JProperty)token).Name);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Comment:
				SetToken(global::Newtonsoft.Json.JsonToken.Comment, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Integer:
				SetToken(global::Newtonsoft.Json.JsonToken.Integer, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Float:
				SetToken(global::Newtonsoft.Json.JsonToken.Float, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.String:
				SetToken(global::Newtonsoft.Json.JsonToken.String, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Boolean:
				SetToken(global::Newtonsoft.Json.JsonToken.Boolean, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Null:
				SetToken(global::Newtonsoft.Json.JsonToken.Null, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Undefined:
				SetToken(global::Newtonsoft.Json.JsonToken.Undefined, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Date:
				SetToken(global::Newtonsoft.Json.JsonToken.Date, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Raw:
				SetToken(global::Newtonsoft.Json.JsonToken.Raw, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Bytes:
				SetToken(global::Newtonsoft.Json.JsonToken.Bytes, ((global::Newtonsoft.Json.Linq.JValue)token).Value);
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Guid:
				SetToken(global::Newtonsoft.Json.JsonToken.String, SafeToString(((global::Newtonsoft.Json.Linq.JValue)token).Value));
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Uri:
				SetToken(global::Newtonsoft.Json.JsonToken.String, SafeToString(((global::Newtonsoft.Json.Linq.JValue)token).Value));
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.TimeSpan:
				SetToken(global::Newtonsoft.Json.JsonToken.String, SafeToString(((global::Newtonsoft.Json.Linq.JValue)token).Value));
				break;
			default:
				throw global::Newtonsoft.Json.Utilities.MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", token.Type, "Unexpected JTokenType.");
			}
		}

		private string SafeToString(object value)
		{
			if (value == null)
			{
				return null;
			}
			return value.ToString();
		}

		bool global::Newtonsoft.Json.IJsonLineInfo.HasLineInfo()
		{
			if (base.CurrentState == global::Newtonsoft.Json.JsonReader.State.Start)
			{
				return false;
			}
			global::Newtonsoft.Json.IJsonLineInfo jsonLineInfo = (IsEndElement ? null : _current);
			if (jsonLineInfo != null)
			{
				return jsonLineInfo.HasLineInfo();
			}
			return false;
		}
	}
}
