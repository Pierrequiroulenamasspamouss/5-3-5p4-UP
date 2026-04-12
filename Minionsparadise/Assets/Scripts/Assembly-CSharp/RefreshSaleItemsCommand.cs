public class RefreshSaleItemsCommand : global::strange.extensions.command.impl.Command
{
	private global::Kampai.Game.MarketplaceRefreshTimer refreshTimer;

	[Inject]
	public global::Kampai.Game.GenerateBuyItemsSignal generateBuyItemsSignal { get; set; }

	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	[Inject]
	public global::Kampai.Game.ITimeService timeService { get; set; }

	[Inject]
	public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

	[Inject]
	public global::Kampai.Game.RefreshSaleItemsSuccessSignal refreshSuccessSignal { get; set; }

	[Inject]
	public global::Kampai.Game.RushRefreshTimerSuccessSignal rushSuccessSignal { get; set; }

	[Inject]
	public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

	[Inject]
	public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

	[Inject]
	public RefreshSaleItemsSignalArgs args { get; set; }

	public override void Execute()
	{
		refreshTimer = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MarketplaceRefreshTimer>(1000008093);
		int cost = ((!args.RushCost.HasValue) ? refreshTimer.Definition.RushCost : args.RushCost.Value);
		if (args.RefreshItems)
		{
			playSFXSignal.Dispatch("Play_marketplace_slotStart_01");
			RefreshMarketplace();
		}
		else if (!args.StopSpinning)
		{
			playerService.ProcessRefreshMarket(cost, true, RushTransactionCallback);
		}
	}

	private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
	{
		if (pct.Success)
		{
			refreshTimer.UTCStartTime = timeService.CurrentTime() - refreshTimer.Definition.RefreshTimeSeconds;
			rushSuccessSignal.Dispatch();
			setPremiumCurrencySignal.Dispatch();
		}
	}

	private void RefreshMarketplace()
	{
		generateBuyItemsSignal.Dispatch();
		refreshSuccessSignal.Dispatch();
		telemetryService.Send_Telemtry_EVT_MARKETPLACE_VIEWED("REFRESH");
	}
}
