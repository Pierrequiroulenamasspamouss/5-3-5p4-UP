public static class MarketplaceUtil
{
	public static int GetPremiumSlotCost(global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService)
	{
		global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
		global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleSlot> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.MarketplaceSaleSlot>(1000008096);
		return byDefinitionId.Count * marketplaceDefinition.PremiumIncrementCost + marketplaceDefinition.PremiumInitialCost;
	}

	public static global::Kampai.Util.QuantityItem GetQuantityItem(global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.MarketplaceItemDefinition itemDefinition)
	{
		global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(itemDefinition.TransactionID);
		if (transactionDefinition.Outputs.Count > 0)
		{
			return transactionDefinition.Outputs[0];
		}
		return null;
	}

	public static int GetRewardValue(global::Kampai.Util.QuantityItem quantityItem, global::Kampai.Game.MarketplaceSaleItem marketplaceItem)
	{
		return (int)quantityItem.Quantity * marketplaceItem.SalePrice;
	}
}
