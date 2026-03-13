namespace Kampai.UI.View
{
	public class OpenRewardedAdWatchModalCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.AdPlacementInstance adPlacementInstance { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllOtherMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

		public override void Execute()
		{
			closeAllOtherMenuSignal.Dispatch(null);
			LoadRewardedModalUI();
			sfxSignal.Dispatch("Play_menu_popUp_01");
		}

		public void LoadRewardedModalUI()
		{
			string text = "popup_WatchRewardedAd";
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, text);
			iGUICommand.skrimScreen = "RewardedAdWatch";
			iGUICommand.darkSkrim = true;
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(text);
			args.Add(typeof(global::Kampai.Game.AdPlacementInstance), adPlacementInstance);
			guiService.Execute(iGUICommand);
		}
	}
}
