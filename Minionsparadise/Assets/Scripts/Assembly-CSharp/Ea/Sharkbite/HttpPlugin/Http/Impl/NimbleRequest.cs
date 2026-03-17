using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Ea.Sharkbite.HttpPlugin.Http.Api;
using Kampai.Util;

namespace Ea.Sharkbite.HttpPlugin.Http.Impl
{
    public class NimbleRequest : DefaultRequest
    {
        private NimbleBridge_HttpRequest.Method _nimbleMethod;
        private NimbleBridge_NetworkConnectionHandle _handle;

        public override string Method
        {
            get { return base.Method; }
            set
            {
                base.Method = value;
                switch (value)
                {
                    case "HEAD":
                        _nimbleMethod = NimbleBridge_HttpRequest.Method.HTTP_HEAD;
                        break;
                    case "POST":
                        _nimbleMethod = NimbleBridge_HttpRequest.Method.HTTP_POST;
                        break;
                    case "PUT":
                        _nimbleMethod = NimbleBridge_HttpRequest.Method.HTTP_PUT;
                        break;
                    default:
                        _nimbleMethod = NimbleBridge_HttpRequest.Method.HTTP_GET;
                        break;
                }
            }
        }

        public NimbleRequest(string uri) : base(uri)
        {
        }

        public override void Execute(Action<IResponse> callback)
        {
            UnityEngine.Debug.LogFormat("Sending HTTP {0} request (Nimble) to: {1}", Method, Uri);
            GetResponse(callback);
        }

        public override void Abort()
        {
            bool wasAborted = _abort;
            base.Abort();

            if (wasAborted || _handle == null)
            {
                return;
            }

            using (NimbleBridge_HttpResponse nimbleResponse = _handle.GetResponse())
            {
                if (!nimbleResponse.IsCompleted())
                {
                    _handle.Cancel();
                }
            }
        }

        public override DownloadProgress GetProgress()
        {
            return _progress;
        }

        protected new virtual NimbleBridge_HttpRequest CreateRequest()
        {
            NimbleBridge_HttpRequest request = NimbleBridge_HttpRequest.RequestWithUrl(GetUriWithQueryParams());
            request.SetMethod(_nimbleMethod);
            request.SetTimeout((double)HttpRequestConfig.HttpRequestTimeout / 1000.0);
            request.SetRunInBackground(RunInBackground);

            if (IsFileDownload())
            {
                string tempFilePath = GetTempFilePath();
                request.SetTargetFilePath(tempFilePath);
                string cachedFilePath = GetCachedFilePath(tempFilePath);

#if !UNITY_WEBPLAYER
                Range = (!TryResume || !File.Exists(cachedFilePath)) ? 0L : new FileInfo(cachedFilePath).Length;
#else
                Range = 0L;
#endif

                if (Range != 0L)
                {
                    if (!UseGZip && DownloadUtil.IsGZipped(cachedFilePath))
                    {
                        Range = 0L;
                    }
                    else
                    {
                        // L'artefact décompilé "range = range;" est réparé ici
                        long currentRange = Range;
                        _progress.Delta = currentRange;
                        _progress.CompletedBytes = currentRange;
                        _progress.TotalBytes = currentRange;
                    }
                }
                request.SetOverwritePolicy((Range != 0L) ? 1 : 0);
            }

            if (Range != 0L)
            {
                Headers["Range"] = string.Format("bytes={0}-", Range);
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
                request.SetHeaders(Headers);
            }

            if (Body != null)
            {
                request.SetData(Body);
            }

            return request;
        }

        protected override void GetResponse(Action<IResponse> callback)
        {
            TryRestart();
            bool isFileDownload = IsFileDownload();

            if (_abort)
            {
                if (callback != null)
                {
                    IResponse response = isFileDownload ? (IResponse)new FileDownloadResponse() : new DefaultResponse();
                    callback(((DefaultResponse)response).WithRequest(this).WithCode(500).WithError("Request was aborted before execution."));
                }
                return;
            }

            if (isFileDownload)
            {
                PrepareDirectory();
            }

            NimbleBridge_HttpRequest request = CreateRequest();
            _progress.StartTimer();

            _handle = NimbleBridge_Network.GetComponent().SendRequest(request, delegate (NimbleBridge_NetworkConnectionHandle connection)
            {
                _progress.StopTimer();
                IResponse responseFinal;

                if (!isFileDownload)
                {
                    responseFinal = OnResponseCallback(connection);
                }
                else
                {
                    responseFinal = OnDownloadCallback(connection);
                }

                request.Dispose();
                _handle = null;

                if (_isRestarted && !_abort)
                {
                    Execute(callback);
                }
                else if (callback != null)
                {
                    callback(responseFinal);
                }
            });

            using (NimbleBridge_HttpResponse nimbleResponse = _handle.GetResponse())
            {
                if (nimbleResponse.IsCompleted()) return;

                using (NimbleBridge_Error error = nimbleResponse.GetError())
                {
                    if (error == null || error.IsNull())
                    {
                        _handle.SetHeaderCallback(OnHeaderCallback);
                        _handle.SetProgressCallback(OnProgressCallback);
                    }
                }
            }
        }

        private void OnHeaderCallback(NimbleBridge_NetworkConnectionHandle connection)
        {
            using (NimbleBridge_HttpRequest request = connection.GetRequest())
            using (NimbleBridge_HttpResponse response = connection.GetResponse())
            {
                Dictionary<string, string> headers = response.GetHeaders();
                // Cast en (long) ajouté ici
                long expectedContentLength = (long)response.GetExpectedContentLength();

                _progress.TotalBytes = (expectedContentLength <= 0) ? GetContentLength(headers) : expectedContentLength;

                if (_progress.TotalBytes != 0L)
                {
                    _progress.TotalBytes += _progress.CompletedBytes;
                }

                _progress.IsGZipped = GetHeader(headers, "Content-Encoding").ToLower().Contains("gzip");

                if (_abort || response.IsCompleted() || request.GetOverwritePolicy() != 1) return;

                string cachedFilePath = GetCachedFilePath(request.GetTargetFilePath());

#if !UNITY_WEBPLAYER
                bool fileExists = File.Exists(cachedFilePath);
#else
                bool fileExists = false;
#endif

                if (response.GetStatusCode() != 206 || !fileExists ||
                    DownloadUtil.IsGZipped(cachedFilePath) != _progress.IsGZipped ||
                    !GetHeader(headers, "Accept-Ranges").ToLower().Contains("bytes") ||
                    !GetHeader(headers, "Content-Range").StartsWith(string.Format("bytes={0}-", Range), StringComparison.OrdinalIgnoreCase))
                {
                    if (fileExists)
                    {
                        File.Delete(cachedFilePath);
                    }
                    Restart();
                    _abort = false;
                }
            }
        }

        private void OnProgressCallback(NimbleBridge_NetworkConnectionHandle connection)
        {
            using (NimbleBridge_HttpResponse response = connection.GetResponse())
            {
                // Ajout des casts (long) pour sécuriser l'opération mathématique
                long downloadedContent = (long)response.GetDownloadedContentLength();
                long currentDownloaded = _progress.DownloadedBytes;

                NotifyProgress(downloadedContent - currentDownloaded, 102400L);
            }
        }

        private IResponse OnResponseCallback(NimbleBridge_NetworkConnectionHandle connection)
        {
            using (NimbleBridge_HttpResponse response = connection.GetResponse())
            using (NimbleBridge_Error error = response.GetError())
            {
                int statusCode = response.GetStatusCode();
                Dictionary<string, string> headers = response.GetHeaders();
                bool isConnectionLost = false;
                string errorText = null;

                if (error == null || error.IsNull())
                {
                    isConnectionLost = !NetworkUtil.IsConnected();
                    if (statusCode < 200 || statusCode > 299)
                    {
                        errorText = string.Format("{0}", statusCode);
                    }
                }
                else
                {
                    switch (error.GetCode())
                    {
                        case NimbleBridge_Error.Code.NETWORK_NO_CONNECTION:
                        case NimbleBridge_Error.Code.NETWORK_UNREACHABLE:
                        case NimbleBridge_Error.Code.NETWORK_TIMEOUT:
                            isConnectionLost = true;
                            break;
                        default:
                            isConnectionLost = !NetworkUtil.IsConnected();
                            break;
                    }
                    errorText = string.Format("[{0}] {1} {2}", (int)error.GetCode(), error.GetCode(), error.GetReason());
                }

                // Ajout des casts (long) et (int)
                DefaultResponse defaultResponse = (DefaultResponse)new DefaultResponse()
                    .WithRequest(this)
                    .WithCode(statusCode)
                    .WithError(errorText)
                    .WithConnectionLoss(isConnectionLost)
                    .WithContentLength((long)response.GetExpectedContentLength())
                    .WithDownloadTime((int)_progress.GetDownloadTime());

                if (headers != null)
                {
                    defaultResponse.Headers = headers;
                    defaultResponse.ContentType = GetHeader(headers, "Content-Type");
                }

                byte[] data = response.GetData();
                if (data != null && data.Length > 0)
                {
                    defaultResponse.Body = Encoding.UTF8.GetString(data);
                }

                return defaultResponse;
            }
        }

        private IResponse OnDownloadCallback(NimbleBridge_NetworkConnectionHandle connection)
        {
            NotifyProgress(0L, 0L);
            using (NimbleBridge_HttpResponse response = connection.GetResponse())
            using (NimbleBridge_Error error = response.GetError())
            {
                bool isConnectionLost = false;
                string errorText = null;
                int statusCode = response.GetStatusCode();
                Dictionary<string, string> headers = response.GetHeaders();
                string tempFilePath = GetTempFilePath();
                bool hasError = error != null && !error.IsNull();

#if !UNITY_WEBPLAYER
                if (File.Exists(tempFilePath) && !hasError)
#else
                if (false)
#endif
                {
                    errorText = _abort ? "Aborting file download." : DownloadUtil.UnpackFile(tempFilePath, FilePath, Md5, AvoidBackup);
                    if (!string.IsNullOrEmpty(errorText))
                    {
                        statusCode = 418;
                    }
                }
                else if (hasError)
                {
                    switch (error.GetCode())
                    {
                        case NimbleBridge_Error.Code.NETWORK_NO_CONNECTION:
                        case NimbleBridge_Error.Code.NETWORK_UNREACHABLE:
                        case NimbleBridge_Error.Code.NETWORK_TIMEOUT:
                            isConnectionLost = true;
                            break;
                        default:
                            isConnectionLost = !NetworkUtil.IsConnected();
                            break;
                    }
                    errorText = string.Format("[{0}] {1} {2}", (int)error.GetCode(), error.GetCode(), error.GetReason());
                }
                else
                {
                    errorText = _abort ? "Aborting file download." : "Temp file doesn't exist upon download finish.";
                    statusCode = 418;
                }

#if !UNITY_WEBPLAYER
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
#endif

                // Casts (long) et (int) pour la réponse de téléchargement de fichier
                return new FileDownloadResponse()
                    .WithError(errorText)
                    .WithCode((statusCode == 0) ? 408 : statusCode)
                    .WithRequest(this)
                    .WithContentLength((long)response.GetExpectedContentLength())
                    .WithContentType(GetHeader(headers, "Content-Type"))
                    .WithHeaders(headers)
                    .WithConnectionLoss(isConnectionLost)
                    .WithDownloadTime((int)_progress.GetDownloadTime());
            }
        }

        private string GetCachedFilePath(string targetFilePath)
        {
            return Path.Combine(NimbleBridge_ApplicationEnvironment.GetComponent().GetCachePath(), Path.GetFileName(targetFilePath));
        }
    }
}