namespace Kampai.Game.Trigger
{
	public class CheckTriggersCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CheckTriggersCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.Game.TSMCharacter tsmCharacter;

		[Inject]
		public int questGiverId { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ITriggerService triggerService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateTSMQuestTaskSignal createTSMQuestTaskSignal { get; set; }

		[Inject]
		public global::Kampai.Game.HideTSMCharacterSignal hideTSMCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TriggerFiredSignal triggerFiredSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveQuestWorldIconSignal removeQuestWorldIconSignal { get; set; }

		public override void Execute()
		{
			playerService = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (playerService == null || definitionService == null || logger == null)
			{
				return;
			}
			tsmCharacter = playerService.GetByInstanceId<global::Kampai.Game.TSMCharacter>(questGiverId);
			if (tsmCharacter == null)
			{
				logger.Error("Unable to find TSM character in player inventory!");
			}
			else
			{
				if (playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) < 3)
				{
					return;
				}
				triggerService.RemoveOldTriggers();
				global::Kampai.Game.Trigger.TriggerInstance activeTrigger = triggerService.ActiveTrigger;
				if (activeTrigger != null)
				{
					global::Kampai.Game.Trigger.TriggerDefinition definition = activeTrigger.Definition;
					if (definition.ForceOverride)
					{
						logger.Info("Trigger fired! {0}", definition);
						triggerFiredSignal.Dispatch(activeTrigger);
						return;
					}
					if (TryToFireTrigger(true))
					{
						return;
					}
				}
				TryToFireTrigger(false);
			}
		}

		private bool TryToFireTrigger(bool activeTriggerExists)
		{
			bool result = false;
			global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerDefinition> triggerDefinitions = definitionService.GetTriggerDefinitions();
			global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition = CheckForFiredTrigger(triggerDefinitions);
			if (triggerDefinition == null && !activeTriggerExists)
			{
				logger.Info("No Trigger found, using the TSM Level Bands");
				UseLevelBands();
				return result;
			}
			if (triggerDefinition != null)
			{
				if (activeTriggerExists && triggerDefinition.ForceOverride)
				{
					ClearTriggerData();
				}
				else if (!activeTriggerExists)
				{
					if (tsmCharacter.Created)
					{
						ClearQuestData();
						return true;
					}
					global::Kampai.Game.Trigger.TriggerInstance type = triggerService.AddActiveTrigger(triggerDefinition);
					logger.Info("Trigger fired! {0}", triggerDefinition);
					triggerFiredSignal.Dispatch(type);
				}
				else if (activeTriggerExists && !tsmCharacter.Created)
				{
					triggerFiredSignal.Dispatch(triggerService.ActiveTrigger);
				}
				result = true;
			}
			return result;
		}

		public global::Kampai.Game.Trigger.TriggerDefinition CheckForFiredTrigger(global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerDefinition> triggerDefs)
		{
			if (triggerDefs == null || triggerDefs.Count == 0)
			{
				return null;
			}
			global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition = null;
			for (int i = 0; i < triggerDefs.Count; i++)
			{
				triggerDefinition = triggerDefs[i];
				if (triggerDefinition == null || triggerDefinition.type != global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier.TSM || !triggerDefinition.IsTriggered(gameContext))
				{
					triggerDefinition = null;
					continue;
				}
				break;
			}
			return triggerDefinition;
		}

		public void UseLevelBands()
		{
			createTSMQuestTaskSignal.Dispatch(questGiverId);
		}

		public void ClearQuestData()
		{
			global::Kampai.Game.IQuestController questControllerByDefinitionID = questService.GetQuestControllerByDefinitionID(77777);
			if (questControllerByDefinitionID != null && questControllerByDefinitionID.Quest != null)
			{
				removeQuestWorldIconSignal.Dispatch(questControllerByDefinitionID.Quest);
				questService.RemoveQuest(questControllerByDefinitionID);
			}
			hideTSMCharacterSignal.Dispatch(global::Kampai.Game.View.TSMCharacterHideState.CelebrateAndReturn);
			tsmCharacter.Created = false;
		}

		public void ClearTriggerData()
		{
			triggerService.ResetCurrentTrigger();
			hideTSMCharacterSignal.Dispatch(global::Kampai.Game.View.TSMCharacterHideState.CelebrateAndReturn);
			tsmCharacter.Created = false;
		}
	}
}
