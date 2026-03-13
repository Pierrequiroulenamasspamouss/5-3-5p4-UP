namespace Ea.Sharkbite.HttpPlugin.Http.Impl
{
	public class DefaultRequest : global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest
	{
		public const long UNDEFINED_CONTENT_LENGTH = 0L;

		private const int BLOCK_SIZE = 8192;

		protected const long MIN_NOTIFY_DELTA = 102400L;

		private bool useGZip;

		protected global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress progress;

		protected global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> notifyAction;

		protected bool abort;

		protected bool isRestarted;

		public string Uri { get; set; }

		public virtual string Method { get; set; }

		public byte[] Body { get; set; }

		public string Accept { get; set; }

		public string ContentType { get; set; }

		public string Username { get; set; }

		public string Password { private get; set; }

		public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<string, string>> QueryParams { get; set; }

		public global::System.Collections.Generic.Dictionary<string, string> Headers { get; set; }

		public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<string, string>> FormParams { get; set; }

		protected long Range { get; set; }

		public bool CanRetry { get; set; }

		public int RetryCount { get; set; }

		public bool TryResume { get; set; }

		public string FilePath { get; set; }

		public string Md5 { get; set; }

		public bool UseUdp { get; set; }

		public int requestCount { get; set; }

		public bool UseGZip
		{
			get
			{
				return useGZip;
			}
			set
			{
				useGZip = value;
				Headers["Accept-Encoding"] = ((!useGZip) ? "identity" : "gzip");
			}
		}

		public bool AvoidBackup { get; set; }

		public bool RunInBackground { get; set; }

		public global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> ResponseSignal { get; set; }

		public global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> ProgressSignal { get; set; }

		public DefaultRequest(string uri)
		{
			if (string.IsNullOrEmpty(uri))
			{
				throw new global::System.ArgumentNullException();
			}
			global::System.Uri uri2 = new global::System.Uri(uri);
			switch (uri2.Scheme.ToLower())
			{
			default:
				throw new global::System.ArgumentException("Only HTTP and HTTPS schemes supported");
			case "http":
			case "https":
				if (!string.IsNullOrEmpty(uri2.Query))
				{
					throw new global::System.ArgumentException("Query parameters should be set using the WithQueryParam method, rather than set directly in the Uri");
				}
				Uri = uri;
				Method = "GET";
				Body = null;
				Accept = string.Empty;
				ContentType = string.Empty;
				QueryParams = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<string, string>>();
				Headers = new global::System.Collections.Generic.Dictionary<string, string>();
				FormParams = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<string, string>>();
				Range = 0L;
				UseGZip = false;
				requestCount = 0;
				AvoidBackup = false;
				progress = new global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress(uri);
				break;
			}
		}

		public virtual void Get(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			Method = "GET";
			Execute(callback);
		}

		public virtual void Head(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			Method = "HEAD";
			Execute(callback);
		}

		public virtual void Options(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			Method = "OPTIONS";
			Execute(callback);
		}

		public virtual void Post(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			Method = "POST";
			Execute(callback);
		}

		public virtual void Put(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			Method = "PUT";
			Execute(callback);
		}

		public virtual void Delete(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			Method = "DELETE";
			Execute(callback);
		}

		public virtual void Execute(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			global::System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				GetResponse(callback);
			});
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithContentType(string contentType)
		{
			ContentType = contentType;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithAccept(string accept)
		{
			Accept = accept;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithQueryParam(string key, string value)
		{
			QueryParams.Add(new global::System.Collections.Generic.KeyValuePair<string, string>(key, value));
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithHeaderParam(string key, string value)
		{
			Headers[key] = value;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithFormParam(string key, string value)
		{
			FormParams.Add(new global::System.Collections.Generic.KeyValuePair<string, string>(key, value));
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithBasicAuth(string username, string password)
		{
			Username = username;
			Password = password;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithBody(byte[] body)
		{
			Body = body;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithPreprocessor(global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestPreprocessor preprocessor)
		{
			preprocessor.preprocess(this);
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithMethod(string method)
		{
			Method = method;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithOutputFile(string filePath)
		{
			FilePath = filePath;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithMd5(string md5)
		{
			Md5 = md5;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithGZip(bool useGZip)
		{
			UseGZip = useGZip;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithUdp(bool useUdp)
		{
			UseUdp = useUdp;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithAvoidBackup(bool avoidBackup)
		{
			AvoidBackup = avoidBackup;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithRunInBackground(bool runInBackground)
		{
			RunInBackground = runInBackground;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithResponseSignal(global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> responseSignal)
		{
			ResponseSignal = responseSignal;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithProgressSignal(global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> progressSignal)
		{
			ProgressSignal = progressSignal;
			return this;
		}

		public void RegisterNotifiable(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> notify)
		{
			notifyAction = notify;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithRetry(bool retry = true, int times = 3)
		{
			CanRetry = retry;
			RetryCount = times;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithResume(bool tryResume)
		{
			TryResume = tryResume;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithEntity(object entity)
		{
			Body = global::Kampai.Util.FastJSONSerializer.SerializeUTF8(entity);
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithRequestCount(int requestCount)
		{
			this.requestCount = requestCount;
			return this;
		}

		public virtual void Abort()
		{
			abort = true;
			if (isRestarted)
			{
				isRestarted = false;
			}
		}

		public virtual bool IsAborted()
		{
			return abort;
		}

		public virtual void Restart()
		{
			Abort();
			isRestarted = true;
		}

		public virtual bool IsRestarted()
		{
			return isRestarted;
		}

		protected virtual void TryRestart()
		{
			if (isRestarted)
			{
				abort = false;
				isRestarted = false;
			}
			if (!abort)
			{
				progress = new global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress(Uri);
			}
		}

		public virtual global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress GetProgress()
		{
			return (progress == null) ? null : progress.Clone();
		}

		public virtual string GetTempFilePath()
		{
			return string.Format("{0}.download", FilePath);
		}

		protected string GetUriWithQueryParams()
		{
			string value = ((!UseUdp) ? Uri : global::Kampai.Util.MediaClient.ConvertUrl(Uri));
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder(value);
			if (QueryParams.Count > 0)
			{
				stringBuilder.Append("?");
				global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
				foreach (global::System.Collections.Generic.KeyValuePair<string, string> queryParam in QueryParams)
				{
					list.Add(string.Format("{0}={1}", global::UnityEngine.WWW.EscapeURL(queryParam.Key), global::UnityEngine.WWW.EscapeURL(queryParam.Value)));
				}
				stringBuilder.Append(string.Join("&", list.ToArray()));
			}
			return stringBuilder.ToString();
		}

		protected string GetBasicAuthHeader()
		{
			return string.Format("Basic {0}", global::System.Convert.ToBase64String(global::System.Text.Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Username, Password))));
		}

		protected virtual global::System.Net.HttpWebRequest CreateRequest()
		{
			global::System.Net.HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = global::System.Net.WebRequest.Create(new global::System.Uri(GetUriWithQueryParams())) as global::System.Net.HttpWebRequest;
				httpWebRequest.Timeout = global::Kampai.Util.HttpRequestConfig.HttpRequestTimeout;
				httpWebRequest.ReadWriteTimeout = global::Kampai.Util.HttpRequestConfig.HttpRequestReadWriteTimeout;
				if (global::Ea.Sharkbite.HttpPlugin.Http.Api.ConnectionSettings.ConnectionLimit != 0)
				{
					httpWebRequest.ServicePoint.ConnectionLimit = global::Ea.Sharkbite.HttpPlugin.Http.Api.ConnectionSettings.ConnectionLimit;
				}
				if (string.IsNullOrEmpty(Method))
				{
					throw new global::System.InvalidOperationException("A request Method (GET, POST, PUT, DELETE) must be provided.");
				}
				httpWebRequest.Method = Method;
				if (!string.IsNullOrEmpty(Accept))
				{
					httpWebRequest.Accept = Accept;
				}
				if (!string.IsNullOrEmpty(ContentType))
				{
					httpWebRequest.ContentType = ContentType;
				}
				foreach (global::System.Collections.Generic.KeyValuePair<string, string> header in Headers)
				{
					httpWebRequest.Headers.Add(header.Key, header.Value);
				}
				if (Range != 0L)
				{
					httpWebRequest.AddRange((int)Range);
				}
				if (!string.IsNullOrEmpty(Username))
				{
					httpWebRequest.Headers.Add(global::System.Net.HttpRequestHeader.Authorization, GetBasicAuthHeader());
				}
				int num = 0;
				if (Body != null)
				{
					num++;
				}
				if (FormParams.Count > 0)
				{
					num++;
				}
				if (num > 1)
				{
					throw new global::System.InvalidOperationException("Request must contain only form params, or a body, or an entity, without combination.");
				}
				if (FormParams.Count > 0)
				{
					if (string.IsNullOrEmpty(ContentType))
					{
						ContentType = "application/x-www-form-urlencoded";
						httpWebRequest.MediaType = "application/x-www-form-urlencoded";
					}
					global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
					foreach (global::System.Collections.Generic.KeyValuePair<string, string> formParam in FormParams)
					{
						list.Add(string.Format("{0}={1}", global::UnityEngine.WWW.EscapeURL(formParam.Key), global::UnityEngine.WWW.EscapeURL(formParam.Value)));
					}
					Body = global::System.Text.Encoding.UTF8.GetBytes(string.Join("&", list.ToArray()));
				}
				global::System.Net.ServicePointManager.ServerCertificateValidationCallback = (global::System.Net.Security.RemoteCertificateValidationCallback)global::System.Delegate.Combine(global::System.Net.ServicePointManager.ServerCertificateValidationCallback, new global::System.Net.Security.RemoteCertificateValidationCallback(CertificateValidationCallback));
				byte[] body = Body;
				if (body != null)
				{
					switch (Method)
					{
					case "GET":
					case "DELETE":
						throw new global::System.Net.ProtocolViolationException();
					default:
					{
						httpWebRequest.ContentLength = body.Length;
						global::System.IO.Stream requestStream = httpWebRequest.GetRequestStream();
						requestStream.Write(body, 0, body.Length);
						requestStream.Close();
						break;
					}
					}
				}
			}
			catch (global::System.Net.WebException ex)
			{
				global::System.Net.ServicePointManager.ServerCertificateValidationCallback = (global::System.Net.Security.RemoteCertificateValidationCallback)global::System.Delegate.Remove(global::System.Net.ServicePointManager.ServerCertificateValidationCallback, new global::System.Net.Security.RemoteCertificateValidationCallback(CertificateValidationCallback));
				global::Kampai.Util.Native.LogError(ex.Message);
			}
			catch (global::System.Exception ex2)
			{
				global::Kampai.Util.Native.LogError(ex2.Message);
			}
			return httpWebRequest;
		}

		protected virtual global::System.Net.HttpWebResponse ExecuteRequest()
		{
			global::System.Net.HttpWebResponse result = null;
			global::System.Net.HttpWebRequest httpWebRequest = CreateRequest();
			if (httpWebRequest == null)
			{
				global::Kampai.Util.Native.LogError(string.Format("Null request for Uri {0}", Uri));
				return null;
			}
			try
			{
				result = httpWebRequest.GetResponse() as global::System.Net.HttpWebResponse;
			}
			catch (global::System.Net.WebException ex)
			{
				global::Kampai.Util.Native.LogWarning(string.Format("WebException Downloading {0}: {1}", Uri, ex.Message));
				if (ex.Response != null)
				{
					result = ex.Response as global::System.Net.HttpWebResponse;
				}
				else
				{
					global::System.Net.ServicePointManager.ServerCertificateValidationCallback = (global::System.Net.Security.RemoteCertificateValidationCallback)global::System.Delegate.Remove(global::System.Net.ServicePointManager.ServerCertificateValidationCallback, new global::System.Net.Security.RemoteCertificateValidationCallback(CertificateValidationCallback));
					global::Kampai.Util.Native.LogError(string.Format("WebException Response is NULL: {0}", ex.Message));
				}
			}
			catch (global::System.Exception ex2)
			{
				global::Kampai.Util.Native.LogError(string.Format("Exception Downloading {0}: {1}", Uri, ex2.Message));
			}
			return result;
		}

		protected virtual global::System.Collections.Generic.Dictionary<string, string> ProcessResponse(global::System.Net.HttpWebResponse response, string body = "")
		{
			global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
			if (response != null && response.Headers != null)
			{
				string[] allKeys = response.Headers.AllKeys;
				foreach (string text in allKeys)
				{
					dictionary.Add(text, response.Headers.Get(text));
				}
			}
			global::System.Net.ServicePointManager.ServerCertificateValidationCallback = (global::System.Net.Security.RemoteCertificateValidationCallback)global::System.Delegate.Remove(global::System.Net.ServicePointManager.ServerCertificateValidationCallback, new global::System.Net.Security.RemoteCertificateValidationCallback(CertificateValidationCallback));
			return dictionary;
		}

		protected virtual string ReadResponse(global::System.Net.HttpWebResponse response)
		{
			if (!IsFileDownload())
			{
				string result = string.Empty;
				try
				{
					if (response.GetResponseStream() != null)
					{
						global::System.Text.Encoding encoding;
						try
						{
							encoding = global::System.Text.Encoding.GetEncoding(response.CharacterSet);
						}
						catch (global::System.ArgumentException)
						{
							encoding = global::System.Text.Encoding.UTF8;
						}
						byte[] array = new byte[8192];
						int num = 0;
						using (global::System.IO.Stream stream = response.GetResponseStream())
						{
							for (int num2 = stream.Read(array, 0, array.Length); num2 > 0; num2 = stream.Read(array, num, array.Length - num))
							{
								num += num2;
								if (num == array.Length)
								{
									global::System.Array.Resize(ref array, array.Length + 8192);
								}
							}
						}
						if (num > 0)
						{
							result = encoding.GetString(array, 0, num);
						}
					}
				}
				catch (global::System.Exception ex2)
				{
					global::Kampai.Util.Native.LogError(ex2.Message);
				}
				return result;
			}
			using (global::System.IO.Stream stream2 = response.GetResponseStream())
			{
				string text = response.Headers["Content-Encoding"];
				if (!string.IsNullOrEmpty(text) && text.ToLower().Contains("gzip"))
				{
					using (global::ICSharpCode.SharpZipLib.GZip.GZipInputStream input = new global::ICSharpCode.SharpZipLib.GZip.GZipInputStream(stream2))
					{
						return ReadFileResponse(input);
					}
				}
				return ReadFileResponse(stream2);
			}
		}

		protected virtual void GetResponse(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse defaultResponse = null;
			TryRestart();
			bool flag = IsFileDownload();
			if (!abort)
			{
				progress.StartTimer();
				using (global::System.Net.HttpWebResponse httpWebResponse = ExecuteRequest())
				{
					if (httpWebResponse != null)
					{
						if (!flag)
						{
							string body = ReadResponse(httpWebResponse);
							defaultResponse = new global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse().WithBody(body).WithCode((int)httpWebResponse.StatusCode).WithRequest(this)
								.WithContentLength(httpWebResponse.ContentLength)
								.WithContentType(httpWebResponse.ContentType)
								.WithHeaders(ProcessResponse(httpWebResponse, body));
						}
						else
						{
							PrepareDirectory();
							try
							{
								global::System.Collections.Generic.Dictionary<string, string> headers = ProcessResponse(httpWebResponse, string.Empty);
								progress.TotalBytes = GetContentLength(headers);
								string text = ((httpWebResponse.StatusCode == global::System.Net.HttpStatusCode.RequestedRangeNotSatisfiable) ? string.Empty : ReadResponse(httpWebResponse));
								string tempFilePath = GetTempFilePath();
								string error = null;
								int code = (int)httpWebResponse.StatusCode;
#if !UNITY_WEBPLAYER
								if (!abort && (string.IsNullOrEmpty(Md5) || Md5 == text))
								{
									global::System.IO.File.Move(tempFilePath, FilePath);
								}
								else
								{
									global::System.IO.File.Delete(tempFilePath);
									error = (abort ? "Aborting file download" : string.Format("Invalid MD5SUM {0} != {1}", Md5, text));
									code = 418;
								}
#endif
								defaultResponse = new global::Ea.Sharkbite.HttpPlugin.Http.Impl.FileDownloadResponse().WithError(error).WithCode(code).WithRequest(this)
									.WithContentLength(httpWebResponse.ContentLength)
									.WithContentType(httpWebResponse.ContentType)
									.WithHeaders(headers);
							}
							catch (global::System.Exception ex)
							{
								defaultResponse = new global::Ea.Sharkbite.HttpPlugin.Http.Impl.FileDownloadResponse().WithError(ex.Message).WithRequest(this).WithCode(500);
								global::Kampai.Util.Native.LogError(ex.Message);
							}
						}
					}
					else
					{
						global::Kampai.Util.Native.LogError("Null response for " + Uri);
						defaultResponse = (flag ? new global::Ea.Sharkbite.HttpPlugin.Http.Impl.FileDownloadResponse().WithError("The request timed out?") : new global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse()).WithCode(flag ? 408 : 500).WithRequest(this).WithConnectionLoss(true);
					}
				}
				progress.StopTimer();
				defaultResponse = defaultResponse.WithDownloadTime(progress.GetDownloadTime());
			}
			else
			{
				defaultResponse = (flag ? new global::Ea.Sharkbite.HttpPlugin.Http.Impl.FileDownloadResponse() : new global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse()).WithRequest(this).WithCode(500).WithError("Request was aborted before execution.");
			}
			if (callback != null)
			{
				callback(defaultResponse);
			}
		}

		private string ReadFileResponse(global::System.IO.Stream input)
		{
#if !UNITY_WEBPLAYER
			try
			{
				using (global::System.Security.Cryptography.MD5 mD = global::System.Security.Cryptography.MD5.Create())
				{
					using (global::System.IO.FileStream fileStream = global::System.IO.File.Create(tempFilePath))
					{
						try
						{
							int num = 0;
							byte[] array = new byte[4096];
							while (!abort && (num = input.Read(array, 0, array.Length)) != 0)
							{
								fileStream.Write(array, 0, num);
								mD.TransformBlock(array, 0, num, array, 0);
								NotifyProgress(num, 102400L);
							}
							if (!abort)
							{
								mD.TransformFinalBlock(array, 0, 0);
							}
						}
						finally
						{
							NotifyProgress(0L, 0L);
						}
						return abort ? string.Empty : global::System.BitConverter.ToString(mD.Hash).Replace("-", string.Empty).ToLower();
					}
				}
			}
			catch (global::System.Exception ex)
			{
				global::Kampai.Util.Native.LogError(ex.Message);
				return string.Empty;
			}
#else
			return string.Empty;
#endif
		}

		protected virtual void NotifyProgress(long downloadedAmount, long downloadSizeDeltaMin = 0L)
		{
			progress.CompletedBytes += downloadedAmount;
			progress.DownloadedBytes += downloadedAmount;
			progress.Delta += downloadedAmount;
			if (notifyAction != null && progress.Delta > downloadSizeDeltaMin)
			{
				notifyAction(GetProgress(), this);
				progress.Delta = 0L;
			}
		}

		protected bool IsFileDownload()
		{
			return !string.IsNullOrEmpty(FilePath);
		}

		protected void PrepareDirectory()
		{
			if (IsFileDownload())
			{
#if !UNITY_WEBPLAYER
				if (global::System.IO.File.Exists(FilePath))
				{
					global::System.IO.File.Delete(FilePath);
				}
				string tempFilePath = GetTempFilePath();
				if (global::System.IO.File.Exists(tempFilePath))
				{
					global::System.IO.File.Delete(tempFilePath);
				}
				string directoryName = global::System.IO.Path.GetDirectoryName(FilePath);
				if (!global::System.IO.Directory.Exists(directoryName))
				{
					global::System.IO.Directory.CreateDirectory(directoryName);
				}
#endif
			}
		}

		protected long GetContentLength(global::System.Collections.Generic.IDictionary<string, string> headers)
		{
			string key = "Content-Length";
			long result;
			if (!headers.ContainsKey(key) || !long.TryParse(headers[key], out result))
			{
				result = -1L;
			}
			return (result <= 0) ? 0 : result;
		}

		protected string GetHeader(global::System.Collections.Generic.IDictionary<string, string> headers, string key)
		{
			string value;
			headers.TryGetValue(key, out value);
			return value ?? string.Empty;
		}

		protected static bool CertificateValidationCallback(object sender, global::System.Security.Cryptography.X509Certificates.X509Certificate certificate, global::System.Security.Cryptography.X509Certificates.X509Chain chain, global::System.Net.Security.SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}
	}
}
