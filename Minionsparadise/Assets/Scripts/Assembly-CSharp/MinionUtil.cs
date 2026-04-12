public static class MinionUtil
{
	public static int RushCost(int minionInstanceID, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.ITimeEventService timeEventService, global::Kampai.Game.IDefinitionService definitionService)
	{
		global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionInstanceID);
		if (byInstanceId.State != global::Kampai.Game.MinionState.Tasking && byInstanceId.State != global::Kampai.Game.MinionState.Leisure)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		global::Kampai.Game.RushActionType rushActionType = global::Kampai.Game.RushActionType.HARVESTING;
		global::Kampai.Game.Building byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Building>(byInstanceId.BuildingID);
		switch (byInstanceId.State)
		{
		case global::Kampai.Game.MinionState.Leisure:
		{
			global::Kampai.Game.LeisureBuilding leisureBuilding = byInstanceId2 as global::Kampai.Game.LeisureBuilding;
			num = leisureBuilding.Definition.LeisureTimeDuration;
			num2 = ((!timeEventService.HasEventID(leisureBuilding.ID)) ? num : timeEventService.GetTimeRemaining(leisureBuilding.ID));
			rushActionType = global::Kampai.Game.RushActionType.LEISURE;
			break;
		}
		case global::Kampai.Game.MinionState.Tasking:
		{
			global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = byInstanceId2 as global::Kampai.Game.VillainLairResourcePlot;
			global::Kampai.Game.ResourceBuilding resourceBuilding = byInstanceId2 as global::Kampai.Game.ResourceBuilding;
			if (villainLairResourcePlot != null)
			{
				num = villainLairResourcePlot.parentLair.Definition.SecondsToHarvest;
				num2 = ((!timeEventService.HasEventID(villainLairResourcePlot.ID)) ? num : timeEventService.GetTimeRemaining(villainLairResourcePlot.ID));
			}
			else
			{
				if (resourceBuilding == null)
				{
					return -1;
				}
				num = BuildingUtil.GetHarvestTimeForTaskableBuilding(resourceBuilding, definitionService);
				num2 = timeEventService.GetTimeRemaining(minionInstanceID);
			}
			rushActionType = global::Kampai.Game.RushActionType.HARVESTING;
			break;
		}
		default:
			return -1;
		}
		return timeEventService.CalculateRushCostForTimer(global::UnityEngine.Mathf.Min(num2, num), rushActionType);
	}
}
