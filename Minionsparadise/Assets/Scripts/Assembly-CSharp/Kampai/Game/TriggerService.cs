namespace Kampai.Game
{
	public class TriggerService : global::Kampai.Game.ITriggerService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("TriggerService") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.Trigger.TriggerInstance currentActiveTrigger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public global::Kampai.Game.TriggerFiredSignal triggerFiredSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CreateTSMCharacterWithTriggerSignal createTSMCharacterWithTriggerSignal { get; set; }

		public global::Kampai.Game.Trigger.TriggerInstance ActiveTrigger
		{
			get
			{
				if (currentActiveTrigger == null)
				{
					Initialize();
				}
				return currentActiveTrigger;
			}
		}

		~TriggerService()
		{
			if (triggerFiredSignal != null)
			{
				triggerFiredSignal.RemoveListener(TriggerFiredCallback);
			}
		}

		private void TriggerFiredCallback(global::Kampai.Game.Trigger.TriggerInstance triggerInstance)
		{
			global::Kampai.Game.TSMCharacter byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TSMCharacter>(301);
			if (!byInstanceId.Created)
			{
				createTSMCharacterWithTriggerSignal.Dispatch();
			}
		}

		public global::Kampai.Game.Trigger.TriggerInstance AddActiveTrigger(global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition)
		{
			currentActiveTrigger = playerService.AddTrigger(triggerDefinition);
			currentActiveTrigger.StartGameTime = -1;
			ProcessRewardTransactionFromTriggerCondition(currentActiveTrigger);
			return currentActiveTrigger;
		}

		public void ResetCurrentTrigger()
		{
			if (currentActiveTrigger != null)
			{
				playerService.RemoveTrigger(currentActiveTrigger);
				currentActiveTrigger = null;
			}
		}

		private void ProcessRewardTransactionFromTriggerCondition(global::Kampai.Game.Trigger.TriggerInstance trigger)
		{
			global::Kampai.Game.Trigger.TriggerDefinition definition = trigger.Definition;
			global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerRewardDefinition> rewards = definition.rewards;
			if (rewards == null || rewards.Count < 1)
			{
				logger.Error("Current Trigger {0} doesn't have any reward defined, ingoring...", definition.ID);
				return;
			}
			global::Kampai.Game.Trigger.TriggerRewardDefinition triggerRewardDefinition = rewards[0];
			if (triggerRewardDefinition.transaction == null)
			{
				triggerRewardDefinition.transaction = new global::Kampai.Game.Transaction.TransactionInstance();
			}
			if (triggerRewardDefinition.transaction.Outputs == null)
			{
				triggerRewardDefinition.transaction.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			}
			for (int i = 0; i < definition.conditions.Count; i++)
			{
				global::Kampai.Game.Trigger.TriggerConditionDefinition triggerConditionDefinition = definition.conditions[i];
				global::Kampai.Game.Transaction.TransactionDefinition dynamicTriggerTransaction = triggerConditionDefinition.GetDynamicTriggerTransaction(gameContext);
				if (dynamicTriggerTransaction != null && global::Kampai.Game.Transaction.TransactionDataExtension.GetInputCount(dynamicTriggerTransaction) > 0)
				{
					global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = dynamicTriggerTransaction.Inputs;
					for (int j = 0; j < inputs.Count; j++)
					{
						triggerRewardDefinition.transaction.Outputs.Add(inputs[j]);
					}
				}
			}
		}

		public void RemoveOldTriggers()
		{
			if (playerService == null)
			{
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerInstance> triggers = playerService.GetTriggers();
			if (triggers == null)
			{
				return;
			}
			for (int i = 0; i < triggers.Count; i++)
			{
				global::Kampai.Game.Trigger.TriggerInstance triggerInstance = triggers[i];
				if (triggerInstance != null && triggerInstance.StartGameTime != -1)
				{
					if (triggerInstance == currentActiveTrigger)
					{
						currentActiveTrigger = null;
					}
					if (playerDurationService.GetGameTimeDuration(triggerInstance) >= triggerInstance.Definition.cooldownSeconds)
					{
						playerService.RemoveTrigger(triggerInstance);
					}
				}
			}
		}

		public void Initialize()
		{
			triggerFiredSignal.AddListener(TriggerFiredCallback);
			global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerInstance> triggers = playerService.GetTriggers();
			for (int i = 0; i < triggers.Count; i++)
			{
				global::Kampai.Game.Trigger.TriggerInstance triggerInstance = triggers[i];
				if (triggerInstance != null && triggerInstance.StartGameTime == -1)
				{
					currentActiveTrigger = triggerInstance;
					ProcessRewardTransactionFromTriggerCondition(currentActiveTrigger);
					break;
				}
			}
		}
	}
}
