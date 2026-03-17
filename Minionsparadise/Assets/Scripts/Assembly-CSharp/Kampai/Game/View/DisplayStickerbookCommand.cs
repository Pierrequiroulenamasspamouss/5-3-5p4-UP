namespace Kampai.Game.View
{
	public class DisplayStickerbookCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_Stickerbook");
			iGUICommand.skrimScreen = "StickerBookSkrim";
			iGUICommand.darkSkrim = true;
			guiService.Execute(iGUICommand);
		}
	}
}
