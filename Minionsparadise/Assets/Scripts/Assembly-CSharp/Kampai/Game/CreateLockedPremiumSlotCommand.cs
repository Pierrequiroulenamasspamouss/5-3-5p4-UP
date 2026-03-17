namespace Kampai.Game
{
	public class CreateLockedPremiumSlotCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateMarketplaceSlotSignal createMarketplaceSlotSignal { get; set; }

		public override void Execute()
		{
			bool flag = false;
			global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleSlot> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.MarketplaceSaleSlot>(1000008096);
			foreach (global::Kampai.Game.MarketplaceSaleSlot item in byDefinitionId)
			{
				if (item.state == global::Kampai.Game.MarketplaceSaleSlot.State.LOCKED)
				{
					flag = true;
				}
			}
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			if (!flag && byDefinitionId.Count < marketplaceDefinition.MaxPremiumSlots)
			{
				createMarketplaceSlotSignal.Dispatch(global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType.PREMIUM_UNLOCKABLE);
			}
		}
	}
}
