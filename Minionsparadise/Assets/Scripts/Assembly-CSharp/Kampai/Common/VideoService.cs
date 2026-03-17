namespace Kampai.Common
{
	public class VideoService : global::Kampai.Common.IVideoService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("VideoService") as global::Kampai.Util.IKampaiLogger;

		private string locale;

		private global::Kampai.Common.VideoRequest request;

		[Inject]
		public global::Kampai.Splash.SplashProgressUpdateSignal splashProgressUpdateSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.SetSplashProgressSignal setSplashProgressSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Splash.ShowNoWiFiPanelSignal showNoWiFiPanelSignal { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkModel networkModel { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[PostConstruct]
		public void PostConstruct()
		{
			locale = global::Kampai.Main.HALService.GetResourcePath(global::Kampai.Util.Native.GetDeviceLanguage());
			if (string.IsNullOrEmpty(locale))
			{
				locale = global::Kampai.Main.HALService.GetResourcePath("en");
			}
			if (string.IsNullOrEmpty(locale))
			{
				locale = "EN-US";
			}
		}

		public void playVideo(string urlOrFilename, bool showControls, bool closeOnTouch)
		{
			logger.Info("[Video] Playing {0}", urlOrFilename);
#if UNITY_IOS || UNITY_ANDROID
			global::UnityEngine.FullScreenMovieControlMode controlMode = global::UnityEngine.FullScreenMovieControlMode.Hidden;
			if (showControls)
			{
				controlMode = global::UnityEngine.FullScreenMovieControlMode.Minimal;
			}
			else if (closeOnTouch)
			{
				controlMode = global::UnityEngine.FullScreenMovieControlMode.CancelOnInput;
			}
#if !UNITY_EDITOR
			global::UnityEngine.Handheld.PlayFullScreenMovie(urlOrFilename, global::UnityEngine.Color.black, controlMode, global::UnityEngine.FullScreenMovieScalingMode.AspectFit);
#else
			logger.Info("[Video] Handheld.PlayFullScreenMovie skipped in Editor. Simulated playback.");
#endif
#else
			logger.Info("[Video] Standalone/Editor: Opening video via Application.OpenURL");
			string path = urlOrFilename;
			if (!path.Contains("://") && global::System.IO.File.Exists(path))
			{
				path = "file://" + global::System.IO.Path.GetFullPath(path);
			}
			global::UnityEngine.Application.OpenURL(path);
#endif
		}

		public void playIntro(bool showControls, bool closeOnTouch, global::System.Action videoPlayingCallback = null, string videoUriTemplate = null)
		{
			if (request == null)
			{
				global::Kampai.Main.LoadState.Set(global::Kampai.Main.LoadStateType.INTRO);
				request = new global::Kampai.Common.VideoRequest();
				request.showControls = showControls;
				request.closeOnTouch = closeOnTouch;
				request.callback = videoPlayingCallback;
				request.videoUriTemplate = videoUriTemplate;
				fetchAndPlayIntroVideo();
			}
			else
			{
				logger.Error("[Video] Intro already playing");
			}
		}

		private string GetIntroVideoUri(string template = null)
		{
			return string.Format(template ?? global::Kampai.Util.GameConstants.Server.VIDEO_PATH, locale);
		}

		private void fetchAndPlayIntroVideo()
		{
			if (request == null)
			{
				logger.Error("[Video] Null request for intro");
			}
			else if (IsIntroCached(request.videoUriTemplate))
			{
				logger.Info("[Video] Cached: {0}", GetIntroVideoUri(request.videoUriTemplate));
				if (request.callback != null)
				{
					logger.Info("[Video] CALLBACK");
					request.callback();
				}
				bool showControls = request.showControls;
				bool closeOnTouch = request.closeOnTouch;
				request = null;
				global::UnityEngine.PlayerPrefs.SetInt("intro_video_played", 1);
				playVideo(global::Kampai.Util.GameConstants.VIDEO_PATH, showControls, closeOnTouch);
			}
			else
			{
				BeginVideoDownload();
			}
		}

		private void BeginVideoDownload()
		{
			string introVideoUri = GetIntroVideoUri(request.videoUriTemplate);
			logger.Info("[Video] Requesting: {0}", introVideoUri);
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest>();
			signal.AddListener(UpdateProgressBar);
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal2 = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal2.AddListener(RequestCallback);
			global::Kampai.Common.VideoRequest videoRequest = request;
			global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest obj = requestFactory.Resource(introVideoUri).WithOutputFile(global::Kampai.Util.GameConstants.VIDEO_PATH).WithProgressSignal(signal)
				.WithResponseSignal(signal2);
			int retries = request.retries;
			videoRequest.networkRequest = obj.WithRetry(true, retries).WithResume(true).WithAvoidBackup(true);
			if (networkModel.reachability == global::UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork && !dlcModel.AllowDownloadOnMobileNetwork)
			{
				request.networkRequest.Restart();
				showNoWiFiPanelSignal.Dispatch(true);
			}
			request.progressBarStart = request.progressBarNow;
			downloadService.Perform(request.networkRequest);
		}

		public bool IsIntroCached(string videoUriTemplate = null)
		{
			return global::UnityEngine.PlayerPrefs.HasKey("VideoCache") && GetIntroVideoUri(videoUriTemplate) == global::UnityEngine.PlayerPrefs.GetString("VideoCache");
		}

		private void SetIntroCached(string videoUriTemplate = null)
		{
			global::UnityEngine.PlayerPrefs.SetString("VideoCache", GetIntroVideoUri(videoUriTemplate));
		}

		private void RequestCallback(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (response.Success)
			{
				telemetryService.Send_Telemetry_EVT_USER_GAME_DOWNLOAD_FUNNEL(response.Request.Uri, response.DownloadTime, response.ContentLength, global::Kampai.Util.NetworkUtil.IsNetworkWiFi());
				telemetryService.Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL("40 - Downloaded Intro Video", "anyVariant", dlcService.GetDownloadQualityLevel());
				SetIntroCached(request.videoUriTemplate);
			}
			else
			{
				logger.Error("[Video] Error fetching video {0}", response.Code);
			}
			setSplashProgressSignal.Dispatch(30f);
			fetchAndPlayIntroVideo();
		}

		private void UpdateProgressBar(global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress progress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest networkRequest)
		{
			long totalBytes = progress.TotalBytes;
			if (totalBytes > 0)
			{
				int num = request.progressBarStart + (int)((100f - (float)request.progressBarStart) * progress.GetProgress());
				int num2 = num - request.progressBarNow;
				logger.Info("[Video] Progress: {0}/{1} {2}", progress.CompletedBytes, totalBytes, num2);
				if (num2 > 0)
				{
					splashProgressUpdateSignal.Dispatch(num2, 1f);
					request.progressBarNow = num;
				}
			}
			else
			{
				logger.Warning("[Video] No progress bar with unknown length");
			}
		}
	}
}
