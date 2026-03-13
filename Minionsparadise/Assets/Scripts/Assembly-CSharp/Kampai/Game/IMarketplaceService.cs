namespace Kampai.Game
{
	public interface IMarketplaceService
	{
		bool isServerKillSwitchEnabled { get; }

		bool IsDebugMode { get; set; }

		float DebugMultiplier { get; set; }

		int DebugSelectedItem { get; set; }

		bool DebugFacebook { get; set; }

		bool GetItemDefinitionByItemID(int itemID, out global::Kampai.Game.MarketplaceItemDefinition itemDefinition);

		global::Kampai.Game.MarketplaceSaleSlot GetSlotByItem(global::Kampai.Game.MarketplaceSaleItem item);

		global::Kampai.Game.MarketplaceSaleSlot GetNextAvailableSlot();

		int GetSlotIndex(global::Kampai.Game.MarketplaceSaleSlot slot);

		bool AreThereValidItemsInStorage();

		bool IsUnlocked();

		bool AreThereSoldItems();

		bool IsSlotVisible(global::Kampai.Game.MarketplaceSaleSlot slot);

		bool AreTherePendingItems();

		void SetMinStrikePrice(int itemID, int price);

		void SetMaxStrikePrice(int itemID, int price);
	}
}
