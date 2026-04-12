namespace Kampai.Game
{
	public interface ILandExpansionService
	{
		void AddBuilding(global::Kampai.Game.LandExpansionBuilding building);

		void TrackDebris(int debrisDefId, global::Kampai.Game.DebrisBuilding building);

		global::Kampai.Game.DebrisBuilding GetDebris(int debrisDefId);

		void TrackAspirationalBuilding(int aspirationalBuildingID, global::Kampai.Game.Building building);

		global::Kampai.Game.Building GetAspirationalBuilding(int aspirationalBuildingID);

		global::System.Collections.Generic.IList<global::Kampai.Game.Building> GetAllAspirationalBuildings();

		global::System.Collections.Generic.IList<global::Kampai.Game.LandExpansionBuilding> GetAllExpansionBuildings();

		global::System.Collections.Generic.IList<global::Kampai.Game.LandExpansionBuilding> GetBuildingsByExpansionID(int expansionID);

		global::Kampai.Game.LandExpansionBuilding GetBuildingByInstanceID(int builidngID);

		int GetExpansionByItemID(int itemID);

		global::System.Collections.Generic.IList<int> GetAllExpansionIDs();

		void AddForSaleSign(int expansionID, global::UnityEngine.GameObject sign);

		bool HasForSaleSign(int expansionID);

		void RemoveForSaleSign(int expansionID);

		global::UnityEngine.GameObject GetForSaleSign(int expansionID);

		void TrackFlower(global::Kampai.Game.LandExpansionBuilding building);

		global::System.Collections.Generic.IList<global::Kampai.Game.LandExpansionBuilding> GetTrackedFlowers();

		void AddToFlowerMap(int expansionID, global::UnityEngine.GameObject flowerObject);

		global::System.Collections.Generic.IList<global::UnityEngine.GameObject> GetFlowersByExpansionID(int expansionID);

		void RemoveFlowersByExpansionID(int expansionID);

		int GetLandExpansionCount();

		bool IsLevelGated(int expansionID, int playerLevel);

		bool ShouldUnlockThislevel(int expansionID, int playerLevel);
	}
}
