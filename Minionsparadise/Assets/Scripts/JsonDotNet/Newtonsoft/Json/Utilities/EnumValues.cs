namespace Newtonsoft.Json.Utilities
{
	internal class EnumValues<T> : global::System.Collections.ObjectModel.KeyedCollection<string, global::Newtonsoft.Json.Utilities.EnumValue<T>> where T : struct
	{
		protected override string GetKeyForItem(global::Newtonsoft.Json.Utilities.EnumValue<T> item)
		{
			return item.Name;
		}
	}
}
