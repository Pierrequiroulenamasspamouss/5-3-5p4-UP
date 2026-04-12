namespace Swrve.REST
{
	public class RESTClient : global::Swrve.REST.IRESTClient
	{
		private global::System.Collections.Generic.List<string> metrics = new global::System.Collections.Generic.List<string>();

		public virtual global::System.Collections.IEnumerator Get(string url, global::System.Action<global::Swrve.REST.RESTResponse> listener)
		{
			global::System.Collections.Generic.Dictionary<string, string> headers = new global::System.Collections.Generic.Dictionary<string, string>();
			if (!global::UnityEngine.Application.isEditor)
			{
				headers = AddMetricsHeader(headers);
				headers.Add("Accept-Encoding", "gzip");
			}
			long start = global::Swrve.Helpers.SwrveHelper.GetMilliseconds();
			using (global::UnityEngine.WWW www = global::Swrve.CrossPlatformUtils.MakeWWW(url, null, headers))
			{
				yield return www;
				long wwwTime = global::Swrve.Helpers.SwrveHelper.GetMilliseconds() - start;
				ProcessResponse(www, wwwTime, url, listener);
			}
		}

		public virtual global::System.Collections.IEnumerator Post(string url, byte[] encodedData, global::System.Collections.Generic.Dictionary<string, string> headers, global::System.Action<global::Swrve.REST.RESTResponse> listener)
		{
			if (!global::UnityEngine.Application.isEditor)
			{
				headers = AddMetricsHeader(headers);
			}
			long start = global::Swrve.Helpers.SwrveHelper.GetMilliseconds();
			using (global::UnityEngine.WWW www = global::Swrve.CrossPlatformUtils.MakeWWW(url, encodedData, headers))
			{
				yield return www;
				long wwwTime = global::Swrve.Helpers.SwrveHelper.GetMilliseconds() - start;
				ProcessResponse(www, wwwTime, url, listener);
			}
		}

		protected global::System.Collections.Generic.Dictionary<string, string> AddMetricsHeader(global::System.Collections.Generic.Dictionary<string, string> headers)
		{
			if (metrics.Count > 0)
			{
				string value = string.Join(";", metrics.ToArray());
				headers.Add("Swrve-Latency-Metrics", value);
				metrics.Clear();
			}
			return headers;
		}

		private void AddMetrics(string url, long wwwTime, bool error)
		{
			global::System.Uri uri = new global::System.Uri(url);
			url = string.Format("{0}{1}{2}", uri.Scheme, global::System.Uri.SchemeDelimiter, uri.Authority);
			string item = ((!error) ? string.Format("u={0},c={1},sh={1},sb={1},rh={1},rb={1}", url, wwwTime.ToString()) : string.Format("u={0},c={1},c_error=1", url, wwwTime.ToString()));
			metrics.Add(item);
		}

		protected void ProcessResponse(global::UnityEngine.WWW www, long wwwTime, string url, global::System.Action<global::Swrve.REST.RESTResponse> listener)
		{
			try
			{
				global::Swrve.Helpers.WwwDeducedError wwwDeducedError = global::Swrve.Helpers.UnityWwwHelper.DeduceWwwError(www);
				if (wwwDeducedError == global::Swrve.Helpers.WwwDeducedError.NoError)
				{
					string decodedString = null;
					bool flag = global::Swrve.Helpers.ResponseBodyTester.TestUTF8(www.bytes, out decodedString);
					global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
					string value = null;
					if (www.responseHeaders != null)
					{
						foreach (string key in www.responseHeaders.Keys)
						{
							if (string.Equals(key, "Content-Encoding", global::System.StringComparison.OrdinalIgnoreCase))
							{
								www.responseHeaders.TryGetValue(key, out value);
								break;
							}
							dictionary.Add(key.ToUpper(), www.responseHeaders[key]);
						}
					}
					if (www.bytes != null && www.bytes.Length > 4 && value != null && string.Equals(value, "gzip", global::System.StringComparison.OrdinalIgnoreCase) && decodedString != null && (!decodedString.StartsWith("{") || !decodedString.EndsWith("}")) && (!decodedString.StartsWith("[") || !decodedString.EndsWith("]")))
					{
						int num = global::System.BitConverter.ToInt32(www.bytes, 0);
						if (num > 0)
						{
							byte[] array = new byte[num];
							using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(www.bytes))
							{
								using (global::ICSharpCode.SharpZipLib.GZip.GZipInputStream gZipInputStream = new global::ICSharpCode.SharpZipLib.GZip.GZipInputStream(memoryStream))
								{
									gZipInputStream.Read(array, 0, array.Length);
									gZipInputStream.Close();
								}
								flag = global::Swrve.Helpers.ResponseBodyTester.TestUTF8(array, out decodedString);
								memoryStream.Close();
							}
						}
					}
					if (flag)
					{
						AddMetrics(url, wwwTime, false);
						listener(new global::Swrve.REST.RESTResponse(decodedString, dictionary));
					}
					else
					{
						AddMetrics(url, wwwTime, true);
						listener(new global::Swrve.REST.RESTResponse(global::Swrve.Helpers.WwwDeducedError.ApplicationErrorBody));
					}
				}
				else
				{
					AddMetrics(url, wwwTime, true);
					listener(new global::Swrve.REST.RESTResponse(wwwDeducedError));
				}
			}
			catch (global::System.Exception message)
			{
				SwrveLog.LogError(message);
			}
		}
	}
}
