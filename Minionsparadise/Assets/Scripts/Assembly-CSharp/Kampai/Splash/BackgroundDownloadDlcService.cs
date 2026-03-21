namespace Kampai.Splash
{
	public class BackgroundDownloadDlcService : global::Kampai.Splash.IBackgroundDownloadDlcService
	{
#if !UNITY_EDITOR && UNITY_ANDROID
		private sealed class OnRequestListener : global::UnityEngine.AndroidJavaProxy
		{
			private static readonly string NATIVE_INTERFACE = string.Format("{0}${1}", NATIVE_CLASS, typeof(global::Kampai.Splash.BackgroundDownloadDlcService.OnRequestListener).Name);

			private global::Kampai.Splash.BackgroundDownloadDlcService.RequestBundleCallback responseCallback;

			public OnRequestListener(global::Kampai.Splash.BackgroundDownloadDlcService.RequestBundleCallback callback)
				: base(NATIVE_INTERFACE)
			{
				responseCallback = callback;
			}

			public void onResponseCallback(string url, string filePath, bool isGZipped, long downloadedContentLength, long expectedContentLength, int statusCode, string error)
			{
				if (responseCallback != null)
				{
					responseCallback(url, filePath, isGZipped, downloadedContentLength, expectedContentLength, statusCode, error);
				}
			}
		}
#endif

		private sealed class Invoker : global::Kampai.Util.IInvokerService
		{
			private global::System.Collections.Generic.Queue<global::System.Action> work = new global::System.Collections.Generic.Queue<global::System.Action>();

			private global::System.Threading.Mutex mutex = new global::System.Threading.Mutex(false);

			public void Add(global::System.Action a)
			{
				try
				{
					mutex.WaitOne();
					work.Enqueue(a);
				}
				finally
				{
					mutex.ReleaseMutex();
				}
			}

			public bool Update()
			{
				if (work.Count > 0)
				{
					try
					{
						mutex.WaitOne();
						if (work.Count > 0)
						{
							global::System.Action action = work.Dequeue();
							action();
							return true;
						}
					}
					finally
					{
						mutex.ReleaseMutex();
					}
				}
				return false;
			}
		}

		private delegate void RequestBundleCallback(string url, string filePath, bool isGZipped, long downloadedContentLength, long expectedContentLength, int statusCode, string error);

		private const int MAX_CONCURRENT_REQUESTS = 5;

		private static readonly string NATIVE_CLASS = string.Format("com.ea.gp.minions.{0}", typeof(global::Kampai.Splash.BackgroundDownloadDlcService).Name);

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("BackgroundDownloadDlcService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> pendingRequests;

		private global::System.Collections.Generic.List<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> runningRequests = new global::System.Collections.Generic.List<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest>();

		private bool isNetworkWifi;

		private bool stopped = true;

		private bool isRunning;

		private global::Kampai.Splash.BackgroundDownloadDlcService.Invoker invoker = new global::Kampai.Splash.BackgroundDownloadDlcService.Invoker();

		private bool udpEnabled;

		private long downloadTotalSize;

		private global::System.DateTime downloadStartTime;

		private global::System.Collections.IEnumerator startDownloadCoroutine;

		private string deviceTypeUrlEscaped;

#if !UNITY_EDITOR
		private global::UnityEngine.AndroidJavaClass nativeService;

		private global::UnityEngine.AndroidJavaObject requestHeaders;

		private object onRequestListener;
#endif

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.IClientHealthService clientHealthService { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.AppPauseSignal pauseSignal { get; set; }

		public bool Stopped
		{
			get
			{
				return stopped;
			}
		}

		[Inject]
		public global::Kampai.Util.IClientVersion clientVersion { get; set; }

		[Inject]
		public global::Kampai.Util.IInvokerService invokerService { get; set; }

		[PostConstruct]
		public void PostConstruct()
		{
			deviceTypeUrlEscaped = global::UnityEngine.WWW.EscapeURL(clientVersion.GetClientDeviceType());
		}

		public void Start()
		{
			logger.Info("[BDLC] Start");
			startDownloadCoroutine = StartDownload();
			routineRunner.StartCoroutine(startDownloadCoroutine);
		}

		private global::System.Collections.IEnumerator StartDownload()
		{
			pauseSignal.AddOnce(OnPause);
			if (!stopped)
			{
				isRunning = false;
				do
				{
					logger.Info("[BDLC] Waiting for service to stop before starting it again...");
					yield return new global::UnityEngine.WaitForSeconds(0.1f);
				}
				while (!stopped);
			}
			pauseSignal.RemoveListener(OnPause);
			startDownloadCoroutine = null;
			Init();
			global::System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				Run();
			});
		}

		private void OnPause()
		{
			if (startDownloadCoroutine != null)
			{
				routineRunner.StopCoroutine(startDownloadCoroutine);
				startDownloadCoroutine = null;
			}
		}

		public void Stop()
		{
			logger.Info("[BDLC] Stop");
			isRunning = false;
		}

        private void Init()
        {
            downloadTotalSize = 0L;
            downloadStartTime = global::System.DateTime.Now;
            pendingRequests = CreateNetworkRequests(dlcModel.NeededBundles, manifestService.GetDLCURL());
            isNetworkWifi = global::Kampai.Util.NetworkUtil.IsNetworkWiFi();
            isRunning = pendingRequests.Count != 0;
            stopped = !isRunning;
            logger.Info("[BDLC] Init :: pendingRequests.Count = {0}", pendingRequests.Count);

            global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
            dictionary.Add("K-Platform", clientVersion.GetClientPlatform());
            dictionary.Add("K-Device", deviceTypeUrlEscaped);
            dictionary.Add("K-Version", clientVersion.GetClientVersion());
            global::System.Collections.Generic.Dictionary<string, string> dictionary2 = dictionary;

            // --- PATCH DÉBUT ---
#if !UNITY_EDITOR
            nativeService = null;
            requestHeaders = null;
#endif

#if !UNITY_EDITOR && UNITY_ANDROID
    try 
    {
        nativeService = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.BackgroundDownloadDlcService");
        requestHeaders = new global::UnityEngine.AndroidJavaObject("java.util.HashMap");
        
        foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in dictionary2)
        {
            requestHeaders.Call<string>("put", new object[2] { item.Key, item.Value });
        }
    }
    catch (global::System.Exception e) 
    {
        logger.Error("[BDLC] JNI Error: Could not initialize native service. " + e.Message);
        nativeService = null;
    }
#else
            logger.Warning("[BDLC] Native Android service skipped: Not on Android device.");
#endif
            // --- PATCH FIN ---

#if !UNITY_EDITOR && UNITY_ANDROID
            if (onRequestListener == null)
            {
                onRequestListener = new global::Kampai.Splash.BackgroundDownloadDlcService.OnRequestListener(OnRequestBundleCallbackProxy);
            }
#endif

#if !UNITY_WEBPLAYER
            string dLC_PATH = global::Kampai.Util.GameConstants.DLC_PATH;
            if (!global::System.IO.Directory.Exists(dLC_PATH))
            {
                global::System.IO.Directory.CreateDirectory(dLC_PATH);
            }
#endif
        }

        private void Run()
		{
			global::UnityEngine.AndroidJNI.AttachCurrentThread();
			while (isRunning)
			{
				if (!ProcessQueue())
				{
					global::System.Threading.Thread.Sleep(100);
				}
				bool flag = invoker.Update();
				if (pendingRequests.Count == 0 && runningRequests.Count == 0)
				{
					logger.Info("[BDLC] nothing to do, reconciling tier");
					int playerDLCTier = global::Kampai.Game.DLCService.GetPlayerDLCTier(localPersistanceService);
					if (playerDLCTier > dlcModel.HighestTierDownloaded)
					{
						logger.Info("[BDLC] setting tier {0} -> {1}", dlcModel.HighestTierDownloaded, playerDLCTier);
						telemetryService.Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL("60 - Downloaded DLC", "anyVariant", dlcService.GetDownloadQualityLevel());
						dlcModel.HighestTierDownloaded = playerDLCTier;
					}
					break;
				}
				if (flag && global::Kampai.Main.LoadState.Get() == global::Kampai.Main.LoadStateType.STARTED)
				{
					global::System.Threading.Thread.Sleep(2000);
				}
			}
			logger.Info("[BDLC] requesting stop");
			if (runningRequests.Count != 0)
			{
				AbortRunning();
				if (runningRequests.Count != 0)
				{
					runningRequests.Clear();
				}
			}
			pendingRequests.Clear();
			long num = (long)(global::System.DateTime.Now - downloadStartTime).TotalMilliseconds;
			logger.Info("DLC Download Speed Stats : DownloadedTotalTime: {0} , DownloadedTotalSize : {1}  ", num, downloadTotalSize);
			if (num > 0)
			{
				string text = ((!udpEnabled) ? "Download.Http" : "Download.Udp");
				float num2 = (float)downloadTotalSize / (float)num;
				clientHealthService.MarkTimerEvent(text, num2);
				logger.Info("DLC Download Speed Stats : eventname: {0} , downloadSpeed : {1} ", text, num2);
			}
#if !UNITY_EDITOR && UNITY_ANDROID
			global::UnityEngine.AndroidJNI.DetachCurrentThread();
#endif
			invokerService.Add(delegate
			{
#if !UNITY_EDITOR
				if (requestHeaders != null) requestHeaders.Dispose();
				requestHeaders = null;
				if (nativeService != null) nativeService.Dispose();
				nativeService = null;
#endif
				stopped = true;
			});
			logger.Info("[BDLC] Stopped");
		}

        private bool ProcessQueue()
        {
            if (isRunning && pendingRequests.Count > 0 && runningRequests.Count < 5 && (global::Kampai.Util.NetworkUtil.IsNetworkWiFi() || dlcModel.AllowDownloadOnMobileNetwork))
            {
                // --- PATCH SÉCURITÉ ---
#if !UNITY_EDITOR
                if (nativeService == null)
                {
                    logger.Error("[BDLC] Cannot process queue: nativeService is null");
                    return false;
                }
#endif

                global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request = pendingRequests.Dequeue();
                runningRequests.Add(request);
                logger.Info("[BDLC] request: " + request.Uri);
                PrepareDirectory(request);

#if !UNITY_EDITOR && UNITY_ANDROID
                nativeService.CallStatic("requestBundle", request.Uri, request.GetTempFilePath(), requestHeaders, request.UseGZip, (global::UnityEngine.AndroidJavaProxy)onRequestListener);
#else
                logger.Warning("[BDLC] Skipping native requestBundle in Editor for: " + request.Uri);
                // Simulation: immediately succeed or fail? 
                // For now, let's just mark it as stopped if we're in the Editor and have no way to download bundles this way.
                // However, the Manifest download already happened via C#. BDLC seems to be for background items.
                // To avoid hanging, we might need to simulate completion if progress is blocked.
                // But typically BDLC is optional for the flow to continue if reconcileDLC is called.
#endif
                return true;
            }
            return false;
        }

        private void AbortRunning()
        {
            foreach (global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest runningRequest in runningRequests)
            {
                runningRequest.Abort();
            }

            // --- PATCH SÉCURITÉ ---
#if !UNITY_EDITOR
            if (nativeService != null)
            {
                nativeService.CallStatic("abortRequest", string.Empty);
            }
#endif

            logger.Info("[BDLC] finalizing running requests");
            int num = 100;
            while (runningRequests.Count > 0 && num-- > 0)
            {
                logger.Info("[BDLC] exiting {0} request(s) [time left: {1:0.##} s]", runningRequests.Count, (float)num / 10f);
                do
                {
                    global::System.Threading.Thread.Sleep(100);
                }
                while (invoker.Update());
            }
            if (runningRequests.Count != 0)
            {
                logger.Error("[BDLC] unstopped requests: {0}", runningRequests.Count);
            }
        }

        private void OnRequestBundleCallbackProxy(string url, string tempFilePath, bool isGZipped, long downloadedContentLength, long expectedContentLength, int statusCode, string error)
		{
			invoker.Add(delegate
			{
				OnRequestBundleCallback(url, tempFilePath, isGZipped, downloadedContentLength, expectedContentLength, statusCode, error);
			});
		}

		private void OnRequestBundleCallback(string url, string tempFilePath, bool isGZipped, long downloadedContentLength, long expectedContentLength, int statusCode, string error)
		{
			global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request = null;
			foreach (global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest runningRequest in runningRequests)
			{
				if (runningRequest.Uri == url)
				{
					request = runningRequest;
					break;
				}
			}
			if (request == null)
			{
				logger.Error("[BDLC] OnRequestBundleCallback(): Unable to find original request for URL = {0}", url);
				return;
			}
			bool flag = request.IsAborted();
			bool flag2 = !string.IsNullOrEmpty(error);
#if !UNITY_WEBPLAYER
			if (global::System.IO.File.Exists(tempFilePath) && !flag2)
			{
				error = (flag ? "Aborting file download." : global::Kampai.Util.DownloadUtil.UnpackFile(tempFilePath, request.FilePath, request.Md5, request.AvoidBackup));
				if (!string.IsNullOrEmpty(error))
				{
					logger.Error("[Download] " + error);
				}
			}
#else
			if (false)
			{
			}
#endif
			else if (!flag2)
			{
				error = (flag ? "Aborting file download." : "Temp file doesn't exist upon download finish.");
				statusCode = 418;
			}
#if !UNITY_WEBPLAYER
			if (global::System.IO.File.Exists(tempFilePath))
			{
				global::System.IO.File.Delete(tempFilePath);
			}
#endif
			HandleResponse(new global::Ea.Sharkbite.HttpPlugin.Http.Impl.FileDownloadResponse().WithError(error).WithCode((statusCode == 0) ? 408 : statusCode).WithRequest(request)
				.WithContentLength(expectedContentLength));
		}

		private void PrepareDirectory(global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request)
		{
#if !UNITY_WEBPLAYER
			string tempFilePath = request.GetTempFilePath();
			if (global::System.IO.File.Exists(tempFilePath))
			{
				global::System.IO.File.Delete(tempFilePath);
			}
			string filePath = request.FilePath;
#if !UNITY_WEBPLAYER
#if !UNITY_WEBPLAYER
			if (global::System.IO.File.Exists(filePath))
#else
			if (false)
#endif
#else
			if (false)
#endif
			{
				global::System.IO.File.Delete(filePath);
			}
			string directoryName = global::System.IO.Path.GetDirectoryName(filePath);
			if (!global::System.IO.Directory.Exists(directoryName))
			{
				global::System.IO.Directory.CreateDirectory(directoryName);
			}
#endif
		}

		private void HandleResponse(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request = response.Request;
			runningRequests.Remove(request);
			string uri = request.Uri;
			long contentLength = response.ContentLength;
			logger.Info("[BDLC] url = {0}", uri);
			if (request.IsAborted())
			{
				logger.Info("[BDLC] aborted, url = {0}", uri);
				return;
			}
			if (!response.Success)
			{
				logger.Info("[BDLC] failure: code = {0}, error = {1}, url = {2}, enqueue request", response.Code, response.Error, uri);
				pendingRequests.Enqueue(request);
			}
			else
			{
				string bundleNameFromUrl = global::Kampai.Util.DownloadUtil.GetBundleNameFromUrl(request.Uri);
				if (!configurationsService.isKillSwitchOn(global::Kampai.Game.KillSwitch.DLC_TELEMETRY))
				{
					telemetryService.Send_Telemetry_EVT_USER_GAME_DOWNLOAD_FUNNEL(bundleNameFromUrl, response.DownloadTime, contentLength, isNetworkWifi);
				}
				logger.Info("[BDLC]: success: url = {0}", uri);
			}
			if (contentLength > 0)
			{
				downloadTotalSize += contentLength;
			}
		}

		private global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> CreateNetworkRequests(global::System.Collections.Generic.IList<global::Kampai.Util.BundleInfo> bundles, string baseDlcUrl)
		{
			string userId = global::UnityEngine.Random.Range(0, 100).ToString();
			udpEnabled = global::Kampai.Util.FeatureAccessUtil.isAccessible(global::Kampai.Game.AccessControlledFeature.AKAMAI_UDP, configurationsService.GetConfigurations(), userId, logger);
			global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> queue = new global::System.Collections.Generic.Queue<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest>(bundles.Count);
			foreach (global::Kampai.Util.BundleInfo bundle in bundles)
			{
				global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest item = CreateRequest(bundle, baseDlcUrl, udpEnabled);
				queue.Enqueue(item);
			}
			return queue;
		}

		private global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest CreateRequest(global::Kampai.Util.BundleInfo bundleInfo, string baseDlcUrl, bool udpEnabled)
		{
			string name = bundleInfo.name;
			string uri = global::Kampai.Util.DownloadUtil.CreateBundleURL(baseDlcUrl, name);
			string filePath = global::Kampai.Util.DownloadUtil.CreateBundlePath(global::Kampai.Util.GameConstants.DLC_PATH, name);
			return requestFactory.Resource(uri).WithOutputFile(filePath).WithMd5(bundleInfo.sum)
				.WithGZip(true)
				.WithUdp(udpEnabled);
		}
	}
}
