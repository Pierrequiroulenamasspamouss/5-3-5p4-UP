namespace Kampai.Game
{
	public class FinishMinionPartyUnlockSequenceCommand : global::strange.extensions.command.impl.Command
	{
		private const string UNLOCK_FINISH_LOC_KEY = "qc2000000174_step2_intro";

		private const int PINATA_DEFINITION_ID = 3123;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PromptReceivedSignal promptReceivedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDialogSignal showDialog { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.OpenBuildingMenuSignal openBuildingMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetXPSignal setXPSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.QuestDialogSetting type = new global::Kampai.Game.QuestDialogSetting();
			global::Kampai.Util.Tuple<int, int> type2 = new global::Kampai.Util.Tuple<int, int>(0, 0);
			setXPSignal.Dispatch();
			promptReceivedSignal.AddOnce(DialogDismissed);
			showDialog.Dispatch("qc2000000174_step2_intro", type, type2);
		}

		private void DialogDismissed(int param1, int param2)
		{
			if (param1 == 0 && param2 == 0)
			{
				guiService.AddToArguments(new global::Kampai.UI.ThrobCallButtons());
				global::Kampai.Game.Building firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(3123);
				global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
				global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(firstInstanceByDefinitionId.ID);
				openBuildingMenuSignal.Dispatch(buildingObject, firstInstanceByDefinitionId);
				guiService.RemoveFromArguments(typeof(global::Kampai.UI.ThrobCallButtons));
			}
		}
	}
}
