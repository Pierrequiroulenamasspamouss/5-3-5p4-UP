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
			
			// Always report success and zero needed bundles to achieve DLC independence.
			model.TotalSize = 0uL;

			
			logger.Info("ReconcileDLCCommand: DLC reconciliation bypassed. reporting success.");
			
			global::Kampai.Util.TimeProfiler.EndSection("reconcile dlc");
		}
	}
}
