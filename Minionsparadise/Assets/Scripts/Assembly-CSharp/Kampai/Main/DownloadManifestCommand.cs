namespace Kampai.Main
{
	public class DownloadManifestCommand : global::strange.extensions.command.impl.Command
	{
		private const string REST_DLC_MANIFEST = "manifests";

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DownloadManifestCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IConfigurationsService configService { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Util.IInvokerService invoker { get; set; }

		[Inject]
		public global::Kampai.Common.PostDownloadManifestSignal postSignal { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			// logger.Info("DownloadManifestCommand: Logic bypassed for bundle-free build.");
			postSignal.Dispatch();
		}
	}
}
