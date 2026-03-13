namespace Kampai.Game
{
	public class ConfirmStartNewMinionPartyCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CancelBuildingMovementSignal cancelBuildingMovementSignal { get; set; }

		public override void Execute()
		{
			closeAllMenuSignal.Dispatch(null);
			cancelBuildingMovementSignal.Dispatch(false);
			global::Kampai.UI.View.IGUICommand iGUICommand = null;
			iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_StartPartyPopup");
			iGUICommand.skrimScreen = "StartPartySkirm";
			iGUICommand.darkSkrim = true;
			iGUICommand.disableSkrimButton = true;
			iGUICommand.alphaAmt = 0.5f;
			iGUICommand.skrimBehavior = global::Kampai.UI.View.SkrimBehavior.partyEffectsAndFade;
			guiService.Execute(iGUICommand);
		}
	}
}
