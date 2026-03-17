namespace Newtonsoft.Json.Utilities
{
	internal class ThreadSafeStore<TKey, TValue>
	{
		private readonly object _lock = new object();

		private global::System.Collections.Generic.Dictionary<TKey, TValue> _store;

		private readonly global::System.Func<TKey, TValue> _creator;

		public ThreadSafeStore(global::System.Func<TKey, TValue> creator)
		{
			if (creator == null)
			{
				throw new global::System.ArgumentNullException("creator");
			}
			_creator = creator;
		}

		public TValue Get(TKey key)
		{
			if (_store == null)
			{
				return AddValue(key);
			}
			TValue value;
			if (!_store.TryGetValue(key, out value))
			{
				return AddValue(key);
			}
			return value;
		}

		private TValue AddValue(TKey key)
		{
			TValue val = _creator(key);
			lock (_lock)
			{
				if (_store == null)
				{
					_store = new global::System.Collections.Generic.Dictionary<TKey, TValue>();
					_store[key] = val;
				}
				else
				{
					TValue value;
					if (_store.TryGetValue(key, out value))
					{
						return value;
					}
					global::System.Collections.Generic.Dictionary<TKey, TValue> dictionary = new global::System.Collections.Generic.Dictionary<TKey, TValue>(_store);
					dictionary[key] = val;
					_store = dictionary;
				}
				return val;
			}
		}
	}
}
