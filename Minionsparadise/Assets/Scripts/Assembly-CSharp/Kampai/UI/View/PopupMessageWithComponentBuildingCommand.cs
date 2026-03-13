namespace Kampai.UI.View
{
	public class PopupMessageWithComponentBuildingCommand : global::strange.extensions.command.impl.Command
	{
		private float fadeTime = 0.5f;

		private float openDuration = 2f;

		[Inject]
		public string localizedText { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public bool autoCloseOverride { get; set; }

		[Inject]
		public int componentBuildingDefinitionID { get; set; }

		[Inject]
		public global::Kampai.UI.IGhostComponentService ghostService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllDialogsSignal { get; set; }

		public override void Execute()
		{
			closeAllDialogsSignal.Dispatch();
			if (ghostService.DisplayAutoCloseGhostComponent(componentBuildingDefinitionID, fadeTime, openDuration))
			{
				global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadStatic, "popup_LairMessageBox");
				global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
				args.Add(localizedText);
				args.Add(global::Kampai.UI.View.MessagePopUpAnchor.TOP_CENTER);
				args.Add(autoCloseOverride);
				args.Add(new global::Kampai.Util.Tuple<float, float>(fadeTime, openDuration));
				guiService.Execute(iGUICommand);
			}
		}
	}
}
