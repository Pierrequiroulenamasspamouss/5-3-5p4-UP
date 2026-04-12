namespace Kampai.Common
{
	public class ShowRateAppPanelCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "RateAppPanel");
			iGUICommand.skrimScreen = "RateAppSkrim";
			iGUICommand.darkSkrim = false;
			guiService.Execute(iGUICommand);
		}
	}
}
