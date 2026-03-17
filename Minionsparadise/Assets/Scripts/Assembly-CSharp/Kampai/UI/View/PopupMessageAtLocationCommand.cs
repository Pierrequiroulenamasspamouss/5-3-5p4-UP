namespace Kampai.UI.View
{
	public class PopupMessageAtLocationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public string localizedText { get; set; }

		[Inject]
		public global::Kampai.UI.View.MessagePopUpAnchor anchor { get; set; }

		[Inject]
		public global::UnityEngine.Vector2 anchorPosition { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadStatic, "popup_MessageBox");
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(localizedText);
			args.Add(anchor);
			args.Add(anchorPosition);
			guiService.Execute(iGUICommand);
		}
	}
}
