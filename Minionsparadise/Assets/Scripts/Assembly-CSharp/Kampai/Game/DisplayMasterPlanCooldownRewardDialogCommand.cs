namespace Kampai.Game
{
	public class DisplayMasterPlanCooldownRewardDialogCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PromptReceivedSignal promptReceivedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDialogSignal showDialogSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.QuestDialogSetting questDialogSetting = new global::Kampai.Game.QuestDialogSetting();
			questDialogSetting.type = global::Kampai.UI.View.QuestDialogType.NORMAL;
			global::Kampai.Game.QuestDialogSetting questDialogSetting2 = questDialogSetting;
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			questDialogSetting2.additionalStringParameter = currentMasterPlan.Definition.LocalizedKey;
			showDialogSignal.Dispatch(currentMasterPlan.Definition.CooldownRewardDialogKey, questDialogSetting2, new global::Kampai.Util.Tuple<int, int>(0, 0));
			promptReceivedSignal.AddOnce(DialogComplete);
		}

		private void DialogComplete(int questID, int stepID)
		{
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			global::Kampai.Game.Transaction.TransactionDefinition value = (masterPlanService.HasReceivedInitialRewardFromCurrentPlan() ? definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(currentMasterPlan.Definition.SubsequentCooldownRewardTransactionID) : definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(currentMasterPlan.Definition.CooldownRewardTransactionID));
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_MasterPlanCooldownReward");
			iGUICommand.skrimScreen = "MasterPlan";
			iGUICommand.darkSkrim = true;
			iGUICommand.Args.Add(value);
			iGUICommand.Args.Add(currentMasterPlan.ID);
			guiService.Execute(iGUICommand);
		}
	}
}
