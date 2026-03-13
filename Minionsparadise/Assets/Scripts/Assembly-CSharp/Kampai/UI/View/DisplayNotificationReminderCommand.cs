namespace Kampai.UI.View
{
	public class DisplayNotificationReminderCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public string message { get; set; }

		[Inject]
		public bool autoClose { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		public override void Execute()
		{
			if (!coppaService.Restricted())
			{
				global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_Notification");
				iGUICommand.Args.Add(message);
				iGUICommand.Args.Add(autoClose);
				iGUICommand.skrimScreen = "NotificationsSkrim";
				iGUICommand.darkSkrim = true;
				guiService.Execute(iGUICommand);
			}
		}
	}
}
