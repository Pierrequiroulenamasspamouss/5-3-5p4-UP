namespace Kampai.Game
{
	public class InsufficientInputsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.PendingCurrencyTransaction model { get; set; }

		[Inject]
		public global::Kampai.Game.LoadRushDialogSignal loadRushDialogSignal { get; set; }

		[Inject]
		public bool GrindFromPremium { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.Game.LoadCurrencyWarningSignal loadCurrencyWarningSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			bool flag = false;
			int rushCost = model.GetRushCost();
			if (model.IsRushing() && model.GetPendingTransaction() != null)
			{
				loadRushDialogSignal.Dispatch(model, global::Kampai.UI.View.RushDialogView.RushDialogType.DEFAULT);
				return;
			}
			global::Kampai.Game.Transaction.TransactionDefinition pendingTransaction = model.GetPendingTransaction();
			if (pendingTransaction != null)
			{
				if (global::Kampai.Game.Transaction.TransactionUtil.IsOnlyGrindInputs(pendingTransaction))
				{
					int num = global::Kampai.Game.Transaction.TransactionUtil.SumInputsForStaticItem(pendingTransaction, global::Kampai.Game.StaticItem.GRIND_CURRENCY_ID);
					num -= (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.GRIND_CURRENCY_ID);
					flag = true;
					loadCurrencyWarningSignal.Dispatch(new global::Kampai.UI.View.CurrencyWarningModel(num, playerService.PremiumCostForGrind(num), global::Kampai.Game.StoreItemType.GrindCurrency, false, model));
				}
				else if (global::Kampai.Game.Transaction.TransactionUtil.IsOnlyPremiumInputs(pendingTransaction))
				{
					int num2 = global::Kampai.Game.Transaction.TransactionUtil.SumInputsForStaticItem(pendingTransaction, global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID);
					int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID);
					int cost = num2 - quantity;
					flag = true;
					loadCurrencyWarningSignal.Dispatch(new global::Kampai.UI.View.CurrencyWarningModel(num2, cost, global::Kampai.Game.StoreItemType.PremiumCurrency, GrindFromPremium, model));
				}
			}
			if (rushCost > 0)
			{
				int num3 = rushCost - (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID);
				if (num3 > 0)
				{
					flag = true;
					loadCurrencyWarningSignal.Dispatch(new global::Kampai.UI.View.CurrencyWarningModel(rushCost, num3, global::Kampai.Game.StoreItemType.PremiumCurrency));
				}
			}
			if (flag)
			{
				currencyService.CurrencyDialogOpened(model);
				return;
			}
			global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback = model.GetCallback();
			if (callback != null)
			{
				model.Success = false;
				callback(model);
			}
		}
	}
}
