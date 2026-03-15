namespace Newtonsoft.Json.Linq
{
	public interface IJEnumerable<T> : global::System.Collections.Generic.IEnumerable<T>, global::System.Collections.IEnumerable where T : global::Newtonsoft.Json.Linq.JToken
	{
		global::Newtonsoft.Json.Linq.IJEnumerable<global::Newtonsoft.Json.Linq.JToken> this[object key] { get; }
	}
}
