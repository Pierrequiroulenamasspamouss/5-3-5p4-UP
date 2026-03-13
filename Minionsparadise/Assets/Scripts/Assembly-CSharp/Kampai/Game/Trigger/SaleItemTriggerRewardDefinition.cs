namespace Kampai.Game.Trigger
{
	public class SaleItemTriggerRewardDefinition : global::Kampai.Game.Trigger.TriggerRewardDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1182;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerRewardType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.MarketplaceSaleItem;
			}
		}

		public override void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context)
		{
			if (base.transaction == null || context == null)
			{
				return;
			}
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = context.injectionBinder;
			global::Kampai.Game.PlayerService playerService = injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>() as global::Kampai.Game.PlayerService;
			if (playerService != null && playerService.timeService != null)
			{
				global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleItem> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.MarketplaceSaleItem>(1000008094);
				global::Kampai.Game.MarketplaceSaleItem nextForSaleItem = null;
				global::Kampai.Game.Trigger.SaleItemTriggerConditionDefinition.GetClosestSaleItem(playerService.timeService, byDefinitionId, int.MaxValue, ref nextForSaleItem);
				if (nextForSaleItem != null)
				{
					nextForSaleItem.state = global::Kampai.Game.MarketplaceSaleItem.State.SOLD;
					injectionBinder.GetInstance<global::Kampai.Game.MarketplaceUpdateSoldItemsSignal>().Dispatch(true);
				}
			}
		}
	}
}
