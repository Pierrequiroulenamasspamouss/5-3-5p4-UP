namespace Kampai.Splash
{
	public class LoadingBarMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private float start;

		private float target;

		private float current;

		private float timeTarget;

		private float timeRemaining;

		private bool dlcMode;

		private global::System.Collections.Generic.IDictionary<string, global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress> dlcProgess = new global::System.Collections.Generic.Dictionary<string, global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress>();

		private bool isDlcStale;

		[Inject]
		public global::Kampai.Splash.LoadingBarView view { get; set; }

		[Inject]
		public global::Kampai.Splash.SplashProgressUpdateSignal splashProgressUpdateSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.SetSplashProgressSignal setSplashProgressSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DownloadInitializeSignal downloadInitializeSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DownloadProgressSignal downloadProgressSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DownloadResponseSignal downloadResponseSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCDownloadFinishedSignal downloadFinishedSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCLoadScreenModel model { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		public override void OnRegister()
		{
			splashProgressUpdateSignal.AddListener(OnSplashProgressUpdate);
			setSplashProgressSignal.AddListener(OnSetSplashProgress);
			downloadInitializeSignal.AddListener(InitializeDLC);
			downloadProgressSignal.AddListener(OnDownloadProgress);
			downloadResponseSignal.AddListener(OnDownloadResponse);
			downloadFinishedSignal.AddListener(OnDownloadFinish);
			view.Init();
			if (!dlcMode)
			{
				current = model.CurrentLoadProgress;
				UpdateView();
			}
		}

		public override void OnRemove()
		{
			splashProgressUpdateSignal.RemoveListener(OnSplashProgressUpdate);
			setSplashProgressSignal.RemoveListener(OnSetSplashProgress);
			downloadInitializeSignal.RemoveListener(InitializeDLC);
			downloadProgressSignal.RemoveListener(OnDownloadProgress);
			downloadResponseSignal.RemoveListener(OnDownloadResponse);
			downloadFinishedSignal.RemoveListener(OnDownloadFinish);
			model.CurrentLoadProgress = current;
		}

		private void OnSplashProgressUpdate(int target, float time)
		{
			this.target += target;
			start = current;
			if (this.target > 100f)
			{
				this.target = 100f;
			}
			timeRemaining = time;
			timeTarget = time;
		}

		private void OnSetSplashProgress(float progress)
		{
			start = (current = (target = global::UnityEngine.Mathf.Min(progress, 100f)));
			timeRemaining = (timeTarget = 0f);
		}

		private void Update()
		{
			float currentProgress = GetCurrentProgress();
			if (!dlcMode && timeRemaining > 0f && timeTarget > 0f)
			{
				float deltaTime = global::UnityEngine.Time.deltaTime;
				timeRemaining -= deltaTime;
				float num = (target - start) * (deltaTime / timeTarget);
				if (num > 0f)
				{
					current = global::UnityEngine.Mathf.Min(current + num, 100f);
				}
			}
			else if (dlcMode && isDlcStale)
			{
				isDlcStale = false;
				long num2 = 0L;
				foreach (global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress value in dlcProgess.Values)
				{
					num2 += value.CompletedBytes;
				}
				model.CurrentProgress = global::UnityEngine.Mathf.Min((float)num2 / 1048576f, model.TotalSize);
			}
			if (currentProgress != GetCurrentProgress())
			{
				UpdateView();
			}
		}

		private float GetCurrentProgress()
		{
			return (dlcMode && model != null) ? model.CurrentProgress : current;
		}

		private void UpdateView()
		{
			float num = (dlcMode ? (model.CurrentProgress / model.TotalSize * 100f) : current);
			view.SetText(dlcMode ? localizationService.GetString("DLCIndicatorProgress", global::UnityEngine.Mathf.RoundToInt(model.CurrentProgress), global::UnityEngine.Mathf.Max(1, global::UnityEngine.Mathf.RoundToInt(model.TotalSize))) : string.Format("{0:0}%", num));
			view.SetMeterFill(num);
		}

		private void ToggleDlcMode(bool isEnabled)
		{
			if (dlcMode != isEnabled)
			{
				dlcMode = isEnabled;
				UpdateView();
			}
		}

		private void InitializeDLC(ulong size)
		{
			model.TotalSize = (float)size / 1048576f;
			model.CurrentProgress = 0f;
			dlcProgess.Clear();
			foreach (global::Kampai.Util.BundleInfo neededBundle in dlcModel.NeededBundles)
			{
				dlcProgess[neededBundle.name] = new global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress(neededBundle.name)
				{
					TotalBytes = (long)neededBundle.size,
					CompressionRatio = (float)neededBundle.size / (float)neededBundle.zipsize
				};
			}
			isDlcStale = true;
			ToggleDlcMode(true);
		}

		private void OnDownloadProgress(global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress progress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request)
		{
			global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress value;
			if (dlcProgess.TryGetValue(global::Kampai.Util.DownloadUtil.GetBundleNameFromUrl(request.Uri), out value))
			{
				value.CompletedBytes = ((!progress.IsGZipped) ? progress.CompletedBytes : ((long)((float)progress.CompletedBytes * value.CompressionRatio)));
				isDlcStale = true;
			}
		}

		private void OnDownloadResponse(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress value;
			if (!response.Success && dlcProgess.TryGetValue(global::Kampai.Util.DownloadUtil.GetBundleNameFromUrl(response.Request.Uri), out value))
			{
				value.CompletedBytes = 0L;
				isDlcStale = true;
			}
		}

		private void OnDownloadFinish()
		{
			ToggleDlcMode(false);
			dlcProgess.Clear();
			isDlcStale = false;
		}
	}
}
