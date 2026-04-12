namespace Newtonsoft.Json.Utilities
{
	internal class DictionaryWrapper<TKey, TValue> : global::System.Collections.Generic.IDictionary<TKey, TValue>, global::System.Collections.Generic.ICollection<global::System.Collections.Generic.KeyValuePair<TKey, TValue>>, global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<TKey, TValue>>, global::Newtonsoft.Json.Utilities.IWrappedDictionary, global::System.Collections.IDictionary, global::System.Collections.ICollection, global::System.Collections.IEnumerable
	{
		private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : global::System.Collections.IDictionaryEnumerator, global::System.Collections.IEnumerator
		{
			private readonly global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

			public global::System.Collections.DictionaryEntry Entry
			{
				get
				{
					return (global::System.Collections.DictionaryEntry)Current;
				}
			}

			public object Key
			{
				get
				{
					return Entry.Key;
				}
			}

			public object Value
			{
				get
				{
					return Entry.Value;
				}
			}

			public object Current
			{
				get
				{
					return new global::System.Collections.DictionaryEntry(_e.Current.Key, _e.Current.Value);
				}
			}

			public DictionaryEnumerator(global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(e, "e");
				_e = e;
			}

			public bool MoveNext()
			{
				return _e.MoveNext();
			}

			public void Reset()
			{
				_e.Reset();
			}
		}

		private readonly global::System.Collections.IDictionary _dictionary;

		private readonly global::System.Collections.Generic.IDictionary<TKey, TValue> _genericDictionary;

		private object _syncRoot;

		public global::System.Collections.Generic.ICollection<TKey> Keys
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary.Keys;
				}
				return global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Cast<TKey>(_dictionary.Keys));
			}
		}

		public global::System.Collections.Generic.ICollection<TValue> Values
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary.Values;
				}
				return global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Cast<TValue>(_dictionary.Values));
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary[key];
				}
				return (TValue)_dictionary[key];
			}
			set
			{
				if (_genericDictionary != null)
				{
					_genericDictionary[key] = value;
				}
				else
				{
					_dictionary[key] = value;
				}
			}
		}

		public int Count
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary.Count;
				}
				return _dictionary.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary.IsReadOnly;
				}
				return _dictionary.IsReadOnly;
			}
		}

		bool global::System.Collections.IDictionary.IsFixedSize
		{
			get
			{
				if (_genericDictionary != null)
				{
					return false;
				}
				return _dictionary.IsFixedSize;
			}
		}

		global::System.Collections.ICollection global::System.Collections.IDictionary.Keys
		{
			get
			{
				if (_genericDictionary != null)
				{
					return global::System.Linq.Enumerable.ToList(_genericDictionary.Keys);
				}
				return _dictionary.Keys;
			}
		}

		global::System.Collections.ICollection global::System.Collections.IDictionary.Values
		{
			get
			{
				if (_genericDictionary != null)
				{
					return global::System.Linq.Enumerable.ToList(_genericDictionary.Values);
				}
				return _dictionary.Values;
			}
		}

		object global::System.Collections.IDictionary.this[object key]
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary[(TKey)key];
				}
				return _dictionary[key];
			}
			set
			{
				if (_genericDictionary != null)
				{
					_genericDictionary[(TKey)key] = (TValue)value;
				}
				else
				{
					_dictionary[key] = value;
				}
			}
		}

		bool global::System.Collections.ICollection.IsSynchronized
		{
			get
			{
				if (_genericDictionary != null)
				{
					return false;
				}
				return _dictionary.IsSynchronized;
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

		public object UnderlyingDictionary
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary;
				}
				return _dictionary;
			}
		}

		public DictionaryWrapper(global::System.Collections.IDictionary dictionary)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			_dictionary = dictionary;
		}

		public DictionaryWrapper(global::System.Collections.Generic.IDictionary<TKey, TValue> dictionary)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			_genericDictionary = dictionary;
		}

		public void Add(TKey key, TValue value)
		{
			if (_genericDictionary != null)
			{
				_genericDictionary.Add(key, value);
			}
			else
			{
				_dictionary.Add(key, value);
			}
		}

		public bool ContainsKey(TKey key)
		{
			if (_genericDictionary != null)
			{
				return _genericDictionary.ContainsKey(key);
			}
			return _dictionary.Contains(key);
		}

		public bool Remove(TKey key)
		{
			if (_genericDictionary != null)
			{
				return _genericDictionary.Remove(key);
			}
			if (_dictionary.Contains(key))
			{
				_dictionary.Remove(key);
				return true;
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (_genericDictionary != null)
			{
				return _genericDictionary.TryGetValue(key, out value);
			}
			if (!_dictionary.Contains(key))
			{
				value = default(TValue);
				return false;
			}
			value = (TValue)_dictionary[key];
			return true;
		}

		public void Add(global::System.Collections.Generic.KeyValuePair<TKey, TValue> item)
		{
			if (_genericDictionary != null)
			{
				_genericDictionary.Add(item);
			}
			else
			{
				((global::System.Collections.IList)_dictionary).Add(item);
			}
		}

		public void Clear()
		{
			if (_genericDictionary != null)
			{
				_genericDictionary.Clear();
			}
			else
			{
				_dictionary.Clear();
			}
		}

		public bool Contains(global::System.Collections.Generic.KeyValuePair<TKey, TValue> item)
		{
			if (_genericDictionary != null)
			{
				return _genericDictionary.Contains(item);
			}
			return ((global::System.Collections.IList)_dictionary).Contains(item);
		}

		public void CopyTo(global::System.Collections.Generic.KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (_genericDictionary != null)
			{
				_genericDictionary.CopyTo(array, arrayIndex);
				return;
			}
			foreach (global::System.Collections.DictionaryEntry item in _dictionary)
			{
				array[arrayIndex++] = new global::System.Collections.Generic.KeyValuePair<TKey, TValue>((TKey)item.Key, (TValue)item.Value);
			}
		}

		public bool Remove(global::System.Collections.Generic.KeyValuePair<TKey, TValue> item)
		{
			if (_genericDictionary != null)
			{
				return _genericDictionary.Remove(item);
			}
			if (_dictionary.Contains(item.Key))
			{
				object objA = _dictionary[item.Key];
				if (object.Equals(objA, item.Value))
				{
					_dictionary.Remove(item.Key);
					return true;
				}
				return false;
			}
			return true;
		}

		public global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (_genericDictionary != null)
			{
				foreach (global::System.Collections.Generic.KeyValuePair<TKey, TValue> item in _genericDictionary)
				{
					yield return item;
				}
				yield break;
			}

			global::System.Collections.IEnumerator enumerator = _dictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				if (current is global::System.Collections.DictionaryEntry)
				{
					global::System.Collections.DictionaryEntry de = (global::System.Collections.DictionaryEntry)current;
					yield return new global::System.Collections.Generic.KeyValuePair<TKey, TValue>((TKey)de.Key, (TValue)de.Value);
				}
				else if (current is global::System.Collections.Generic.KeyValuePair<TKey, TValue>)
				{
					yield return (global::System.Collections.Generic.KeyValuePair<TKey, TValue>)current;
				}
				else
				{
					// Fallback for other potential types or just try to cast key/value if possible
					// Reflection or dynamic could be used but let's stick to these two for now
					// as they cover the known cases.
					global::System.Type type = current.GetType();
					global::System.Reflection.PropertyInfo keyProp = type.GetProperty("Key");
					global::System.Reflection.PropertyInfo valueProp = type.GetProperty("Value");
					if (keyProp != null && valueProp != null)
					{
						yield return new global::System.Collections.Generic.KeyValuePair<TKey, TValue>((TKey)keyProp.GetValue(current, null), (TValue)valueProp.GetValue(current, null));
					}
				}
			}
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		void global::System.Collections.IDictionary.Add(object key, object value)
		{
			if (_genericDictionary != null)
			{
				_genericDictionary.Add((TKey)key, (TValue)value);
			}
			else
			{
				_dictionary.Add(key, value);
			}
		}

		bool global::System.Collections.IDictionary.Contains(object key)
		{
			if (_genericDictionary != null)
			{
				return _genericDictionary.ContainsKey((TKey)key);
			}
			return _dictionary.Contains(key);
		}

		global::System.Collections.IDictionaryEnumerator global::System.Collections.IDictionary.GetEnumerator()
		{
			if (_genericDictionary != null)
			{
				return new global::Newtonsoft.Json.Utilities.DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(_genericDictionary.GetEnumerator());
			}
			return _dictionary.GetEnumerator();
		}

		public void Remove(object key)
		{
			if (_genericDictionary != null)
			{
				_genericDictionary.Remove((TKey)key);
			}
			else
			{
				_dictionary.Remove(key);
			}
		}

		void global::System.Collections.ICollection.CopyTo(global::System.Array array, int index)
		{
			if (_genericDictionary != null)
			{
				_genericDictionary.CopyTo((global::System.Collections.Generic.KeyValuePair<TKey, TValue>[])array, index);
			}
			else
			{
				_dictionary.CopyTo(array, index);
			}
		}
	}
}
