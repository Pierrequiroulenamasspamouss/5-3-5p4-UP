namespace Kampai.Util
{
	public static class IdentifiableUtils
	{
		public static T FindIdentifiable<T>(global::System.Collections.Generic.IEnumerable<T> bucket, int id) where T : global::Kampai.Util.Identifiable
		{
			if (bucket != null)
			{
				foreach (T item in bucket)
				{
					if (item.ID == id)
					{
						return item;
					}
				}
			}
			return default(T);
		}

		public static global::System.Collections.Generic.Dictionary<int, T> MapIdentifiables<T>(global::System.Collections.Generic.IEnumerable<T> bucket) where T : global::Kampai.Util.Identifiable
		{
			global::System.Collections.Generic.Dictionary<int, T> dictionary = new global::System.Collections.Generic.Dictionary<int, T>();
			if (bucket != null)
			{
				foreach (T item in bucket)
				{
					dictionary.Add(item.ID, item);
				}
			}
			return dictionary;
		}

		public static void SortById<T>(global::System.Collections.Generic.IList<T> source, bool ascending = true) where T : global::Kampai.Util.Identifiable
		{
			(source as global::System.Collections.Generic.List<T>).Sort((T p1, T p2) => (!ascending) ? (p2.ID - p1.ID) : (p1.ID - p2.ID));
		}

		public static global::System.Collections.Generic.IList<T> ListIdentifiables<T>(global::System.Collections.Generic.IDictionary<int, T> map) where T : global::Kampai.Util.Identifiable
		{
			return new global::System.Collections.Generic.List<T>(map.Values);
		}
	}
}
