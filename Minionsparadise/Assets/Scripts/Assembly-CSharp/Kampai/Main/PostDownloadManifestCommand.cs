namespace Kampai.Main
{
	public class PostDownloadManifestCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PostDownloadManifestCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.SetupManifestSignal setupManifestSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.ReconcileDLCSignal reconcileDLCSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.CheckAvailableStorageSignal checkAvailableStorageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.LoginUserSignal loginSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.LaunchDownloadSignal launchDownloadSignal { get; set; }

		[Inject]
		public global::Kampai.Common.IVideoService videoService { get; set; }

		[Inject]
		public global::Kampai.Splash.IBackgroundDownloadDlcService backgroundDownloadDlcService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Main.IAssetsPreloadService assetsPreloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public IResourceService resourceService { get; set; }

		public override void Execute()
		{
			// logger.Info("PostDownloadManifestCommand: DLC bypass flow triggered.");
			
			// logger.Info("PostDownloadManifestCommand: Dispatching setupManifestSignal...");
			setupManifestSignal.Dispatch();
			
			logger.Info("PostDownloadManifestCommand: Preloading all assets...");
			assetsPreloadService.PreloadAllAssets();
			
			logger.Info("PostDownloadManifestCommand: Dispatching loginSignal...");
			loginSignal.Dispatch();
			
			// logger.Info("PostDownloadManifestCommand: Flow complete.");
		}
	}
}
