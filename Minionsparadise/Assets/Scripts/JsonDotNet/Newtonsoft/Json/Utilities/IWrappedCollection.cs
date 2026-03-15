namespace Newtonsoft.Json.Utilities
{
	internal interface IWrappedCollection : global::System.Collections.IList, global::System.Collections.ICollection, global::System.Collections.IEnumerable
	{
		object UnderlyingCollection { get; }
	}
}
