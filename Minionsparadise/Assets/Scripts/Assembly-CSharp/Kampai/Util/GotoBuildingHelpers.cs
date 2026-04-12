namespace Kampai.Util
{
	internal static class GotoBuildingHelpers
	{
		public static global::Kampai.Game.Building GetSuitableBuilding(global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> playerBuildings)
		{
			global::Kampai.Game.Building building = null;
			foreach (global::Kampai.Game.Building playerBuilding in playerBuildings)
			{
				if (building == null)
				{
					building = playerBuilding;
				}
				if (playerBuilding.State != global::Kampai.Game.BuildingState.Inventory)
				{
					global::Kampai.Game.ResourceBuilding resourceBuilding = playerBuilding as global::Kampai.Game.ResourceBuilding;
					if (resourceBuilding != null && !resourceBuilding.AreAllMinionSlotsFilled())
					{
						return playerBuilding;
					}
					global::Kampai.Game.CraftingBuilding craftingBuilding = playerBuilding as global::Kampai.Game.CraftingBuilding;
					if (craftingBuilding != null && craftingBuilding.Slots > craftingBuilding.RecipeInQueue.Count)
					{
						return playerBuilding;
					}
					global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = playerBuilding as global::Kampai.Game.VillainLairResourcePlot;
					if (villainLairResourcePlot != null)
					{
						return GetSuitableResourcePlot(playerBuildings);
					}
				}
			}
			return building;
		}

		public static global::Kampai.Game.VillainLairResourcePlot GetSuitableResourcePlot(global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> playerPlotBuildings)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Building> list = (global::System.Collections.Generic.IList<global::Kampai.Game.Building>)playerPlotBuildings;
			global::System.Collections.Generic.IList<int> list2 = new global::System.Collections.Generic.List<int>();
			global::System.Collections.Generic.IList<int> list3 = new global::System.Collections.Generic.List<int>();
			for (int i = 0; i < list.Count; i++)
			{
				global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = list[i] as global::Kampai.Game.VillainLairResourcePlot;
				global::Kampai.Game.BuildingState state = villainLairResourcePlot.State;
				if (state != global::Kampai.Game.BuildingState.Inaccessible)
				{
					list2.Add(i);
					if (state == global::Kampai.Game.BuildingState.Idle)
					{
						list3.Add(i);
					}
				}
			}
			global::System.Random random = new global::System.Random();
			if (list3.Count > 0)
			{
				return list[list3[random.Next(0, list3.Count)]] as global::Kampai.Game.VillainLairResourcePlot;
			}
			if (list2.Count > 0)
			{
				return list[list2[random.Next(0, list2.Count)]] as global::Kampai.Game.VillainLairResourcePlot;
			}
			return list[random.Next(0, list.Count)] as global::Kampai.Game.VillainLairResourcePlot;
		}

		public static int GetSuitableMignette(global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService)
		{
			int num = 0;
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			global::System.Collections.Generic.List<global::Kampai.Game.MignetteBuilding> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.MignetteBuilding>();
			global::System.Random random = new global::System.Random();
			global::System.Collections.Generic.IList<global::Kampai.Game.AspirationalBuildingDefinition> all = definitionService.GetAll<global::Kampai.Game.AspirationalBuildingDefinition>();
			int i = 0;
			for (int count = all.Count; i < count; i++)
			{
				global::Kampai.Game.AspirationalBuildingDefinition aspirationalBuildingDefinition = all[i];
				global::Kampai.Game.Building firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(aspirationalBuildingDefinition.BuildingDefinitionID);
				if (firstInstanceByDefinitionId == null)
				{
					list.Add(aspirationalBuildingDefinition.BuildingDefinitionID);
					continue;
				}
				global::Kampai.Game.MignetteBuilding mignetteBuilding = firstInstanceByDefinitionId as global::Kampai.Game.MignetteBuilding;
				if (mignetteBuilding != null)
				{
					list2.Add(mignetteBuilding);
				}
			}
			if (list2.Count == 1)
			{
				return list2[0].Definition.ID;
			}
			global::System.Collections.Generic.List<int> list3 = new global::System.Collections.Generic.List<int>();
			foreach (global::Kampai.Game.MignetteBuilding item in list2)
			{
				if (item.State != global::Kampai.Game.BuildingState.Cooldown)
				{
					list3.Add(item.Definition.ID);
				}
			}
			if (list3.Count > 0)
			{
				return list3[random.Next(0, list3.Count)];
			}
			if (list2.Count > 0)
			{
				return list2[random.Next(0, list2.Count)].Definition.ID;
			}
			return list[random.Next(0, list.Count)];
		}

		public static bool BuildingMenuIsAccessible(global::Kampai.Game.Building building)
		{
			global::Kampai.Game.BuildingState state = building.State;
			return state != global::Kampai.Game.BuildingState.Inactive && state != global::Kampai.Game.BuildingState.Complete && state != global::Kampai.Game.BuildingState.Construction;
		}

		public static bool BuildingLivesInsideLair(global::Kampai.Game.Building building)
		{
			if (building is global::Kampai.Game.VillainLairResourcePlot || building is global::Kampai.Game.MasterPlanComponentBuilding)
			{
				return true;
			}
			return false;
		}

		public static bool BuildingLivesInsideLair(global::Kampai.Game.BuildingDefinition buildingDef)
		{
			if (buildingDef.Type == BuildingType.BuildingTypeIdentifier.LAIR_RESOURCEPLOT || buildingDef.Type == BuildingType.BuildingTypeIdentifier.MASTER_COMPONENT)
			{
				return true;
			}
			return false;
		}
	}
}
