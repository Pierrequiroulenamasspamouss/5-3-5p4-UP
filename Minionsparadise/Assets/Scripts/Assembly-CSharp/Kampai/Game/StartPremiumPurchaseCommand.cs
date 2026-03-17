namespace Kampai.Game
{
	public class StartPremiumPurchaseCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.KampaiPendingTransaction kampaiPendingTransaction { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		public override void Execute()
		{
			if (playerService.GetPendingTransactions().Count > 0)
			{
				string type = localService.GetString("PendingTransaction");
				popupMessageSignal.Dispatch(type, global::Kampai.UI.View.PopupMessageType.QUEUED);
			}
			else
			{
				playerService.QueuePendingTransaction(kampaiPendingTransaction);
				currencyService.RequestPurchase(kampaiPendingTransaction);
			}
		}
	}
}
