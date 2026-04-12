public class OpenVillainLairResourcePlotBuildingCommand : global::strange.extensions.command.impl.Command
{
	[Inject]
	public global::Kampai.Game.VillainLairResourcePlot resourcePlot { get; set; }

	[Inject]
	public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

	[Inject]
	public global::Kampai.UI.View.IGUIService guiService { get; set; }

	[Inject]
	public global::Kampai.UI.View.CloseAllMessageDialogs closeAllMessageDialogsSignal { get; set; }

	[Inject]
	public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

	public override void Execute()
	{
		if (!villainLairModel.leavingLair)
		{
			if (resourcePlot.State == global::Kampai.Game.BuildingState.Inaccessible)
			{
				OpenModal("screen_Resource_Lair_Locked");
			}
			else
			{
				OpenModal("screen_Resource_Lair_Unlocked");
			}
		}
	}

	private void OpenModal(string prefabName)
	{
		closeAllMessageDialogsSignal.Dispatch();
		sfxSignal.Dispatch("Play_menu_popUp_01");
		global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, prefabName);
		iGUICommand.skrimScreen = "VillainLairResourceSkrim";
		global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
		args.Add(resourcePlot);
		args.Add(global::Kampai.UI.View.RushDialogView.RushDialogType.VILLAIN_LAIR_RESOURCE_PLOT);
		guiService.Execute(iGUICommand);
	}
}
