namespace Prime31
{
	public class P31RestKit
	{
		protected string _baseUrl;

		public bool debugRequests = false;

		protected bool forceJsonResponse;

		private global::UnityEngine.GameObject _surrogateGameObject;

		private global::UnityEngine.MonoBehaviour _surrogateMonobehaviour;

		protected virtual global::UnityEngine.GameObject surrogateGameObject
		{
			get
			{
				if (_surrogateGameObject == null)
				{
					_surrogateGameObject = global::UnityEngine.GameObject.Find("P31CoroutineSurrogate");
					if (_surrogateGameObject == null)
					{
						_surrogateGameObject = new global::UnityEngine.GameObject("P31CoroutineSurrogate");
						global::UnityEngine.Object.DontDestroyOnLoad(_surrogateGameObject);
					}
				}
				return _surrogateGameObject;
			}
			set
			{
				_surrogateGameObject = value;
			}
		}

		protected global::UnityEngine.MonoBehaviour surrogateMonobehaviour
		{
			get
			{
				if (_surrogateMonobehaviour == null)
				{
					_surrogateMonobehaviour = surrogateGameObject.AddComponent<global::UnityEngine.MonoBehaviour>();
				}
				return _surrogateMonobehaviour;
			}
			set
			{
				_surrogateMonobehaviour = value;
			}
		}

		protected virtual global::System.Collections.IEnumerator send(string path, global::Prime31.HTTPVerb httpVerb, global::System.Collections.Generic.Dictionary<string, object> parameters, global::System.Action<string, object> onComplete)
		{
			if (path.StartsWith("/"))
			{
				path = path.Substring(1);
			}
			global::UnityEngine.WWW www = processRequest(path, httpVerb, parameters);
			yield return www;
			if (debugRequests)
			{
				global::UnityEngine.Debug.Log("response error: " + www.error);
				global::UnityEngine.Debug.Log("response text: " + www.text);
				global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
				stringBuilder.Append("Response Headers:\n");
				foreach (global::System.Collections.Generic.KeyValuePair<string, string> responseHeader in www.responseHeaders)
				{
					stringBuilder.AppendFormat("{0}: {1}\n", responseHeader.Key, responseHeader.Value);
				}
				global::UnityEngine.Debug.Log(stringBuilder.ToString());
			}
			if (onComplete != null)
			{
				processResponse(www, onComplete);
			}
			www.Dispose();
		}

		protected virtual global::UnityEngine.WWW processRequest(string path, global::Prime31.HTTPVerb httpVerb, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			if (!path.StartsWith("http"))
			{
				stringBuilder.Append(_baseUrl).Append(path);
			}
			else
			{
				stringBuilder.Append(path);
			}
			bool flag = httpVerb != global::Prime31.HTTPVerb.GET;
			global::UnityEngine.WWWForm wWWForm = ((!flag) ? null : new global::UnityEngine.WWWForm());
			if (parameters != null && parameters.Count > 0)
			{
				if (flag)
				{
					foreach (global::System.Collections.Generic.KeyValuePair<string, object> parameter in parameters)
					{
						if (parameter.Value is string)
						{
							wWWForm.AddField(parameter.Key, parameter.Value as string);
						}
						else if (parameter.Value is byte[])
						{
							wWWForm.AddBinaryData(parameter.Key, parameter.Value as byte[]);
						}
					}
				}
				else
				{
					bool flag2 = true;
					if (path.Contains("?"))
					{
						flag2 = false;
					}
					foreach (global::System.Collections.Generic.KeyValuePair<string, object> parameter2 in parameters)
					{
						if (parameter2.Value is string)
						{
							stringBuilder.AppendFormat("{0}{1}={2}", (!flag2) ? "&" : "?", global::UnityEngine.WWW.EscapeURL(parameter2.Key), global::UnityEngine.WWW.EscapeURL(parameter2.Value as string));
							flag2 = false;
						}
					}
				}
			}
			if (debugRequests)
			{
				global::UnityEngine.Debug.Log("url: " + stringBuilder.ToString());
			}
			global::System.Collections.Generic.Dictionary<string, string> dictionary = null;
			if (flag)
			{
				global::System.Collections.IDictionary headersFromForm = getHeadersFromForm(wWWForm);
				if (headersFromForm != null)
				{
					dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
					if (headersFromForm.Contains("Content-Type"))
					{
						dictionary.Add("Content-Type", headersFromForm["Content-Type"].ToString());
					}
					if (debugRequests)
					{
						global::UnityEngine.Debug.Log("Found a POST request. Fetching headers from WWWForm and starting with these as a base: ");
						global::Prime31.Utils.logObject(dictionary);
					}
				}
			}
			return (!flag) ? new global::UnityEngine.WWW(stringBuilder.ToString()) : new global::UnityEngine.WWW(stringBuilder.ToString(), wWWForm.data, headersForRequest(httpVerb, dictionary));
		}

		protected virtual global::System.Collections.Generic.Dictionary<string, string> headersForRequest(global::Prime31.HTTPVerb httpVerb, global::System.Collections.Generic.Dictionary<string, string> headers = null)
		{
			headers = headers ?? new global::System.Collections.Generic.Dictionary<string, string>();
			switch (httpVerb)
			{
			case global::Prime31.HTTPVerb.GET:
				headers.Add("METHOD", "GET");
				break;
			case global::Prime31.HTTPVerb.POST:
				headers.Add("METHOD", "POST");
				break;
			case global::Prime31.HTTPVerb.PUT:
				headers.Add("METHOD", "PUT");
				headers.Add("X-HTTP-Method-Override", "PUT");
				break;
			case global::Prime31.HTTPVerb.DELETE:
				headers.Add("METHOD", "DELETE");
				headers.Add("X-HTTP-Method-Override", "DELETE");
				break;
			}
			return headers;
		}

		protected virtual void processResponse(global::UnityEngine.WWW www, global::System.Action<string, object> onComplete)
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				onComplete(www.error, null);
			}
			else if (isResponseJson(www))
			{
				object obj = global::Prime31.Json.decode(www.text);
				if (obj == null)
				{
					obj = www.text;
				}
				onComplete(null, obj);
			}
			else
			{
				onComplete(null, www.text);
			}
		}

		protected bool isResponseJson(global::UnityEngine.WWW www)
		{
			bool flag = false;
			if (forceJsonResponse)
			{
				flag = true;
			}
			if (!flag)
			{
				foreach (global::System.Collections.Generic.KeyValuePair<string, string> responseHeader in www.responseHeaders)
				{
					if (responseHeader.Key.ToLower() == "content-type" && (responseHeader.Value.ToLower().Contains("/json") || responseHeader.Value.ToLower().Contains("/javascript")))
					{
						flag = true;
					}
				}
			}
			if (flag && !www.text.StartsWith("[") && !www.text.StartsWith("{"))
			{
				return false;
			}
			return flag;
		}

		protected virtual global::System.Collections.IDictionary getHeadersFromForm(global::UnityEngine.WWWForm form)
		{
			try
			{
				global::System.Reflection.PropertyInfo property = form.GetType().GetProperty("headers");
				if (property != null)
				{
					return property.GetValue(form, null) as global::System.Collections.IDictionary;
				}
				global::UnityEngine.Debug.Log("couldnt find the 'headers' property on the WWWForm object: " + form);
			}
			catch (global::System.Exception ex)
			{
				global::UnityEngine.Debug.Log("ran into a problem transferring headers from WWWForm to the WWW request: " + ex);
			}
			return null;
		}

		public void setBaseUrl(string baseUrl)
		{
			_baseUrl = baseUrl;
		}

		public void get(string path, global::System.Action<string, object> completionHandler)
		{
			get(path, null, completionHandler);
		}

		public void get(string path, global::System.Collections.Generic.Dictionary<string, object> parameters, global::System.Action<string, object> completionHandler)
		{
			surrogateMonobehaviour.StartCoroutine(send(path, global::Prime31.HTTPVerb.GET, parameters, completionHandler));
		}

		public void post(string path, global::System.Action<string, object> completionHandler)
		{
			post(path, null, completionHandler);
		}

		public void post(string path, global::System.Collections.Generic.Dictionary<string, object> parameters, global::System.Action<string, object> completionHandler)
		{
			surrogateMonobehaviour.StartCoroutine(send(path, global::Prime31.HTTPVerb.POST, parameters, completionHandler));
		}

		public void put(string path, global::System.Action<string, object> completionHandler)
		{
			put(path, null, completionHandler);
		}

		public void put(string path, global::System.Collections.Generic.Dictionary<string, object> parameters, global::System.Action<string, object> completionHandler)
		{
			surrogateMonobehaviour.StartCoroutine(send(path, global::Prime31.HTTPVerb.PUT, parameters, completionHandler));
		}
	}
}
