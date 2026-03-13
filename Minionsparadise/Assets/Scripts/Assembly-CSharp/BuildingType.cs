public static class BuildingType
{
	public enum BuildingTypeIdentifier
	{
		UNKNOWN = 0,
		CRAFTING = 1,
		DECORATION = 2,
		LEISURE = 3,
		RESOURCE = 4,
		SPECIAL = 5,
		BLACKMARKETBOARD = 6,
		STORAGE = 7,
		LANDEXPANSION = 8,
		DEBRIS = 9,
		MIGNETTE = 10,
		TIKIBAR = 11,
		LAIR_ENTRANCE = 12,
		LAIR_RESOURCEPLOT = 13,
		MINION_UPGRADE = 14,
		CABANA = 15,
		BRIDGE = 16,
		COMPOSITE = 17,
		STAGE = 18,
		WELCOMEHUT = 19,
		FOUNTAIN = 20,
		MESSAGEINABOTTLE = 21,
		DCN = 22,
		MASTER_COMPONENT = 23,
		MASTER_LEFTOVER = 24,
		CONNECTABLE = 25
	}

	public static BuildingType.BuildingTypeIdentifier ParseIdentifier(string identifier)
	{
		if (identifier != null)
		{
			return (BuildingType.BuildingTypeIdentifier)(int)global::System.Enum.Parse(typeof(BuildingType.BuildingTypeIdentifier), identifier);
		}
		return BuildingType.BuildingTypeIdentifier.UNKNOWN;
	}
}
