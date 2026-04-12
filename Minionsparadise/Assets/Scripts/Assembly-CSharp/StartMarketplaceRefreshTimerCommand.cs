public class StartMarketplaceRefreshTimerCommand : global::strange.extensions.command.impl.Command
{
	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	[Inject]
	public global::Kampai.Game.IDefinitionService definitionService { get; set; }

	[Inject]
	public global::Kampai.Game.ITimeService timeService { get; set; }

	[Inject]
	public bool isRush { get; set; }

	public override void Execute()
	{
		global::Kampai.Game.MarketplaceRefreshTimerDefinition def = definitionService.Get<global::Kampai.Game.MarketplaceRefreshTimerDefinition>(1000008093);
		global::Kampai.Game.MarketplaceRefreshTimer marketplaceRefreshTimer = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MarketplaceRefreshTimer>(1000008093);
		if (marketplaceRefreshTimer == null)
		{
			marketplaceRefreshTimer = new global::Kampai.Game.MarketplaceRefreshTimer(def);
			marketplaceRefreshTimer.UTCStartTime = timeService.CurrentTime();
			playerService.Add(marketplaceRefreshTimer);
		}
		if (isRush)
		{
			marketplaceRefreshTimer.UTCStartTime = timeService.CurrentTime();
		}
	}
}
