namespace Kampai.Game
{
	public class LandExpansionService : global::Kampai.Game.ILandExpansionService
	{
		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.LandExpansionBuilding> byID = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.LandExpansionBuilding>();

		private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>> expansionMap = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>>();

		private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::UnityEngine.GameObject>> flowerMap = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::UnityEngine.GameObject>>();

		private global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding> flowerList = new global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>();

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.DebrisBuilding> debrisLookupMap = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.DebrisBuilding>();

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.Building> aspirationalLookupMap = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.Building>();

		private global::System.Collections.Generic.List<int> allExpansionIDs = new global::System.Collections.Generic.List<int>();

		private global::System.Collections.Generic.Dictionary<int, global::UnityEngine.GameObject> forSaleSignMap = new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.GameObject>();

		public void AddBuilding(global::Kampai.Game.LandExpansionBuilding building)
		{
			AddToAllExpansionIDs(building.ExpansionID);
			AddToExpansionMap(building.ExpansionID, building);
			AddToIDMap(building);
		}

		public void TrackDebris(int debrisDefId, global::Kampai.Game.DebrisBuilding building)
		{
			debrisLookupMap.Add(debrisDefId, building);
		}

		public global::Kampai.Game.DebrisBuilding GetDebris(int debrisDefId)
		{
			return debrisLookupMap[debrisDefId];
		}

		public void TrackAspirationalBuilding(int aspirationalBuildingID, global::Kampai.Game.Building building)
		{
			if (!aspirationalLookupMap.ContainsKey(aspirationalBuildingID))
			{
				aspirationalLookupMap.Add(aspirationalBuildingID, building);
			}
		}

		public global::Kampai.Game.Building GetAspirationalBuilding(int aspirationalBuildingID)
		{
			global::Kampai.Game.Building value = null;
			aspirationalLookupMap.TryGetValue(aspirationalBuildingID, out value);
			return value;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Building> GetAllAspirationalBuildings()
		{
			return new global::System.Collections.Generic.List<global::Kampai.Game.Building>(aspirationalLookupMap.Values);
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.LandExpansionBuilding> GetAllExpansionBuildings()
		{
			return new global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>(byID.Values);
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.LandExpansionBuilding> GetBuildingsByExpansionID(int expansionID)
		{
			if (!expansionMap.ContainsKey(expansionID))
			{
				return new global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>();
			}
			return expansionMap[expansionID];
		}

		public global::Kampai.Game.LandExpansionBuilding GetBuildingByInstanceID(int builidngID)
		{
			return byID[builidngID];
		}

		public int GetExpansionByItemID(int itemID)
		{
			return byID[itemID].ExpansionID;
		}

		public int GetLandExpansionCount()
		{
			return allExpansionIDs.Count;
		}

		private void AddToIDMap(global::Kampai.Game.LandExpansionBuilding building)
		{
			byID[building.ID] = building;
		}

		private void AddToExpansionMap(int expansionID, global::Kampai.Game.LandExpansionBuilding building)
		{
			if (!expansionMap.ContainsKey(expansionID))
			{
				expansionMap[expansionID] = new global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>();
			}
			expansionMap[expansionID].Add(building);
		}

		private void AddToAllExpansionIDs(int expansionID)
		{
			if (!allExpansionIDs.Contains(expansionID))
			{
				allExpansionIDs.Add(expansionID);
			}
		}

		public global::System.Collections.Generic.IList<int> GetAllExpansionIDs()
		{
			return allExpansionIDs;
		}

		public void AddForSaleSign(int expansionID, global::UnityEngine.GameObject sign)
		{
			if (!forSaleSignMap.ContainsKey(expansionID))
			{
				forSaleSignMap[expansionID] = sign;
			}
		}

		public bool HasForSaleSign(int expansionID)
		{
			return forSaleSignMap.ContainsKey(expansionID);
		}

		public global::UnityEngine.GameObject GetForSaleSign(int expansionID)
		{
			if (!forSaleSignMap.ContainsKey(expansionID))
			{
				return null;
			}
			return forSaleSignMap[expansionID];
		}

		public void RemoveForSaleSign(int expansionID)
		{
			global::UnityEngine.GameObject forSaleSign = GetForSaleSign(expansionID);
			if (!(forSaleSign == null))
			{
				global::UnityEngine.Object.Destroy(forSaleSign);
				forSaleSignMap.Remove(expansionID);
			}
		}

		public void TrackFlower(global::Kampai.Game.LandExpansionBuilding building)
		{
			if (!flowerList.Contains(building))
			{
				flowerList.Add(building);
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.LandExpansionBuilding> GetTrackedFlowers()
		{
			return flowerList;
		}

		public void AddToFlowerMap(int expansionID, global::UnityEngine.GameObject flowerObject)
		{
			if (!flowerMap.ContainsKey(expansionID))
			{
				flowerMap[expansionID] = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
			}
			flowerMap[expansionID].Add(flowerObject);
		}

		public global::System.Collections.Generic.IList<global::UnityEngine.GameObject> GetFlowersByExpansionID(int expansionID)
		{
			if (flowerMap.ContainsKey(expansionID))
			{
				return flowerMap[expansionID];
			}
			return null;
		}

		public void RemoveFlowersByExpansionID(int expansionID)
		{
			if (!flowerMap.ContainsKey(expansionID))
			{
				return;
			}
			foreach (global::UnityEngine.GameObject item in flowerMap[expansionID])
			{
				global::UnityEngine.Object.Destroy(item);
			}
			flowerMap[expansionID] = null;
		}

		public bool IsLevelGated(int expansionID, int playerLevel)
		{
			if (!expansionMap.ContainsKey(expansionID))
			{
				return false;
			}
			int minimumLevel = expansionMap[expansionID][0].MinimumLevel;
			return minimumLevel > playerLevel;
		}

		public bool ShouldUnlockThislevel(int expansionID, int playerLevel)
		{
			int minimumLevel = expansionMap[expansionID][0].MinimumLevel;
			return minimumLevel == playerLevel;
		}
	}
}
