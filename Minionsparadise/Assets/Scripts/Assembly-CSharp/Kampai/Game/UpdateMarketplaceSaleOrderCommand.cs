namespace Kampai.Game
{
	public class UpdateMarketplaceSaleOrderCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSaleSlotSignal updateSaleSlot { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.MarketplaceSaleItem> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceSaleItem>();
			instancesByType.Sort();
			global::System.Collections.Generic.List<global::Kampai.Game.MarketplaceSaleSlot> instancesByType2 = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceSaleSlot>();
			for (int i = 0; i < instancesByType2.Count; i++)
			{
				if (i < instancesByType.Count)
				{
					instancesByType2[i].itemId = instancesByType[i].ID;
				}
				else
				{
					instancesByType2[i].itemId = 0;
				}
				updateSaleSlot.Dispatch(instancesByType2[i].ID);
			}
		}
	}
}
