namespace Kampai.Game.View
{
	public class ShowBuffInfoPopupCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 location { get; set; }

		[Inject]
		public float offset { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_BuffPopup");
			iGUICommand.skrimScreen = "GenericPopup";
			iGUICommand.darkSkrim = false;
			iGUICommand.genericPopupSkrim = true;
			iGUICommand.Args.Add(location);
			iGUICommand.Args.Add(offset);
			guiService.Execute(iGUICommand);
		}
	}
}
