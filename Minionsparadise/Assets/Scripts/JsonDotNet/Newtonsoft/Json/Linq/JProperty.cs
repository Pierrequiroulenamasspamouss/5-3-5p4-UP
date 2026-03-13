namespace Newtonsoft.Json.Linq
{
	public class JProperty : global::Newtonsoft.Json.Linq.JContainer
	{
		private readonly global::System.Collections.Generic.List<global::Newtonsoft.Json.Linq.JToken> _content = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Linq.JToken>();

		private readonly string _name;

		protected override global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken> ChildrenTokens
		{
			get
			{
				return _content;
			}
		}

		public string Name
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return _name;
			}
		}

		public new global::Newtonsoft.Json.Linq.JToken Value
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				if (ChildrenTokens.Count <= 0)
				{
					return null;
				}
				return ChildrenTokens[0];
			}
			set
			{
				CheckReentrancy();
				global::Newtonsoft.Json.Linq.JToken item = value ?? new global::Newtonsoft.Json.Linq.JValue((object)null);
				if (ChildrenTokens.Count == 0)
				{
					InsertItem(0, item);
				}
				else
				{
					SetItem(0, item);
				}
			}
		}

		public override global::Newtonsoft.Json.Linq.JTokenType Type
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Property;
			}
		}

		public JProperty(global::Newtonsoft.Json.Linq.JProperty other)
			: base(other)
		{
			_name = other.Name;
		}

		internal override global::Newtonsoft.Json.Linq.JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new global::System.ArgumentOutOfRangeException();
			}
			return Value;
		}

		internal override void SetItem(int index, global::Newtonsoft.Json.Linq.JToken item)
		{
			if (index != 0)
			{
				throw new global::System.ArgumentOutOfRangeException();
			}
			if (!global::Newtonsoft.Json.Linq.JContainer.IsTokenUnchanged(Value, item))
			{
				if (base.Parent != null)
				{
					((global::Newtonsoft.Json.Linq.JObject)base.Parent).InternalPropertyChanging(this);
				}
				base.SetItem(0, item);
				if (base.Parent != null)
				{
					((global::Newtonsoft.Json.Linq.JObject)base.Parent).InternalPropertyChanged(this);
				}
			}
		}

		internal override bool RemoveItem(global::Newtonsoft.Json.Linq.JToken item)
		{
			throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot add or remove items from {0}.", global::System.Globalization.CultureInfo.InvariantCulture, typeof(global::Newtonsoft.Json.Linq.JProperty)));
		}

		internal override void RemoveItemAt(int index)
		{
			throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot add or remove items from {0}.", global::System.Globalization.CultureInfo.InvariantCulture, typeof(global::Newtonsoft.Json.Linq.JProperty)));
		}

		internal override void InsertItem(int index, global::Newtonsoft.Json.Linq.JToken item)
		{
			if (Value != null)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("{0} cannot have multiple values.", global::System.Globalization.CultureInfo.InvariantCulture, typeof(global::Newtonsoft.Json.Linq.JProperty)));
			}
			base.InsertItem(0, item);
		}

		internal override bool ContainsItem(global::Newtonsoft.Json.Linq.JToken item)
		{
			return Value == item;
		}

		internal override void ClearItems()
		{
			throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot add or remove items from {0}.", global::System.Globalization.CultureInfo.InvariantCulture, typeof(global::Newtonsoft.Json.Linq.JProperty)));
		}

		internal override bool DeepEquals(global::Newtonsoft.Json.Linq.JToken node)
		{
			global::Newtonsoft.Json.Linq.JProperty jProperty = node as global::Newtonsoft.Json.Linq.JProperty;
			if (jProperty != null && _name == jProperty.Name)
			{
				return ContentsEqual(jProperty);
			}
			return false;
		}

		internal override global::Newtonsoft.Json.Linq.JToken CloneToken()
		{
			return new global::Newtonsoft.Json.Linq.JProperty(this);
		}

		internal JProperty(string name)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(name, "name");
			_name = name;
		}

		public JProperty(string name, params object[] content)
			: this(name, (object)content)
		{
		}

		public JProperty(string name, object content)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(name, "name");
			_name = name;
			Value = (IsMultiContent(content) ? new global::Newtonsoft.Json.Linq.JArray(content) : CreateFromContent(content));
		}

		public override void WriteTo(global::Newtonsoft.Json.JsonWriter writer, params global::Newtonsoft.Json.JsonConverter[] converters)
		{
			writer.WritePropertyName(_name);
			Value.WriteTo(writer, converters);
		}

		internal override int GetDeepHashCode()
		{
			return _name.GetHashCode() ^ ((Value != null) ? Value.GetDeepHashCode() : 0);
		}

		public new static global::Newtonsoft.Json.Linq.JProperty Load(global::Newtonsoft.Json.JsonReader reader)
		{
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.None && !reader.Read())
			{
				throw new global::System.Exception("Error reading JProperty from JsonReader.");
			}
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.PropertyName)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType));
			}
			global::Newtonsoft.Json.Linq.JProperty jProperty = new global::Newtonsoft.Json.Linq.JProperty((string)reader.Value);
			jProperty.SetLineInfo(reader as global::Newtonsoft.Json.IJsonLineInfo);
			jProperty.ReadTokenFrom(reader);
			return jProperty;
		}
	}
}
