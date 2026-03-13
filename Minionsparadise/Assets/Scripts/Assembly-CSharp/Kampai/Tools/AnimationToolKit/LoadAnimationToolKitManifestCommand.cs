namespace Kampai.Tools.AnimationToolKit
{
	public class LoadAnimationToolKitManifestCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		[Inject]
		public global::Kampai.Main.IAssetBundlesService assetBundlesService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalContentService localContentService { get; set; }

		public override void Execute()
		{
			manifestService.GenerateMasterManifest();
			global::Kampai.Util.KampaiResources.SetManifestService(manifestService);
			global::Kampai.Util.KampaiResources.SetAssetBundlesService(assetBundlesService);
			global::Kampai.Util.KampaiResources.SetLocalContentService(localContentService);
		}
	}
}
