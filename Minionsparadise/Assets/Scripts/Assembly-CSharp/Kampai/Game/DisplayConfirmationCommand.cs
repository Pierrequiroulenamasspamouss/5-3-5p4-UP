namespace Kampai.Game
{
	public class DisplayConfirmationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.PopupConfirmationSetting confirmationSetting { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_Confirmation");
			iGUICommand.Args.Add(confirmationSetting);
			iGUICommand.skrimScreen = "ConfirmationSkrim";
			iGUICommand.disableSkrimButton = true;
			guiService.Execute(iGUICommand);
		}
	}
}
