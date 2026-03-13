public static class IngredientsItemUtil
{
	public static int GetHarvestTimeFromIngredientDefinition(global::Kampai.Game.IngredientsItemDefinition iid, global::Kampai.Game.IDefinitionService definitionService)
	{
		int timeToHarvest = (int)iid.TimeToHarvest;
		if (timeToHarvest != 0)
		{
			return timeToHarvest;
		}
		global::System.Collections.Generic.List<global::Kampai.Game.VillainLairDefinition> all = definitionService.GetAll<global::Kampai.Game.VillainLairDefinition>();
		foreach (global::Kampai.Game.VillainLairDefinition item in all)
		{
			if (item.ResourceItemID == iid.ID)
			{
				return item.SecondsToHarvest;
			}
		}
		return timeToHarvest;
	}
}
