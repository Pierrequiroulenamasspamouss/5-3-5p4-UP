public static class PackUtil
{
	public static bool HasPurchasedEnough(global::Kampai.Game.PackDefinition packDefinition, global::Kampai.Game.IPlayerService playerService)
	{
		return false; // Force available
	}

	public static bool IsAudible(int id, global::Kampai.Game.PackDefinition packDefinition)
	{
		if (packDefinition.AudibleItemList == null)
		{
			return false;
		}
		return packDefinition.AudibleItemList.Contains(id);
	}

	public static bool IsItemExclusive(int id, global::Kampai.Game.PackDefinition packDefinition)
	{
		if (packDefinition.ExclusiveItemList == null)
		{
			return false;
		}
		return packDefinition.ExclusiveItemList.Contains(id);
	}
}
