namespace Facebook.Unity
{
	internal class AsyncRequestString : global::UnityEngine.MonoBehaviour
	{
		private global::System.Uri url;

		private global::Facebook.Unity.HttpMethod method;

		private global::System.Collections.Generic.IDictionary<string, string> formData;

		private global::UnityEngine.WWWForm query;

		private global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback;

		internal static void Post(global::System.Uri url, global::System.Collections.Generic.Dictionary<string, string> formData = null, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback = null)
		{
			Request(url, global::Facebook.Unity.HttpMethod.POST, formData, callback);
		}

		internal static void Get(global::System.Uri url, global::System.Collections.Generic.Dictionary<string, string> formData = null, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback = null)
		{
			Request(url, global::Facebook.Unity.HttpMethod.GET, formData, callback);
		}

		internal static void Request(global::System.Uri url, global::Facebook.Unity.HttpMethod method, global::UnityEngine.WWWForm query = null, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback = null)
		{
			global::Facebook.Unity.ComponentFactory.AddComponent<global::Facebook.Unity.AsyncRequestString>().SetUrl(url).SetMethod(method)
				.SetQuery(query)
				.SetCallback(callback);
		}

		internal static void Request(global::System.Uri url, global::Facebook.Unity.HttpMethod method, global::System.Collections.Generic.IDictionary<string, string> formData = null, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback = null)
		{
			global::Facebook.Unity.ComponentFactory.AddComponent<global::Facebook.Unity.AsyncRequestString>().SetUrl(url).SetMethod(method)
				.SetFormData(formData)
				.SetCallback(callback);
		}

		internal global::System.Collections.IEnumerator Start()
		{
			global::UnityEngine.WWW www;
			if (method == global::Facebook.Unity.HttpMethod.GET)
			{
				string urlParams = ((!url.AbsoluteUri.Contains("?")) ? "?" : "&");
				if (formData != null)
				{
					foreach (global::System.Collections.Generic.KeyValuePair<string, string> pair in formData)
					{
						urlParams += string.Format("{0}={1}&", global::System.Uri.EscapeDataString(pair.Key), global::System.Uri.EscapeDataString(pair.Value));
					}
				}
				global::System.Collections.Generic.Dictionary<string, string> headers = new global::System.Collections.Generic.Dictionary<string, string>();
				headers["User-Agent"] = global::Facebook.Unity.Constants.GraphApiUserAgent;
				www = new global::UnityEngine.WWW(string.Concat(url, urlParams), null, headers);
			}
			else
			{
				if (query == null)
				{
					query = new global::UnityEngine.WWWForm();
				}
				if (method == global::Facebook.Unity.HttpMethod.DELETE)
				{
					query.AddField("method", "delete");
				}
				if (formData != null)
				{
					foreach (global::System.Collections.Generic.KeyValuePair<string, string> pair2 in formData)
					{
						query.AddField(pair2.Key, pair2.Value);
					}
				}
				query.headers["User-Agent"] = global::Facebook.Unity.Constants.GraphApiUserAgent;
				www = new global::UnityEngine.WWW(url.AbsoluteUri, query);
			}
			yield return www;
			if (callback != null)
			{
				callback(new global::Facebook.Unity.GraphResult(www));
			}
			www.Dispose();
			global::UnityEngine.Object.Destroy(this);
		}

		internal global::Facebook.Unity.AsyncRequestString SetUrl(global::System.Uri url)
		{
			this.url = url;
			return this;
		}

		internal global::Facebook.Unity.AsyncRequestString SetMethod(global::Facebook.Unity.HttpMethod method)
		{
			this.method = method;
			return this;
		}

		internal global::Facebook.Unity.AsyncRequestString SetFormData(global::System.Collections.Generic.IDictionary<string, string> formData)
		{
			this.formData = formData;
			return this;
		}

		internal global::Facebook.Unity.AsyncRequestString SetQuery(global::UnityEngine.WWWForm query)
		{
			this.query = query;
			return this;
		}

		internal global::Facebook.Unity.AsyncRequestString SetCallback(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback)
		{
			this.callback = callback;
			return this;
		}
	}
}
