namespace Kampai.Main
{
	public class ReconcileDLCCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ReconcileDLCCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public bool purge { get; set; }

		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel model { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		public override void Execute()
		{
			global::Kampai.Util.TimeProfiler.StartSection("reconcile dlc");
			
			// Always report zero needed bundles to achieve DLC independence.
			model.NeededBundles = new global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo>();
			model.TotalSize = 0uL;
			model.HighestTierDownloaded = 999;
			
			logger.Info("ReconcileDLCCommand: DLC reconciliation disabled. Reporting 0 needed bundles.");
			
			global::Kampai.Util.TimeProfiler.EndSection("reconcile dlc");
		}

		private string GetBundlePath(string name)
		{
			string bundleLocation = manifestService.GetBundleLocation(name);
			if (string.IsNullOrEmpty(bundleLocation))
			{
				return global::System.IO.Path.Combine(global::Kampai.Util.GameConstants.DLC_PATH, name + ".unity3d");
			}
			return global::System.IO.Path.Combine(bundleLocation, name + ".unity3d");
		}

		private bool BundleExists(string name)
		{
#if !UNITY_WEBPLAYER
			return global::System.IO.File.Exists(GetBundlePath(name));
#else
			return false;
#endif
		}

		private bool IsValidBundle(global::Kampai.Util.BundleInfo info)
		{
			if (info.isStreamable)
			{
				return true;
			}
			if (!BundleExists(info.name))
			{
				return false;
			}
			string bundlePath = GetBundlePath(info.name);
			ulong length = 0uL;
#if !UNITY_WEBPLAYER
			length = (ulong)new global::System.IO.FileInfo(bundlePath).Length;
#endif
			if (length != info.size)
			{
				logger.Error("SIZE CHECK FAILED: {0} {1}!={2} failed", info.name, length, info.size);
				return false;
			}
			return true;
		}
	}
}
