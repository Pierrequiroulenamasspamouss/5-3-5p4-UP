namespace Ea.Sharkbite.HttpPlugin.Http.Impl
{
	public class NimbleRequest : global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultRequest
	{
		private NimbleBridge_HttpRequest.Method method;

		private NimbleBridge_NetworkConnectionHandle handle;

		public override string Method
		{
			get
			{
				return base.Method;
			}
			set
			{
				base.Method = value;
				switch (value)
				{
				case "HEAD":
					method = NimbleBridge_HttpRequest.Method.HTTP_HEAD;
					break;
				case "POST":
					method = NimbleBridge_HttpRequest.Method.HTTP_POST;
					break;
				case "PUT":
					method = NimbleBridge_HttpRequest.Method.HTTP_PUT;
					break;
				default:
					method = NimbleBridge_HttpRequest.Method.HTTP_GET;
					break;
				}
			}
		}

		public NimbleRequest(string uri)
			: base(uri)
		{
		}

		public override void Execute(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			GetResponse(callback);
		}

		public override void Abort()
		{
			bool flag = abort;
			base.Abort();
			if (flag || handle == null)
			{
				return;
			}
			using (NimbleBridge_HttpResponse nimbleBridge_HttpResponse = handle.GetResponse())
			{
				if (!nimbleBridge_HttpResponse.IsCompleted())
				{
					handle.Cancel();
				}
			}
		}

		public override global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress GetProgress()
		{
			return progress;
		}

		protected new virtual NimbleBridge_HttpRequest CreateRequest()
		{
			NimbleBridge_HttpRequest nimbleBridge_HttpRequest = NimbleBridge_HttpRequest.RequestWithUrl(GetUriWithQueryParams());
			nimbleBridge_HttpRequest.SetMethod(method);
			nimbleBridge_HttpRequest.SetTimeout((double)global::Kampai.Util.HttpRequestConfig.HttpRequestTimeout / 1000.0);
			nimbleBridge_HttpRequest.SetRunInBackground(RunInBackground);
			if (IsFileDownload())
			{
				string tempFilePath = GetTempFilePath();
				nimbleBridge_HttpRequest.SetTargetFilePath(tempFilePath);
				string cachedFilePath = GetCachedFilePath(tempFilePath);
#if !UNITY_WEBPLAYER
				base.Range = ((!TryResume || !global::System.IO.File.Exists(cachedFilePath)) ? 0 : new global::System.IO.FileInfo(cachedFilePath).Length);
#else
				base.Range = 0L;
#endif
				if (base.Range != 0L)
				{
					if (!UseGZip && global::Kampai.Util.DownloadUtil.IsGZipped(cachedFilePath))
					{
						base.Range = 0L;
					}
					else
					{
						global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress downloadProgress = progress;
						long range = base.Range;
						progress.Delta = range;
						range = range;
						progress.CompletedBytes = range;
						downloadProgress.TotalBytes = range;
					}
				}
				nimbleBridge_HttpRequest.SetOverwritePolicy((base.Range != 0L) ? 1 : 0);
			}
			if (base.Range != 0L)
			{
				Headers["Range"] = string.Format("{0}={1}-", "bytes", base.Range);
			}
			else if (Headers.ContainsKey("Range"))
			{
				Headers.Remove("Range");
			}
			if (!string.IsNullOrEmpty(ContentType))
			{
				Headers["Content-Type"] = ContentType;
			}
			if (!string.IsNullOrEmpty(Username))
			{
				Headers["Authorization"] = GetBasicAuthHeader();
			}
			if (Headers.Count != 0)
			{
				nimbleBridge_HttpRequest.SetHeaders(Headers);
			}
			if (Body != null)
			{
				nimbleBridge_HttpRequest.SetData(Body);
			}
			return nimbleBridge_HttpRequest;
		}

		protected override void GetResponse(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback)
		{
			TryRestart();
			bool isFileDownload = IsFileDownload();
			if (abort)
			{
				if (callback != null)
				{
					callback((isFileDownload ? new global::Ea.Sharkbite.HttpPlugin.Http.Impl.FileDownloadResponse() : new global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse()).WithRequest(this).WithCode(500).WithError("Request was aborted before execution."));
				}
				return;
			}
			if (isFileDownload)
			{
				PrepareDirectory();
			}
			NimbleBridge_HttpRequest request = CreateRequest();
			progress.StartTimer();
			handle = NimbleBridge_Network.GetComponent().SendRequest(request, delegate(NimbleBridge_NetworkConnectionHandle connection)
			{
				progress.StopTimer();
				global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response2;
				if (!isFileDownload)
				{
					global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response = OnResponseCallback(connection);
					response2 = response;
				}
				else
				{
					response2 = OnDownloadCallback(connection);
				}
				global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse obj = response2;
				request.Dispose();
				handle = null;
				if (isRestarted && !abort)
				{
					Execute(callback);
				}
				else if (callback != null)
				{
					callback(obj);
				}
			});
			using (NimbleBridge_HttpResponse nimbleBridge_HttpResponse = handle.GetResponse())
			{
				if (nimbleBridge_HttpResponse.IsCompleted())
				{
					return;
				}
				using (NimbleBridge_Error nimbleBridge_Error = nimbleBridge_HttpResponse.GetError())
				{
					if (nimbleBridge_Error == null || nimbleBridge_Error.IsNull())
					{
						handle.SetHeaderCallback(OnHeaderCallback);
						handle.SetProgressCallback(OnProgressCallback);
					}
				}
			}
		}

		private void OnHeaderCallback(NimbleBridge_NetworkConnectionHandle connection)
		{
			using (NimbleBridge_HttpRequest nimbleBridge_HttpRequest = connection.GetRequest())
			{
				using (NimbleBridge_HttpResponse nimbleBridge_HttpResponse = connection.GetResponse())
				{
					global::System.Collections.Generic.Dictionary<string, string> headers = nimbleBridge_HttpResponse.GetHeaders();
					long expectedContentLength = nimbleBridge_HttpResponse.GetExpectedContentLength();
					progress.TotalBytes = ((expectedContentLength <= 0) ? GetContentLength(headers) : expectedContentLength);
					if (progress.TotalBytes != 0L)
					{
						progress.TotalBytes += progress.CompletedBytes;
					}
					progress.IsGZipped = GetHeader(headers, "Content-Encoding").ToLower().Contains("gzip");
					if (abort || nimbleBridge_HttpResponse.IsCompleted() || nimbleBridge_HttpRequest.GetOverwritePolicy() != 1)
					{
						return;
					}
					string cachedFilePath = GetCachedFilePath(nimbleBridge_HttpRequest.GetTargetFilePath());
#if !UNITY_WEBPLAYER
					bool flag = global::System.IO.File.Exists(cachedFilePath);
#else
					bool flag = false;
#endif
					if (nimbleBridge_HttpResponse.GetStatusCode() != 206 || !flag || global::Kampai.Util.DownloadUtil.IsGZipped(cachedFilePath) != progress.IsGZipped || !GetHeader(headers, "Accept-Ranges").ToLower().Contains("bytes") || !GetHeader(headers, "Content-Range").StartsWith(string.Format("{0}={1}-".Replace('=', ' '), "bytes", base.Range), global::System.StringComparison.OrdinalIgnoreCase))
					{
						if (flag)
						{
							global::System.IO.File.Delete(cachedFilePath);
						}
						Restart();
						abort = false;
					}
				}
			}
		}

		private void OnProgressCallback(NimbleBridge_NetworkConnectionHandle connection)
		{
			using (NimbleBridge_HttpResponse nimbleBridge_HttpResponse = connection.GetResponse())
			{
				NotifyProgress(nimbleBridge_HttpResponse.GetDownloadedContentLength() - progress.DownloadedBytes, 102400L);
			}
		}

		private global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse OnResponseCallback(NimbleBridge_NetworkConnectionHandle connection)
		{
			using (NimbleBridge_HttpResponse nimbleBridge_HttpResponse = connection.GetResponse())
			{
				using (NimbleBridge_Error nimbleBridge_Error = nimbleBridge_HttpResponse.GetError())
				{
					int statusCode = nimbleBridge_HttpResponse.GetStatusCode();
					global::System.Collections.Generic.Dictionary<string, string> headers = nimbleBridge_HttpResponse.GetHeaders();
					bool flag = false;
					string text = null;
					if (nimbleBridge_Error == null || nimbleBridge_Error.IsNull())
					{
						flag = !global::Kampai.Util.NetworkUtil.IsConnected();
						text = null;
						if (statusCode < 200 || statusCode > 299)
						{
							text = string.Format("{0}", statusCode);
						}
					}
					else
					{
						switch (nimbleBridge_Error.GetCode())
						{
						case NimbleBridge_Error.Code.NETWORK_NO_CONNECTION:
						case NimbleBridge_Error.Code.NETWORK_UNREACHABLE:
						case NimbleBridge_Error.Code.NETWORK_TIMEOUT:
							flag = true;
							break;
						default:
							flag = !global::Kampai.Util.NetworkUtil.IsConnected();
							break;
						}
						text = string.Format("[{0}] {1} {2}", (int)nimbleBridge_Error.GetCode(), nimbleBridge_Error.GetCode(), nimbleBridge_Error.GetReason());
					}
					global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse defaultResponse = new global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse().WithRequest(this).WithCode(statusCode).WithError(text)
						.WithConnectionLoss(flag)
						.WithContentLength(nimbleBridge_HttpResponse.GetExpectedContentLength())
						.WithDownloadTime(progress.GetDownloadTime());
					if (headers != null)
					{
						defaultResponse.Headers = headers;
						defaultResponse.ContentType = GetHeader(headers, "Content-Type");
					}
					byte[] data = nimbleBridge_HttpResponse.GetData();
					if (data != null && data.Length > 0)
					{
						defaultResponse.Body = global::System.Text.Encoding.UTF8.GetString(data);
					}
					return defaultResponse;
				}
			}
		}

		private global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse OnDownloadCallback(NimbleBridge_NetworkConnectionHandle connection)
		{
			NotifyProgress(0L, 0L);
			using (NimbleBridge_HttpResponse nimbleBridge_HttpResponse = connection.GetResponse())
			{
				using (NimbleBridge_Error nimbleBridge_Error = nimbleBridge_HttpResponse.GetError())
				{
					bool isConnectionLost = false;
					string text = null;
					int num = nimbleBridge_HttpResponse.GetStatusCode();
					global::System.Collections.Generic.Dictionary<string, string> headers = nimbleBridge_HttpResponse.GetHeaders();
					string tempFilePath = GetTempFilePath();
					bool flag = nimbleBridge_Error != null && !nimbleBridge_Error.IsNull();
#if !UNITY_WEBPLAYER
					if (global::System.IO.File.Exists(tempFilePath) && !flag)
#else
					if (false)
#endif
					{
						text = (abort ? "Aborting file download." : global::Kampai.Util.DownloadUtil.UnpackFile(tempFilePath, FilePath, Md5, AvoidBackup));
						if (!string.IsNullOrEmpty(text))
						{
							num = 418;
						}
					}
					else if (flag)
					{
						switch (nimbleBridge_Error.GetCode())
						{
						case NimbleBridge_Error.Code.NETWORK_NO_CONNECTION:
						case NimbleBridge_Error.Code.NETWORK_UNREACHABLE:
						case NimbleBridge_Error.Code.NETWORK_TIMEOUT:
							isConnectionLost = true;
							break;
						default:
							isConnectionLost = !global::Kampai.Util.NetworkUtil.IsConnected();
							break;
						}
						text = string.Format("[{0}] {1} {2}", (int)nimbleBridge_Error.GetCode(), nimbleBridge_Error.GetCode(), nimbleBridge_Error.GetReason());
					}
					else
					{
						text = (abort ? "Aborting file download." : "Temp file doesn't exist upon download finish.");
						num = 418;
					}
#if !UNITY_WEBPLAYER
					if (global::System.IO.File.Exists(tempFilePath))
#else
					if (false)
#endif
					{
						global::System.IO.File.Delete(tempFilePath);
					}
					return new global::Ea.Sharkbite.HttpPlugin.Http.Impl.FileDownloadResponse().WithError(text).WithCode((num == 0) ? 408 : num).WithRequest(this)
						.WithContentLength(nimbleBridge_HttpResponse.GetExpectedContentLength())
						.WithContentType(GetHeader(headers, "Content-Type"))
						.WithHeaders(headers)
						.WithConnectionLoss(isConnectionLost)
						.WithDownloadTime(progress.GetDownloadTime());
				}
			}
		}

		private string GetCachedFilePath(string targetFilePath)
		{
			return global::System.IO.Path.Combine(NimbleBridge_ApplicationEnvironment.GetComponent().GetCachePath(), global::System.IO.Path.GetFileName(targetFilePath));
		}
	}
}
