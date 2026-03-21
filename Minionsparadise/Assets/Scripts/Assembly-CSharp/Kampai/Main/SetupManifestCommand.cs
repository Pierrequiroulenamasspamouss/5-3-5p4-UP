namespace Kampai.Main
{
	public class SetupManifestCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		[Inject]
		public global::Kampai.Main.IAssetBundlesService assetBundlesService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalContentService localContentService { get; set; }

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupManifestCommand") as global::Kampai.Util.IKampaiLogger;

		public override void Execute()
		{
			logger.Info("SetupManifestCommand started");
			global::Kampai.Util.TimeProfiler.StartSection("read manifest");
			manifestService.GenerateMasterManifest();
			global::Kampai.Util.TimeProfiler.EndSection("read manifest");
			logger.Info("SetupManifestCommand finished GenerateMasterManifest");
			global::Kampai.Util.KampaiResources.SetManifestService(manifestService);
			global::Kampai.Util.KampaiResources.SetAssetBundlesService(assetBundlesService);
			global::Kampai.Util.KampaiResources.SetLocalContentService(localContentService);
			logger.Info("SetupManifestCommand finished");
		}
	}
}
