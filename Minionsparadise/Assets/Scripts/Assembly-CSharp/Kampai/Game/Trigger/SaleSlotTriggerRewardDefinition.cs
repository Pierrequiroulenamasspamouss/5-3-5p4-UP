namespace Kampai.Game.Trigger
{
	public class SaleSlotTriggerRewardDefinition : global::Kampai.Game.Trigger.TriggerRewardDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1183;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerRewardType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.MarketplaceSaleSlot;
			}
		}

		public override void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context)
		{
			if (base.transaction != null && context != null)
			{
				global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = context.injectionBinder;
				injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>().RunEntireTransaction(base.transaction.ToDefinition(), global::Kampai.Game.TransactionTarget.NO_VISUAL, null);
				injectionBinder.GetInstance<global::Kampai.Game.InitializeMarketplaceSlotsSignal>().Dispatch();
			}
		}
	}
}
