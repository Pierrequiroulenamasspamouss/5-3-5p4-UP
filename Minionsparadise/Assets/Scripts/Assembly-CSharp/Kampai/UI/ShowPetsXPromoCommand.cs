namespace Kampai.UI
{
	internal sealed class ShowPetsXPromoCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_Pets");
			iGUICommand.skrimScreen = "PetsXPromo";
			iGUICommand.darkSkrim = true;
			guiService.Execute(iGUICommand);
		}
	}
}
