namespace Swrve.REST
{
	public interface IRESTClient
	{
		global::System.Collections.IEnumerator Get(string url, global::System.Action<global::Swrve.REST.RESTResponse> listener);

		global::System.Collections.IEnumerator Post(string url, byte[] encodedData, global::System.Collections.Generic.Dictionary<string, string> headers, global::System.Action<global::Swrve.REST.RESTResponse> listener);
	}
}
