namespace Kampai.Game.Mtx
{
	internal sealed class ProcessNextPendingTransactionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ProcessNextPendingTransactionCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.KampaiPendingTransaction> pendingTransactions = playerService.GetPendingTransactions();
			if (pendingTransactions.Count > 0)
			{
				logger.Debug("ProcessNextPendingTransactionCommand: A pending transaction found, repurchasing");
				currencyService.RequestPurchase(pendingTransactions[0]);
			}
		}
	}
}
