namespace Kampai.Game
{
	public static class RewardedAdUtil
	{
		public static bool GetFirstItemDefintion(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> rewards, out global::Kampai.Game.ItemDefinition itemDefinition, out int rewardAmount, global::Kampai.Game.IDefinitionService definitionService)
		{
			if (rewards != null && rewards.Count > 0)
			{
				global::Kampai.Util.QuantityItem quantityItem = rewards[0];
				if (definitionService.TryGet<global::Kampai.Game.ItemDefinition>(quantityItem.ID, out itemDefinition))
				{
					rewardAmount = (int)quantityItem.Quantity;
					return true;
				}
			}
			itemDefinition = null;
			rewardAmount = 0;
			return false;
		}
	}
}
