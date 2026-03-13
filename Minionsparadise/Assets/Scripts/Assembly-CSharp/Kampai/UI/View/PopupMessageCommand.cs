namespace Kampai.UI.View
{
	public class PopupMessageCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public string localizedText { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageType type { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand((type == global::Kampai.UI.View.PopupMessageType.QUEUED) ? global::Kampai.UI.View.GUIOperation.Queue : global::Kampai.UI.View.GUIOperation.LoadStatic, "popup_MessageBox");
			iGUICommand.Args.Add(localizedText);
			iGUICommand.Args.Add(type == global::Kampai.UI.View.PopupMessageType.AUTO_CLOSE_OVERRIDE);
			guiService.Execute(iGUICommand);
		}
	}
}
