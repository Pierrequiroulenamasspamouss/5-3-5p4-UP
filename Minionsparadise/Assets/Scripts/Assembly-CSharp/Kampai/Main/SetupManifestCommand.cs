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

		public override void Execute()
		{
			global::Kampai.Util.TimeProfiler.StartSection("read manifest");
			manifestService.GenerateMasterManifest();
			global::Kampai.Util.TimeProfiler.EndSection("read manifest");
			global::Kampai.Util.KampaiResources.SetManifestService(manifestService);
			global::Kampai.Util.KampaiResources.SetAssetBundlesService(assetBundlesService);
			global::Kampai.Util.KampaiResources.SetLocalContentService(localContentService);
		}
	}
}
