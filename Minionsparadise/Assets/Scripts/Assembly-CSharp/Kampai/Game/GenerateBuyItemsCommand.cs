namespace Kampai.Game
{
	public class GenerateBuyItemsCommand : global::strange.extensions.command.impl.Command
	{
		private enum ItemCategory
		{
			craftableType = 0,
			baseResourceType = 1,
			dropType = 2
		}

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GenerateBuyItemsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.MarketplaceBuyItem> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceBuyItem>();
			foreach (global::Kampai.Game.MarketplaceBuyItem item in instancesByType)
			{
				playerService.Remove(item);
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> unlockedDefsByType = playerService.GetUnlockedDefsByType<global::Kampai.Game.IngredientsItemDefinition>();
			if (unlockedDefsByType.Count <= 0)
			{
				logger.Error("No items are unlocked yet, so no Marketplace should exist.");
				return;
			}
			global::System.Collections.Generic.IList<int> craftableItems = FindUnlockedInCategory("Craftable", unlockedDefsByType);
			global::System.Collections.Generic.IList<int> resourceItems = FindUnlockedInCategory("Base Resource", unlockedDefsByType);
			global::System.Collections.Generic.IList<global::Kampai.Game.DropItemDefinition> all = definitionService.GetAll<global::Kampai.Game.DropItemDefinition>();
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			int totalBuyAds = marketplaceDefinition.TotalBuyAds;
			for (int i = 0; i < totalBuyAds; i++)
			{
				global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory categoryPicked = PickItemCategory(marketplaceDefinition, craftableItems, resourceItems, all.Count);
				int itemID = ((marketplaceService.DebugSelectedItem <= 0) ? PickExactItemType(categoryPicked, craftableItems, resourceItems, all) : marketplaceService.DebugSelectedItem);
				int num = FindSizeOfStack(categoryPicked, marketplaceDefinition);
				global::Kampai.Game.MarketplaceItemDefinition itemDefinition;
				marketplaceService.GetItemDefinitionByItemID(itemID, out itemDefinition);
				int num2 = randomService.NextInt(itemDefinition.MinStrikePrice, itemDefinition.MaxStrikePrice + 1);
				int buyPrice = num2 * num;
				global::Kampai.Game.MarketplaceBuyItem i2 = CreateMarketplaceBuyItem(itemDefinition, num, buyPrice);
				playerService.Add(i2);
			}
		}

		private global::System.Collections.Generic.IList<int> FindUnlockedInCategory(string category, global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> unlockedItems)
		{
			global::System.Collections.Generic.IList<int> list = new global::System.Collections.Generic.List<int>();
			if (unlockedItems != null)
			{
				foreach (global::Kampai.Game.IngredientsItemDefinition unlockedItem in unlockedItems)
				{
					if (unlockedItem.TaxonomySpecific == category)
					{
						int iD = unlockedItem.ID;
						global::Kampai.Game.MarketplaceItemDefinition itemDefinition;
						if (!marketplaceService.GetItemDefinitionByItemID(iD, out itemDefinition))
						{
							logger.Error("Marketplace item doesn't exists {0}.", iD);
						}
						else
						{
							int probabilityWeight = itemDefinition.ProbabilityWeight;
							for (int i = 0; i < probabilityWeight; i++)
							{
								list.Add(iD);
							}
						}
					}
				}
				return list;
			}
			return null;
		}

		private global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory PickItemCategory(global::Kampai.Game.MarketplaceDefinition marketplaceDefinition, global::System.Collections.Generic.IList<int> craftableItems, global::System.Collections.Generic.IList<int> resourceItems, int dropCount)
		{
			global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory itemCategory = global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory.craftableType;
			int craftableWeight = marketplaceDefinition.CraftableWeight;
			int baseResourceWeight = marketplaceDefinition.BaseResourceWeight;
			int dropWeight = marketplaceDefinition.DropWeight;
			int num = craftableWeight + baseResourceWeight + dropWeight;
			int num2 = randomService.NextInt(0, num);
			if (num2 < dropWeight && dropCount > 0)
			{
				return global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory.dropType;
			}
			if (num2 < craftableWeight + dropWeight && craftableItems.Count > 0)
			{
				return global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory.craftableType;
			}
			if (num2 < num && resourceItems.Count > 0)
			{
				return global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory.baseResourceType;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> unlockedDefsByType = playerService.GetUnlockedDefsByType<global::Kampai.Game.IngredientsItemDefinition>();
			craftableItems = FindUnlockedInCategory("Craftable", unlockedDefsByType);
			resourceItems = FindUnlockedInCategory("Base Resource", unlockedDefsByType);
			logger.Error("No item category was picked. There's an error! Look into it.");
			return global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory.baseResourceType;
		}

		private int PickExactItemType(global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory categoryPicked, global::System.Collections.Generic.IList<int> craftableItems, global::System.Collections.Generic.IList<int> resourceItems, global::System.Collections.Generic.IList<global::Kampai.Game.DropItemDefinition> dropItems)
		{
			int num = 0;
			int result = 0;
			switch (categoryPicked)
			{
			case global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory.craftableType:
				num = randomService.NextInt(craftableItems.Count);
				result = craftableItems[num];
				craftableItems.Remove(num);
				break;
			case global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory.baseResourceType:
				num = randomService.NextInt(resourceItems.Count);
				result = resourceItems[num];
				resourceItems.Remove(num);
				break;
			case global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory.dropType:
				num = randomService.NextInt(dropItems.Count);
				result = dropItems[num].ID;
				dropItems.Remove(dropItems[num]);
				break;
			default:
				logger.Error("This should never happens. It means no category was picked or at least no new item will be found.");
				break;
			}
			return result;
		}

		private int FindSizeOfStack(global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory categoryPicked, global::Kampai.Game.MarketplaceDefinition marketplaceDefinition)
		{
			int num = 1;
			if (categoryPicked != global::Kampai.Game.GenerateBuyItemsCommand.ItemCategory.dropType)
			{
				return randomService.NextInt(1, marketplaceDefinition.MaxSellQuantity + 1);
			}
			return randomService.NextInt(1, marketplaceDefinition.MaxDropQuantity + 1);
		}

		private global::Kampai.Game.MarketplaceBuyItem CreateMarketplaceBuyItem(global::Kampai.Game.MarketplaceItemDefinition marketplaceItemDefinition, int stackSize, int buyPrice)
		{
			global::Kampai.Game.MarketplaceBuyItem marketplaceBuyItem = new global::Kampai.Game.MarketplaceBuyItem(marketplaceItemDefinition);
			marketplaceBuyItem.BuyQuantity = stackSize;
			marketplaceBuyItem.BuyPrice = buyPrice;
			marketplaceBuyItem.BoughtFlag = false;
			return marketplaceBuyItem;
		}
	}
}
