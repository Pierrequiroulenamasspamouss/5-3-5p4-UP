namespace Kampai.Main
{
	public class CheckDLCTierCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CheckDLCTierCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		public override void Execute()
		{
			// logger.Info("CheckDLCTierCommand: Logic bypassed for bundle-free build.");
		}
	}
}
