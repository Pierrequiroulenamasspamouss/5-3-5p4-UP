namespace Kampai.Util
{
	public static class ItemUtil
	{
		public static global::System.Collections.Generic.IList<T> SortQIByQuantity<T>(global::System.Collections.Generic.IEnumerable<T> items, bool ascending = true) where T : global::Kampai.Util.QuantityItem
		{
			global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>(items);
			list.Sort((T p1, T p2) => (int)((!ascending) ? (p2.Quantity - p1.Quantity) : (p1.Quantity - p2.Quantity)));
			return list;
		}

		public static global::System.Collections.Generic.IList<T> SortItemsByQuantity<T>(global::System.Collections.Generic.IEnumerable<T> items, bool ascending = true) where T : global::Kampai.Game.Item
		{
			global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>(items);
			list.Sort((T p1, T p2) => (int)((!ascending) ? (p2.Quantity - p1.Quantity) : (p1.Quantity - p2.Quantity)));
			return list;
		}

		public static bool CompareSKU(string SKU, string ExternalIdentifier)
		{
			return SKU.Trim().ToLower().Equals(ExternalIdentifier.Trim().ToLower());
		}
	}
}
