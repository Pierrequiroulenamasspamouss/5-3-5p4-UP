namespace Kampai.Main
{
	public class MainCompleteCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MainCompleteCommand") as global::Kampai.Util.IKampaiLogger;

		private global::UnityEngine.AsyncOperation async;

		[Inject]
		public global::Kampai.Game.AutoSavePlayerStateSignal autoSavePlayerSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReloadConfigurationsPeriodicSignal reloadConfigs { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Main.LoadDevicePrefsSignal loadDevicePrefsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.LoadSharedBundlesSignal bundleSignal { get; set; }

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.IClientHealthService clientHealthService { get; set; }

		[Inject]
		public global::Kampai.Common.LogTapEventMetricsSignal logTapEventMetricsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.LoadLocalizationServiceSignal localServiceSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetupPushNotificationsSignal setupPushNotificationsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Main.IAssetBundlesService assetBundlesService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimedSocialEventService socialEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Game.NimbleOTSignal nimbleOTSignal { get; set; }

		[Inject]
		public global::Kampai.Util.ICoroutineProgressMonitor coroutineProgressMonitor { get; set; }

		[Inject]
		public global::Kampai.Main.LoadAudioSignal loadAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.LaunchDownloadSignal launchDownloadSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.SplashProgressUpdateSignal splashProgressDoneSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.ILoadInService loadInService { get; set; }

		[Inject]
		public global::Kampai.Util.FastCommandPool fastCommandPool { get; set; }

		[Inject]
		public global::Kampai.Main.CheckDLCTierSignal checkDLCTier { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject]
		public global::Kampai.Game.LoadVillainLairAssetsSignal loadVillainLairAssetsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.IAssetsPreloadService assetsPreloadService { get; set; }

		[Inject]
		public global::Kampai.Main.IHindsightService hindsightService { get; set; }

		public override void Execute()
		{
			logger.EventStart("MainCompleteCommand.Execute");
			checkDLCTier.Dispatch();
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.TIER_ID);
			int quantity2 = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.TIER_GATE_ID);
			dlcService.SetPlayerDLCTier(quantity);
			if (dlcModel.HighestTierDownloaded < quantity2)
			{
				logger.Debug("DLC Highest Tier Downloaded is less than the tier Gate, launching DLC Download");
				launchDownloadSignal.Dispatch(false);
			}
			else
			{
				global::Kampai.Util.TimeProfiler.StartSection("loading scenes");
				loadDevicePrefsSignal.Dispatch();
				loadAudioSignal.Dispatch();
				routineRunner.StartCoroutine(PostExternalScenes());
			}
			hindsightService.Initialize();
			logger.EventStop("MainCompleteCommand.Execute");
		}

		private global::System.Collections.IEnumerator PostExternalScenes()
		{
			yield return null;
			while (coroutineProgressMonitor.HasRunningTasks())
			{
				yield return null;
			}
			logger.Debug("Starting Load Post External Scene");
			bundleSignal.Dispatch();
			assetsPreloadService.StopAssetsPreload();
			localServiceSignal.Dispatch();
			global::Kampai.Util.DeviceCapabilities.Initialize();
			global::Kampai.Util.TimeProfiler.StartSection("loading game scene");
			logger.EventStart("MainCompleteCommand.LoadGame");
			global::UnityEngine.SceneManagement.SceneManager.LoadScene("Game", global::UnityEngine.SceneManagement.LoadSceneMode.Additive);
			global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("UI", global::UnityEngine.SceneManagement.LoadSceneMode.Additive);
			splashProgressDoneSignal.Dispatch(100, 3f);
			routineRunner.StartCoroutine(LevelLoadComplete());
		}

		private global::System.Collections.IEnumerator LevelLoadComplete()
		{
			yield return null;
			while (coroutineProgressMonitor.HasRunningTasks())
			{
				yield return null;
			}
			logger.EventStop("MainCompleteCommand.LoadGame");
			global::Kampai.Util.TimeProfiler.EndSection("loading game scene");
			logger.EventStart("MainCompleteCommand.LoadUI");
			autoSavePlayerSignal.Dispatch();
			reloadConfigs.Dispatch();
			nimbleOTSignal.Dispatch();
			clientHealthService.MarkMeterEvent("AppFlow.AppStart");
			telemetryService.Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL("100 - Load Complete", playerService.SWRVEGroup, dlcService.GetDownloadQualityLevel());
			loadDevicePrefsSignal.Dispatch();
			logTapEventMetricsSignal.Dispatch();
			setupPushNotificationsSignal.Dispatch();
			socialEventService.GetPastEventsWithUnclaimedReward();
			loadInService.SaveTipsForNextLaunch((int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID));
			while (coroutineProgressMonitor.HasRunningTasks())
			{
				yield return null;
			}
			logger.EventStop("MainCompleteCommand.LoadUI");
			global::Kampai.Util.TimeProfiler.EndSection("loading scenes");
			global::Kampai.Util.TimeProfiler.StartSection("cleanup");
			logger.EventStart("MainCompleteCommand.CleanUp");
			assetBundlesService.UnloadDLCBundles();
			async = global::UnityEngine.Resources.UnloadUnusedAssets();
			routineRunner.StartCoroutine(CleanupComplete());
		}

		private global::System.Collections.IEnumerator CleanupComplete()
		{
			while (!async.isDone)
			{
				yield return new global::UnityEngine.WaitForEndOfFrame();
			}
			fastCommandPool.Warmup();
			global::System.GC.Collect();
			global::System.GC.WaitForPendingFinalizers();
			logger.EventStop("MainCompleteCommand.CleanUp");
			global::Kampai.Util.TimeProfiler.EndSection("cleanup");
			global::strange.extensions.context.api.ICrossContextCapable splashContext = null;
			try
			{
				splashContext = base.injectionBinder.GetInstance<global::strange.extensions.context.api.ICrossContextCapable>(global::Kampai.Splash.SplashElement.CONTEXT);
			}
			catch (global::strange.extensions.injector.impl.InjectionException ex)
			{
				global::strange.extensions.injector.impl.InjectionException e = ex;
				logger.Warning(e.ToString());
			}
			global::UnityEngine.Shader.WarmupAllShaders();
			if (splashContext != null)
			{
				splashContext.injectionBinder.GetInstance<global::Kampai.Splash.HideSplashSignal>().Dispatch();
				yield return null;
				ResumeCurrencyService();
			}
			global::Kampai.Game.VillainLairEntranceBuilding portal = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(374);
			if (portal != null && portal.IsUnlocked)
			{
				loadVillainLairAssetsSignal.Dispatch(portal.VillainLairInstanceID);
			}
			if (localPersistService.HasKey("RelinkingAccount"))
			{
				localPersistService.DeleteKey("RelinkingAccount");
			}
		}

		private void ResumeCurrencyService()
		{
			global::Kampai.Game.ICurrencyService instance = base.injectionBinder.GetInstance<global::Kampai.Game.ICurrencyService>();
			instance.ResumeTransactionsHandling();
		}
	}
}
