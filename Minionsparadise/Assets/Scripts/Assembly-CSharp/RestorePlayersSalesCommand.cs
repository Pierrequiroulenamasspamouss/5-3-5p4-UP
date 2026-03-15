public class RestorePlayersSalesCommand : global::strange.extensions.command.impl.Command
{
	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	[Inject]
	public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

	[Inject]
	public global::Kampai.Game.ITimeService timeService { get; set; }

	[Inject]
	public global::Kampai.Game.MarketplaceItemSoldSignal marketplaceItemSoldSignal { get; set; }

	[Inject]
	public global::Kampai.Game.MarketplaceUpdateSoldItemsSignal updateSoldItemsSignal { get; set; }

	public override void Execute()
	{
		global::System.Collections.Generic.IList<global::Kampai.Game.MarketplaceSaleItem> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceSaleItem>();
		foreach (global::Kampai.Game.MarketplaceSaleItem item in instancesByType)
		{
			int num = timeService.CurrentTime() - item.SaleStartTime;
			if (num > item.LengthOfSale)
			{
				item.state = global::Kampai.Game.MarketplaceSaleItem.State.SOLD;
			}
			else
			{
				timeEventService.AddEvent(item.ID, item.SaleStartTime, item.LengthOfSale, marketplaceItemSoldSignal);
			}
		}
		updateSoldItemsSignal.Dispatch(true);
	}
}
