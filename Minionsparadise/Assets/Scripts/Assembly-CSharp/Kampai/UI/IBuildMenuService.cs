namespace Kampai.UI
{
	public interface IBuildMenuService
	{
		void SetStoreUnlockChecked();

		void AddNewUnlockedItem(global::Kampai.Game.StoreItemType type, int buildingDefinitionID);

		bool RemoveNewUnlockedItem(global::Kampai.Game.StoreItemType type, int buildingDefinitionID);

		void AddUncheckedInventoryItem(global::Kampai.Game.StoreItemType type, int buildingDefinitionID);

		void RemoveUncheckedInventoryItem(global::Kampai.Game.StoreItemType type, int buildingDefinitionID);

		void ClearTab(global::Kampai.Game.StoreItemType type);

		void RetoreBuidMenuState(global::System.Collections.Generic.Dictionary<global::Kampai.Game.StoreItemType, global::System.Collections.Generic.List<global::Kampai.UI.View.StoreButtonView>> buttonViews);

		void ClearAllNewUnlockItems();

		void UpdateNewUnlockList(global::System.Collections.Generic.Dictionary<global::Kampai.Game.StoreItemType, global::System.Collections.Generic.List<global::Kampai.UI.View.StoreButtonView>> buttonViews, bool updateBuildMenuButton = true, bool updateBadge = true);

		int GetStoreItemDefinitionIDFromBuildingID(int buildingID);

		bool ShouldRenderStoreDef(global::Kampai.Game.StoreItemDefinition storeDef);

		bool ShowingAChild(global::System.Collections.Generic.List<global::Kampai.UI.View.StoreButtonView> children, bool notifyShouldBeRendered = true);

		void CompleteBuildMenuUpdate(BuildingType.BuildingTypeIdentifier buildingDefType, int buildingDefinitionID);
	}
}
