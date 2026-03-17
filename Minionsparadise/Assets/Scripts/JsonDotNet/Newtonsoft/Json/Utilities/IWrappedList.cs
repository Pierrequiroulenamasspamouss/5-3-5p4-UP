namespace Newtonsoft.Json.Utilities
{
	internal interface IWrappedList : global::System.Collections.IList, global::System.Collections.ICollection, global::System.Collections.IEnumerable
	{
		object UnderlyingList { get; }
	}
}
