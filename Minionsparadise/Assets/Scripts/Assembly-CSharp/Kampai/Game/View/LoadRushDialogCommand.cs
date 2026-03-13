namespace Kampai.Game.View
{
	public class LoadRushDialogCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.PendingCurrencyTransaction pendingCurrencyTransaction { get; set; }

		[Inject]
		public global::Kampai.UI.View.RushDialogView.RushDialogType type { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		public override void Execute()
		{
			playSFXSignal.Dispatch("Play_not_enough_items_01");
			global::Kampai.UI.View.IGUICommand iGUICommand;
			if (type == global::Kampai.UI.View.RushDialogView.RushDialogType.STORAGE_EXPAND)
			{
				iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_MissingResources", "popup_OutOfResourceForStorage");
				iGUICommand.skrimScreen = "RushStorageSkrim";
			}
			else
			{
				iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_MissingResources");
				iGUICommand.skrimScreen = "RushSkrim";
			}
			iGUICommand.darkSkrim = true;
			iGUICommand.singleSkrimClose = true;
			iGUICommand.Args.Add(pendingCurrencyTransaction);
			iGUICommand.Args.Add(type);
			guiService.Execute(iGUICommand);
		}
	}
}
