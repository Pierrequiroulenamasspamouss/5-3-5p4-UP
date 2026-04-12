namespace Kampai.UI.View
{
	public class ShowProceduralQuestPanelCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public int questInstanceId { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

		public override void Execute()
		{
			sfxSignal.Dispatch("Play_menu_popUp_01");
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_TSM_SellItems");
			iGUICommand.skrimScreen = "ProceduralTaskSkrim";
			iGUICommand.darkSkrim = false;
			iGUICommand.Args.Add(questInstanceId);
			guiService.Execute(iGUICommand);
		}
	}
}
