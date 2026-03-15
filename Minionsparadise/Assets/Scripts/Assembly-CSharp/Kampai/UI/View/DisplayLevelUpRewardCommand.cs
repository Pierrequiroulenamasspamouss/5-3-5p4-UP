namespace Kampai.UI.View
{
	public class DisplayLevelUpRewardCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockCharacterModel unlockCharacterModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		[Inject]
		public bool isInspiratioReward { get; set; }

		public override void Execute()
		{
			uiModel.LevelUpUIOpen = true;
			global::Kampai.Game.Transaction.TransactionDefinition transaction = ((!isInspiratioReward) ? RewardUtil.GetPartyTransaction(definitionService, playerService) : RewardUtil.GetRewardTransaction(definitionService, playerService));
			global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity> rewardQuantityFromTransaction = RewardUtil.GetRewardQuantityFromTransaction(transaction, definitionService, playerService);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_PhilsInspiration");
			iGUICommand.Args.Add(rewardQuantityFromTransaction);
			iGUICommand.Args.Add(isInspiratioReward);
			iGUICommand.ShouldShowPredicate = () => unlockCharacterModel.characterUnlocks.Count == 0;
			guiService.Execute(iGUICommand);
		}
	}
}
