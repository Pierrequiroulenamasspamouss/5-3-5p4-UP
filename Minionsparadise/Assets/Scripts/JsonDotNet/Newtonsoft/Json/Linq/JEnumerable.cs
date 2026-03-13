namespace Newtonsoft.Json.Linq
{
	public struct JEnumerable<T> : global::Newtonsoft.Json.Linq.IJEnumerable<T>, global::System.Collections.Generic.IEnumerable<T>, global::System.Collections.IEnumerable where T : global::Newtonsoft.Json.Linq.JToken
	{
		public static readonly global::Newtonsoft.Json.Linq.JEnumerable<T> Empty = new global::Newtonsoft.Json.Linq.JEnumerable<T>(global::System.Linq.Enumerable.Empty<T>());

		private global::System.Collections.Generic.IEnumerable<T> _enumerable;

		public global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken> this[object key]
		{
			get
			{
				return new global::Newtonsoft.Json.Linq.JEnumerable<global::Newtonsoft.Json.Linq.JToken>(_enumerable.Values<T, global::Newtonsoft.Json.Linq.JToken>(key));
			}
		}

		public JEnumerable(global::System.Collections.Generic.IEnumerable<T> enumerable)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			_enumerable = enumerable;
		}

		public global::System.Collections.Generic.IEnumerator<T> GetEnumerator()
		{
			return _enumerable.GetEnumerator();
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override bool Equals(object obj)
		{
			if (obj is global::Newtonsoft.Json.Linq.JEnumerable<T>)
			{
				return _enumerable.Equals(((global::Newtonsoft.Json.Linq.JEnumerable<T>)obj)._enumerable);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return _enumerable.GetHashCode();
		}
	}
}
