namespace Kampai.Game
{
	public class MasterPlanRushTaskCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlanComponent component { get; set; }

		[Inject]
		public int taskIndex { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanTaskCompleteSignal taskCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MasterPlanComponentTask masterPlanComponentTask = component.tasks[taskIndex];
			global::Kampai.Game.MasterPlanComponentTaskType type = masterPlanComponentTask.Definition.Type;
			if (type != global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders && type != global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars && type != global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore)
			{
				global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> missingItemList = MasterPlanUtil.GetMissingItemList(masterPlanComponentTask);
				int rushCost = playerService.CalculateRushCost(missingItemList);
				if (masterPlanComponentTask.Definition.Type == global::Kampai.Game.MasterPlanComponentTaskType.Deliver)
				{
					playerService.ProcessRush(rushCost, missingItemList, true, "MasterPlan", RushTransactionCallback, true);
				}
				else
				{
					playerService.ProcessRush(rushCost, null, true, "MasterPlan", RushTransactionCallback, true);
				}
			}
		}

		private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				global::Kampai.Game.MasterPlanComponentTask masterPlanComponentTask = component.tasks[taskIndex];
				masterPlanComponentTask.earnedQuantity = masterPlanComponentTask.Definition.requiredQuantity;
				taskCompleteSignal.Dispatch(component, taskIndex);
				setPremiumCurrencySignal.Dispatch();
				soundFXSignal.Dispatch("Play_button_premium_01");
			}
		}
	}
}
