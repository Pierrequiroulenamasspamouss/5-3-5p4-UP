namespace Newtonsoft.Json.Linq
{
	public class JArray : global::Newtonsoft.Json.Linq.JContainer, global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken>, global::System.Collections.Generic.ICollection<global::Newtonsoft.Json.Linq.JToken>, global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken>, global::System.Collections.IEnumerable
	{
		private global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken> _values = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Linq.JToken>();

		protected override global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken> ChildrenTokens
		{
			get
			{
				return _values;
			}
		}

		public override global::Newtonsoft.Json.Linq.JTokenType Type
		{
			get
			{
				return global::Newtonsoft.Json.Linq.JTokenType.Array;
			}
		}

		public override global::Newtonsoft.Json.Linq.JToken this[object key]
		{
			get
			{
				global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Accessed JArray values with invalid key value: {0}. Array position index expected.", global::System.Globalization.CultureInfo.InvariantCulture, global::Newtonsoft.Json.Utilities.MiscellaneousUtils.ToString(key)));
				}
				return GetItem((int)key);
			}
			set
			{
				global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Set JArray values with invalid key value: {0}. Array position index expected.", global::System.Globalization.CultureInfo.InvariantCulture, global::Newtonsoft.Json.Utilities.MiscellaneousUtils.ToString(key)));
				}
				SetItem((int)key, value);
			}
		}

		public global::Newtonsoft.Json.Linq.JToken this[int index]
		{
			get
			{
				return GetItem(index);
			}
			set
			{
				SetItem(index, value);
			}
		}

		bool global::System.Collections.Generic.ICollection<global::Newtonsoft.Json.Linq.JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public JArray()
		{
		}

		public JArray(global::Newtonsoft.Json.Linq.JArray other)
			: base(other)
		{
		}

		public JArray(params object[] content)
			: this((object)content)
		{
		}

		public JArray(object content)
		{
			Add(content);
		}

		internal override bool DeepEquals(global::Newtonsoft.Json.Linq.JToken node)
		{
			global::Newtonsoft.Json.Linq.JArray jArray = node as global::Newtonsoft.Json.Linq.JArray;
			if (jArray != null)
			{
				return ContentsEqual(jArray);
			}
			return false;
		}

		internal override global::Newtonsoft.Json.Linq.JToken CloneToken()
		{
			return new global::Newtonsoft.Json.Linq.JArray(this);
		}

		public new static global::Newtonsoft.Json.Linq.JArray Load(global::Newtonsoft.Json.JsonReader reader)
		{
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.None && !reader.Read())
			{
				throw new global::System.Exception("Error reading JArray from JsonReader.");
			}
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.StartArray)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}", global::System.Globalization.CultureInfo.InvariantCulture, reader.TokenType));
			}
			global::Newtonsoft.Json.Linq.JArray jArray = new global::Newtonsoft.Json.Linq.JArray();
			jArray.SetLineInfo(reader as global::Newtonsoft.Json.IJsonLineInfo);
			jArray.ReadTokenFrom(reader);
			return jArray;
		}

		public new static global::Newtonsoft.Json.Linq.JArray Parse(string json)
		{
			global::Newtonsoft.Json.JsonReader reader = new global::Newtonsoft.Json.JsonTextReader(new global::System.IO.StringReader(json));
			return Load(reader);
		}

		public new static global::Newtonsoft.Json.Linq.JArray FromObject(object o)
		{
			return FromObject(o, new global::Newtonsoft.Json.JsonSerializer());
		}

		public new static global::Newtonsoft.Json.Linq.JArray FromObject(object o, global::Newtonsoft.Json.JsonSerializer jsonSerializer)
		{
			global::Newtonsoft.Json.Linq.JToken jToken = global::Newtonsoft.Json.Linq.JToken.FromObjectInternal(o, jsonSerializer);
			if (jToken.Type != global::Newtonsoft.Json.Linq.JTokenType.Array)
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Object serialized to {0}. JArray instance expected.", global::System.Globalization.CultureInfo.InvariantCulture, jToken.Type));
			}
			return (global::Newtonsoft.Json.Linq.JArray)jToken;
		}

		public override void WriteTo(global::Newtonsoft.Json.JsonWriter writer, params global::Newtonsoft.Json.JsonConverter[] converters)
		{
			writer.WriteStartArray();
			foreach (global::Newtonsoft.Json.Linq.JToken childrenToken in ChildrenTokens)
			{
				childrenToken.WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		public int IndexOf(global::Newtonsoft.Json.Linq.JToken item)
		{
			return IndexOfItem(item);
		}

		public void Insert(int index, global::Newtonsoft.Json.Linq.JToken item)
		{
			InsertItem(index, item);
		}

		public void RemoveAt(int index)
		{
			RemoveItemAt(index);
		}

		public void Add(global::Newtonsoft.Json.Linq.JToken item)
		{
			Add((object)item);
		}

		public void Clear()
		{
			ClearItems();
		}

		public bool Contains(global::Newtonsoft.Json.Linq.JToken item)
		{
			return ContainsItem(item);
		}

		void global::System.Collections.Generic.ICollection<global::Newtonsoft.Json.Linq.JToken>.CopyTo(global::Newtonsoft.Json.Linq.JToken[] array, int arrayIndex)
		{
			CopyItemsTo(array, arrayIndex);
		}

		public bool Remove(global::Newtonsoft.Json.Linq.JToken item)
		{
			return RemoveItem(item);
		}

		internal override int GetDeepHashCode()
		{
			return ContentsHashCode();
		}
	}
}
