namespace Newtonsoft.Json.Utilities
{
	internal class BidirectionalDictionary<TFirst, TSecond>
	{
		private readonly global::System.Collections.Generic.IDictionary<TFirst, TSecond> _firstToSecond;

		private readonly global::System.Collections.Generic.IDictionary<TSecond, TFirst> _secondToFirst;

		public BidirectionalDictionary()
			: this((global::System.Collections.Generic.IEqualityComparer<TFirst>)global::System.Collections.Generic.EqualityComparer<TFirst>.Default, (global::System.Collections.Generic.IEqualityComparer<TSecond>)global::System.Collections.Generic.EqualityComparer<TSecond>.Default)
		{
		}

		public BidirectionalDictionary(global::System.Collections.Generic.IEqualityComparer<TFirst> firstEqualityComparer, global::System.Collections.Generic.IEqualityComparer<TSecond> secondEqualityComparer)
		{
			_firstToSecond = new global::System.Collections.Generic.Dictionary<TFirst, TSecond>(firstEqualityComparer);
			_secondToFirst = new global::System.Collections.Generic.Dictionary<TSecond, TFirst>(secondEqualityComparer);
		}

		public void Add(TFirst first, TSecond second)
		{
			if (_firstToSecond.ContainsKey(first) || _secondToFirst.ContainsKey(second))
			{
				throw new global::System.ArgumentException("Duplicate first or second");
			}
			_firstToSecond.Add(first, second);
			_secondToFirst.Add(second, first);
		}

		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return _firstToSecond.TryGetValue(first, out second);
		}

		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return _secondToFirst.TryGetValue(second, out first);
		}
	}
}
