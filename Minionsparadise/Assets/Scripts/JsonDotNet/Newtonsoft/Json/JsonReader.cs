namespace Newtonsoft.Json
{
	public abstract class JsonReader : global::System.IDisposable
	{
		protected enum State
		{
			Start = 0,
			Complete = 1,
			Property = 2,
			ObjectStart = 3,
			Object = 4,
			ArrayStart = 5,
			Array = 6,
			Closed = 7,
			PostValue = 8,
			ConstructorStart = 9,
			Constructor = 10,
			Error = 11,
			Finished = 12
		}

		private global::Newtonsoft.Json.JsonToken _token;

		private object _value;

		private global::System.Type _valueType;

		private char _quoteChar;

		private global::Newtonsoft.Json.JsonReader.State _currentState;

		private global::Newtonsoft.Json.Linq.JTokenType _currentTypeContext;

		private int _top;

		private readonly global::System.Collections.Generic.List<global::Newtonsoft.Json.Linq.JTokenType> _stack;

		protected global::Newtonsoft.Json.JsonReader.State CurrentState
		{
			get
			{
				return _currentState;
			}
		}

		public bool CloseInput { get; set; }

		public virtual char QuoteChar
		{
			get
			{
				return _quoteChar;
			}
			protected internal set
			{
				_quoteChar = value;
			}
		}

		public virtual global::Newtonsoft.Json.JsonToken TokenType
		{
			get
			{
				return _token;
			}
		}

		public virtual object Value
		{
			get
			{
				return _value;
			}
		}

		public virtual global::System.Type ValueType
		{
			get
			{
				return _valueType;
			}
		}

		public virtual int Depth
		{
			get
			{
				int num = _top - 1;
				if (IsStartToken(TokenType))
				{
					return num - 1;
				}
				return num;
			}
		}

		protected JsonReader()
		{
			_currentState = global::Newtonsoft.Json.JsonReader.State.Start;
			_stack = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Linq.JTokenType>();
			CloseInput = true;
			Push(global::Newtonsoft.Json.Linq.JTokenType.None);
		}

		private void Push(global::Newtonsoft.Json.Linq.JTokenType value)
		{
			_stack.Add(value);
			_top++;
			_currentTypeContext = value;
		}

		private global::Newtonsoft.Json.Linq.JTokenType Pop()
		{
			global::Newtonsoft.Json.Linq.JTokenType result = Peek();
			_stack.RemoveAt(_stack.Count - 1);
			_top--;
			_currentTypeContext = _stack[_top - 1];
			return result;
		}

		private global::Newtonsoft.Json.Linq.JTokenType Peek()
		{
			return _currentTypeContext;
		}

		public abstract bool Read();

		public abstract byte[] ReadAsBytes();

		public abstract decimal? ReadAsDecimal();

		public abstract global::System.DateTimeOffset? ReadAsDateTimeOffset();

		public void Skip()
		{
			if (IsStartToken(TokenType))
			{
				int depth = Depth;
				while (Read() && depth < Depth)
				{
				}
			}
		}

		protected void SetToken(global::Newtonsoft.Json.JsonToken newToken)
		{
			SetToken(newToken, null);
		}

		protected virtual void SetToken(global::Newtonsoft.Json.JsonToken newToken, object value)
		{
			_token = newToken;
			switch (newToken)
			{
			case global::Newtonsoft.Json.JsonToken.StartObject:
				_currentState = global::Newtonsoft.Json.JsonReader.State.ObjectStart;
				Push(global::Newtonsoft.Json.Linq.JTokenType.Object);
				break;
			case global::Newtonsoft.Json.JsonToken.StartArray:
				_currentState = global::Newtonsoft.Json.JsonReader.State.ArrayStart;
				Push(global::Newtonsoft.Json.Linq.JTokenType.Array);
				break;
			case global::Newtonsoft.Json.JsonToken.StartConstructor:
				_currentState = global::Newtonsoft.Json.JsonReader.State.ConstructorStart;
				Push(global::Newtonsoft.Json.Linq.JTokenType.Constructor);
				break;
			case global::Newtonsoft.Json.JsonToken.EndObject:
				ValidateEnd(global::Newtonsoft.Json.JsonToken.EndObject);
				_currentState = global::Newtonsoft.Json.JsonReader.State.PostValue;
				break;
			case global::Newtonsoft.Json.JsonToken.EndArray:
				ValidateEnd(global::Newtonsoft.Json.JsonToken.EndArray);
				_currentState = global::Newtonsoft.Json.JsonReader.State.PostValue;
				break;
			case global::Newtonsoft.Json.JsonToken.EndConstructor:
				ValidateEnd(global::Newtonsoft.Json.JsonToken.EndConstructor);
				_currentState = global::Newtonsoft.Json.JsonReader.State.PostValue;
				break;
			case global::Newtonsoft.Json.JsonToken.PropertyName:
				_currentState = global::Newtonsoft.Json.JsonReader.State.Property;
				Push(global::Newtonsoft.Json.Linq.JTokenType.Property);
				break;
			case global::Newtonsoft.Json.JsonToken.Raw:
			case global::Newtonsoft.Json.JsonToken.Integer:
			case global::Newtonsoft.Json.JsonToken.Float:
			case global::Newtonsoft.Json.JsonToken.String:
			case global::Newtonsoft.Json.JsonToken.Boolean:
			case global::Newtonsoft.Json.JsonToken.Null:
			case global::Newtonsoft.Json.JsonToken.Undefined:
			case global::Newtonsoft.Json.JsonToken.Date:
			case global::Newtonsoft.Json.JsonToken.Bytes:
				_currentState = global::Newtonsoft.Json.JsonReader.State.PostValue;
				break;
			}
			global::Newtonsoft.Json.Linq.JTokenType jTokenType = Peek();
			if (jTokenType == global::Newtonsoft.Json.Linq.JTokenType.Property && _currentState == global::Newtonsoft.Json.JsonReader.State.PostValue)
			{
				Pop();
			}
			if (value != null)
			{
				_value = value;
				_valueType = value.GetType();
			}
			else
			{
				_value = null;
				_valueType = null;
			}
		}

		private void ValidateEnd(global::Newtonsoft.Json.JsonToken endToken)
		{
			global::Newtonsoft.Json.Linq.JTokenType jTokenType = Pop();
			if (GetTypeForCloseToken(endToken) != jTokenType)
			{
				throw new global::Newtonsoft.Json.JsonReaderException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("JsonToken {0} is not valid for closing JsonType {1}.", global::System.Globalization.CultureInfo.InvariantCulture, endToken, jTokenType));
			}
		}

		protected void SetStateBasedOnCurrent()
		{
			global::Newtonsoft.Json.Linq.JTokenType jTokenType = Peek();
			switch (jTokenType)
			{
			case global::Newtonsoft.Json.Linq.JTokenType.Object:
				_currentState = global::Newtonsoft.Json.JsonReader.State.Object;
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Array:
				_currentState = global::Newtonsoft.Json.JsonReader.State.Array;
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.Constructor:
				_currentState = global::Newtonsoft.Json.JsonReader.State.Constructor;
				break;
			case global::Newtonsoft.Json.Linq.JTokenType.None:
				_currentState = global::Newtonsoft.Json.JsonReader.State.Finished;
				break;
			default:
				throw new global::Newtonsoft.Json.JsonReaderException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("While setting the reader state back to current object an unexpected JsonType was encountered: {0}", global::System.Globalization.CultureInfo.InvariantCulture, jTokenType));
			}
		}

		internal static bool IsPrimitiveToken(global::Newtonsoft.Json.JsonToken token)
		{
			switch (token)
			{
			case global::Newtonsoft.Json.JsonToken.Integer:
			case global::Newtonsoft.Json.JsonToken.Float:
			case global::Newtonsoft.Json.JsonToken.String:
			case global::Newtonsoft.Json.JsonToken.Boolean:
			case global::Newtonsoft.Json.JsonToken.Null:
			case global::Newtonsoft.Json.JsonToken.Undefined:
			case global::Newtonsoft.Json.JsonToken.Date:
			case global::Newtonsoft.Json.JsonToken.Bytes:
				return true;
			default:
				return false;
			}
		}

		internal static bool IsStartToken(global::Newtonsoft.Json.JsonToken token)
		{
			switch (token)
			{
			case global::Newtonsoft.Json.JsonToken.StartObject:
			case global::Newtonsoft.Json.JsonToken.StartArray:
			case global::Newtonsoft.Json.JsonToken.StartConstructor:
			case global::Newtonsoft.Json.JsonToken.PropertyName:
				return true;
			case global::Newtonsoft.Json.JsonToken.None:
			case global::Newtonsoft.Json.JsonToken.Comment:
			case global::Newtonsoft.Json.JsonToken.Raw:
			case global::Newtonsoft.Json.JsonToken.Integer:
			case global::Newtonsoft.Json.JsonToken.Float:
			case global::Newtonsoft.Json.JsonToken.String:
			case global::Newtonsoft.Json.JsonToken.Boolean:
			case global::Newtonsoft.Json.JsonToken.Null:
			case global::Newtonsoft.Json.JsonToken.Undefined:
			case global::Newtonsoft.Json.JsonToken.EndObject:
			case global::Newtonsoft.Json.JsonToken.EndArray:
			case global::Newtonsoft.Json.JsonToken.EndConstructor:
			case global::Newtonsoft.Json.JsonToken.Date:
			case global::Newtonsoft.Json.JsonToken.Bytes:
				return false;
			default:
				throw global::Newtonsoft.Json.Utilities.MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected JsonToken value.");
			}
		}

		private global::Newtonsoft.Json.Linq.JTokenType GetTypeForCloseToken(global::Newtonsoft.Json.JsonToken token)
		{
			switch (token)
			{
			case global::Newtonsoft.Json.JsonToken.EndObject:
				return global::Newtonsoft.Json.Linq.JTokenType.Object;
			case global::Newtonsoft.Json.JsonToken.EndArray:
				return global::Newtonsoft.Json.Linq.JTokenType.Array;
			case global::Newtonsoft.Json.JsonToken.EndConstructor:
				return global::Newtonsoft.Json.Linq.JTokenType.Constructor;
			default:
				throw new global::Newtonsoft.Json.JsonReaderException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Not a valid close JsonToken: {0}", global::System.Globalization.CultureInfo.InvariantCulture, token));
			}
		}

		void global::System.IDisposable.Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_currentState != global::Newtonsoft.Json.JsonReader.State.Closed && disposing)
			{
				Close();
			}
		}

		public virtual void Close()
		{
			_currentState = global::Newtonsoft.Json.JsonReader.State.Closed;
			_token = global::Newtonsoft.Json.JsonToken.None;
			_value = null;
			_valueType = null;
		}
	}
}
