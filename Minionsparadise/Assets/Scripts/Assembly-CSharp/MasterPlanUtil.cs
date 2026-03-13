public static class MasterPlanUtil
{
	public static global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> GetMissingItemList(global::Kampai.Game.MasterPlanComponentTask task)
	{
		global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> list = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
		switch (task.Definition.Type)
		{
		case global::Kampai.Game.MasterPlanComponentTaskType.Deliver:
		case global::Kampai.Game.MasterPlanComponentTaskType.Collect:
		{
			global::Kampai.Util.QuantityItem item3 = new global::Kampai.Util.QuantityItem(task.Definition.requiredItemId, task.remainingQuantity);
			list.Add(item3);
			break;
		}
		case global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame:
		{
			global::Kampai.Util.QuantityItem item2 = new global::Kampai.Util.QuantityItem(186, task.remainingQuantity);
			list.Add(item2);
			break;
		}
		case global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints:
		case global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints:
		case global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints:
		{
			global::Kampai.Util.QuantityItem item = new global::Kampai.Util.QuantityItem(2, task.remainingQuantity);
			list.Add(item);
			break;
		}
		}
		return list;
	}
}
