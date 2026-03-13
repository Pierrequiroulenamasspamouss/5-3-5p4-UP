public class OpenMinionUpgradeBuildingCommand : global::strange.extensions.command.impl.Command
{
	[Inject]
	public global::Kampai.UI.View.IGUIService guiService { get; set; }

	public override void Execute()
	{
		global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_MinionUpgrade");
		iGUICommand.darkSkrim = false;
		guiService.Execute(iGUICommand);
	}
}
