namespace Kampai.Game.View
{
	public class AwardLevelCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Transaction.TransactionDefinition transaction { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateUIButtonsSignal updateStoreButtonsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CheckResourceBuildingSlotsSignal resourceBuildingSignal { get; set; }

		public override void Execute()
		{
			playerService.RunEntireTransaction(transaction, global::Kampai.Game.TransactionTarget.NO_VISUAL, null);
			updateStoreButtonsSignal.Dispatch(true);
			resourceBuildingSignal.Dispatch();
		}
	}
}
