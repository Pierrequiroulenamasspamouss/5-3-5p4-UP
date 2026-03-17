namespace Kampai.Game
{
	public class CollectTSMQuestTaskRewardCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.Quest quest { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateTSMQuestTaskSignal updateTSMQuestTaskSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageCapacitySignal { get; set; }

		[Inject]
		public global::Kampai.Game.GoToNextQuestStateSignal goToNextQuestStateSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.DynamicQuestDefinition dynamicQuestDefinition = quest.GetActiveDefinition() as global::Kampai.Game.DynamicQuestDefinition;
			if (dynamicQuestDefinition != null)
			{
				global::Kampai.Game.Transaction.TransactionDefinition reward = dynamicQuestDefinition.GetReward(definitionService);
				if (reward != null)
				{
					global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg();
					global::Kampai.Game.TaskTransactionArgument taskTransactionArgument = new global::Kampai.Game.TaskTransactionArgument();
					taskTransactionArgument.DropStep = dynamicQuestDefinition.DropStep;
					transactionArg.Add(taskTransactionArgument);
					transactionArg.InstanceId = quest.QuestIconTrackedInstanceId;
					transactionArg.Source = "TSMTrigger";
					playerService.RunEntireTransaction(reward, global::Kampai.Game.TransactionTarget.TASK_COMPLETE, transactionCallback, transactionArg);
				}
			}
		}

		private void transactionCallback(global::Kampai.Game.PendingCurrencyTransaction pendingTransaction)
		{
			if (pendingTransaction.Success)
			{
				goToNextQuestStateSignal.Dispatch(quest.GetActiveDefinition().ID);
				updateTSMQuestTaskSignal.Dispatch(quest, true);
				setStorageCapacitySignal.Dispatch();
			}
		}
	}
}
