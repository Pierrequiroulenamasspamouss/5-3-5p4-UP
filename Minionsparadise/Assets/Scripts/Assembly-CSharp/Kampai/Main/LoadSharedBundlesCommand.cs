namespace Kampai.Main
{
	public class LoadSharedBundlesCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadSharedBundlesCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		[Inject]
		public global::Kampai.Main.IAssetBundlesService assetBundlesService { get; set; }

		public override void Execute()
		{
			logger.Debug("Start Loading Shared Bundles");
			global::Kampai.Util.TimeProfiler.StartSection("loading shared bundles");
#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN
			foreach (string shaderBundle in manifestService.GetShaderBundles())
			{
				assetBundlesService.LoadSharedBundle(shaderBundle);
			}
#else
			logger.Info("Bypassing shader bundle loading on Windows/Editor");
#endif
			global::Kampai.Util.TimeProfiler.EndSection("loading shared bundles");
		}
	}
}
