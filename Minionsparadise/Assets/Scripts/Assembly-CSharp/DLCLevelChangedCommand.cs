public class DLCLevelChangedCommand : global::strange.extensions.command.impl.Command
{
	[Inject]
	public global::Kampai.Game.IDLCService dlcService { get; set; }

	[Inject]
	public global::Kampai.Main.ILocalContentService localContentService { get; set; }

	public override void Execute()
	{
		string downloadQualityLevel = dlcService.GetDownloadQualityLevel();
		localContentService.SetDLCQuality(downloadQualityLevel);
	}
}
