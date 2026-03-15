namespace Newtonsoft.Json.Utilities
{
	internal interface IWrappedDictionary : global::System.Collections.IDictionary, global::System.Collections.ICollection, global::System.Collections.IEnumerable
	{
		object UnderlyingDictionary { get; }
	}
}
