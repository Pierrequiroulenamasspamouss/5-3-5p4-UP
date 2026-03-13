namespace Kampai.Game
{
	public class DeliverTaskItemCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DeliverTaskItemCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.Transaction.TransactionDefinition currentTransactionDefinition;

		private global::Kampai.Game.Quest quest;

		private int questStepIndex;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateProceduralQuestPanelSignal updateSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Util.Tuple<int, int> tuple { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageCapacitySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetGrindCurrencySignal setGrindCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		public override void Execute()
		{
			int item = tuple.Item1;
			questStepIndex = tuple.Item2;
			quest = playerService.GetByInstanceId<global::Kampai.Game.Quest>(item);
			if (quest == null)
			{
				logger.Error("Quest instance doesn't exist for id {0}", item);
				return;
			}
			global::Kampai.Game.QuestDefinition activeDefinition = quest.GetActiveDefinition();
			global::Kampai.Game.QuestStepDefinition questStepDefinition = activeDefinition.QuestSteps[questStepIndex];
			global::Kampai.Game.QuestStep questStep = quest.Steps[questStepIndex];
			if (questStep.state == global::Kampai.Game.QuestStepState.WaitComplete)
			{
				quest.AutoGrantReward = true;
				int iD = activeDefinition.ID;
				global::Kampai.Game.IQuestStepController questStepController = questService.GetQuestStepController(iD, questStepIndex);
				questStepController.GoToNextState();
				if (quest.state == global::Kampai.Game.QuestState.RunningTasks)
				{
					globalSFXSignal.Dispatch("Play_completePartQuest_01");
				}
			}
			else if (questStepDefinition.Type == global::Kampai.Game.QuestStepType.Delivery || questStepDefinition.Type == global::Kampai.Game.QuestStepType.Harvest)
			{
				HandleDeliveryAndHarvestTask(questStepDefinition, activeDefinition);
			}
		}

		private void HandleDeliveryAndHarvestTask(global::Kampai.Game.QuestStepDefinition questStepDefinition, global::Kampai.Game.QuestDefinition questDefinition)
		{
			currentTransactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
			currentTransactionDefinition.ID = quest.ID;
			currentTransactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			currentTransactionDefinition.Inputs.Add(new global::Kampai.Util.QuantityItem(questStepDefinition.ItemDefinitionID, (uint)quest.GetActiveDefinition().QuestSteps[questStepIndex].ItemAmount));
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> missingItemListFromTransaction = playerService.GetMissingItemListFromTransaction(currentTransactionDefinition);
			if (missingItemListFromTransaction.Count != 0 && questDefinition.SurfaceType != global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated)
			{
				int rushCost = playerService.CalculateRushCost(missingItemListFromTransaction);
				playerService.ProcessRush(rushCost, missingItemListFromTransaction, true, RushItemCallBack, true);
				return;
			}
			if (questStepDefinition.Type == global::Kampai.Game.QuestStepType.Harvest)
			{
				currentTransactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				currentTransactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(questStepDefinition.ItemDefinitionID, (uint)quest.GetActiveDefinition().QuestSteps[questStepIndex].ItemAmount));
			}
			RunActualTransaction();
		}

		private void RushItemCallBack(global::Kampai.Game.PendingCurrencyTransaction pendingTransaction)
		{
			if (pendingTransaction.Success)
			{
				globalSFXSignal.Dispatch("Play_button_premium_01");
				RunActualTransaction();
			}
		}

		private void RunActualTransaction()
		{
			global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg();
			transactionArg.IsFromQuestSource = ((!quest.IsDynamic()) ? 1 : 2);
			playerService.RunEntireTransaction(currentTransactionDefinition, global::Kampai.Game.TransactionTarget.INGREDIENT, transactionCallback, transactionArg);
		}

		private void transactionCallback(global::Kampai.Game.PendingCurrencyTransaction pendingTransaction)
		{
			if (!pendingTransaction.Success)
			{
				return;
			}
			setGrindCurrencySignal.Dispatch();
			setPremiumCurrencySignal.Dispatch();
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = pendingTransaction.GetInputs();
			if (inputs.Count >= 1)
			{
				global::Kampai.Game.QuestStepDefinition questStepDefinition = quest.GetActiveDefinition().QuestSteps[questStepIndex];
				if (questStepDefinition.ItemDefinitionID == inputs[0].ID)
				{
					quest.AutoGrantReward = true;
					int iD = quest.GetActiveDefinition().ID;
					global::Kampai.Game.IQuestStepController questStepController = questService.GetQuestStepController(iD, questStepIndex);
					questStepController.GoToNextState(true);
				}
				if (questStepDefinition.Type == global::Kampai.Game.QuestStepType.Delivery)
				{
					questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Delivery);
				}
				else
				{
					questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Harvest);
				}
				if (quest.state == global::Kampai.Game.QuestState.RunningTasks)
				{
					globalSFXSignal.Dispatch("Play_completePartQuest_01");
				}
				updateSignal.Dispatch(quest.ID);
				setStorageCapacitySignal.Dispatch();
			}
		}
	}
}
