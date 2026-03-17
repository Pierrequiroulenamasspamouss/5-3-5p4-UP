namespace Kampai.Common
{
	internal static class RewardedAdTelemetryUtil
	{
		public static string GetSurfaceLocation(global::Kampai.Game.AdPlacementName placementName)
		{
			return placementName.ToString();
		}

		public static string GetRewardType(global::Kampai.Game.AdPlacementName placementName, global::Kampai.Game.ItemDefinition reward)
		{
			if (reward == null)
			{
				return "null reward";
			}
			switch (placementName)
			{
			case global::Kampai.Game.AdPlacementName.ORDERBOARD:
				return "Buyout";
			case global::Kampai.Game.AdPlacementName.CRAFTING:
			case global::Kampai.Game.AdPlacementName.MARKETPLACE:
				return "Rush";
			case global::Kampai.Game.AdPlacementName.QUEST:
				return "Double Reward";
			default:
				return GetDefaultRewardType(reward);
			}
		}

		public static string GetDefaultRewardType(global::Kampai.Game.ItemDefinition itemDefinition)
		{
			switch (itemDefinition.ID)
			{
			case 0:
				return "Grind";
			case 1:
				return "Premium";
			case 50:
				return "Token";
			default:
				switch (itemDefinition.TaxonomySpecific)
				{
				case "Base Resource":
					return "Base Resource";
				case "Decoration":
					return "Deco";
				case "Craftable":
					return "Crafts";
				default:
					if (itemDefinition.TaxonomyHighLevel == "Drop")
					{
						return "Drops";
					}
					return itemDefinition.TaxonomySpecific;
				}
			}
		}
	}
}
