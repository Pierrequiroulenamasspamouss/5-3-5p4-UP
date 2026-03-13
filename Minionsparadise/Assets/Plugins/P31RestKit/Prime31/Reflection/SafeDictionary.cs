namespace Prime31.Reflection
{
	public class SafeDictionary<TKey, TValue>
	{
		private readonly object _padlock = new object();

		private readonly global::System.Collections.Generic.Dictionary<TKey, TValue> _dictionary = new global::System.Collections.Generic.Dictionary<TKey, TValue>();

		public TValue this[TKey key]
		{
			get
			{
				return _dictionary[key];
			}
		}

		public bool tryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return ((global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<TKey, TValue>>)_dictionary).GetEnumerator();
		}

		public void add(TKey key, TValue value)
		{
			lock (_padlock)
			{
				if (!_dictionary.ContainsKey(key))
				{
					_dictionary.Add(key, value);
				}
			}
		}
	}
}
