namespace Newtonsoft.Json.Linq
{
	public abstract class JContainer : global::Newtonsoft.Json.Linq.JToken, global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken>, global::System.Collections.Generic.ICollection<global::Newtonsoft.Json.Linq.JToken>, global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken>, global::System.Collections.IList, global::System.Collections.ICollection, global::System.Collections.IEnumerable
	{
		private class JTokenReferenceEqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Newtonsoft.Json.Linq.JToken>
		{
			public static readonly global::Newtonsoft.Json.Linq.JContainer.JTokenReferenceEqualityComparer Instance = new global::Newtonsoft.Json.Linq.JContainer.JTokenReferenceEqualityComparer();

			public bool Equals(global::Newtonsoft.Json.Linq.JToken x, global::Newtonsoft.Json.Linq.JToken y)
			{
				return object.ReferenceEquals(x, y);
			}

			public int GetHashCode(global::Newtonsoft.Json.Linq.JToken obj)
			{
				if (obj == null)
				{
					return 0;
				}
				return obj.GetHashCode();
			}
		}

		private object _syncRoot;

		private bool _busy;

		protected abstract global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken> ChildrenTokens { get; }

		public override bool HasValues
		{
			get
			{
				return ChildrenTokens.Count > 0;
			}
		}

		public override global::Newtonsoft.Json.Linq.JToken First
		{
			get
			{
				return global::System.Linq.Enumerable.FirstOrDefault(ChildrenTokens);
			}
		}

		public override global::Newtonsoft.Json.Linq.JToken Last
		{
			get
			{
				return global::System.Linq.Enumerable.LastOrDefault(ChildrenTokens);
			}
		}

		global::Newtonsoft.Json.Linq.JToken global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken>.this[int index]
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

		bool global::System.Collections.IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		bool global::System.Collections.IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		object global::System.Collections.IList.this[int index]
		{
			get
			{
				return GetItem(index);
			}
			set
			{
				SetItem(index, EnsureValue(value));
			}
		}

		public int Count
		{
			get
			{
				return ChildrenTokens.Count;
			}
		}

		bool global::System.Collections.ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object global::System.Collections.ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					global::System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);
				}
				return _syncRoot;
			}
		}

		internal JContainer()
		{
		}

		internal JContainer(global::Newtonsoft.Json.Linq.JContainer other)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(other, "c");
			foreach (global::Newtonsoft.Json.Linq.JToken item in (global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken>)other)
			{
				Add(item);
			}
		}

		internal void CheckReentrancy()
		{
			if (_busy)
			{
				throw new global::System.InvalidOperationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Cannot change {0} during a collection change event.", global::System.Globalization.CultureInfo.InvariantCulture, GetType()));
			}
		}

		internal bool ContentsEqual(global::Newtonsoft.Json.Linq.JContainer container)
		{
			global::Newtonsoft.Json.Linq.JToken jToken = First;
			global::Newtonsoft.Json.Linq.JToken jToken2 = container.First;
			if (jToken == jToken2)
			{
				return true;
			}
			while (true)
			{
				if (jToken == null && jToken2 == null)
				{
					return true;
				}
				if (jToken == null || jToken2 == null || !jToken.DeepEquals(jToken2))
				{
					break;
				}
				jToken = ((jToken != Last) ? jToken.Next : null);
				jToken2 = ((jToken2 != container.Last) ? jToken2.Next : null);
			}
			return false;
		}

		public override global::Newtonsoft.Json.Linq.JEnumerable<global::Newtonsoft.Json.Linq.JToken> Children()
		{
			return new global::Newtonsoft.Json.Linq.JEnumerable<global::Newtonsoft.Json.Linq.JToken>(ChildrenTokens);
		}

		public override global::System.Collections.Generic.IEnumerable<T> Values<T>()
		{
			return ChildrenTokens.Convert<global::Newtonsoft.Json.Linq.JToken, T>();
		}

		public global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Linq.JToken> Descendants()
		{
			foreach (global::Newtonsoft.Json.Linq.JToken o in ChildrenTokens)
			{
				yield return o;
				global::Newtonsoft.Json.Linq.JContainer c = o as global::Newtonsoft.Json.Linq.JContainer;
				if (c == null)
				{
					continue;
				}
				foreach (global::Newtonsoft.Json.Linq.JToken item in c.Descendants())
				{
					yield return item;
				}
			}
		}

		internal bool IsMultiContent(object content)
		{
			if (content is global::System.Collections.IEnumerable && !(content is string) && !(content is global::Newtonsoft.Json.Linq.JToken))
			{
				return !(content is byte[]);
			}
			return false;
		}

		internal global::Newtonsoft.Json.Linq.JToken EnsureParentToken(global::Newtonsoft.Json.Linq.JToken item)
		{
			if (item == null)
			{
				return new global::Newtonsoft.Json.Linq.JValue((object)null);
			}
			if (item.Parent != null)
			{
				item = item.CloneToken();
			}
			else
			{
				global::Newtonsoft.Json.Linq.JContainer jContainer = this;
				while (jContainer.Parent != null)
				{
					jContainer = jContainer.Parent;
				}
				if (item == jContainer)
				{
					item = item.CloneToken();
				}
			}
			return item;
		}

		internal int IndexOfItem(global::Newtonsoft.Json.Linq.JToken item)
		{
			return global::Newtonsoft.Json.Utilities.CollectionUtils.IndexOf(ChildrenTokens, item, global::Newtonsoft.Json.Linq.JContainer.JTokenReferenceEqualityComparer.Instance);
		}

		internal virtual void InsertItem(int index, global::Newtonsoft.Json.Linq.JToken item)
		{
			if (index > ChildrenTokens.Count)
			{
				throw new global::System.ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
			}
			CheckReentrancy();
			item = EnsureParentToken(item);
			global::Newtonsoft.Json.Linq.JToken jToken = ((index == 0) ? null : ChildrenTokens[index - 1]);
			global::Newtonsoft.Json.Linq.JToken jToken2 = ((index == ChildrenTokens.Count) ? null : ChildrenTokens[index]);
			ValidateToken(item, null);
			item.Parent = this;
			item.Previous = jToken;
			if (jToken != null)
			{
				jToken.Next = item;
			}
			item.Next = jToken2;
			if (jToken2 != null)
			{
				jToken2.Previous = item;
			}
			ChildrenTokens.Insert(index, item);
		}

		internal virtual void RemoveItemAt(int index)
		{
			if (index < 0)
			{
				throw new global::System.ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= ChildrenTokens.Count)
			{
				throw new global::System.ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			CheckReentrancy();
			global::Newtonsoft.Json.Linq.JToken jToken = ChildrenTokens[index];
			global::Newtonsoft.Json.Linq.JToken jToken2 = ((index == 0) ? null : ChildrenTokens[index - 1]);
			global::Newtonsoft.Json.Linq.JToken jToken3 = ((index == ChildrenTokens.Count - 1) ? null : ChildrenTokens[index + 1]);
			if (jToken2 != null)
			{
				jToken2.Next = jToken3;
			}
			if (jToken3 != null)
			{
				jToken3.Previous = jToken2;
			}
			jToken.Parent = null;
			jToken.Previous = null;
			jToken.Next = null;
			ChildrenTokens.RemoveAt(index);
		}

		internal virtual bool RemoveItem(global::Newtonsoft.Json.Linq.JToken item)
		{
			int num = IndexOfItem(item);
			if (num >= 0)
			{
				RemoveItemAt(num);
				return true;
			}
			return false;
		}

		internal virtual global::Newtonsoft.Json.Linq.JToken GetItem(int index)
		{
			return ChildrenTokens[index];
		}

		internal virtual void SetItem(int index, global::Newtonsoft.Json.Linq.JToken item)
		{
			if (index < 0)
			{
				throw new global::System.ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= ChildrenTokens.Count)
			{
				throw new global::System.ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			global::Newtonsoft.Json.Linq.JToken jToken = ChildrenTokens[index];
			if (!IsTokenUnchanged(jToken, item))
			{
				CheckReentrancy();
				item = EnsureParentToken(item);
				ValidateToken(item, jToken);
				global::Newtonsoft.Json.Linq.JToken jToken2 = ((index == 0) ? null : ChildrenTokens[index - 1]);
				global::Newtonsoft.Json.Linq.JToken jToken3 = ((index == ChildrenTokens.Count - 1) ? null : ChildrenTokens[index + 1]);
				item.Parent = this;
				item.Previous = jToken2;
				if (jToken2 != null)
				{
					jToken2.Next = item;
				}
				item.Next = jToken3;
				if (jToken3 != null)
				{
					jToken3.Previous = item;
				}
				ChildrenTokens[index] = item;
				jToken.Parent = null;
				jToken.Previous = null;
				jToken.Next = null;
			}
		}

		internal virtual void ClearItems()
		{
			CheckReentrancy();
			foreach (global::Newtonsoft.Json.Linq.JToken childrenToken in ChildrenTokens)
			{
				childrenToken.Parent = null;
				childrenToken.Previous = null;
				childrenToken.Next = null;
			}
			ChildrenTokens.Clear();
		}

		internal virtual void ReplaceItem(global::Newtonsoft.Json.Linq.JToken existing, global::Newtonsoft.Json.Linq.JToken replacement)
		{
			if (existing != null && existing.Parent == this)
			{
				int index = IndexOfItem(existing);
				SetItem(index, replacement);
			}
		}

		internal virtual bool ContainsItem(global::Newtonsoft.Json.Linq.JToken item)
		{
			return IndexOfItem(item) != -1;
		}

		internal virtual void CopyItemsTo(global::System.Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new global::System.ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new global::System.ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length)
			{
				throw new global::System.ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (Count > array.Length - arrayIndex)
			{
				throw new global::System.ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (global::Newtonsoft.Json.Linq.JToken childrenToken in ChildrenTokens)
			{
				array.SetValue(childrenToken, arrayIndex + num);
				num++;
			}
		}

		internal static bool IsTokenUnchanged(global::Newtonsoft.Json.Linq.JToken currentValue, global::Newtonsoft.Json.Linq.JToken newValue)
		{
			global::Newtonsoft.Json.Linq.JValue jValue = currentValue as global::Newtonsoft.Json.Linq.JValue;
			if (jValue != null)
			{
				if (jValue.Type == global::Newtonsoft.Json.Linq.JTokenType.Null && newValue == null)
				{
					return true;
				}
				return jValue.Equals(newValue);
			}
			return false;
		}

		internal virtual void ValidateToken(global::Newtonsoft.Json.Linq.JToken o, global::Newtonsoft.Json.Linq.JToken existing)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type == global::Newtonsoft.Json.Linq.JTokenType.Property)
			{
				throw new global::System.ArgumentException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Can not add {0} to {1}.", global::System.Globalization.CultureInfo.InvariantCulture, o.GetType(), GetType()));
			}
		}

		public virtual void Add(object content)
		{
			AddInternal(ChildrenTokens.Count, content);
		}

		public void AddFirst(object content)
		{
			AddInternal(0, content);
		}

		internal void AddInternal(int index, object content)
		{
			if (IsMultiContent(content))
			{
				global::System.Collections.IEnumerable enumerable = (global::System.Collections.IEnumerable)content;
				int num = index;
				{
					foreach (object item2 in enumerable)
					{
						AddInternal(num, item2);
						num++;
					}
					return;
				}
			}
			global::Newtonsoft.Json.Linq.JToken item = CreateFromContent(content);
			InsertItem(index, item);
		}

		internal global::Newtonsoft.Json.Linq.JToken CreateFromContent(object content)
		{
			if (content is global::Newtonsoft.Json.Linq.JToken)
			{
				return (global::Newtonsoft.Json.Linq.JToken)content;
			}
			return new global::Newtonsoft.Json.Linq.JValue(content);
		}

		public global::Newtonsoft.Json.JsonWriter CreateWriter()
		{
			return new global::Newtonsoft.Json.Linq.JTokenWriter(this);
		}

		public void ReplaceAll(object content)
		{
			ClearItems();
			Add(content);
		}

		public void RemoveAll()
		{
			ClearItems();
		}

		internal void ReadTokenFrom(global::Newtonsoft.Json.JsonReader r)
		{
			int depth = r.Depth;
			if (!r.Read())
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error reading {0} from JsonReader.", global::System.Globalization.CultureInfo.InvariantCulture, GetType().Name));
			}
			ReadContentFrom(r);
			int depth2 = r.Depth;
			if (depth2 > depth)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected end of content while loading {0}.", global::System.Globalization.CultureInfo.InvariantCulture, GetType().Name));
			}
		}

		internal void ReadContentFrom(global::Newtonsoft.Json.JsonReader r)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(r, "r");
			global::Newtonsoft.Json.IJsonLineInfo lineInfo = r as global::Newtonsoft.Json.IJsonLineInfo;
			global::Newtonsoft.Json.Linq.JContainer jContainer = this;
			do
			{
				if (jContainer is global::Newtonsoft.Json.Linq.JProperty && ((global::Newtonsoft.Json.Linq.JProperty)jContainer).Value != null)
				{
					if (jContainer == this)
					{
						break;
					}
					jContainer = jContainer.Parent;
				}
				switch (r.TokenType)
				{
				case global::Newtonsoft.Json.JsonToken.StartArray:
				{
					global::Newtonsoft.Json.Linq.JArray jArray = new global::Newtonsoft.Json.Linq.JArray();
					jArray.SetLineInfo(lineInfo);
					jContainer.Add(jArray);
					jContainer = jArray;
					break;
				}
				case global::Newtonsoft.Json.JsonToken.EndArray:
					if (jContainer == this)
					{
						return;
					}
					jContainer = jContainer.Parent;
					break;
				case global::Newtonsoft.Json.JsonToken.StartObject:
				{
					global::Newtonsoft.Json.Linq.JObject jObject2 = new global::Newtonsoft.Json.Linq.JObject();
					jObject2.SetLineInfo(lineInfo);
					jContainer.Add(jObject2);
					jContainer = jObject2;
					break;
				}
				case global::Newtonsoft.Json.JsonToken.EndObject:
					if (jContainer == this)
					{
						return;
					}
					jContainer = jContainer.Parent;
					break;
				case global::Newtonsoft.Json.JsonToken.StartConstructor:
				{
					global::Newtonsoft.Json.Linq.JConstructor jConstructor = new global::Newtonsoft.Json.Linq.JConstructor(r.Value.ToString());
					jConstructor.SetLineInfo(jConstructor);
					jContainer.Add(jConstructor);
					jContainer = jConstructor;
					break;
				}
				case global::Newtonsoft.Json.JsonToken.EndConstructor:
					if (jContainer == this)
					{
						return;
					}
					jContainer = jContainer.Parent;
					break;
				case global::Newtonsoft.Json.JsonToken.Integer:
				case global::Newtonsoft.Json.JsonToken.Float:
				case global::Newtonsoft.Json.JsonToken.String:
				case global::Newtonsoft.Json.JsonToken.Boolean:
				case global::Newtonsoft.Json.JsonToken.Date:
				case global::Newtonsoft.Json.JsonToken.Bytes:
				{
					global::Newtonsoft.Json.Linq.JValue jValue = new global::Newtonsoft.Json.Linq.JValue(r.Value);
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					break;
				}
				case global::Newtonsoft.Json.JsonToken.Comment:
				{
					global::Newtonsoft.Json.Linq.JValue jValue = global::Newtonsoft.Json.Linq.JValue.CreateComment(r.Value.ToString());
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					break;
				}
				case global::Newtonsoft.Json.JsonToken.Null:
				{
					global::Newtonsoft.Json.Linq.JValue jValue = new global::Newtonsoft.Json.Linq.JValue(null, global::Newtonsoft.Json.Linq.JTokenType.Null);
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					break;
				}
				case global::Newtonsoft.Json.JsonToken.Undefined:
				{
					global::Newtonsoft.Json.Linq.JValue jValue = new global::Newtonsoft.Json.Linq.JValue(null, global::Newtonsoft.Json.Linq.JTokenType.Undefined);
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					break;
				}
				case global::Newtonsoft.Json.JsonToken.PropertyName:
				{
					string name = r.Value.ToString();
					global::Newtonsoft.Json.Linq.JProperty jProperty = new global::Newtonsoft.Json.Linq.JProperty(name);
					jProperty.SetLineInfo(lineInfo);
					global::Newtonsoft.Json.Linq.JObject jObject = (global::Newtonsoft.Json.Linq.JObject)jContainer;
					global::Newtonsoft.Json.Linq.JProperty jProperty2 = jObject.Property(name);
					if (jProperty2 == null)
					{
						jContainer.Add(jProperty);
					}
					else
					{
						jProperty2.Replace(jProperty);
					}
					jContainer = jProperty;
					break;
				}
				default:
					throw new global::System.InvalidOperationException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("The JsonReader should not be on a token of type {0}.", global::System.Globalization.CultureInfo.InvariantCulture, r.TokenType));
				case global::Newtonsoft.Json.JsonToken.None:
					break;
				}
			}
			while (r.Read());
		}

		internal int ContentsHashCode()
		{
			int num = 0;
			foreach (global::Newtonsoft.Json.Linq.JToken childrenToken in ChildrenTokens)
			{
				num ^= childrenToken.GetDeepHashCode();
			}
			return num;
		}

		int global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken>.IndexOf(global::Newtonsoft.Json.Linq.JToken item)
		{
			return IndexOfItem(item);
		}

		void global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken>.Insert(int index, global::Newtonsoft.Json.Linq.JToken item)
		{
			InsertItem(index, item);
		}

		void global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken>.RemoveAt(int index)
		{
			RemoveItemAt(index);
		}

		void global::System.Collections.Generic.ICollection<global::Newtonsoft.Json.Linq.JToken>.Add(global::Newtonsoft.Json.Linq.JToken item)
		{
			Add(item);
		}

		void global::System.Collections.Generic.ICollection<global::Newtonsoft.Json.Linq.JToken>.Clear()
		{
			ClearItems();
		}

		bool global::System.Collections.Generic.ICollection<global::Newtonsoft.Json.Linq.JToken>.Contains(global::Newtonsoft.Json.Linq.JToken item)
		{
			return ContainsItem(item);
		}

		void global::System.Collections.Generic.ICollection<global::Newtonsoft.Json.Linq.JToken>.CopyTo(global::Newtonsoft.Json.Linq.JToken[] array, int arrayIndex)
		{
			CopyItemsTo(array, arrayIndex);
		}

		bool global::System.Collections.Generic.ICollection<global::Newtonsoft.Json.Linq.JToken>.Remove(global::Newtonsoft.Json.Linq.JToken item)
		{
			return RemoveItem(item);
		}

		private global::Newtonsoft.Json.Linq.JToken EnsureValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is global::Newtonsoft.Json.Linq.JToken)
			{
				return (global::Newtonsoft.Json.Linq.JToken)value;
			}
			throw new global::System.ArgumentException("Argument is not a JToken.");
		}

		int global::System.Collections.IList.Add(object value)
		{
			Add(EnsureValue(value));
			return Count - 1;
		}

		void global::System.Collections.IList.Clear()
		{
			ClearItems();
		}

		bool global::System.Collections.IList.Contains(object value)
		{
			return ContainsItem(EnsureValue(value));
		}

		int global::System.Collections.IList.IndexOf(object value)
		{
			return IndexOfItem(EnsureValue(value));
		}

		void global::System.Collections.IList.Insert(int index, object value)
		{
			InsertItem(index, EnsureValue(value));
		}

		void global::System.Collections.IList.Remove(object value)
		{
			RemoveItem(EnsureValue(value));
		}

		void global::System.Collections.IList.RemoveAt(int index)
		{
			RemoveItemAt(index);
		}

		void global::System.Collections.ICollection.CopyTo(global::System.Array array, int index)
		{
			CopyItemsTo(array, index);
		}
	}
}
