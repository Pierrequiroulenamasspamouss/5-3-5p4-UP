namespace Kampai.UI
{
	public interface ICurrencyStoreService
	{
		void Initialize();

		void MarkCategoryAsViewed(int categoryDefinitionID);

		void MarkCategoryAsViewed(global::Kampai.Game.CurrencyStoreCategoryDefinition currencyStoreCategoryDef);

		int GetBadgeCount(global::Kampai.Game.CurrencyStoreCategoryDefinition currencyStoreCategoryDef);

		global::Kampai.Game.PackDefinition GetCurrencyStorePackDefinition(int packDefinitionId);

		bool ShouldPackBeVisuallyLocked(global::Kampai.Game.PackDefinition currencyStorePackDefinition);

		bool HasPurchasedEnough(global::Kampai.Game.PackDefinition currencyStorePackDefinition);

		bool IsValidCurrencyItem(int storeItemDefinitionID, global::Kampai.Game.StoreCategoryType type, bool countInLocked = true);
	}
}
